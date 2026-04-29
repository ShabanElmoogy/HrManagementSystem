using HrManagementSystem.Contracts.BasicContracts.KanbanCardAssignees;
using HrManagementSystem.Services.BasicServices.KanbanCardAssigneesService;

namespace HrManagementSystem.Controllers.V1;

[ApiVersion("1.0")]
[Route(ApiRoutes.BaseRoute)]
[ApiController]
[Authorize]

public class KanbanCardAssigneesController(IKanbanCardAssigneeService service) : ControllerBase
{
    private readonly IKanbanCardAssigneeService _service = service;

    [HttpGet]
    [HasPermission(Permissions.ViewKanbanCardAssignees)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var assignees = await _service.GetAllAsync(cancellationToken);
        return Ok(assignees);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id, CancellationToken cancellationToken)
    {
        var response = await _service.GetAsync(id, cancellationToken);
        return response.IsSuccess ? Ok(response.Value) : response.ToProblem();
    }

    [HttpGet("card/{cardId}")]
    [HasPermission(Permissions.ViewKanbanCardAssignees)]
    public async Task<IActionResult> GetByCardId([FromRoute] int cardId, CancellationToken cancellationToken)
    {
        var assignees = await _service.GetByCardIdAsync(cardId, cancellationToken);
        return Ok(assignees);
    }

    [HttpPost]
    [HasPermission(Permissions.CreateKanbanCardAssignees)]
    public async Task<IActionResult> Add([FromBody] KanbanCardAssigneeRequest request, CancellationToken cancellationToken)
    {
        var result = await _service.AddAsync(request, cancellationToken);
        return result.IsSuccess
            ? CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value)
            : result.ToProblem();
    }

    [HttpPut]
    [HasPermission(Permissions.EditKanbanCardAssignees)]
    public async Task<IActionResult> Update([FromBody] KanbanCardAssigneeRequest request, CancellationToken cancellationToken)
    {
        var result = await _service.UpdateAsync(request, cancellationToken);
        return result.IsSuccess
           ? CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value)
           : result.ToProblem();
    }

    [HttpDelete("{id}")]
    [HasPermission(Permissions.DeleteKanbanCardAssignees)]
    public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await _service.ToggleAsync(id, cancellationToken);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
}
