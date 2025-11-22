using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using Domain.Entities.MachineManagement;
using Domain.Entities.MaintenanceOperations;
using Domain.Entities.ProjectManagement;
using Domain.Entities.UserManagement;
using Domain.Entities.Notifications;
using Application.DTOs.MachineManagement;
using Application.Interfaces;
using Application.Interfaces.UserManagement;
using Application.Interfaces.MachineManagement;
using System.Text.Json;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace API.Controllers
{
    [ApiController]
    [Route("api/machines")]
    [Authorize]
    public class MachinesController : BaseApiController
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<MachinesController> _logger;
        private readonly INotificationRepository _notificationRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMachineServiceConfigService _serviceConfigService;

        public MachinesController(
            ApplicationDbContext context,
            ILogger<MachinesController> logger,
            INotificationRepository notificationRepository,
            IUserRepository userRepository,
            IMachineServiceConfigService serviceConfigService)
        {
            _context = context;
            _logger = logger;
            _notificationRepository = notificationRepository;
            _userRepository = userRepository;
            _serviceConfigService = serviceConfigService;
        }

        // GET: api/machines
        [HttpGet]
        public async Task<IActionResult> GetMachines()
            {
                var machines = await _context.Machines
                    .Include(m => m.Project)
                    .Include(m => m.Operator)
                    .Include(m => m.Region)
                    .ToListAsync();

                return Ok(machines);
        }

        // GET: api/machines/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMachine(int id)
            {
                var machine = await _context.Machines
                    .Include(m => m.Project)
                    .Include(m => m.Operator)
                    .Include(m => m.Region)
                    .FirstOrDefaultAsync(m => m.Id == id);

                if (machine == null)
                {
                    return NotFound($"Machine with ID {id} not found");
                }

                return Ok(machine);
        }

        // GET: api/machines/operator/5
        [HttpGet("operator/{operatorId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetMachineByOperator(int operatorId)
        {
            // Authorization check temporarily disabled for testing
            // var userId = int.Parse(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
            // if (userId != operatorId && !User.IsInRole("Admin") && !User.IsInRole("MechanicalEngineer"))
            // {
            //     return Forbid();
            // }

            var machine = await _context.Machines
                .Include(m => m.Region)
                .FirstOrDefaultAsync(m => m.OperatorId == operatorId);

            if (machine == null)
            {
                _logger.LogInformation("No machine found assigned to operator {OperatorId}", operatorId);
                return NotFound($"No machine assigned to operator {operatorId}");
            }

            var operatorMachine = new
            {
                id = machine.Id.ToString(),
                name = machine.Name,
                model = machine.Model,
                serialNumber = machine.SerialNumber,
                currentLocation = machine.CurrentLocation ?? machine.Region?.Name ?? "Unknown",
                status = machine.Status.ToString(),
                assignedOperatorId = machine.OperatorId.ToString()
            };

            return Ok(operatorMachine);
        }

        // GET: api/machines/available
        [HttpGet("available")]
        public async Task<IActionResult> GetAvailableMachines()
        {
            var availableMachines = await _context.Machines
                .Include(m => m.Project)
                .Include(m => m.Operator)
                .Include(m => m.Region)
                .Where(m => m.Status == MachineStatus.Available && m.OperatorId == null)
                .ToListAsync();

            return Ok(availableMachines);
        }

        // POST: api/machines
        [HttpPost]
        [Authorize(Policy = "ManageMachines")]
        public async Task<IActionResult> CreateMachine(CreateMachineRequest request)
        {
                var existingMachine = await _context.Machines
                    .FirstOrDefaultAsync(m => m.SerialNumber == request.SerialNumber);
                if (existingMachine != null)
                {
                return Conflict($"A machine with serial number '{request.SerialNumber}' already exists");
                }

                // Project validation (optional)
                if (request.ProjectId.HasValue)
                {
                    var projectExists = await _context.Projects.AnyAsync(p => p.Id == request.ProjectId.Value);
                if (!projectExists)
                {
                        return BadRequest($"Project with ID {request.ProjectId.Value} not found");
                    }
                }

                if (request.OperatorId.HasValue)
                {
                    var operatorExists = await _context.Users.AnyAsync(u => u.Id == request.OperatorId.Value);
                    if (!operatorExists)
                    {
                        return BadRequest($"Operator with ID {request.OperatorId.Value} not found");
                    }
                }

                if (request.RegionId.HasValue)
                {
                    var regionExists = await _context.Regions.AnyAsync(r => r.Id == request.RegionId.Value);
                    if (!regionExists)
                    {
                        return BadRequest($"Region with ID {request.RegionId.Value} not found");
                    }
                }

                if (!Enum.TryParse<MachineStatus>(request.Status, true, out var statusEnum))
                {
                    return BadRequest($"Invalid status '{request.Status}'. Valid values: {string.Join(", ", Enum.GetNames<MachineStatus>())}");
                }

                var machine = Machine.Create(
                    request.Name,
                    request.Type,
                    request.Model,
                    request.Manufacturer,
                    request.SerialNumber,
                    request.ProjectId);

                machine.RigNo = request.RigNo;
                machine.PlateNo = request.PlateNo;
                machine.ChassisDetails = request.ChassisDetails;
                machine.ManufacturingYear = request.ManufacturingYear;
                machine.ChangeStatus(statusEnum);
                machine.CurrentLocation = request.CurrentLocation;
                machine.OperatorId = request.OperatorId;
                machine.RegionId = request.RegionId;
                machine.SpecificationsJson = request.Specifications != null ? JsonSerializer.Serialize(request.Specifications) : null;

                // Configure service intervals if provided
                if (request.EngineServiceInterval.HasValue)
                {
                    machine.ConfigureServiceIntervals(
                        request.EngineServiceInterval.Value,
                        request.DrifterServiceInterval
                    );
                }

                // Set current service hours if provided (use Update methods for initial values, not Increment)
                if (request.CurrentEngineServiceHours.HasValue && request.CurrentEngineServiceHours.Value > 0)
                {
                    machine.UpdateCurrentEngineServiceHours(request.CurrentEngineServiceHours.Value);
                }

                if (request.CurrentDrifterServiceHours.HasValue && request.CurrentDrifterServiceHours.Value > 0)
                {
                    machine.UpdateCurrentDrifterServiceHours(request.CurrentDrifterServiceHours.Value);
                }

                // Set last service dates if provided
                if (request.LastEngineServiceDate.HasValue)
                {
                    machine.UpdateLastEngineServiceDate(request.LastEngineServiceDate.Value);
                }

                if (request.LastDrifterServiceDate.HasValue)
                {
                    machine.UpdateLastDrifterServiceDate(request.LastDrifterServiceDate.Value);
                }

                if (request.ProjectId.HasValue)
                {
                    var project = await _context.Projects.FindAsync(request.ProjectId.Value);
                machine.AssignedToProject = project?.Name;
                }

                if (request.OperatorId.HasValue)
                {
                    var operatorUser = await _context.Users.FindAsync(request.OperatorId.Value);
                    machine.AssignedToOperator = operatorUser?.Name;
                }

                _context.Machines.Add(machine);
                await _context.SaveChangesAsync();

            return Created(machine, nameof(GetMachine));
        }

        // PUT: api/machines/5
        [HttpPut("{id}")]
        [Authorize(Policy = "ManageMachines")]
        public async Task<IActionResult> UpdateMachine(int id, UpdateMachineRequest request)
        {
                try
                {
                    _logger.LogInformation($"Updating machine {id} with data: {JsonSerializer.Serialize(request)}");
                    
                    var machine = await _context.Machines.FindAsync(id);
                    if (machine == null)
                    {
                        return NotFound($"Machine with ID {id} not found");
                    }

                var existingMachine = await _context.Machines
                    .FirstOrDefaultAsync(m => m.SerialNumber == request.SerialNumber && m.Id != id);
                if (existingMachine != null)
                {
                return Conflict($"A machine with serial number '{request.SerialNumber}' already exists");
                }

                if (request.ProjectId.HasValue && request.ProjectId.Value > 0)
                {
                    var projectExists = await _context.Projects.AnyAsync(p => p.Id == request.ProjectId.Value);
                    if (!projectExists)
                    {
                        return BadRequest($"Project with ID {request.ProjectId.Value} not found");
                    }
                }

                if (request.OperatorId.HasValue)
                {
                    var operatorExists = await _context.Users.AnyAsync(u => u.Id == request.OperatorId.Value);
                    if (!operatorExists)
                    {
                        return BadRequest($"Operator with ID {request.OperatorId.Value} not found");
                    }
                }

                if (request.RegionId.HasValue)
                {
                    var regionExists = await _context.Regions.AnyAsync(r => r.Id == request.RegionId.Value);
                    if (!regionExists)
                    {
                        return BadRequest($"Region with ID {request.RegionId.Value} not found");
                    }
                }

                machine.Name = request.Name;
                machine.Type = request.Type;
                machine.Model = request.Model;
                machine.Manufacturer = request.Manufacturer;
                machine.SerialNumber = request.SerialNumber;
                machine.RigNo = request.RigNo;
                machine.PlateNo = request.PlateNo;
                machine.ChassisDetails = request.ChassisDetails;
                machine.ManufacturingYear = request.ManufacturingYear;
                machine.MarkUpdated();
                machine.SpecificationsJson = request.Specifications != null ? 
                    JsonSerializer.Serialize(request.Specifications) : null;

                if (request.ProjectId.HasValue && request.ProjectId.Value > 0)
                {
                    var project = await _context.Projects.FindAsync(request.ProjectId.Value);
                    machine.AssignedToProject = project?.Name;
                }
                else
                {
                    machine.AssignedToProject = null;
                }

                if (request.OperatorId.HasValue)
                {
                    var operatorUser = await _context.Users.FindAsync(request.OperatorId.Value);
                    machine.AssignedToOperator = operatorUser?.Name;
                }
                else
                {
                    machine.AssignedToOperator = null;
                }

            if (request.RegionId.HasValue)
                    {
                var region = await _context.Regions.FindAsync(request.RegionId.Value);
                machine.CurrentLocation = region?.Name;
                }
            else
            {
                machine.CurrentLocation = null;
            }

            // If operator is assigned, but no region is specified, ensure a default region
            if (request.OperatorId.HasValue && !request.RegionId.HasValue)
            {
                // Use the first available region as default
                var defaultRegion = await _context.Regions.FirstOrDefaultAsync();
                if (defaultRegion != null)
                {
                    request.RegionId = defaultRegion.Id;
                    machine.RegionId = defaultRegion.Id;
                    machine.CurrentLocation = defaultRegion.Name;
                }
            }
            
            machine.ProjectId = request.ProjectId;
            machine.OperatorId = request.OperatorId;
            machine.RegionId = request.RegionId;

            await _context.SaveChangesAsync();

            return Ok();
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error updating machine {id}: {ex.Message}");
                    return BadRequest(new { error = ex.Message, details = ex.ToString() });
                }
        }

        // DELETE: api/machines/5
        [HttpDelete("{id}")]
        [Authorize(Policy = "ManageMachines")]
        public async Task<IActionResult> DeleteMachine(int id)
        {
            try
            {
                var machine = await _context.Machines.FindAsync(id);
                if (machine == null)
                {
                    return NotFound($"Machine with ID {id} not found");
                }

                // Delete all related records first (due to Restrict foreign key constraints)

                // 1. Delete usage logs
                var usageLogs = await _context.Set<MachineUsageLog>()
                    .Where(l => l.MachineId == id)
                    .ToListAsync();

                if (usageLogs.Any())
                {
                    _context.Set<MachineUsageLog>().RemoveRange(usageLogs);
                    _logger.LogInformation($"Deleting {usageLogs.Count} usage logs for machine {id}");
                }

                // 2. Delete maintenance jobs
                var maintenanceJobs = await _context.Set<MaintenanceJob>()
                    .Where(j => j.MachineId == id)
                    .ToListAsync();

                if (maintenanceJobs.Any())
                {
                    _context.Set<MaintenanceJob>().RemoveRange(maintenanceJobs);
                    _logger.LogInformation($"Deleting {maintenanceJobs.Count} maintenance jobs for machine {id}");
                }

                // 3. Delete maintenance reports
                var maintenanceReports = await _context.Set<MaintenanceReport>()
                    .Where(r => r.MachineId == id)
                    .ToListAsync();

                if (maintenanceReports.Any())
                {
                    _context.Set<MaintenanceReport>().RemoveRange(maintenanceReports);
                    _logger.LogInformation($"Deleting {maintenanceReports.Count} maintenance reports for machine {id}");
                }

                // 4. Now delete the machine
                _context.Machines.Remove(machine);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Successfully deleted machine {id} and all related records");
                return Ok(new { message = $"Machine {machine.Name} and all related records deleted successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting machine {id}: {ex.Message}");
                return StatusCode(500, new { error = "Failed to delete machine", details = ex.Message });
            }
        }

        [HttpPatch("{id}/status")]
        [Authorize(Policy = "ManageMachines")]
        public async Task<IActionResult> UpdateMachineStatus(int id, [FromBody] UpdateMachineStatusRequest request)
        {
                var machine = await _context.Machines.FindAsync(id);
                if (machine == null)
                {
                    return NotFound($"Machine with ID {id} not found");
                }

                if (!Enum.TryParse<MachineStatus>(request.Status, true, out var statusEnum))
                {
                    return BadRequest($"Invalid status '{request.Status}'. Valid values: {string.Join(", ", Enum.GetNames<MachineStatus>())}");
                }

                machine.ChangeStatus(statusEnum);
                machine.MarkUpdated();

                await _context.SaveChangesAsync();

            return Ok();
        }

        // GET: api/machines/search
        [HttpGet("search")]
        public async Task<IActionResult> SearchMachines(
            [FromQuery] string? name = null,
            [FromQuery] string? type = null,
            [FromQuery] string? status = null,
            [FromQuery] string? manufacturer = null,
            [FromQuery] string? serialNumber = null)
        {
            var query = _context.Machines.AsQueryable();

                if (!string.IsNullOrEmpty(name))
                {
                    query = query.Where(m => m.Name.Contains(name));
                }
                if (!string.IsNullOrEmpty(type))
                {
                query = query.Where(m => m.Type.Contains(type));
                }
            if (!string.IsNullOrEmpty(status) && Enum.TryParse<MachineStatus>(status, true, out var statusEnum))
                    {
                    query = query.Where(m => m.Status == statusEnum);
                }
                if (!string.IsNullOrEmpty(manufacturer))
                {
                    query = query.Where(m => m.Manufacturer.Contains(manufacturer));
                }
                if (!string.IsNullOrEmpty(serialNumber))
                {
                    query = query.Where(m => m.SerialNumber.Contains(serialNumber));
                }

                var machines = await query
                .Include(m => m.Project)
                .Include(m => m.Operator)
                .Include(m => m.Region)
                    .ToListAsync();

                return Ok(machines);
        }

        // GET: api/machines/statistics
        [HttpGet("statistics")]
        public async Task<IActionResult> GetMachineStatistics()
            {
                var totalMachines = await _context.Machines.CountAsync();
            var availableMachines = await _context.Machines.CountAsync(m => m.Status == MachineStatus.Available);
            var assignedMachines = await _context.Machines.CountAsync(m => m.Status == MachineStatus.Assigned);
            var maintenanceMachines = await _context.Machines.CountAsync(m => m.Status == MachineStatus.InMaintenance);
            var outOfServiceMachines = await _context.Machines.CountAsync(m =>
                m.Status == MachineStatus.OutOfService || m.Status == MachineStatus.UnderRepair);
            var pendingAssignmentRequests = await _context.MachineAssignmentRequests.CountAsync(r => r.Status == AssignmentRequestStatus.Pending);

            var statistics = new
                {
                    TotalMachines = totalMachines,
                AvailableMachines = availableMachines,
                AssignedMachines = assignedMachines,
                MaintenanceMachines = maintenanceMachines,
                OutOfServiceMachines = outOfServiceMachines,
                PendingAssignmentRequests = pendingAssignmentRequests
            };

            return Ok(statistics);
        }

        // Machine Assignment Request Endpoints
        [HttpGet("assignment-requests")]
        [Authorize(Policy = "ManageMachines")]
        public async Task<IActionResult> GetAssignmentRequests()
        {
            var requests = await _context.MachineAssignmentRequests
                .Include(r => r.Project)
                .OrderByDescending(r => r.RequestedDate)
                .ToListAsync();

            var requestDtos = requests.Select(r =>
            {
                List<int>? assignedMachines = null;
                if (!string.IsNullOrEmpty(r.AssignedMachinesJson))
                {
                    try
                    {
                        // Try int array first
                        assignedMachines = JsonSerializer.Deserialize<List<int>>(r.AssignedMachinesJson);
                    }
                    catch
                    {
                        try
                        {
                            // Fallback to string array, then convert to int
                            var stringIds = JsonSerializer.Deserialize<List<string>>(r.AssignedMachinesJson);
                            assignedMachines = stringIds?.Select(id => int.TryParse(id, out var machineId) ? machineId : 0)
                                                         .Where(id => id > 0)
                                                         .ToList();
                        }
                        catch
                        {
                            assignedMachines = null;
                        }
                    }
                }

                return new Application.DTOs.MachineManagement.MachineAssignmentRequestDto
                {
                    Id = r.Id,
                    ProjectId = r.ProjectId,
                    ProjectName = r.Project?.Name ?? "Unknown Project",
                    MachineType = r.MachineType,
                    Quantity = r.Quantity,
                    RequestedBy = r.RequestedBy,
                    RequestedDate = r.RequestedDate,
                    Status = r.Status.ToString(),
                    Urgency = r.Urgency.ToString(),
                    DetailsOrExplanation = r.DetailsOrExplanation,
                    ApprovedBy = r.ApprovedBy,
                    ApprovedDate = r.ApprovedDate,
                    AssignedMachines = assignedMachines,
                    Comments = r.Comments,
                    ExpectedUsageDuration = r.ExpectedUsageDuration,
                    ExpectedReturnDate = r.ExpectedReturnDate,
                    CreatedAt = r.CreatedAt,
                    UpdatedAt = r.UpdatedAt
                };
            }).ToList();

            return Ok(requestDtos);
        }

        [HttpPost("assignment-requests")]
        [Authorize(Policy = "ManageMachines")]
        public async Task<IActionResult> SubmitAssignmentRequest([FromBody] Application.DTOs.MachineManagement.CreateAssignmentRequestDto request)
        {
            // Validate project exists
            var projectExists = await _context.Projects.AnyAsync(p => p.Id == request.ProjectId);
            if (!projectExists)
            {
                return BadRequest($"Project with ID {request.ProjectId} not found");
            }

            // Parse urgency enum
            if (!Enum.TryParse<RequestUrgency>(request.Urgency, true, out var urgencyEnum))
            {
                return BadRequest($"Invalid urgency '{request.Urgency}'. Valid values: {string.Join(", ", Enum.GetNames<RequestUrgency>())}");
            }

            // Create assignment request
            var assignmentRequest = MachineAssignmentRequest.Create(
                request.ProjectId,
                request.MachineType,
                request.Quantity,
                request.RequestedBy,
                urgencyEnum,
                request.DetailsOrExplanation,
                request.ExpectedUsageDuration,
                request.ExpectedReturnDate
            );

            _context.MachineAssignmentRequests.Add(assignmentRequest);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAssignmentRequests), new { id = assignmentRequest.Id }, assignmentRequest);
        }

        [HttpPatch("assignment-requests/{id}/approve")]
        [Authorize(Policy = "ManageMachines")]
        public async Task<IActionResult> ApproveAssignmentRequest(int id, [FromBody] Application.DTOs.MachineManagement.ApproveAssignmentRequestDto request)
        {
            var assignmentRequest = await _context.MachineAssignmentRequests
                .Include(r => r.Project)
                .FirstOrDefaultAsync(r => r.Id == id);
            if (assignmentRequest == null)
            {
                return NotFound($"Assignment request with ID {id} not found");
            }

            if (assignmentRequest.Status != AssignmentRequestStatus.Pending)
            {
                return BadRequest($"Cannot approve request with status {assignmentRequest.Status}");
            }

            // Validate assigned machines exist and are available
            foreach (var machineId in request.AssignedMachines)
            {
                var machine = await _context.Machines.FindAsync(machineId);
                if (machine == null)
                {
                    return BadRequest($"Machine with ID {machineId} not found");
                }
                if (machine.Status != MachineStatus.Available)
                {
                    return BadRequest($"Machine '{machine.Name}' (ID: {machineId}) is not available (current status: {machine.Status})");
                }
            }

            // Get approver name from claims
            var approverName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value ?? "Unknown";

            // Approve the request
            assignmentRequest.Approve(approverName, request.AssignedMachines.Select(m => m.ToString()).ToArray(), request.Comments);

            // Update machine statuses to Assigned
            foreach (var machineId in request.AssignedMachines)
            {
                var machine = await _context.Machines.FindAsync(machineId);
                if (machine != null)
                {
                    machine.ChangeStatus(MachineStatus.Assigned);
                    machine.ProjectId = assignmentRequest.ProjectId;
                    var project = await _context.Projects.FindAsync(assignmentRequest.ProjectId);
                    machine.AssignedToProject = project?.Name;
                }
            }

            await _context.SaveChangesAsync();

            // Create notification for the requester
            try
            {
                var requester = await _userRepository.GetByNameAsync(assignmentRequest.RequestedBy);
                if (requester != null)
                {
                    var isPartiallyFulfilled = request.AssignedMachines.Count < assignmentRequest.Quantity;
                    var notificationType = isPartiallyFulfilled
                        ? NotificationType.MachineRequestPartial
                        : NotificationType.MachineRequestApproved;

                    var title = isPartiallyFulfilled
                        ? "Machine Request Partially Approved"
                        : "Machine Request Approved";

                    var message = isPartiallyFulfilled
                        ? $"Your request for {assignmentRequest.Quantity} {assignmentRequest.MachineType} machines has been partially approved. {request.AssignedMachines.Count} machine(s) have been assigned for project '{assignmentRequest.Project?.Name ?? "Unknown"}'."
                        : $"Your request for {assignmentRequest.Quantity} {assignmentRequest.MachineType} machine(s) has been approved for project '{assignmentRequest.Project?.Name ?? "Unknown"}'.";

                    if (!string.IsNullOrWhiteSpace(request.Comments))
                    {
                        message += $" Comments: {request.Comments}";
                    }

                    var notification = Notification.Create(
                        userId: requester.Id,
                        type: notificationType,
                        title: title,
                        message: message,
                        priority: NotificationPriority.High,
                        relatedEntityType: "MachineAssignmentRequest",
                        relatedEntityId: assignmentRequest.Id,
                        actionUrl: $"/machine-requests/{assignmentRequest.Id}"
                    );

                    await _notificationRepository.CreateAsync(notification);
                    _logger.LogInformation($"Notification created for user {requester.Id} - Machine request {id} approved");
                }
                else
                {
                    _logger.LogWarning($"Could not find user with name '{assignmentRequest.RequestedBy}' to send approval notification");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating notification for machine request approval: {ex.Message}");
                // Don't fail the request if notification fails
            }

            return Ok(assignmentRequest);
        }

        [HttpPatch("assignment-requests/{id}/reject")]
        [Authorize(Policy = "ManageMachines")]
        public async Task<IActionResult> RejectAssignmentRequest(int id, [FromBody] Application.DTOs.MachineManagement.RejectAssignmentRequestDto request)
        {
            var assignmentRequest = await _context.MachineAssignmentRequests
                .Include(r => r.Project)
                .FirstOrDefaultAsync(r => r.Id == id);
            if (assignmentRequest == null)
            {
                return NotFound($"Assignment request with ID {id} not found");
            }

            if (assignmentRequest.Status != AssignmentRequestStatus.Pending)
            {
                return BadRequest($"Cannot reject request with status {assignmentRequest.Status}");
            }

            // Get rejector name from claims
            var rejectorName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value ?? "Unknown";

            // Reject the request
            assignmentRequest.Reject(rejectorName, request.Comments);

            await _context.SaveChangesAsync();

            // Create notification for the requester
            try
            {
                var requester = await _userRepository.GetByNameAsync(assignmentRequest.RequestedBy);
                if (requester != null)
                {
                    var message = $"Your request for {assignmentRequest.Quantity} {assignmentRequest.MachineType} machine(s) for project '{assignmentRequest.Project?.Name ?? "Unknown"}' has been rejected.";

                    if (!string.IsNullOrWhiteSpace(request.Comments))
                    {
                        message += $" Reason: {request.Comments}";
                    }

                    var notification = Notification.Create(
                        userId: requester.Id,
                        type: NotificationType.MachineRequestRejected,
                        title: "Machine Request Rejected",
                        message: message,
                        priority: NotificationPriority.High,
                        relatedEntityType: "MachineAssignmentRequest",
                        relatedEntityId: assignmentRequest.Id,
                        actionUrl: $"/machine-requests/{assignmentRequest.Id}"
                    );

                    await _notificationRepository.CreateAsync(notification);
                    _logger.LogInformation($"Notification created for user {requester.Id} - Machine request {id} rejected");
                }
                else
                {
                    _logger.LogWarning($"Could not find user with name '{assignmentRequest.RequestedBy}' to send rejection notification");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating notification for machine request rejection: {ex.Message}");
                // Don't fail the request if notification fails
            }

            return Ok(assignmentRequest);
        }

        // Machine Assignment Endpoints
        [HttpGet("assignments/active")]
        [Authorize(Policy = "ManageMachines")]
        public async Task<IActionResult> GetActiveAssignments()
        {
            var activeAssignments = await _context.MachineAssignments
                .Include(a => a.Machine)
                .Include(a => a.Project)
                .Include(a => a.Operator)
                .Where(a => a.Status == AssignmentStatus.Active || a.Status == AssignmentStatus.Overdue)
                .OrderByDescending(a => a.AssignedDate)
                .ToListAsync();

            var assignmentDtos = activeAssignments.Select(a => new Application.DTOs.MachineManagement.MachineAssignmentDto
            {
                Id = a.Id,
                MachineId = a.MachineId,
                MachineName = a.Machine?.Name ?? "Unknown Machine",
                MachineSerialNumber = a.Machine?.SerialNumber ?? "N/A",
                ProjectId = a.ProjectId,
                ProjectName = a.Project?.Name ?? "Unknown Project",
                OperatorId = a.OperatorId,
                OperatorName = a.Operator?.Name ?? "Unknown Operator",
                AssignedBy = a.AssignedBy,
                AssignedDate = a.AssignedDate,
                ExpectedReturnDate = a.ExpectedReturnDate,
                ActualReturnDate = a.ActualReturnDate,
                Status = a.Status.ToString(),
                Location = a.Location,
                Notes = a.Notes,
                CreatedAt = a.CreatedAt,
                UpdatedAt = a.UpdatedAt
            }).ToList();

            return Ok(assignmentDtos);
        }

        [HttpPost("assignments")]
        [Authorize(Policy = "ManageMachines")]
        public async Task<IActionResult> AssignMachine([FromBody] Application.DTOs.MachineManagement.CreateMachineAssignmentDto assignment)
        {
            // Validate machine exists and is available
            var machine = await _context.Machines.FindAsync(assignment.MachineId);
            if (machine == null)
            {
                return NotFound($"Machine with ID {assignment.MachineId} not found");
            }
            if (machine.Status != MachineStatus.Available)
            {
                return BadRequest($"Machine '{machine.Name}' is not available (current status: {machine.Status})");
            }

            // Validate project exists
            var projectExists = await _context.Projects.AnyAsync(p => p.Id == assignment.ProjectId);
            if (!projectExists)
            {
                return BadRequest($"Project with ID {assignment.ProjectId} not found");
            }

            // Validate operator exists
            var operatorExists = await _context.Users.AnyAsync(u => u.Id == assignment.OperatorId);
            if (!operatorExists)
            {
                return BadRequest($"Operator with ID {assignment.OperatorId} not found");
            }

            // Create machine assignment
            var machineAssignment = MachineAssignment.Create(
                assignment.MachineId,
                assignment.ProjectId,
                assignment.OperatorId,
                assignment.AssignedBy,
                assignment.ExpectedReturnDate,
                assignment.Location,
                assignment.Notes
            );

            _context.MachineAssignments.Add(machineAssignment);

            // Update machine status to Assigned
            machine.ChangeStatus(MachineStatus.Assigned);
            machine.ProjectId = assignment.ProjectId;
            machine.OperatorId = assignment.OperatorId;

            var project = await _context.Projects.FindAsync(assignment.ProjectId);
            machine.AssignedToProject = project?.Name;

            var operatorUser = await _context.Users.FindAsync(assignment.OperatorId);
            machine.AssignedToOperator = operatorUser?.Name;

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetActiveAssignments), new { id = machineAssignment.Id }, machineAssignment);
        }

        [HttpPatch("assignments/{id}/return")]
        [Authorize(Policy = "ManageMachines")]
        public async Task<IActionResult> ReturnMachine(int id)
        {
            var assignment = await _context.MachineAssignments
                .Include(a => a.Machine)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (assignment == null)
            {
                return NotFound($"Machine assignment with ID {id} not found");
            }

            if (assignment.Status != AssignmentStatus.Active && assignment.Status != AssignmentStatus.Overdue)
            {
                return BadRequest($"Cannot return machine with assignment status {assignment.Status}");
            }

            // Return the machine
            assignment.Return();

            // Update machine status to Available
            if (assignment.Machine != null)
            {
                assignment.Machine.ChangeStatus(MachineStatus.Available);
                assignment.Machine.ProjectId = null;
                assignment.Machine.OperatorId = null;
                assignment.Machine.AssignedToProject = null;
                assignment.Machine.AssignedToOperator = null;
            }

            await _context.SaveChangesAsync();

            return Ok(assignment);
        }

        // POST: api/machines/test/operator/{operatorId}
        [HttpPost("test/operator/{operatorId}")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateTestMachine(int operatorId)
        {
            var operatorExists = await _context.Users.AnyAsync(u => u.Id == operatorId);
            if (!operatorExists)
            {
                return BadRequest($"Operator with ID {operatorId} not found");
            }
            
            var operatorUser = await _context.Users.FindAsync(operatorId);
            var regionId = 1; // Default region ID
            
            // Create a region if none exists
            var region = await _context.Regions.FirstOrDefaultAsync();
            if (region == null)
            {
                region = new Domain.Entities.ProjectManagement.Region
                {
                    Name = "Test Region",
                    Description = "Test Region Description",
                    Country = "Test Country",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                
                _context.Regions.Add(region);
                await _context.SaveChangesAsync();
                regionId = region.Id;
            }
            else
            {
                regionId = region.Id;
            }

            // Create test machine
            var machine = Domain.Entities.MachineManagement.Machine.Create(
                $"Test Machine for Operator {operatorId}",
                "Drill",
                "Test Model",
                "Test Manufacturer",
                $"SN-TEST-{Guid.NewGuid().ToString().Substring(0, 8)}",
                null);
                
            machine.CurrentLocation = "Test Location";
            machine.OperatorId = operatorId;
            machine.RegionId = regionId;
            machine.AssignedToOperator = operatorUser?.Name ?? $"Operator {operatorId}";
            machine.ChangeStatus(Domain.Entities.MachineManagement.MachineStatus.Available);
                
            _context.Machines.Add(machine);
            await _context.SaveChangesAsync();
            
            var operatorMachine = new
            {
                id = machine.Id.ToString(),
                name = machine.Name,
                model = machine.Model,
                serialNumber = machine.SerialNumber,
                currentLocation = machine.CurrentLocation,
                status = machine.Status.ToString(),
                assignedOperatorId = machine.OperatorId.ToString()
            };
            
            return Ok(operatorMachine);
        }

        private bool MachineExists(int id)
        {
            return _context.Machines.Any(e => e.Id == id);
        }

        // ========== NEW SERVICE TRACKING ENDPOINTS ==========

        /// <summary>
        /// Get service status for a machine
        /// </summary>
        [HttpGet("{id}/service-status")]
        public async Task<IActionResult> GetServiceStatus(int id)
        {
            try
            {
                var result = await _serviceConfigService.GetMachineServiceStatusAsync(id);

                if (!result.IsSuccess)
                {
                    return NotFound(result.Error);
                }

                return base.Ok(result.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving service status for machine {MachineId}", id);
                return InternalServerError("An error occurred while retrieving service status");
            }
        }

        /// <summary>
        /// Get all service due alerts (Machine Manager)
        /// </summary>
        [HttpGet("service-alerts")]
        // [Authorize(Roles = "MachineManager,MechanicalEngineer")]
        public async Task<IActionResult> GetServiceAlerts()
        {
            try
            {
                var result = await _serviceConfigService.GetServiceDueAlertsAsync();

                if (!result.IsSuccess)
                {
                    return BadRequest(result.Error);
                }

                return base.Ok(result.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving service alerts");
                return InternalServerError("An error occurred while retrieving service alerts");
            }
        }

        /// <summary>
        /// Get service alerts by region
        /// </summary>
        [HttpGet("regions/{regionId}/service-alerts")]
        public async Task<IActionResult> GetServiceAlertsByRegion(int regionId)
        {
            try
            {
                var result = await _serviceConfigService.GetServiceDueAlertsByRegionAsync(regionId);

                if (!result.IsSuccess)
                {
                    return BadRequest(result.Error);
                }

                return base.Ok(result.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving service alerts for region {RegionId}", regionId);
                return InternalServerError("An error occurred while retrieving service alerts");
            }
        }

        /// <summary>
        /// Reset service hours (Mechanical Engineer after completing service)
        /// </summary>
        [HttpPost("{id}/reset-service-hours")]
        // [Authorize(Roles = "MechanicalEngineer")]
        public async Task<IActionResult> ResetServiceHours(int id, [FromBody] ResetServiceHoursRequest request)
        {
            try
            {
                var machine = await _context.Machines.FindAsync(id);
                if (machine == null)
                {
                    return NotFound($"Machine with ID {id} not found");
                }

                machine.ResetServiceHours(request.ResetEngine, request.ResetDrifter);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Service hours reset for machine {MachineId}. Engine: {ResetEngine}, Drifter: {ResetDrifter}",
                    id, request.ResetEngine, request.ResetDrifter);

                return base.Ok(new
                {
                    message = "Service hours reset successfully",
                    currentEngineServiceHours = machine.CurrentEngineServiceHours,
                    currentDrifterServiceHours = machine.CurrentDrifterServiceHours,
                    lastEngineServiceDate = machine.LastEngineServiceDate,
                    lastDrifterServiceDate = machine.LastDrifterServiceDate
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error resetting service hours for machine {MachineId}", id);
                return InternalServerError("An error occurred while resetting service hours");
            }
        }

        /// <summary>
        /// Update machine service configuration (Machine Manager)
        /// </summary>
        [HttpPatch("{id}/service-config")]
        // [Authorize(Roles = "MachineManager")]
        public async Task<IActionResult> UpdateServiceConfig(int id, [FromBody] UpdateMachineServiceConfigRequest request)
        {
            try
            {
                var result = await _serviceConfigService.UpdateServiceConfigAsync(id, request);

                if (!result.IsSuccess)
                {
                    return BadRequest(result.Error);
                }

                return base.Ok(new { message = "Service configuration updated successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating service configuration for machine {MachineId}", id);
                return InternalServerError("An error occurred while updating service configuration");
            }
        }
    }

    public class UpdateMachineStatusRequest
    {
        [Required]
        public string Status { get; set; } = string.Empty;
    }

    public class ResetServiceHoursRequest
    {
        public bool ResetEngine { get; set; }
        public bool ResetDrifter { get; set; }
    }
}