using Microsoft.AspNetCore.Mvc;
using ZalohovacServer.Database;
using ZalohovacServer.Entities.DB;
using ApiEntities = ZalohovacServer.Entities.API;

namespace ZalohovacServer.Controllers
{
    [Route("api/client")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private DatabaseContext _context;

        public ClientController(DatabaseContext context)
        {
            _context = context;
        }

        // GET api/client/{uuid}/jobs
        // tohle je verejne (bez JWT) - pouziva to klientska aplikace
        [HttpGet("{uuid}/jobs")]
        public ActionResult<List<ApiEntities.BackupJob>> GetJobsForComputer([FromRoute] Guid uuid)
        {
            Computer? computer = _context.Computers.SingleOrDefault(c => c.Uuid == uuid);

            if (computer == null)
                return NotFound("Počítač s tímto UUID neexistuje.");

            if (!computer.Enabled)
                return Ok(new List<ApiEntities.BackupJob>());

            List<int> jobIds = _context.Assignments
                .Where(a => a.ComputerUuid == uuid)
                .Select(a => a.JobId)
                .ToList();

            List<ApiEntities.BackupJob> jobs = _context.Jobs
                .Where(j => jobIds.Contains(j.Id))
                .ToList()
                .Select(j => new ApiEntities.BackupJob
                {
                    Id = j.Id,
                    Name = j.Name,
                    Sources = j.Sources.Select(s => s.Directory).ToList(),
                    Targets = j.Targets.Select(t => t.Directory).ToList(),
                    Timing = j.Timing,
                    Method = j.Method,
                    Retention = new ApiEntities.BackupRetention
                    {
                        Count = j.RetentionCount,
                        Size = j.RetentionSize
                    }
                })
                .ToList();

            return Ok(jobs);
        }
    }
}