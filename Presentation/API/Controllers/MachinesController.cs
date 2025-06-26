using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using Domain.Entities;
using Application.DTOs;
using System.Text.Json;
using System.ComponentModel.DataAnnotations;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MachinesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<MachinesController> _logger;

        public MachinesController(
            ApplicationDbContext context, 
            ILogger<MachinesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/machines
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MachineDto>>> GetMachines()
        {
            try
            {
                var machines = await _context.Machines
                    .Include(m => m.Project)
                    .Include(m => m.Operator)
                    .Include(m => m.Region)
                    .Select(m => new MachineDto
                    {
                        Id = m.Id,
                        Name = m.Name,
                        Type = m.Type,
                        Model = m.Model,
                        Manufacturer = m.Manufacturer,
                        SerialNumber = m.SerialNumber,
                        RigNo = m.RigNo,
                        PlateNo = m.PlateNo,
                        ChassisDetails = m.ChassisDetails,
                        ManufacturingYear = m.ManufacturingYear,
                        Status = m.Status,
                        CurrentLocation = m.CurrentLocation,
                        LastMaintenanceDate = m.LastMaintenanceDate,
                        NextMaintenanceDate = m.NextMaintenanceDate,
                        CreatedAt = m.CreatedAt,
                        UpdatedAt = m.UpdatedAt,
                        ProjectId = m.ProjectId,
                        OperatorId = m.OperatorId,
                        RegionId = m.RegionId,
                        ProjectName = m.Project != null ? m.Project.Name : null,
                        OperatorName = m.Operator != null ? m.Operator.Name : null,
                        RegionName = m.Region != null ? m.Region.Name : null,
                        Specifications = ParseSpecifications(m.SpecificationsJson)
                    })
                    .ToListAsync();

                return Ok(machines);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching machines");
                return StatusCode(500, "Internal server error occurred while fetching machines");
            }
        }

        // GET: api/machines/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MachineDto>> GetMachine(int id)
        {
            try
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

                var machineDto = new MachineDto
                {
                    Id = machine.Id,
                    Name = machine.Name,
                    Type = machine.Type,
                    Model = machine.Model,
                    Manufacturer = machine.Manufacturer,
                    SerialNumber = machine.SerialNumber,
                    RigNo = machine.RigNo,
                    PlateNo = machine.PlateNo,
                    ChassisDetails = machine.ChassisDetails,
                    ManufacturingYear = machine.ManufacturingYear,
                    Status = machine.Status,
                    CurrentLocation = machine.CurrentLocation,
                    LastMaintenanceDate = machine.LastMaintenanceDate,
                    NextMaintenanceDate = machine.NextMaintenanceDate,
                    CreatedAt = machine.CreatedAt,
                    UpdatedAt = machine.UpdatedAt,
                    ProjectId = machine.ProjectId,
                    OperatorId = machine.OperatorId,
                    RegionId = machine.RegionId,
                    ProjectName = machine.Project?.Name,
                    OperatorName = machine.Operator?.Name,
                    RegionName = machine.Region?.Name,
                    Specifications = ParseSpecifications(machine.SpecificationsJson)
                };

                return Ok(machineDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching machine with ID {MachineId}", id);
                return StatusCode(500, "Internal server error occurred while fetching machine");
            }
        }

        // POST: api/machines
        [HttpPost]
        public async Task<ActionResult<MachineDto>> CreateMachine(CreateMachineRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Check if serial number already exists
                var existingMachine = await _context.Machines
                    .FirstOrDefaultAsync(m => m.SerialNumber == request.SerialNumber);
                if (existingMachine != null)
                {
                    return BadRequest($"A machine with serial number '{request.SerialNumber}' already exists");
                }

                // Validate project exists
                var projectExists = await _context.Projects.AnyAsync(p => p.Id == request.ProjectId);
                if (!projectExists)
                {
                    return BadRequest($"Project with ID {request.ProjectId} not found");
                }

                // Validate operator exists if provided
                if (request.OperatorId.HasValue)
                {
                    var operatorExists = await _context.Users.AnyAsync(u => u.Id == request.OperatorId.Value);
                    if (!operatorExists)
                    {
                        return BadRequest($"Operator with ID {request.OperatorId.Value} not found");
                    }
                }

                // Validate region exists if provided
                if (request.RegionId.HasValue)
                {
                    var regionExists = await _context.Regions.AnyAsync(r => r.Id == request.RegionId.Value);
                    if (!regionExists)
                    {
                        return BadRequest($"Region with ID {request.RegionId.Value} not found");
                    }
                }

                var machine = new Machine
                {
                    Name = request.Name,
                    Type = request.Type,
                    Model = request.Model,
                    Manufacturer = request.Manufacturer,
                    SerialNumber = request.SerialNumber,
                    RigNo = request.RigNo,
                    PlateNo = request.PlateNo,
                    ChassisDetails = request.ChassisDetails,
                    ManufacturingYear = request.ManufacturingYear,
                    Status = request.Status,
                    CurrentLocation = request.CurrentLocation,
                    ProjectId = request.ProjectId,
                    OperatorId = request.OperatorId,
                    RegionId = request.RegionId,
                    SpecificationsJson = request.Specifications != null ? 
                        JsonSerializer.Serialize(request.Specifications) : null
                };

                // Note: Project and operator assignments are handled via foreign keys

                _context.Machines.Add(machine);
                await _context.SaveChangesAsync();

                // Load the machine with related data for response
                var createdMachine = await _context.Machines
                    .Include(m => m.Project)
                    .Include(m => m.Operator)
                    .Include(m => m.Region)
                    .FirstOrDefaultAsync(m => m.Id == machine.Id);

                var machineDto = new MachineDto
                {
                    Id = createdMachine!.Id,
                    Name = createdMachine.Name,
                    Type = createdMachine.Type,
                    Model = createdMachine.Model,
                    Manufacturer = createdMachine.Manufacturer,
                    SerialNumber = createdMachine.SerialNumber,
                    RigNo = createdMachine.RigNo,
                    PlateNo = createdMachine.PlateNo,
                    ChassisDetails = createdMachine.ChassisDetails,
                    ManufacturingYear = createdMachine.ManufacturingYear,
                    Status = createdMachine.Status,
                    CurrentLocation = createdMachine.CurrentLocation,
                    LastMaintenanceDate = createdMachine.LastMaintenanceDate,
                    NextMaintenanceDate = createdMachine.NextMaintenanceDate,
                    CreatedAt = createdMachine.CreatedAt,
                    UpdatedAt = createdMachine.UpdatedAt,
                    ProjectId = createdMachine.ProjectId,
                    OperatorId = createdMachine.OperatorId,
                    RegionId = createdMachine.RegionId,
                    ProjectName = createdMachine.Project?.Name,
                    OperatorName = createdMachine.Operator?.Name,
                    RegionName = createdMachine.Region?.Name,
                    Specifications = ParseSpecifications(createdMachine.SpecificationsJson)
                };

                return CreatedAtAction(nameof(GetMachine), new { id = machine.Id }, machineDto);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error occurred while creating machine");
                return StatusCode(500, "Database error occurred while creating machine");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating machine");
                return StatusCode(500, "Internal server error occurred while creating machine");
            }
        }

        // PUT: api/machines/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMachine(int id, UpdateMachineRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var machine = await _context.Machines.FindAsync(id);
                if (machine == null)
                {
                    return NotFound($"Machine with ID {id} not found");
                }

                // Check if serial number already exists for a different machine
                var existingMachine = await _context.Machines
                    .FirstOrDefaultAsync(m => m.SerialNumber == request.SerialNumber && m.Id != id);
                if (existingMachine != null)
                {
                    return BadRequest($"A machine with serial number '{request.SerialNumber}' already exists");
                }

                // Validate project exists
                var projectExists = await _context.Projects.AnyAsync(p => p.Id == request.ProjectId);
                if (!projectExists)
                {
                    return BadRequest($"Project with ID {request.ProjectId} not found");
                }

                // Validate operator exists if provided
                if (request.OperatorId.HasValue)
                {
                    var operatorExists = await _context.Users.AnyAsync(u => u.Id == request.OperatorId.Value);
                    if (!operatorExists)
                    {
                        return BadRequest($"Operator with ID {request.OperatorId.Value} not found");
                    }
                }

                // Validate region exists if provided
                if (request.RegionId.HasValue)
                {
                    var regionExists = await _context.Regions.AnyAsync(r => r.Id == request.RegionId.Value);
                    if (!regionExists)
                    {
                        return BadRequest($"Region with ID {request.RegionId.Value} not found");
                    }
                }

                // Update machine properties
                machine.Name = request.Name;
                machine.Type = request.Type;
                machine.Model = request.Model;
                machine.Manufacturer = request.Manufacturer;
                machine.SerialNumber = request.SerialNumber;
                machine.RigNo = request.RigNo;
                machine.PlateNo = request.PlateNo;
                machine.ChassisDetails = request.ChassisDetails;
                machine.ManufacturingYear = request.ManufacturingYear;
                machine.Status = request.Status;
                machine.CurrentLocation = request.CurrentLocation;
                machine.ProjectId = request.ProjectId;
                machine.OperatorId = request.OperatorId;
                machine.RegionId = request.RegionId;
                machine.LastMaintenanceDate = request.LastMaintenanceDate;
                machine.NextMaintenanceDate = request.NextMaintenanceDate;
                machine.UpdatedAt = DateTime.UtcNow;
                machine.SpecificationsJson = request.Specifications != null ? 
                    JsonSerializer.Serialize(request.Specifications) : null;

                // Note: Project and operator assignments are now handled via foreign keys and navigation properties

                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!MachineExists(id))
                {
                    return NotFound($"Machine with ID {id} not found");
                }
                _logger.LogError(ex, "Concurrency error occurred while updating machine with ID {MachineId}", id);
                return StatusCode(500, "Concurrency error occurred while updating machine");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error occurred while updating machine with ID {MachineId}", id);
                return StatusCode(500, "Database error occurred while updating machine");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating machine with ID {MachineId}", id);
                return StatusCode(500, "Internal server error occurred while updating machine");
            }
        }

        // DELETE: api/machines/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMachine(int id)
        {
            try
            {
                var machine = await _context.Machines.FindAsync(id);
                if (machine == null)
                {
                    return NotFound($"Machine with ID {id} not found");
                }

                _context.Machines.Remove(machine);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error occurred while deleting machine with ID {MachineId}", id);
                return StatusCode(500, "Database error occurred while deleting machine");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting machine with ID {MachineId}", id);
                return StatusCode(500, "Internal server error occurred while deleting machine");
            }
        }

        // PATCH: api/machines/5/status
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateMachineStatus(int id, [FromBody] UpdateMachineStatusRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var machine = await _context.Machines.FindAsync(id);
                if (machine == null)
                {
                    return NotFound($"Machine with ID {id} not found");
                }

                machine.Status = request.Status;
                machine.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating machine status for ID {MachineId}", id);
                return StatusCode(500, "Internal server error occurred while updating machine status");
            }
        }

        // GET: api/machines/search
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<MachineDto>>> SearchMachines(
            [FromQuery] string? name = null,
            [FromQuery] string? type = null,
            [FromQuery] string? status = null,
            [FromQuery] string? manufacturer = null,
            [FromQuery] string? serialNumber = null)
        {
            try
            {
                var query = _context.Machines
                    .Include(m => m.Project)
                    .Include(m => m.Operator)
                    .AsQueryable();

                if (!string.IsNullOrEmpty(name))
                {
                    query = query.Where(m => m.Name.Contains(name));
                }

                if (!string.IsNullOrEmpty(type))
                {
                    query = query.Where(m => m.Type == type);
                }

                if (!string.IsNullOrEmpty(status))
                {
                    query = query.Where(m => m.Status == status);
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
                    .Select(m => new MachineDto
                    {
                        Id = m.Id,
                        Name = m.Name,
                        Type = m.Type,
                        Model = m.Model,
                        Manufacturer = m.Manufacturer,
                        SerialNumber = m.SerialNumber,
                        RigNo = m.RigNo,
                        PlateNo = m.PlateNo,
                        ChassisDetails = m.ChassisDetails,
                        ManufacturingYear = m.ManufacturingYear,
                        Status = m.Status,
                        CurrentLocation = m.CurrentLocation,
                        LastMaintenanceDate = m.LastMaintenanceDate,
                        NextMaintenanceDate = m.NextMaintenanceDate,
                        CreatedAt = m.CreatedAt,
                        UpdatedAt = m.UpdatedAt,
                        ProjectId = m.ProjectId,
                        OperatorId = m.OperatorId,
                        ProjectName = m.Project != null ? m.Project.Name : null,
                        OperatorName = m.Operator != null ? m.Operator.Name : null,
                        Specifications = ParseSpecifications(m.SpecificationsJson)
                    })
                    .ToListAsync();

                return Ok(machines);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while searching machines");
                return StatusCode(500, "Internal server error occurred while searching machines");
            }
        }

        // GET: api/machines/statistics
        [HttpGet("statistics")]
        public async Task<ActionResult<object>> GetMachineStatistics()
        {
            try
            {
                var totalMachines = await _context.Machines.CountAsync();
                var availableMachines = await _context.Machines.CountAsync(m => m.Status == "Available");
                var assignedMachines = await _context.Machines.CountAsync(m => m.Status == "Assigned");
                var maintenanceMachines = await _context.Machines.CountAsync(m => m.Status == "In Maintenance");
                var outOfServiceMachines = await _context.Machines.CountAsync(m => 
                    m.Status == "Out of Service" || m.Status == "Under Repair");

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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching machine statistics");
                return StatusCode(500, "Internal server error occurred while fetching machine statistics");
            }
        }

        private bool MachineExists(int id)
        {
            return _context.Machines.Any(e => e.Id == id);
        }

        private static MachineSpecificationsDto? ParseSpecifications(string? specificationsJson)
        {
            if (string.IsNullOrEmpty(specificationsJson))
                return null;

            try
            {
                return JsonSerializer.Deserialize<MachineSpecificationsDto>(specificationsJson);
            }
            catch
            {
                return null;
            }
        }
    }

    public class UpdateMachineStatusRequest
    {
        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = string.Empty;
    }
} 
