using HrManagementSystem.Contracts.BasicContracts.ArchiveData;
using HrManagementSystem.Entities.BasicEntities;

namespace HrManagementSystem.Services.BasicServices.ArchiveDataService
{
  public class BackupService : IBackupService
  {
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly BackupErrors _backupErrors;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly string _backupFolderPath;

    public BackupService(
        ApplicationDbContext context,
        BackupErrors backupErrors,
        IStringLocalizer<BackupRequest> localizer,
        IMapper mapper,
        IWebHostEnvironment webHostEnvironment)
    {
      _context = context;
      _mapper = mapper;
      _backupErrors = backupErrors;
      _webHostEnvironment = webHostEnvironment;

      _backupFolderPath = Path.Combine(_webHostEnvironment.WebRootPath, "archieveData");

      if (!Directory.Exists(_backupFolderPath))
      {
        Directory.CreateDirectory(_backupFolderPath);
      }
    }

    public async Task<IEnumerable<BackupResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {

      return await _context.Backups
                           .AsNoTracking()
                           .ProjectToType<BackupResponse>()
                           .ToListAsync(cancellationToken);

    }

    public async Task<Result<BackupResponse>>? GetAsync(int id, CancellationToken cancellationToken = default)
    {
      var response = await _context.Backups.FindAsync(id, cancellationToken);

      return response is not null
          ? Result.Success(response.Adapt<BackupResponse>())
          : Result.Failure<BackupResponse>(_backupErrors.BackupNotFound);
    }

    public async Task<Result<BackupResponse>> CreateBackupAsync(string description, CancellationToken cancellationToken = default)
    {
      try
      {
        string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        string databaseName = _context.Database.GetDbConnection().Database;
        string backupFilename = $"{databaseName}_backup_{timestamp}.bak";
        string fullFilePath = Path.Combine(_backupFolderPath, backupFilename);
        string backupPath = Path.GetFileName(fullFilePath);

        Directory.CreateDirectory(Path.GetDirectoryName(fullFilePath));

        string backupCommand = $@"
                BACKUP DATABASE [{databaseName}]
                TO DISK = N'{fullFilePath}'
                WITH 
                    INIT, 
                    FORMAT,
                    COMPRESSION, 
                    NAME = N'{databaseName} - Full Database Backup ({timestamp})';";

        await _context.Database.ExecuteSqlRawAsync(backupCommand, cancellationToken);

        // Get backup file size
        long fileSize = new FileInfo(fullFilePath).Length;

        // Record backup in database
        var backup = new Backup
        {
          FileName = backupFilename,
          DatabaseName = databaseName,
          BackupPath = backupPath,
          Description = description,
          Size = fileSize,
          Status = Strings.Completed,
        };

        await _context.Backups.AddAsync(backup, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        var response = _mapper.Map<BackupResponse>(backup);
        return Result.Success(response);
      }
      catch (Exception ex)
      {
        // Record failed backup attempt
        //TODO : Add logging
        string databaseName = _context.Database.GetDbConnection().Database;
        var backup = new Backup
        {
          FileName = $"{databaseName}_backup_failed_{DateTime.Now:yyyyMMdd_HHmmss}.bak",
          DatabaseName = databaseName,
          BackupPath = string.Empty,
          Description = $"Failed backup attempt: {ex.Message}",
          Size = 0,
          Status = Strings.Failed
        };

        _context.Backups.Add(backup);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Failure<BackupResponse>(_backupErrors.BackupError);
      }
    }

    public async Task<Result> RestoreBackupAsync(int id, CancellationToken cancellationToken = default)
    {
      var backup = await _context.Backups.FindAsync(id, cancellationToken);

      if (backup == null)
        return Result.Failure<BackupResponse>(_backupErrors.BackupNotFound);

      string fullpath = Path.Combine(_backupFolderPath, backup.BackupPath);

      if (!File.Exists(fullpath))
        return Result.Failure<BackupResponse>(_backupErrors.BackupFileNotFound);

      try
      {
        string databaseName = _context.Database.GetDbConnection().Database;

        string restoreCommand = $@"
                USE master;
                ALTER DATABASE [{databaseName}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
                RESTORE DATABASE [{databaseName}] 
                FROM DISK = N'{fullpath}' 
                WITH REPLACE;
                ALTER DATABASE [{databaseName}] SET MULTI_USER WITH ROLLBACK IMMEDIATE;";

        await _context.Database.ExecuteSqlRawAsync(restoreCommand, cancellationToken);

        return Result.Success();
      }
      catch (Exception)
      {
        return Result.Failure<BackupResponse>(_backupErrors.BackupRestoreError);
      }
    }
    public async Task<Result> DeleteOldBackupsAsync(int intervalValue, IntervalType intervalType, CancellationToken cancellationToken = default)
    {
      if (intervalValue <= 0)
      {
        return Result.Failure(_backupErrors.InvalidIntervalValue);
      }

      var now = DateTime.UtcNow;

      var olderThan = intervalType switch
      {
        IntervalType.Minute => TimeSpan.FromMinutes(intervalValue),
        IntervalType.Hour => TimeSpan.FromHours(intervalValue),
        IntervalType.Day => TimeSpan.FromDays(intervalValue),
        IntervalType.Month => TimeSpan.FromDays(intervalValue * 30),
        IntervalType.Year => TimeSpan.FromDays(intervalValue * 365),
        _ => throw new ArgumentOutOfRangeException(nameof(intervalType), $"Unsupported interval type: {intervalType}")
      };

      var cutoffDate = now - olderThan;

      var oldBackups = await _context.Backups
          .Where(b => b.CreatedOn < cutoffDate && b.Status == Strings.Completed && !b.IsDeleted)
          .ToListAsync(cancellationToken);

      if (oldBackups.Count == 0)
      {
        return Result.Success();
      }

      int deletedCount = 0;

      foreach (var backup in oldBackups)
      {
        cancellationToken.ThrowIfCancellationRequested();

        if (string.IsNullOrWhiteSpace(backup.BackupPath))
        {
          continue;
        }

        var fullPath = Path.Combine(_backupFolderPath, backup.BackupPath);

        if (File.Exists(fullPath))
        {
          File.Delete(fullPath);
          deletedCount++;
        }

        backup.IsDeleted = true;
        backup.Status = Strings.Deleted;
      }

      await _context.SaveChangesAsync(cancellationToken);
      return Result.Success();
    }
  }
}