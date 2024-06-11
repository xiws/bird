// See https://aka.ms/new-console-template for more information
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BirdServices;
using DotnetSpider;
using DotnetSpider.MySql.Scheduler;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = Builder.CreateDefaultBuilder<PokemonHomeSpider>(x =>
{
    x.Speed = 2;
});

builder.ConfigureAppConfiguration(v=>v.AddJsonFile("appsettings.json"));

builder.UseMySqlQueueBfsScheduler((context, options) =>
{
    options.ConnectionString = context.Configuration["SchedulerConnectionString"];
});
builder.ConfigureLogging(vl=>vl.AddConsole());
builder.IgnoreServerCertificateError();
await builder.Build().RunAsync();