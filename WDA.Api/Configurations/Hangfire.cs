using Hangfire;
using Hangfire.Dashboard;
using Hangfire.SqlServer;
using Newtonsoft.Json;
using WDA.Shared;

namespace WDA.Api.Configurations;

public static class Hangfire
{
    public static void AddHangfire(this IServiceCollection services)
    {
        services.AddHangfire(configuration =>
        {
            configuration.UseSqlServerStorage(AppSettings.Instance.ConnectionStrings.SqlServer,
                new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(1),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    DisableGlobalLocks = true
                });
            configuration.UseSerializerSettings(new JsonSerializerSettings
                { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
        });
        services.AddHangfireServer();
    }
    
    public static void RegisterRecurringJob()
    {
        // //Send work pass expiry reminder
        // RecurringJob
        //     .AddOrUpdate<IEmailNotificationService>(x => x.SendApplicantRenewWorkPassReminder(), CronExpression.CRON_EXP_EVERY_DAY_AT_6_PM_UTC);
        //
        // //Reminder for document submission
        // RecurringJob
        //     .AddOrUpdate<IEmailNotificationService>(x => x.SendApplicantDocumentSubmissionReminder(), CronExpression.CRON_EXP_EVERY_DAY_AT_6_PM_UTC);
    }
}

public class HangfireDashboardAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context)
    {
        // Temporary enable Hangfire dashboard on development server
        return true;
    }
}