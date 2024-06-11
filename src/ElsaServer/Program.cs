using Elsa.EntityFrameworkCore.Extensions;
using Elsa.EntityFrameworkCore.Modules.Management;
using Elsa.EntityFrameworkCore.Modules.Runtime;
using Elsa.Extensions;
using ElsaServer.Activies;
using ElsaServer.Model;
using ElsaServer.Workflows;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Protocol;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseStaticWebAssets();

var services = builder.Services;
var configuration = builder.Configuration;
var connection = builder.Configuration.GetSection("DataConnection").Get<DataConnection>();

services
    .AddElsa(elsa => {
        elsa
            .UseIdentity(identity =>
            {
                identity.TokenOptions = options => options.SigningKey = "large-signing-key-for-signing-JWT-tokens";
                identity.UseAdminUserProvider();
            }).AddActivity<PrintMessageV2>()
            .AddActivity<PrintMessageCode>()
            .AddActivity<IfTest>()
            .UseDefaultAuthentication()
            .UseWorkflowManagement(management => management.UseEntityFrameworkCore(val => val.UsePostgreSql(connection.Pgsql)))
            .UseWorkflowRuntime(runtime => runtime.UseEntityFrameworkCore(val=>val.UsePostgreSql(connection.Pgsql)))
            .UseScheduling()
            .UseJavaScript()
            .UseLiquid()
            .UseCSharp()
            .UseHttp(http => http.ConfigureHttpOptions = options => configuration.GetSection("Http").Bind(options))
            .UseWorkflowsApi()
            .UseRealTimeWorkflows()
            .AddActivitiesFrom<Program>()
            
            .AddWorkflowsFrom<Program>();
        //elsa.AddWorkflow<TestWorkflow>();
    });

services.AddCors(cors => cors.AddDefaultPolicy(policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().WithExposedHeaders("*")));
services.AddRazorPages(options => options.Conventions.ConfigureFilter(new IgnoreAntiforgeryTokenAttribute()));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles();
app.UseRouting();
app.UseCors();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.UseWorkflowsApi();
app.UseWorkflows();
app.UseWorkflowsSignalRHubs();
app.MapFallbackToPage("/_Host");
app.Run();