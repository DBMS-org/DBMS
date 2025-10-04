using Application.Interfaces.Infrastructure;
using Application.Interfaces.Infrastructure.Repositories;
using Application.Interfaces.UserManagement;
using Application.Interfaces.ProjectManagement;
using Application.Interfaces.DrillingOperations;
using Application.Interfaces.BlastingOperations;
using Application.Interfaces.StoreManagement;
using Application.Services.UserManagement;
using Application.Services.ProjectManagement;
using Application.Services.DrillingOperations;
using Application.Services.BlastingOperations;
using Application.Services.StoreManagement;
using Domain.Services;
using Infrastructure.Repositories.UserManagement;
using Infrastructure.Repositories.ProjectManagement;
using Infrastructure.Repositories.DrillingOperations;
using Infrastructure.Repositories.BlastingOperations;
using Infrastructure.Repositories.StoreManagement;
using Infrastructure.Data;
using Infrastructure.Services;
using Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json;
using FluentValidation;
using FluentValidation.AspNetCore;
using Application.Validators.UserManagement;
using Application.Validators.ProjectManagement;
using Application.Validators.MachineManagement;
using Application.Validators.DrillingOperations;
using API.Middleware;
using Microsoft.AspNetCore.Authorization;
using API.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Configure JSON serialization to handle camelCase from frontend
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        options.JsonSerializerOptions.WriteIndented = true;
        
        // Ignore circular references (Project -> Sites -> Project)
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        
        // Ensure DateTime is serialized as UTC with proper timezone information
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<CreateUserRequestValidator>();
builder.Services.AddFluentValidationAutoValidation();

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(Application.Mapping.UserManagementMappingProfile).Assembly);
builder.Services.AddScoped<IMappingService, Application.Services.Infrastructure.AutoMapperService>();

// Add Infrastructure layer services (DbContext, dispatcher, handlers, misc)
builder.Services.AddInfrastructure();

// Core shared services
builder.Services.AddMemoryCache();
builder.Services.AddScoped<ICacheService, Application.Services.Infrastructure.CacheService>();
builder.Services.AddScoped<IPerformanceMonitor, Application.Services.Infrastructure.PerformanceMonitor>();
builder.Services.AddScoped<IValidationService, Application.Services.Infrastructure.ValidationService>();
builder.Services.AddScoped(typeof(IStructuredLogger<>), typeof(Application.Services.Infrastructure.StructuredLogger<>));

// Add Entity Framework
builder.Services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy.WithOrigins("http://localhost:4200", "http://localhost:4201") // Angular ports
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// Register Authentication services (Infrastructure implementations)
builder.Services.AddScoped<IPasswordService, PasswordService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IEmailService, EmailService>();

// Add JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"] ?? "your-very-long-secret-key-here-make-it-at-least-32-characters";

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"] ?? "DBMS-API",
            ValidAudience = jwtSettings["Audience"] ?? "DBMS-UI",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
        };
    });

builder.Services.AddScoped<IAuthorizationHandler, OwnershipAuthorizationHandler>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin", "Administrator"));
    options.AddPolicy("RequireUserRole", policy => policy.RequireRole("User", "StandardUser"));
    options.AddPolicy("RequireOwnership", policy => policy.Requirements.Add(new OwnershipRequirement()));
    options.AddPolicy("ReadDrillData", policy => policy.RequireRole("Admin", "Administrator", "BlastingEngineer", "Operator"));
    options.AddPolicy("ManageDrillData", policy => policy.RequireRole("BlastingEngineer"));
    options.AddPolicy("ManageProjectSites", policy => policy.RequireRole("BlastingEngineer"));
    options.AddPolicy("ManageMachines", policy => policy.RequireRole("Admin", "Administrator", "MachineManager"));
    options.AddPolicy("ReadProjectData", policy => policy.RequireRole("Admin", "Administrator", "BlastingEngineer", "Operator", "MachineManager"));
    options.AddPolicy("ManageExplosiveRequests", policy => policy.RequireRole("Admin", "Administrator", "StoreManager", "BlastingEngineer"));
    });

