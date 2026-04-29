using HrManagementSystem.Contracts.BasicContracts.KanbanBoards;
using HrManagementSystem.Services.BasicServices.KanbanBoardsService;

namespace HrManagementSystem.Controllers.V1;

[ApiVersion("1.0")]
[Route(ApiRoutes.BaseRoute)]
[ApiController]
[Authorize]

public class KanbanBoardsController(IKanbanBoardService service) : ControllerBase
{
    private readonly IKanbanBoardService _service = service;

    [HttpGet]
    [HasPermission(Permissions.ViewKanbanBoards)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var boards = await _service.GetAllAsync(cancellationToken);
        return Ok(boards);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id, CancellationToken cancellationToken)
    {
        var response = await _service.GetAsync(id, cancellationToken);
        return response.IsSuccess ? Ok(response.Value) : response.ToProblem();
    }

    [HttpPost]
    [HasPermission(Permissions.CreateKanbanBoards)]
    public async Task<IActionResult> Add([FromBody] KanbanBoardRequest request, CancellationToken cancellationToken)
    {
        var result = await _service.AddAsync(request, cancellationToken);
        return result.IsSuccess
            ? CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value)
            : result.ToProblem();
    }

    [HttpPut]
    [HasPermission(Permissions.EditKanbanBoards)]
    public async Task<IActionResult> Update([FromBody] KanbanBoardRequest request, CancellationToken cancellationToken)
    {
        var result = await _service.UpdateAsync(request, cancellationToken);
        return result.IsSuccess
           ? CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value)
           : result.ToProblem();
    }

    [HttpDelete("{id}")]
    [HasPermission(Permissions.DeleteKanbanBoards)]
    public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await _service.ToggleAsync(id, cancellationToken);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
}