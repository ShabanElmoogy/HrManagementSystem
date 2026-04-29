using HrManagementSystem.Hangfire;

public static class HangfireJobConfig
{
    public static void RegisterJobs()
    {
        RecurringJob.AddOrUpdate<RecurringJobExecutor>(
            "database-backup-job",
            executor => executor.RunDatabaseBackup(),
            CronExpressions.EveryMinute // or any interval
        );

        // Add more jobs similarly
    }
}
