using Application.Workflows;
using Bird.Extensions;
using Bird.Model;
using Elsa.EntityFrameworkCore.Extensions;
using Elsa.EntityFrameworkCore.Modules.Management;
using Elsa.EntityFrameworkCore.Modules.Runtime;
using Elsa.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseStaticWebAssets();
var connection = builder.Configuration.GetSection("DataConnection").Get<DataConnection>();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" ,});
    
    c.CustomSchemaIds(type => type.FullName); // 使用类型的完整名称作为schemaId
        //c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API V1");
});
builder.Services.AddControllers();
builder.Services.AddElsa(elsa =>
{
    // Configure Management layer to use EF Core.
    elsa.UseWorkflowManagement(management => management.UseEntityFrameworkCore(val => val.UsePostgreSql(connection.Pgsql)));

    // Configure Runtime layer to use EF Core.
    elsa.UseWorkflowRuntime(runtime => runtime.UseEntityFrameworkCore(val => val.UsePostgreSql(connection.Pgsql)));

    // Default Identity features for authentication/authorization.
    elsa.UseIdentity(identity =>
    {
        identity.TokenOptions =
            options => options.SigningKey =
                "sufficiently-large-secret-signing-key"; // This key needs to be at least 256 bits long.
        identity.UseAdminUserProvider();
    });

    elsa.AddWorkflow<HelloWorld>();

    // Configure ASP.NET authentication/authorization.
    elsa.UseDefaultAuthentication(auth => auth.UseAdminApiKey());

    // Expose Elsa API endpoints.
    elsa.UseWorkflowsApi();

    // Setup a SignalR hub for real-time updates from the server.
    elsa.UseRealTimeWorkflows();
    elsa.UseJavaScript();

    // Enable C# workflow expressions
    elsa.UseCSharp();

    // Enable HTTP activities.
    //elsa.UseHttp();
    elsa.UseHttp(http => http.ConfigureHttpOptions = options => builder.Configuration.GetSection("Http").Bind(options));
    // Use timer activities.
    elsa.UseScheduling();

    // Register custom activities from the application, if any.
    elsa.AddActivitiesFrom<Program>();

    // Register custom workflows from the application, if any.
    elsa.AddWorkflowsFrom<Program>();
    //elsa.UseHttp(http => http.ConfigureHttpOptions = options => options. )

});


builder.Services.AddCors(cors => cors
    .AddDefaultPolicy(policy => policy
        .AllowAnyOrigin() // For demo purposes only. Use a specific origin instead.
        .AllowAnyHeader()
        .AllowAnyMethod()
        .WithExposedHeaders("x-elsa-workflow-instance-id"))); 
builder.Services.AddRazorPages(options => options.Conventions.ConfigureFilter(new IgnoreAntiforgeryTokenAttribute()));

//builder.Services.AddHealthChecksUI();
builder.Services.AddHealthChecks()
    .AddCheck<PgyHealthCheck>(nameof(PgyHealthCheck));

InjectService.ServiceInject(builder.Services);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "bird");
});

app.UseCors();
app.UseHttpsRedirection();
//app.UseBlazorFrameworkFiles();
app.UseRouting();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.UseWorkflowsApi(); // Use Elsa API endpoints.
app.UseWorkflows(); // Use Elsa middleware to handle HTTP requests mapped to HTTP Endpoint activities.
app.UseWorkflowsSignalRHubs();

app.UseEndpoints(endpoints =>
{
    //endpoints.MapHealthChecksUI(); 

    endpoints.MapControllers();
    //endpoints.MapSwagger();
    endpoints.MapHealthChecks("/health"); // 配置健康检查终结点的路由
    // 其他终结点的配置
});
app.MapFallbackToPage("/_Host");
app.Run();