// Register DrillHole services (split into focused services)
builder.Services.AddScoped<IDrillHoleRepository, DrillHoleRepository>();
builder.Services.AddScoped<IDrillHoleValidationService, DrillHoleValidationService>();
builder.Services.AddScoped<ICsvImportService, CsvImportApplicationService>();
builder.Services.AddScoped<IDrillHoleService, DrillHoleApplicationService>();

// Register Drill Point Pattern services
builder.Services.AddScoped<IDrillPointRepository, DrillPointRepository>();
builder.Services.AddScoped<IDrillPointPatternService, DrillPointPatternApplicationService>();
builder.Services.AddScoped<DrillPointDomainService>();

// Register Explosive Calculation services
builder.Services.AddScoped<IExplosiveCalculationResultRepository, ExplosiveCalculationResultRepository>();
builder.Services.AddScoped<IExplosiveCalculationResultService, ExplosiveCalculationResultApplicationService>();

// Register Site Blasting services
builder.Services.AddScoped<ISiteBlastingRepository, SiteBlastingRepository>();

builder.Services.AddScoped<ISiteBlastingService, SiteBlastingApplicationService>();

// Register Blast Connection services
builder.Services.AddScoped<IBlastConnectionRepository, BlastConnectionRepository>();
builder.Services.AddScoped<IBlastConnectionService, BlastConnectionApplicationService>();

// Register Region services
builder.Services.AddScoped<IRegionRepository, RegionRepository>();
builder.Services.AddScoped<IRegionService, RegionApplicationService>();

// Register Project services
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<IProjectService, ProjectApplicationService>();
builder.Services.AddScoped<IProjectSiteRepository, ProjectSiteRepository>();
builder.Services.AddScoped<IProjectSiteService, ProjectSiteApplicationService>();

// Register Explosive Approval Request services
builder.Services.AddScoped<IExplosiveApprovalRequestRepository, ExplosiveApprovalRequestRepository>();
builder.Services.AddScoped<IExplosiveApprovalRequestService, ExplosiveApprovalRequestApplicationService>();

// Register User/Auth services
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserApplicationService>();
builder.Services.AddScoped<IAuthService, AuthApplicationService>();

// Register Store Management services
builder.Services.AddScoped<Application.Interfaces.StoreManagement.IStoreRepository, Infrastructure.Repositories.StoreManagement.StoreRepository>();
builder.Services.AddScoped<Application.Interfaces.StoreManagement.IStoreService, Application.Services.StoreManagement.StoreApplicationService>();
builder.Services.AddScoped<Application.Interfaces.StoreManagement.IStoreInventoryRepository, Infrastructure.Repositories.StoreManagement.StoreInventoryRepository>();
builder.Services.AddScoped<Application.Interfaces.StoreManagement.IStoreInventoryService, Application.Services.StoreManagement.StoreInventoryApplicationService>();
builder.Services.AddScoped<Application.Interfaces.StoreManagement.IStoreTransactionRepository, Infrastructure.Repositories.StoreManagement.StoreTransactionRepository>();
builder.Services.AddScoped<Application.Interfaces.StoreManagement.IStoreTransactionService, Application.Services.StoreManagement.StoreTransactionApplicationService>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUserContext, Infrastructure.Services.UserContext>();

var app = builder.Build();

// Create database if it doesn't exist and apply migrations
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    try
    {
        context.Database.EnsureCreated();
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while creating the database.");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Temporarily disable HTTPS redirection for testing
// app.UseHttpsRedirection();

// Use CORS
app.UseCors("AllowAngularApp");

// Use the global exception handler
app.UseMiddleware<GlobalExceptionMiddleware>();

// Add Authentication and Authorization middleware
app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<API.Middleware.AuditPerformanceMiddleware>();

app.MapControllers();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
