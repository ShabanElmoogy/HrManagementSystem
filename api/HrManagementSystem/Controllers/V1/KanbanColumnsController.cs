using HrManagementSystem.Contracts.BasicContracts.KanbanColumns;
using HrManagementSystem.Services.BasicServices.KanbanColumnsService;

namespace HrManagementSystem.Controllers.V1;

[ApiVersion("1.0")]
[Route(ApiRoutes.BaseRoute)]
[ApiController]
[Authorize]
public class KanbanColumnsController(IKanbanColumnService service) : ControllerBase
{
    private readonly IKanbanColumnService _service = service;

    [HttpGet]
    [HasPermission(Permissions.ViewKanbanColumns)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var columns = await _service.GetAllAsync(cancellationToken);
        return Ok(columns);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id, CancellationToken cancellationToken)
    {
        var response = await _service.GetAsync(id, cancellationToken);
        return response.IsSuccess ? Ok(response.Value) : response.ToProblem();
    }

    [HttpPost]
    [HasPermission(Permissions.CreateKanbanColumns)]
    public async Task<IActionResult> Add([FromBody] KanbanColumnRequest request, CancellationToken cancellationToken)
    {
        var result = await _service.AddAsync(request, cancellationToken);
        return result.IsSuccess
            ? CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value)
            : result.ToProblem();
    }

    [HttpPut]
    [HasPermission(Permissions.EditKanbanColumns)]
    public async Task<IActionResult> Update([FromBody] KanbanColumnRequest request, CancellationToken cancellationToken)
    {
        var result = await _service.UpdateAsync(request, cancellationToken);
        return result.IsSuccess
           ? CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value)
           : result.ToProblem();
    }

    [HttpDelete("{id}")]
    [HasPermission(Permissions.DeleteKanbanColumns)]
    public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await _service.ToggleAsync(id, cancellationToken);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
}