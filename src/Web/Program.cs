using Application;
using Infrastructure;
using IronForge.Contracts.AuthService;
using Scalar.AspNetCore;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using Swashbuckle.AspNetCore.Filters;
using Web.Infrastructure;
using LoginRequest = Application.Features.Authentication.Login.LoginRequest;

var builder = WebApplication.CreateBuilder(args);

// Projects DI
builder.Services.AddApplication();
builder.Services.AddInfrastructure();

// Auth
builder.Services.AddAuthentication()
    .AddJwtBearer();
builder.Services.AddAuthorization();

// Services
builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// Microservices
builder.Services.AddGrpcClient<Auth.AuthClient>(options =>
{
    options.Address = new Uri("http://localhost:5102");
});

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SupportNonNullableReferenceTypes();
    options.EnableAnnotations();
    options.ExampleFilters();
});
builder.Services.AddSwaggerExamplesFromAssemblyOf<LoginRequest>();

// Application
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger(options => options.RouteTemplate = "/openapi/{documentName}.json");
    app.MapScalarApiReference("/api-docs");
}

app.UseStatusCodePages();

// Middlewares
app.UseExceptionHandler();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();