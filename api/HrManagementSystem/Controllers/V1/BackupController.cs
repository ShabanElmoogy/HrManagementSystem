namespace HrManagementSystem.Controllers.V1;

[ApiVersion("1.0")]
[Route(ApiRoutes.BaseRoute)]
[ApiController]
[Authorize]

public class BackupsController(IBackupService backupService) : ControllerBase
{
    private readonly IBackupService _backupService = backupService;

    [HttpGet]
    [HasPermission(Permissions.ViewBackups)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var backups = await _backupService.GetAllAsync(cancellationToken);
        return Ok(backups);
    }

    [HttpGet("{id}")]
    [HasPermission(Permissions.ViewBackups)]
    public async Task<IActionResult> GetById([FromRoute] int id, CancellationToken cancellationToken)
    {
        var response = await _backupService.GetAsync(id, cancellationToken);

        return response.IsSuccess ? Ok(response.Value) : response.ToProblem();
    }

    [HttpPost]
    [HasPermission(Permissions.CreateBackups)]
    public async Task<IActionResult> CreateBackup([FromBody] string description, CancellationToken cancellationToken)
    {
        var result = await _backupService.CreateBackupAsync(description, cancellationToken);

        return result.IsSuccess
            ? CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value)
            : result.ToProblem();
    }

    [HttpPost("{id}/restore")]
    [HasPermission(Permissions.RestoreBackups)]
    public async Task<IActionResult> RestoreBackup([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await _backupService.RestoreBackupAsync(id, cancellationToken);

        return result.IsSuccess ? Ok(result) : result.ToProblem();
    }

    [HttpDelete("old/{interval}/{intervalType}")]
    [HasPermission(Permissions.DeleteBackups)]
    public async Task<IActionResult> DeleteOldBackups(int interval, IntervalType intervalType, CancellationToken cancellationToken)
    {
        var result = await _backupService.DeleteOldBackupsAsync(interval, intervalType, cancellationToken);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
}