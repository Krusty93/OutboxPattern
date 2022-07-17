using System.Reflection;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OutboxPattern.Application;
using OutboxPattern.Infrastructure;
using OutboxPattern.Infrastructure.Quartz;
using Quartz;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMediatR(typeof(OutboxPattern.Application.Bootstrapper));
builder.Services.AddSingleton<DapperContext>();

builder.Services.InitializeInfrastructure();
builder.Services.InitializeApplication();

builder.Services.AddDbContext<OutboxDbContext>(opt =>
{
    var cs = builder.Configuration.GetConnectionString("SqlServer");
    opt.UseSqlServer(cs);
});

builder.Services.AddSwaggerGen(opt =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    opt.IncludeXmlComments(xmlPath);
});

builder.Services.AddControllers();

builder.Services.AddQuartz(q =>
{
    q.UseMicrosoftDependencyInjectionJobFactory();

    var jobKey = new JobKey("InboxProcessor");

    q.AddJob<ProcessOutboxJob>(opts => opts.WithIdentity(jobKey));

    q.AddTrigger(t => t
        .ForJob(jobKey)
        .WithIdentity("InboxProcessor-trigger")
        .StartNow()
        .WithCronSchedule("0/15 * * ? * *")
        .WithDescription("cron trigger to read messages from outbox"));
});

builder.Services.AddQuartzServer(opt =>
{
    opt.WaitForJobsToComplete = true;
});

var app = builder.Build();

app.UseRouting();
app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

app.UseSwagger();
app.UseSwaggerUI(opt =>
{
    opt.DisplayRequestDuration();
});

app.Run();
