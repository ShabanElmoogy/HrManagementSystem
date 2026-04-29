using HrManagementSystem.Contracts.BasicContracts.BoardTaskComments;
using HrManagementSystem.Services.BasicServices.BoardTaskCommentsService;

namespace HrManagementSystem.Controllers.V1;

[ApiVersion("1.0")]
[Route(ApiRoutes.BaseRoute)]
[ApiController]
[Authorize]
public class BoardTaskCommentsController(IBoardTaskCommentService service) : ControllerBase
{
    private readonly IBoardTaskCommentService _service = service;

    [HttpGet]
    [HasPermission(Permissions.ViewBoardTaskComments)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var comments = await _service.GetAllAsync(cancellationToken);
        return Ok(comments);
    }

    [HttpGet("task/{taskId}")]
    [HasPermission(Permissions.ViewBoardTaskComments)]
    public async Task<IActionResult> GetByTaskId([FromRoute] int taskId, CancellationToken cancellationToken)
    {
        var comments = await _service.GetByTaskIdAsync(taskId, cancellationToken);
        return Ok(comments);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id, CancellationToken cancellationToken)
    {
        var response = await _service.GetAsync(id, cancellationToken);
        return response.IsSuccess ? Ok(response.Value) : response.ToProblem();
    }

    [HttpPost]
    [HasPermission(Permissions.CreateBoardTaskComments)]
    public async Task<IActionResult> Add([FromBody] BoardTaskCommentRequest request, CancellationToken cancellationToken)
    {
        var result = await _service.AddAsync(request, cancellationToken);
        return result.IsSuccess
            ? CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value)
            : result.ToProblem();
    }

    [HttpPut]
    [HasPermission(Permissions.EditBoardTaskComments)]
    public async Task<IActionResult> Update([FromBody] BoardTaskCommentRequest request, CancellationToken cancellationToken)
    {
        var result = await _service.UpdateAsync(request, cancellationToken);
        return result.IsSuccess
            ? CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value)
            : result.ToProblem();
    }

    [HttpDelete("{id}")]
    [HasPermission(Permissions.DeleteBoardTaskComments)]
    public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await _service.ToggleAsync(id, cancellationToken);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
}
