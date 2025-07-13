using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using Domain.Entities.MachineManagement;
using Domain.Entities.ProjectManagement;
using Domain.Entities.UserManagement;
using Application.DTOs.MachineManagement;
using System.Text.Json;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MachinesController : BaseApiController
    {
        private readonly ApplicationDbContext _context;

        public MachinesController(ApplicationDbContext context)
        {
            _context = context;
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

                if (request.ProjectId.HasValue)
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

            machine.ProjectId = request.ProjectId;
            machine.OperatorId = request.OperatorId;
            machine.RegionId = request.RegionId;

            await _context.SaveChangesAsync();

            return Ok();
        }

        // DELETE: api/machines/5
        [HttpDelete("{id}")]
        [Authorize(Policy = "ManageMachines")]
        public async Task<IActionResult> DeleteMachine(int id)
            {
                var machine = await _context.Machines.FindAsync(id);
                if (machine == null)
                {
                    return NotFound($"Machine with ID {id} not found");
                }

                _context.Machines.Remove(machine);
                await _context.SaveChangesAsync();

            return Ok();
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

            var statistics = new
                {
                    TotalMachines = totalMachines,
                AvailableMachines = availableMachines,
                AssignedMachines = assignedMachines,
                MaintenanceMachines = maintenanceMachines,
                OutOfServiceMachines = outOfServiceMachines
            };

            return Ok(statistics);
        }

        // Machine Assignment Endpoints (Placeholder implementations)
        [HttpGet("assignment-requests")]
        [Authorize(Policy = "ManageMachines")]
        public async Task<IActionResult> GetAssignmentRequests()
        {
            // TODO: Implement assignment requests functionality
            return Ok(new List<object>());
        }

        [HttpPost("assignment-requests")]
        [Authorize(Policy = "ManageMachines")]
        public async Task<IActionResult> SubmitAssignmentRequest([FromBody] object request)
        {
            // TODO: Implement assignment request submission
            return Ok(new { message = "Assignment request submitted successfully" });
        }

        [HttpPatch("assignment-requests/{id}/approve")]
        [Authorize(Policy = "ManageMachines")]
        public async Task<IActionResult> ApproveAssignmentRequest(string id, [FromBody] object request)
        {
            // TODO: Implement assignment request approval
            return Ok(new { message = "Assignment request approved successfully" });
        }

        [HttpPatch("assignment-requests/{id}/reject")]
        [Authorize(Policy = "ManageMachines")]
        public async Task<IActionResult> RejectAssignmentRequest(string id, [FromBody] object request)
        {
            // TODO: Implement assignment request rejection
            return Ok(new { message = "Assignment request rejected successfully" });
        }

        [HttpGet("assignments/active")]
        [Authorize(Policy = "ManageMachines")]
        public async Task<IActionResult> GetActiveAssignments()
        {
            // TODO: Implement active assignments retrieval
            return Ok(new List<object>());
        }

        [HttpPost("assignments")]
        [Authorize(Policy = "ManageMachines")]
        public async Task<IActionResult> AssignMachine([FromBody] object assignment)
        {
            // TODO: Implement machine assignment
            return Ok(new { message = "Machine assigned successfully" });
        }

        [HttpPatch("assignments/{id}/return")]
        [Authorize(Policy = "ManageMachines")]
        public async Task<IActionResult> ReturnMachine(string id)
        {
            // TODO: Implement machine return
            return Ok(new { message = "Machine returned successfully" });
        }

        private bool MachineExists(int id)
        {
            return _context.Machines.Any(e => e.Id == id);
        }
    }

    public class UpdateMachineStatusRequest
    {
        [Required]
        public string Status { get; set; } = string.Empty;
    }
} 