using Api;
using Application;
using Infrastructure;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Projects DI
builder.Services.AddApi(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddInfrastructure();

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

app.MapGet("/", () => Results.Redirect("/api-docs")).ExcludeFromDescription();
app.MapGet("/swagger", () => Results.Redirect("/api-docs")).ExcludeFromDescription();

app.Run();