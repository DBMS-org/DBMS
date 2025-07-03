using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using Domain.Entities.MachineManagement;
using Domain.Entities.ProjectManagement;
using Domain.Entities.UserManagement;
using Application.DTOs.MachineManagement;
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
        public async Task<ActionResult<IEnumerable<Machine>>> GetMachines()
        {
            try
            {
                var machines = await _context.Machines
                    .Include(m => m.Project)
                    .Include(m => m.Operator)
                    .Include(m => m.Region)
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
        public async Task<ActionResult<Machine?>> GetMachine(int id)
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

                return Ok(machine);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching machine with ID {MachineId}", id);
                return StatusCode(500, "Internal server error occurred while fetching machine");
            }
        }

        // POST: api/machines
        [HttpPost]
        public async Task<ActionResult<Machine>> CreateMachine(CreateMachineRequest request)
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

                // Parse status string to enum (defaults to Active)
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

                // optional fields
                machine.RigNo = request.RigNo;
                machine.PlateNo = request.PlateNo;
                machine.ChassisDetails = request.ChassisDetails;
                machine.ManufacturingYear = request.ManufacturingYear;
                machine.ChangeStatus(statusEnum);
                machine.CurrentLocation = request.CurrentLocation;
                machine.OperatorId = request.OperatorId;
                machine.RegionId = request.RegionId;
                machine.SpecificationsJson = request.Specifications != null ? JsonSerializer.Serialize(request.Specifications) : null;

                // Set assigned project and operator names if applicable
                var project = await _context.Projects.FindAsync(request.ProjectId);
                machine.AssignedToProject = project?.Name;

                if (request.OperatorId.HasValue)
                {
                    var operatorUser = await _context.Users.FindAsync(request.OperatorId.Value);
                    machine.AssignedToOperator = operatorUser?.Name;
                }

                _context.Machines.Add(machine);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetMachine), new { id = machine.Id }, machine);
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
                machine.MarkUpdated();
                machine.SpecificationsJson = request.Specifications != null ? 
                    JsonSerializer.Serialize(request.Specifications) : null;

                // Update assigned project and operator names
                var project = await _context.Projects.FindAsync(request.ProjectId);
                machine.AssignedToProject = project?.Name;

                if (request.OperatorId.HasValue)
                {
                    var operatorUser = await _context.Users.FindAsync(request.OperatorId.Value);
                    machine.AssignedToOperator = operatorUser?.Name;
                }
                else
                {
                    machine.AssignedToOperator = null;
                }

                // Update status
                if (request.Status != null)
                {
                    if (!Enum.TryParse<MachineStatus>(request.Status, true, out var newStatus))
                    {
                        return BadRequest($"Invalid status '{request.Status}'.");
                    }
                    machine.ChangeStatus(newStatus);
                }

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

                // Parse status string to enum (defaults to Active)
                if (!Enum.TryParse<MachineStatus>(request.Status, true, out var statusEnum))
                {
                    return BadRequest($"Invalid status '{request.Status}'. Valid values: {string.Join(", ", Enum.GetNames<MachineStatus>())}");
                }

                machine.ChangeStatus(statusEnum);
                machine.MarkUpdated();

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
        public async Task<ActionResult<IEnumerable<Machine>>> SearchMachines(
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
                    if (!Enum.TryParse<MachineStatus>(status, true, out var statusEnum))
                    {
                        return BadRequest($"Invalid status '{status}'. Valid values: {string.Join(", ", Enum.GetNames<MachineStatus>())}");
                    }
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
                var availableMachines = await _context.Machines.CountAsync(m => m.Status == MachineStatus.Active);
                var assignedMachines = await _context.Machines.CountAsync(m => m.Status == MachineStatus.Active && m.OperatorId != null);
                var maintenanceMachines = await _context.Machines.CountAsync(m => m.Status == MachineStatus.Maintenance);
                var outOfServiceMachines = await _context.Machines.CountAsync(m => m.Status == MachineStatus.OutOfService || m.Status == MachineStatus.Retired);

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
    }

    public class UpdateMachineStatusRequest
    {
        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = string.Empty;
    }
} 