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
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> CreateMachine(CreateMachineRequest request)
        {
            var existingMachine = await _context.Machines
                .FirstOrDefaultAsync(m => m.SerialNumber == request.SerialNumber);
            if (existingMachine != null)
            {
                return Conflict($"A machine with serial number '{request.SerialNumber}' already exists");
            }

            var projectExists = await _context.Projects.AnyAsync(p => p.Id == request.ProjectId);
            if (!projectExists)
            {
                return BadRequest($"Project with ID {request.ProjectId} not found");
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
            
            var project = await _context.Projects.FindAsync(request.ProjectId);
            machine.AssignedToProject = project?.Name;

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
        [Authorize(Policy = "RequireAdminRole")]
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
            
            var projectExists = await _context.Projects.AnyAsync(p => p.Id == request.ProjectId);
            if (!projectExists)
            {
                return BadRequest($"Project with ID {request.ProjectId} not found");
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
        [Authorize(Policy = "RequireAdminRole")]
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
        [Authorize(Policy = "RequireAdminRole")]
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
            var machinesByStatus = await _context.Machines
                .GroupBy(m => m.Status)
                .Select(g => new { Status = g.Key.ToString(), Count = g.Count() })
                .ToListAsync();
            var machinesByType = await _context.Machines
                .GroupBy(m => m.Type)
                .Select(g => new { Type = g.Key, Count = g.Count() })
                .ToListAsync();

            var stats = new
            {
                TotalMachines = totalMachines,
                MachinesByStatus = machinesByStatus,
                MachinesByType = machinesByType
            };

            return Ok(stats);
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