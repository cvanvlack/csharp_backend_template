// <copyright file="CustomWebApplicationFactory.cs" company="CsharpBackendService">
// Copyright (c) CsharpBackendService. All rights reserved.
// </copyright>

namespace CsharpBackendService.Tests;

using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;

/// <summary>
/// Custom WebApplicationFactory for integration tests.
/// </summary>
/// <typeparam name="TProgram">The entry point class of the application.</typeparam>
public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram>
    where TProgram : class
{
    /// <summary>
    /// Creates a new instance of the web application factory with custom configuration.
    /// </summary>
    protected override IHost CreateHost(IHostBuilder builder)
    {
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");
        return base.CreateHost(builder);
    }

    /// <summary>
    /// Configures the web host for testing.
    /// </summary>
    /// <param name="builder">The IWebHostBuilder to configure.</param>
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        builder.UseEnvironment("Development");

        var contentRoot = Path.GetFullPath(
    Path.Combine(AppContext.BaseDirectory,
        "..", "..", "..", "..", "..",       // back to repo root
        "src", "CsharpBackendService"));

        builder.UseContentRoot(contentRoot);

        builder.ConfigureAppConfiguration((context, config) =>
        {
            // Add any test-specific configuration here
        });

        builder.ConfigureServices(services =>
        {
            // Add any test-specific services here
        });
    }
}
