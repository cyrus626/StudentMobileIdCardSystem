using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MobileCard.API;
using MobileCard.API.Models;
using MobileCard.API.Models.Entities;
using MobileCard.API.Models.Options;
using MobileCard.API.Services;
using NLog.Extensions.Logging;
using NLog.Web;
using NSwag;
using NSwag.Generation.Processors.Security;
using Sieve.Services;
using System.Reflection;

Core.Init(args);
var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

// Add services to the container.
services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAuthorization().AddResponseCompression();

builder.Services.AddRouting(opt => { opt.LowercaseUrls = true; });

builder.Services.AddOpenApiDocument(doc =>
{
    const string ProductName = Core.PRODUCT_NAME;
    const string ProductVersion = Core.PRODUCT_VERSION;

    doc.Title = ProductName;
    doc.DocumentName = ProductVersion;
    doc.Description = $"API Documentation for {ProductName}";
    doc.Version = ProductVersion;

    doc.AddSecurity("JWT", Enumerable.Empty<string>(), new OpenApiSecurityScheme
    {
        Type = OpenApiSecuritySchemeType.ApiKey,
        Name = "Authorization",
        In = OpenApiSecurityApiKeyLocation.Header,
        Description = "Type into the textbox: Bearer {your JWT token}."
    });

    doc.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("JWT"));
});

builder.Services.AddAutoMapper(config =>
{
    config.AddProfile<ViewModelToEntityProfile>();
    config.AddProfile<EntityToViewModelProfile>();

}, Assembly.GetCallingAssembly());

services.Configure<JwtIssuerOptions>(opt =>
{
    var auth = AuthSettings.Instance;
    JwtIssuerOptions options = new JwtIssuerOptions();

    options.SigningCredentials = new SigningCredentials(auth.SymmetricKey, SecurityAlgorithms.HmacSha512Signature);

    opt.Issuer = "MobileCard";
    opt.SigningCredentials = options.SigningCredentials;
    opt.Subject = options.Subject;
    opt.Audience = options.Audience;
});


builder.Logging.AddNLog();
services.AddTransient<IJwtFactory, JwtFactory>();
services.AddIdentityCore<ApplicationUser>(opt =>
{
    opt.Password.RequireDigit = false;
    opt.Password.RequireLowercase = false;
    opt.Password.RequireUppercase = false;
    opt.Password.RequireNonAlphanumeric = false;
    opt.Password.RequiredLength = 6;

    opt.User.RequireUniqueEmail = true;
    opt.Lockout.AllowedForNewUsers = false;
}).AddEntityFrameworkStores<ApplicationContext>()
.AddDefaultTokenProviders();

services.AddDataProtection();

services.AddDbContext<ApplicationContext>(opt =>
{
    string connectionString = new SqliteConnectionStringBuilder()
    {
        Mode = SqliteOpenMode.ReadWriteCreate,
        DataSource = Core.DATABASE_PATH,
    }.ConnectionString;

    opt.UseSqlite(connectionString);
});

services.AddScoped<ISieveProcessor, SieveProcessor>();

services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    // options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(configureOptions =>
{
    // TODO: This is for demonstrations purposes only. Such a key should not be present in code

    const string Issuer = "MobileCard";
    var auth = AuthSettings.Instance;

    configureOptions.ClaimsIssuer = Issuer;
    configureOptions.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidIssuer = Issuer,

        ValidateAudience = false,

        ValidateIssuerSigningKey = true,
        IssuerSigningKey = auth.SymmetricKey,

        RequireExpirationTime = true,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
    };
    configureOptions.SaveToken = true;
    configureOptions.RequireHttpsMetadata = false;
});

services.AddAuthorization();





var app = builder.Build();

app.UseCors(x => 
	x.AllowAnyMethod()
	.AllowAnyHeader()
	// .AllowCredentials()
	.WithOrigins("*"));

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    const string docsRoute = "/api/docs";
    const string ProductName = Core.PRODUCT_NAME;
    const string ProductVersion = Core.PRODUCT_VERSION;

    app.UseOpenApi(config =>
    {
        config.Path = docsRoute + "/swagger/{documentName}/swagger.json";
    });

    app.UseSwaggerUi3(config =>
    {
        config.Path = $"{docsRoute}/swagger";
        config.DocumentPath = docsRoute + "/swagger/{documentName}/swagger.json";
    });

    app.UseReDoc(config =>
    {
        config.Path = $"{docsRoute}/redoc";
        config.DocumentPath = docsRoute + $"/swagger/{ProductVersion}/swagger.json";
    });
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();

Core.Finalize(app.Services);
app.Run();