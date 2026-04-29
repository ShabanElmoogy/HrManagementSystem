using HrManagementSystem.Contracts.BasicContracts.KanbanCardAttachments;
using HrManagementSystem.Services.BasicServices.KanbanCardAttachmentsService;

namespace HrManagementSystem.Controllers.V1;

[ApiVersion("1.0")]
[Route(ApiRoutes.BaseRoute)]
[ApiController]
[Authorize]
public class KanbanCardAttachmentsController(IKanbanCardAttachmentService service) : ControllerBase
{
    private readonly IKanbanCardAttachmentService _service = service;

    [HttpGet]
    [HasPermission(Permissions.ViewKanbanCardAttachments)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var attachments = await _service.GetAllAsync(cancellationToken);
        return Ok(attachments);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id, CancellationToken cancellationToken)
    {
        var response = await _service.GetAsync(id, cancellationToken);
        return response.IsSuccess ? Ok(response.Value) : response.ToProblem();
    }

    [HttpGet("card/{cardId}")]
    [HasPermission(Permissions.ViewKanbanCardAttachments)]
    public async Task<IActionResult> GetByCardId([FromRoute] int cardId, CancellationToken cancellationToken)
    {
        var attachments = await _service.GetByCardIdAsync(cardId, cancellationToken);
        return Ok(attachments);
    }

    [HttpPost]
    [HasPermission(Permissions.CreateKanbanCardAttachments)]
    public async Task<IActionResult> Add([FromBody] KanbanCardAttachmentRequest request, CancellationToken cancellationToken)
    {
        var result = await _service.AddAsync(request, cancellationToken);
        return result.IsSuccess
            ? CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value)
            : result.ToProblem();
    }

    [HttpPut]
    [HasPermission(Permissions.EditKanbanCardAttachments)]
    public async Task<IActionResult> Update([FromBody] KanbanCardAttachmentRequest request, CancellationToken cancellationToken)
    {
        var result = await _service.UpdateAsync(request, cancellationToken);
        return result.IsSuccess
           ? CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value)
           : result.ToProblem();
    }

    [HttpDelete("{id}")]
    [HasPermission(Permissions.DeleteKanbanCardAttachments)]
    public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await _service.ToggleAsync(id, cancellationToken);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
}
