using HrManagementSystem.Contracts.BasicContracts.KanbanLabels;
using HrManagementSystem.Services.BasicServices.KanbanLabelsService;

namespace HrManagementSystem.Controllers.V1;

[ApiVersion("1.0")]
[Route(ApiRoutes.BaseRoute)]
[ApiController]
[Authorize]
public class KanbanLabelsController(IKanbanLabelService service) : ControllerBase
{
    private readonly IKanbanLabelService _service = service;

    [HttpGet]
    [HasPermission(Permissions.ViewKanbanLabels)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var labels = await _service.GetAllAsync(cancellationToken);
        return Ok(labels);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id, CancellationToken cancellationToken)
    {
        var response = await _service.GetAsync(id, cancellationToken);
        return response.IsSuccess ? Ok(response.Value) : response.ToProblem();
    }

    [HttpPost]
    [HasPermission(Permissions.CreateKanbanLabels)]
    public async Task<IActionResult> Add([FromBody] KanbanLabelRequest request, CancellationToken cancellationToken)
    {
        var result = await _service.AddAsync(request, cancellationToken);
        return result.IsSuccess
            ? CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value)
            : result.ToProblem();
    }

    [HttpPut]
    [HasPermission(Permissions.EditKanbanLabels)]
    public async Task<IActionResult> Update([FromBody] KanbanLabelRequest request, CancellationToken cancellationToken)
    {
        var result = await _service.UpdateAsync(request, cancellationToken);
        return result.IsSuccess
           ? CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value)
           : result.ToProblem();
    }

    [HttpDelete("{id}")]
    [HasPermission(Permissions.DeleteKanbanLabels)]
    public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await _service.ToggleAsync(id, cancellationToken);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
}