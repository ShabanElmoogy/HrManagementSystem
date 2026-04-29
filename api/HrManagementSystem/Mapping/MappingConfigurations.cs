namespace HrManagementSystem.Mapping;

public class MappingConfigurations : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<(ApplicationUser user, IList<string> roles), UserResponse>()
            .Map(dest => dest, src => src.user)
            .Map(dest => dest.Roles, src => src.roles);

        config.NewConfig<CreateUserRequest, ApplicationUser>()
            .Map(dest => dest.EmailConfirmed, src => true);

        config.NewConfig<Category, CategoryResponse>()
              .Map(dest => dest.SubCategories, src => src.CategorySubcategories.Select(cs => cs.SubCategory));

        //config.NewConfig<Category, CategoryResponse>()
        //      .Map(dest => dest.SubCategories, src => src.CategorySubcategories
        //      .Select(cs => cs.SubCategory)
        //      .Where(sc => !sc.IsDeleted));

        // Configure this in your startup or wherever you initialize Mapster
        config.NewConfig<SubCategory, SubCategoryResponse>()
            .Map(dest => dest.Categories, src => src.CategorySubcategories.Select(cs => cs.Category).ToList());


        // Configure this in your startup or wherever you initialize Mapster
        config.NewConfig<SubCategoryRequest, SubCategory>()
            .Map(dest => dest.CategorySubcategories,
                src => (src.CategoryIds != null && src.CategoryIds.Count > 0)
                ? src.CategoryIds.Select(id => new CategorySubcategory { CategoryId = id }).ToList()
                : new List<CategorySubcategory>());

        config.NewConfig<ApplicationUser, UserResponse>();

        //config.NewConfig<UpdateUserRequest, ApplicationUser>()
        //    .Map(dest => dest.NormalizedUserName, src => src.UserName.ToUpper())
        //    .Map(dest => dest.NormalizedEmail, src => src.Email.ToUpper());

        // KanbanCard mappings
        config.NewConfig<KanbanCardRequest, KanbanCard>();
        config.NewConfig<KanbanCard, KanbanCardResponse>();

        // KanbanColumn mappings
        config.NewConfig<HrManagementSystem.Contracts.BasicContracts.KanbanColumns.KanbanColumnRequest, KanbanColumn>();
        config.NewConfig<KanbanColumn, HrManagementSystem.Contracts.BasicContracts.KanbanColumns.KanbanColumnResponse>()
              .Map(dest => dest.Cards, src => src.Cards.Where(c => !c.IsDeleted)
                                                        .OrderBy(c => c.Order)
                                                        .Select(c => new HrManagementSystem.Contracts.BasicContracts.KanbanCards.SimpleKanbanCardResponse(c.Id, c.Title, c.Description, c.Order, c.DueDate, c.IsArchived)));

        // KanbanCardAssignee mappings
        config.NewConfig<KanbanCardAssigneeRequest, KanbanCardAssignee>();
        config.NewConfig<KanbanCardAssignee, KanbanCardAssigneeResponse>();

        // KanbanCardAttachment mappings
        config.NewConfig<KanbanCardAttachmentRequest, KanbanCardAttachment>();
        config.NewConfig<KanbanCardAttachment, KanbanCardAttachmentResponse>();

        // KanbanCardComment mappings
        config.NewConfig<KanbanCardCommentRequest, KanbanCardComment>();

        // BoardTaskComment mappings
        config.NewConfig<HrManagementSystem.Contracts.BasicContracts.BoardTaskComments.BoardTaskCommentRequest, BoardTaskComment>();

        // BoardTask mappings
        config.NewConfig<HrManagementSystem.Contracts.BasicContracts.BoardTasks.BoardTaskRequest, BoardTask>();
        config.NewConfig<BoardTask, HrManagementSystem.Contracts.BasicContracts.BoardTasks.BoardTaskResponse>();

        // BoardTaskAttachment mappings
        config.NewConfig<HrManagementSystem.Contracts.BasicContracts.BoardTaskAttachments.BoardTaskAttachmentRequest, BoardTaskAttachment>();
        config.NewConfig<BoardTaskAttachment, HrManagementSystem.Contracts.BasicContracts.BoardTaskAttachments.BoardTaskAttachmentResponse>();

    }
}
