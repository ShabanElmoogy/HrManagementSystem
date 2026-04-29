namespace HrManagementSystem.Services.BasicServices.NotificationService
{
    public interface INotificationService
    {
        Task SendNewCompanyNotification(int? companyId);
    }
}
