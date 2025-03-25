using eReservation.Data;
using eReservation.Helpers.Auth;
using eReservation.Services;
using Microsoft.EntityFrameworkCore;
using Quartz;
using Quartz.Impl;
using eReservation.Data;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x=>x.OperationFilter<AutorizacijaSwaggerHeader>());
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddTransient<AuthService>();
builder.Services.AddTransient<ActionLogService>();
builder.Services.AddTransient<EmailSenderService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<FirebaseService>();
builder.Services.AddTransient<EmailJob>();

builder.Services.AddQuartz(q =>
{
    q.UseSimpleTypeLoader();
    q.UseInMemoryStore();
});

builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

builder.Services.AddSingleton<IScheduler>(provider =>
{
    var schedulerFactory = provider.GetRequiredService<ISchedulerFactory>();
    return schedulerFactory.GetScheduler().Result;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseCors(
    options => options
        .SetIsOriginAllowed(x => _ = true)
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials()
); //This needs to set everything allowed

var scheduler = app.Services.GetRequiredService<IScheduler>();
await scheduler.Start();

IJobDetail emailJob = JobBuilder.Create<EmailJob>()
    .WithIdentity("emailJob", "group1")
    .Build();

ITrigger trigger = TriggerBuilder.Create()
    .WithIdentity("emailTrigger", "group1")
    .StartNow()
    .WithSimpleSchedule(x => x
        .WithIntervalInMinutes(1) 
        .RepeatForever())
    .Build();

await scheduler.ScheduleJob(emailJob, trigger);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
