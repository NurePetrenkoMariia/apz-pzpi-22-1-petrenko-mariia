using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace FarmKeeper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BackupController : ControllerBase
    {
        private readonly string connectionString;
        public BackupController(IConfiguration configuration)
        {
            this.connectionString = configuration.GetConnectionString("FarmKeeperConnectionString");
        }

        [HttpGet("download-backup")]
        [Authorize(Roles = "DatabaseAdmin")]
        public async Task<IActionResult> DownloadBackup()
        {
           
            string downloadPath = @"C:\Backup";
            if (!Directory.Exists(downloadPath))
            {
                Directory.CreateDirectory(downloadPath);
            }

            string backupFilePath = Path.Combine(downloadPath, "FarmKeeperDatabaseBackup.bak");

            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    string query = $@"
                    BACKUP DATABASE [FarmKeeperDb]
                    TO DISK = @BackupPath
                    WITH FORMAT, INIT;";

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@BackupPath", backupFilePath);
                        await command.ExecuteNonQueryAsync();
                    }
                }

                var memoryStream = new MemoryStream(await System.IO.File.ReadAllBytesAsync(backupFilePath));
                return File(memoryStream, "application/octet-stream", "DatabaseBackup.bak");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error creating backup: {ex.Message}");
            }

        }

        [HttpPost("restore-database")]
        [Authorize(Roles = "DatabaseAdmin")]
        public async Task<IActionResult> RestoreDatabase(IFormFile backupFile)
        {
            if (backupFile == null || backupFile.Length == 0)
            {
                return BadRequest("Error! File was not found or empty.");
            }

            string downloadPath = @"C:\Backup";

            string filePath = Path.Combine(downloadPath, "FarmKeeperDatabaseBackup.bak");
            try
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await backupFile.CopyToAsync(stream);
                }
                string query = $@" USE master;
                    ALTER DATABASE [FarmKeeperDb] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
                    RESTORE DATABASE [FarmKeeperDb]
                    FROM DISK = @BackupPath
                    WITH REPLACE;
                    ALTER DATABASE [FarmKeeperDb] SET MULTI_USER;";

                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@BackupPath", filePath);
                        await command.ExecuteNonQueryAsync();
                    }
                }

                return Ok("Database restored successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error restoring database: {ex.Message}");
            }
        }
    }
}
