using System.Text;
using GridSyncInterface.Data;
using GridSyncInterface.Parser;
using GridSyncInterface.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// ?? Controllers ?????????????????????????????????????????????????????????????
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// ?? Swagger with JWT support ?????????????????????????????????????????????????
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "GridSync Interface", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name         = "Authorization",
        Type         = SecuritySchemeType.ApiKey,
        Scheme       = "Bearer",
        BearerFormat = "JWT",
        In           = ParameterLocation.Header,
        Description  = "Enter: Bearer {your token}"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});

// ?? EF Core ??????????????????????????????????????????????????????????????????
builder.Services.AddDbContext<SclDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SclDb")));

// ?? JWT Authentication ???????????????????????????????????????????????????????
var jwtSecret = builder.Configuration["Jwt:Secret"]
    ?? throw new InvalidOperationException("Jwt:Secret not configured.");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer           = true,
            ValidateAudience         = true,
            ValidateLifetime         = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer              = builder.Configuration["Jwt:Issuer"],
            ValidAudience            = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey         = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
            ClockSkew                = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

// ?? CORS (allow any origin in dev; tighten for production) ???????????????????
builder.Services.AddCors(o => o.AddDefaultPolicy(p =>
    p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

// ?? Application Services ?????????????????????????????????????????????????????
builder.Services.AddScoped<IAuthService,               AuthService>();
builder.Services.AddScoped<IProjectService,            ProjectService>();
builder.Services.AddScoped<IAuditService,              AuditService>();
builder.Services.AddScoped<ILockService,               LockService>();
builder.Services.AddScoped<IConflictResolutionService, ConflictResolutionService>();
builder.Services.AddScoped<ISclElementService,         SclElementService>();
builder.Services.AddScoped<SclParser>();

// ?? Build ????????????????????????????????????????????????????????????????????
var app = builder.Build();

// ?? Auto-migrate database on startup + seed default admin ??????????????????
using (var scope = app.Services.CreateScope())
{
    var db   = scope.ServiceProvider.GetRequiredService<SclDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

    db.Database.Migrate();

    // Seed a default Admin user if the Users table is empty
    if (!db.Users.Any())
    {
        db.Users.Add(new GridSyncInterface.Models.Auth.AppUser
        {
            Username     = "admin",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
            Role         = GridSyncInterface.Models.Auth.UserRoles.Admin,
            FullName     = "System Administrator",
            Email        = "admin@gridsync.local",
            IsActive     = true,
            CreatedAt    = DateTime.UtcNow
        });
        db.SaveChanges();
        logger.LogInformation("Default admin user seeded  (username: admin / password: Admin@123)");
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
