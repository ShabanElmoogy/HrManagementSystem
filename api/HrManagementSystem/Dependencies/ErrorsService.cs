using TechnicalSupportApi.Errors.EntitiesErrors;

namespace HrManagementSystem.Dependencies;

public static class ErrorsService
{
    public static IServiceCollection AddErrorsService(this IServiceCollection service)
    {
        service.AddScoped<RoleErrors>();
        service.AddScoped<UserErrors>();
        service.AddScoped<LocalizationError>();
        service.AddScoped<CategoryErrors>();
        service.AddScoped<SubCategoryErrors>();
        service.AddScoped<ReportCategoryErrors>();
        service.AddScoped<ApiKeyErrors>();
        service.AddScoped<BackupErrors>();
        service.AddScoped<CountryErrors>();
        service.AddScoped<StateErrors>();
        service.AddScoped<DistrictErrors>();
        service.AddScoped<AddressErrors>();
        service.AddScoped<AddressTypeErrors>();
        service.AddScoped<KanbanBoardErrors>();
        service.AddScoped<KanbanBoardMemberErrors>();
        service.AddScoped<KanbanCardErrors>();
        service.AddScoped<KanbanCardAssigneeErrors>();
        service.AddScoped<KanbanCardAttachmentErrors>();
        service.AddScoped<KanbanCardCommentErrors>();
        service.AddScoped<BoardTaskAttachmentErrors>();
        service.AddScoped<BoardTaskCommentErrors>();
        service.AddScoped<KanbanColumnErrors>();
        service.AddScoped<KanbanLabelErrors>();
        service.AddScoped<BoardTaskErrors>();
        service.AddScoped<AppointmentErrors>();

        return service;
    }
}
