using System.Transactions;
using Hangfire;
using Hangfire.MySql;
using Microsoft.EntityFrameworkCore;
using ProtonDbApi.Services;
using ProtonDbApi.Jobs;
using ProtonDbApi.Repos;

var builder = WebApplication.CreateBuilder(args);
// string connectionString = Environment.GetEnvironmentVariable("PROTON_DB_CONNECTION") ?? string.Empty;
string connectionString = "Server=localhost;Port=33060;Database=proton_db;Uid=proton_db_user;Pwd=Abc1234";

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.IgnoreNullValues = true;
});

// Add Hangfire services.
builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseStorage(
        new MySqlStorage(connectionString, new MySqlStorageOptions
        {
            TransactionIsolationLevel = IsolationLevel.ReadCommitted,
            QueuePollInterval = TimeSpan.FromSeconds(15),
            JobExpirationCheckInterval = TimeSpan.FromHours(1),
            CountersAggregateInterval = TimeSpan.FromMinutes(5),
            PrepareSchemaIfNecessary = true,
            DashboardJobListLimit = 50000,
            TransactionTimeout = TimeSpan.FromMinutes(1),
            TablesPrefix = "Hangfire"
        })));

// Add the processing server as IHostedService
builder.Services.AddHangfireServer();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register EF Core Context
builder.Services.AddDbContext<ReportContext>(
    opt => opt.UseMySQL(connectionString)
        .EnableSensitiveDataLogging()
        .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
);

// Register services
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddTransient<IGetNewReportsJob, GetNewReportsJob>();

var app = builder.Build();

// Check environment
if (builder.Environment.IsDevelopment())
{
    app.UseHangfireDashboard();
}
else
{
    app.Services.GetService<IBackgroundJobClient>();
}

var scope = app.Services.CreateScope();
var getNewReportsJob = scope.ServiceProvider.GetRequiredService<IGetNewReportsJob>();

// Run hangfire job on startup
BackgroundJob.Enqueue(() => getNewReportsJob.GetAndExtractReportsFromGithub());

// Queue hangfire job on startup
RecurringJob.AddOrUpdate(
    "MonthlyGithubPull",
    () => getNewReportsJob.GetAndExtractReportsFromGithub(),
    "* * 3 * *");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();