using MobileCard.API;
using MobileCard.API.Models;
using NLog.Extensions.Logging;
using NLog.Web;
using NSwag;
using NSwag.Generation.Processors.Security;
using System.Reflection;

Core.Init(args);
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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


builder.Logging.AddNLog();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    const string docsRoute = "/api/docs";
    const string ProductName = Core.PRODUCT_NAME;

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
        config.DocumentPath = docsRoute + $"/swagger/{ProductName}/swagger.json";
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();