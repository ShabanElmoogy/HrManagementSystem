using HrManagementSystem.Contracts.BasicContracts.KanbanCards;
using HrManagementSystem.Services.BasicServices.KanbanCardsService;

namespace HrManagementSystem.Controllers.V1;

[ApiVersion("1.0")]
[Route(ApiRoutes.BaseRoute)]
[ApiController]
[Authorize]

public class KanbanCardsController(IKanbanCardService service) : ControllerBase
{
    private readonly IKanbanCardService _service = service;

    [HttpGet]
    [HasPermission(Permissions.ViewKanbanCards)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var cards = await _service.GetAllAsync(cancellationToken);
        return Ok(cards);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id, CancellationToken cancellationToken)
    {
        var response = await _service.GetAsync(id, cancellationToken);
        return response.IsSuccess ? Ok(response.Value) : response.ToProblem();
    }

    [HttpPost]
    [HasPermission(Permissions.CreateKanbanCards)]
    public async Task<IActionResult> Add([FromBody] KanbanCardRequest request, CancellationToken cancellationToken)
    {
        var result = await _service.AddAsync(request, cancellationToken);
        return result.IsSuccess
            ? CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value)
            : result.ToProblem();
    }

    [HttpPut]
    [HasPermission(Permissions.EditKanbanCards)]
    public async Task<IActionResult> Update([FromBody] KanbanCardRequest request, CancellationToken cancellationToken)
    {
        var result = await _service.UpdateAsync(request, cancellationToken);
        return result.IsSuccess
           ? CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value)
           : result.ToProblem();
    }

    [HttpDelete("{id}")]
    [HasPermission(Permissions.DeleteKanbanCards)]
    public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await _service.ToggleAsync(id, cancellationToken);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
}
