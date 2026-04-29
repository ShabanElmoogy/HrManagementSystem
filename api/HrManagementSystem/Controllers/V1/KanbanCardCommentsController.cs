using HrManagementSystem.Contracts.BasicContracts.KanbanCardComments;
using HrManagementSystem.Services.BasicServices.KanbanCardCommentsService;

namespace HrManagementSystem.Controllers.V1;

[ApiVersion("1.0")]
[Route(ApiRoutes.BaseRoute)]
[ApiController]
[Authorize]
public class KanbanCardCommentsController(IKanbanCardCommentService service) : ControllerBase
{
    private readonly IKanbanCardCommentService _service = service;

    [HttpGet]
    [HasPermission(Permissions.ViewKanbanCardComments)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var comments = await _service.GetAllAsync(cancellationToken);
        return Ok(comments);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id, CancellationToken cancellationToken)
    {
        var response = await _service.GetAsync(id, cancellationToken);
        return response.IsSuccess ? Ok(response.Value) : response.ToProblem();
    }

    [HttpGet("card/{cardId}")]
    [HasPermission(Permissions.ViewKanbanCardComments)]
    public async Task<IActionResult> GetByCardId([FromRoute] int cardId, CancellationToken cancellationToken)
    {
        var comments = await _service.GetByCardIdAsync(cardId, cancellationToken);
        return Ok(comments);
    }

    [HttpPost]
    [HasPermission(Permissions.CreateKanbanCardComments)]
    public async Task<IActionResult> Add([FromBody] KanbanCardCommentRequest request, CancellationToken cancellationToken)
    {
        var result = await _service.AddAsync(request, cancellationToken);
        return result.IsSuccess
            ? CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value)
            : result.ToProblem();
    }

    [HttpPut]
    [HasPermission(Permissions.EditKanbanCardComments)]
    public async Task<IActionResult> Update([FromBody] KanbanCardCommentRequest request, CancellationToken cancellationToken)
    {
        var result = await _service.UpdateAsync(request, cancellationToken);
        return result.IsSuccess
           ? CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value)
           : result.ToProblem();
    }

    [HttpDelete("{id}")]
    [HasPermission(Permissions.DeleteKanbanCardComments)]
    public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await _service.ToggleAsync(id, cancellationToken);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
}
