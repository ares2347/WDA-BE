namespace WDA.Shared;

public static class CronExpression
{
    //Run every day at 16:00 (UTC+0) ~ 00:00 Malaysia time
    public const string CRON_EXP_EVERY_DAY_AT_6_PM_UTC = "0 16 * * *";
}