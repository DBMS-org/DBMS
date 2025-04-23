using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Collections.Generic;
using System;
using System.Linq;
using Microsoft.Extensions.Logging;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class DashboardController : ControllerBase
    {
        private readonly ILogger<DashboardController> _logger;

        public DashboardController(ILogger<DashboardController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<object> GetDashboard()
        {
            try
            {
                var userRoles = User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();
                
                // Based on the user's role, return the appropriate dashboard data
                if (userRoles.Contains("Admin"))
                {
                    return Ok(new
                    {
                        dashboard = "admin",
                        modules = new[] { "Users", "Roles", "Permissions", "SystemSettings", "AuditLogs", "AllProjects" }
                    });
                }
                else if (userRoles.Contains("ProjectManager"))
                {
                    return Ok(new
                    {
                        dashboard = "project_manager",
                        modules = new[] { "MyProjects", "BlastingSchedule", "ResourceManagement", "Reports" }
                    });
                }
                else if (userRoles.Contains("BlastingEngineer"))
                {
                    return Ok(new
                    {
                        dashboard = "blasting_engineer",
                        modules = new[] { "BlastPlanning", "BlastResults", "SiteInspection" }
                    });
                }
                else if (userRoles.Contains("Surveyor"))
                {
                    return Ok(new
                    {
                        dashboard = "surveyor",
                        modules = new[] { "SiteSurveys", "MapView", "MeasurementTools" }
                    });
                }
                else if (userRoles.Contains("Foreman"))
                {
                    return Ok(new
                    {
                        dashboard = "foreman",
                        modules = new[] { "CrewManagement", "DailyTasks", "SafetyChecks" }
                    });
                }    
                else if (userRoles.Contains("MaintenanceStaff"))
                {
                    return Ok(new
                    {
                        dashboard = "maintenance",
                        modules = new[] { "EquipmentStatus", "MaintenanceTasks", "InventoryManagement" }
                    });
                }
                
                // Default dashboard for users without specific roles
                return Ok(new
                {
                    dashboard = "default",
                    modules = new[] { "Profile", "Notifications" }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving dashboard: {ex.Message}");
                return StatusCode(500, new { message = "Error retrieving dashboard information" });
            }
        }
    }