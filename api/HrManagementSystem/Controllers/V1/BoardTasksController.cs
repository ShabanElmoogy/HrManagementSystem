using HrManagementSystem.Contracts.BasicContracts.BoardTasks;
using HrManagementSystem.Services.BasicServices.BoardTasksService;

namespace HrManagementSystem.Controllers.V1;

[ApiVersion("1.0")]
[Route(ApiRoutes.BaseRoute)]
[ApiController]
[Authorize]
public class BoardTasksController(IBoardTaskService service) : ControllerBase
{
    private readonly IBoardTaskService _service = service;

    [HttpGet]
    [HasPermission(Permissions.ViewKanbanBoards)] // Adjust if BoardTasks-specific permissions are added later
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var tasks = await _service.GetAllAsync(cancellationToken);
        return Ok(tasks);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id, CancellationToken cancellationToken)
    {
        var response = await _service.GetAsync(id, cancellationToken);
        return response.IsSuccess ? Ok(response.Value) : response.ToProblem();
    }

    [HttpPost]
    [HasPermission(Permissions.EditKanbanBoards)] // Adjust if BoardTasks-specific permissions are added later
    public async Task<IActionResult> Add([FromBody] BoardTaskRequest request, CancellationToken cancellationToken)
    {
        var result = await _service.AddAsync(request, cancellationToken);
        return result.IsSuccess
            ? CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value)
            : result.ToProblem();
    }

    [HttpPut]
    [HasPermission(Permissions.EditKanbanBoards)] // Adjust if BoardTasks-specific permissions are added later
    public async Task<IActionResult> Update([FromBody] BoardTaskRequest request, CancellationToken cancellationToken)
    {
        var result = await _service.UpdateAsync(request, cancellationToken);
        return result.IsSuccess
           ? CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value)
           : result.ToProblem();
    }

    [HttpDelete("{id}")]
    [HasPermission(Permissions.DeleteKanbanBoards)] // Adjust if BoardTasks-specific permissions are added later
    public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await _service.ToggleAsync(id, cancellationToken);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
}