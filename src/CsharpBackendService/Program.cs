// <copyright file="Program.cs" company="CsharpBackendService">
// Copyright (c) CsharpBackendService. All rights reserved.
// </copyright>

namespace CsharpBackendService;

using CsharpBackendService.Services;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Main entry point for the application.
/// </summary>
public class Program
{
    /// <summary>
    /// Entry point for the application.
    /// </summary>
    /// <param name="args">Command line arguments.</param>
    public static void Main(string[] args)
    {
        var projectDir = Directory.GetCurrentDirectory();
        var contentRoot = projectDir;

        if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development" && projectDir.Contains("tests"))
        {
            // Walk up to the repo root no matter how deep /bin/* is,
            // then point at the real web project.
            var repoRoot = projectDir;
            while (repoRoot is not null && Path.GetFileName(repoRoot) != "tests")
            {
                repoRoot = Path.GetDirectoryName(repoRoot)!;
            }

            repoRoot = Path.GetDirectoryName(repoRoot!)!;

            contentRoot = Path.Combine(repoRoot, "src", "CsharpBackendService");

            // Fallback safety – if we mis‑calculated, stay with the original dir
            if (!Directory.Exists(contentRoot))
            {
                contentRoot = projectDir;
            }
        }

        var options = new WebApplicationOptions
        {
            ContentRootPath = contentRoot,
            Args = args,
        };

        var builder = WebApplication.CreateBuilder(options);

        // Add services to the container
        builder.Services.AddControllers();

        // Register the ITodoStore service
        builder.Services.AddSingleton<ITodoStore, TodoStore>();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            // Include XML comments for Swagger documentation
            var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            options.IncludeXmlComments(xmlPath);
        });

        // Add Windows Service support when not running interactively
        if (!Environment.UserInteractive)
        {
            builder.Services.AddWindowsService();
        }

        // Add CORS policy
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
                policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
        });

        // Add health checks
        builder.Services.AddHealthChecks();

        // Configure logging
        builder.Logging.AddConsole();

        var app = builder.Build();

        // Configure the HTTP request pipeline
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseCors("AllowAll");
        app.UseAuthorization();

        // Add global error handler
        app.UseExceptionHandler("/error");
        app.Map("/error", (HttpContext http) =>
            Results.Problem(new ProblemDetails { Title = "An error occurred" }));

        // Map health endpoint
        app.MapHealthChecks("/healthz");

        app.MapControllers();
        app.Run();
    }
}
