using HrManagementSystem.Contracts.BasicContracts.KanbanBoardMembers;
using HrManagementSystem.Services.BasicServices.KanbanBoardMembersService;

namespace HrManagementSystem.Controllers.V1;

[ApiVersion("1.0")]
[Route(ApiRoutes.BaseRoute)]
[ApiController]
[Authorize]
public class KanbanBoardMembersController(IKanbanBoardMemberService service) : ControllerBase
{
    private readonly IKanbanBoardMemberService _service = service;

    [HttpGet]
    [HasPermission(Permissions.ViewKanbanBoardMembers)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var members = await _service.GetAllAsync(cancellationToken);
        return Ok(members);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id, CancellationToken cancellationToken)
    {
        var response = await _service.GetAsync(id, cancellationToken);
        return response.IsSuccess ? Ok(response.Value) : response.ToProblem();
    }

    [HttpGet("board/{boardId}")]
    [HasPermission(Permissions.ViewKanbanBoardMembers)]
    public async Task<IActionResult> GetByBoardId([FromRoute] int boardId, CancellationToken cancellationToken)
    {
        var members = await _service.GetByBoardIdAsync(boardId, cancellationToken);
        return Ok(members);
    }

    [HttpPost]
    [HasPermission(Permissions.CreateKanbanBoardMembers)]
    public async Task<IActionResult> Add([FromBody] KanbanBoardMemberRequest request, CancellationToken cancellationToken)
    {
        var result = await _service.AddAsync(request, cancellationToken);
        return result.IsSuccess
            ? CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value)
            : result.ToProblem();
    }

    [HttpPut]
    [HasPermission(Permissions.EditKanbanBoardMembers)]
    public async Task<IActionResult> Update([FromBody] KanbanBoardMemberRequest request, CancellationToken cancellationToken)
    {
        var result = await _service.UpdateAsync(request, cancellationToken);
        return result.IsSuccess
           ? CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value)
           : result.ToProblem();
    }

    [HttpDelete("{id}")]
    [HasPermission(Permissions.DeleteKanbanBoardMembers)]
    public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await _service.ToggleAsync(id, cancellationToken);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
}
