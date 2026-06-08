using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZalohovacServer.Database;
using ZalohovacServer.Entities.DB;
using ApiEntities = ZalohovacServer.Entities.API;

namespace ZalohovacServer.Controllers
{
    [Route("api/job")]
    [ApiController]
    [Authorize]
    public class JobController : ControllerBase
    {
        private static readonly string[] VALID_METHODS = { "full", "incremental", "differential" };

        private DatabaseContext _context;

        public JobController(DatabaseContext context)
        {
            _context = context;
        }

        // GET api/job
        [HttpGet]
        public ActionResult<List<ApiEntities.BackupJob>> GetAll()
        {
            List<Job> jobs = _context.Jobs.ToList();
            List<ApiEntities.BackupJob> result = jobs.Select(j => ConvertToApi(j)).ToList();
            return Ok(result);
        }

        // GET api/job/{id}
        [HttpGet("{id}")]
        public ActionResult<ApiEntities.BackupJob> GetById([FromRoute] int id)
        {
            Job? job = _context.Jobs.SingleOrDefault(j => j.Id == id);

            if (job == null)
                return NotFound("Úloha nenalezena.");

            return Ok(ConvertToApi(job));
        }

        // POST api/job
        [HttpPost]
        public ActionResult<ApiEntities.BackupJob> Create([FromBody] ApiEntities.BackupJob apiJob)
        {
            string? validationError = Validate(apiJob);
            if (validationError != null)
                return BadRequest(validationError);

            Job job = ConvertToDB(apiJob);
            _context.Jobs.Add(job);
            _context.SaveChanges();

            // ulozime sources a targets
            foreach (string dir in apiJob.Sources)
                _context.Sources.Add(new Source { Directory = dir, JobId = job.Id });

            foreach (string dir in apiJob.Targets)
                _context.Targets.Add(new Target { Directory = dir, JobId = job.Id });

            _context.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = job.Id }, ConvertToApi(job));
        }

        // PUT api/job/{id}
        [HttpPut("{id}")]
        public ActionResult Update([FromRoute] int id, [FromBody] ApiEntities.BackupJob apiJob)
        {
            Job? job = _context.Jobs.SingleOrDefault(j => j.Id == id);

            if (job == null)
                return NotFound("Úloha nenalezena.");

            string? validationError = Validate(apiJob);
            if (validationError != null)
                return BadRequest(validationError);

            // aktualizujeme job
            job.Name = apiJob.Name;
            job.Timing = apiJob.Timing;
            job.Method = apiJob.Method.ToLower();
            job.RetentionCount = apiJob.Retention.Count;
            job.RetentionSize = apiJob.Retention.Size;

            // smažeme stare sources a targets a dame nove
            List<Source> oldSources = _context.Sources.Where(s => s.JobId == id).ToList();
            List<Target> oldTargets = _context.Targets.Where(t => t.JobId == id).ToList();
            _context.Sources.RemoveRange(oldSources);
            _context.Targets.RemoveRange(oldTargets);

            foreach (string dir in apiJob.Sources)
                _context.Sources.Add(new Source { Directory = dir, JobId = id });

            foreach (string dir in apiJob.Targets)
                _context.Targets.Add(new Target { Directory = dir, JobId = id });

            _context.SaveChanges();

            return NoContent();
        }

        // DELETE api/job/{id}
        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            Job? job = _context.Jobs.SingleOrDefault(j => j.Id == id);

            if (job == null)
                return NotFound("Úloha nenalezena.");

            _context.Jobs.Remove(job);
            _context.SaveChanges();

            return NoContent();
        }

        private ApiEntities.BackupJob ConvertToApi(Job job)
        {
            return new ApiEntities.BackupJob
            {
                Id = job.Id,
                Name = job.Name,
                Sources = job.Sources.Select(s => s.Directory).ToList(),
                Targets = job.Targets.Select(t => t.Directory).ToList(),
                Timing = job.Timing,
                Method = job.Method,
                Retention = new ApiEntities.BackupRetention
                {
                    Count = job.RetentionCount,
                    Size = job.RetentionSize
                }
            };
        }

        private Job ConvertToDB(ApiEntities.BackupJob apiJob)
        {
            return new Job
            {
                Name = apiJob.Name,
                Timing = apiJob.Timing,
                Method = apiJob.Method.ToLower(),
                RetentionCount = apiJob.Retention.Count,
                RetentionSize = apiJob.Retention.Size
            };
        }

        private string? Validate(ApiEntities.BackupJob apiJob)
        {
            if (string.IsNullOrWhiteSpace(apiJob.Name))
                return "Název nesmí být prázdný.";

            if (string.IsNullOrWhiteSpace(apiJob.Timing))
                return "Timing nesmí být prázdný.";

            if (!VALID_METHODS.Contains(apiJob.Method?.ToLower()))
                return "Method musí být 'full', 'incremental' nebo 'differential'.";

            if (apiJob.Retention.Count <= 0)
                return "Retention count musí být větší než 0.";

            if (apiJob.Retention.Size <= 0)
                return "Retention size musí být větší než 0.";

            if (apiJob.Sources == null || apiJob.Sources.Count == 0)
                return "Job musí mít alespoň jeden source.";

            if (apiJob.Targets == null || apiJob.Targets.Count == 0)
                return "Job musí mít alespoň jeden target.";

            return null;
        }
    }
}
