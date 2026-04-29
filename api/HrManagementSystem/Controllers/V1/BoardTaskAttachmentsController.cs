using HrManagementSystem.Contracts.BasicContracts.BoardTaskAttachments;
using HrManagementSystem.Services.BasicServices.BoardTaskAttachmentsService;

namespace HrManagementSystem.Controllers.V1;

[ApiVersion("1.0")]
[Route(ApiRoutes.BaseRoute)]
[ApiController]
[Authorize]
public class BoardTaskAttachmentsController(IBoardTaskAttachmentService service) : ControllerBase
{
    private readonly IBoardTaskAttachmentService _service = service;

    [HttpGet]
    [HasPermission(Permissions.ViewBoardTaskAttachments)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var attachments = await _service.GetAllAsync(cancellationToken);
        return Ok(attachments);
    }

    [HttpGet("task/{taskId}")]
    [HasPermission(Permissions.ViewBoardTaskAttachments)]
    public async Task<IActionResult> GetByTaskId([FromRoute] int taskId, CancellationToken cancellationToken)
    {
        var attachments = await _service.GetByTaskIdAsync(taskId, cancellationToken);
        return Ok(attachments);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id, CancellationToken cancellationToken)
    {
        var response = await _service.GetAsync(id, cancellationToken);
        return response.IsSuccess ? Ok(response.Value) : response.ToProblem();
    }

    [HttpPost]
    [HasPermission(Permissions.CreateBoardTaskAttachments)]
    public async Task<IActionResult> Add([FromBody] BoardTaskAttachmentRequest request, CancellationToken cancellationToken)
    {
        var result = await _service.AddAsync(request, cancellationToken);
        return result.IsSuccess
            ? CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value)
            : result.ToProblem();
    }

    [HttpPut]
    [HasPermission(Permissions.EditBoardTaskAttachments)]
    public async Task<IActionResult> Update([FromBody] BoardTaskAttachmentRequest request, CancellationToken cancellationToken)
    {
        var result = await _service.UpdateAsync(request, cancellationToken);
        return result.IsSuccess
            ? CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value)
            : result.ToProblem();
    }

    [HttpDelete("{id}")]
    [HasPermission(Permissions.DeleteBoardTaskAttachments)]
    public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await _service.ToggleAsync(id, cancellationToken);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
}
