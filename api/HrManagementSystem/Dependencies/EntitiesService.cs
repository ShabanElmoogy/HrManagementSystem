

using HrManagementSystem.Services.BasicServices.AppointmentService;
using HrManagementSystem.Services.BasicServices.BoardTaskAttachmentsService;
using HrManagementSystem.Services.BasicServices.BoardTaskCommentsService;
using HrManagementSystem.Services.BasicServices.BoardTasksService;
using HrManagementSystem.Services.BasicServices.KanbanBoardMembersService;
using HrManagementSystem.Services.BasicServices.KanbanBoardsService;
using HrManagementSystem.Services.BasicServices.KanbanCardAssigneesService;
using HrManagementSystem.Services.BasicServices.KanbanCardAttachmentsService;
using HrManagementSystem.Services.BasicServices.KanbanCardCommentsService;
using HrManagementSystem.Services.BasicServices.KanbanCardsService;
using HrManagementSystem.Services.BasicServices.KanbanColumnsService;
using HrManagementSystem.Services.BasicServices.KanbanLabelsService;

namespace HrManagementSystem.Dependencies;

public static class EntitiesService
{
    public static IServiceCollection AddEntitiesService(this IServiceCollection services)
    {
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IEmailSender, EmailService>();
        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<IEntityChangeLogService, EntityChangeLogService>();
        services.AddScoped<ILocalizationService, LocalizationService>();
        services.AddScoped<ICacheService, CacheService>();
        services.AddScoped<IFileService, FileService>();
        services.AddScoped<IExportExcelService, ExportExcelService>();
        services.AddScoped<IExportPdfFileService, ExportPdfFileService>();
        services.AddScoped<IDashboardService, DashboardService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<ISubCategoryService, SubcategoryService>();
        services.AddScoped<IReportCategoryService, ReportCategoryService>();
        services.AddScoped<IViewService, ViewService>();
        services.AddScoped<IApiKeyService, ApiKeyService>();
        services.AddScoped<IBackupService, BackupService>();
        services.AddScoped<ICountryService, CountryService>();
        services.AddScoped<IStateService, StateService>();
        services.AddScoped<IDistrictService, DistrictService>();
        services.AddScoped<IAddressTypeService, AddressTypeService>();
        services.AddScoped<IKanbanBoardService, KanbanBoardService>();
        services.AddScoped<IKanbanBoardMemberService, KanbanBoardMemberService>();
        services.AddScoped<IKanbanCardService, KanbanCardService>();
        services.AddScoped<IKanbanCardAssigneeService, KanbanCardAssigneeService>();
        services.AddScoped<IKanbanCardAttachmentService, KanbanCardAttachmentService>();
        services.AddScoped<IKanbanCardCommentService, KanbanCardCommentService>();
        services.AddScoped<IBoardTaskAttachmentService, BoardTaskAttachmentService>();
        services.AddScoped<IBoardTaskCommentService, BoardTaskCommentService>();
        services.AddScoped<IKanbanColumnService, KanbanColumnService>();
        services.AddScoped<IKanbanLabelService, KanbanLabelService>();
        services.AddScoped<IBoardTaskService, BoardTaskService>();
        services.AddScoped<IAppointmentService, AppointmentService>();

        return services;
    }
}
