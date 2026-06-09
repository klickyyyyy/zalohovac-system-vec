using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZalohovacServer.Database;
using ZalohovacServer.Entities.DB;

namespace ZalohovacServer.Controllers
{
    [Route("api/assignment")]
    [ApiController]
    [Authorize]
    public class AssignmentController : ControllerBase
    {
        private DatabaseContext _context;

        public AssignmentController(DatabaseContext context)
        {
            _context = context;
        }

        // GET api/assignment
        [HttpGet]
        public ActionResult<List<Assignment>> GetAll()
        {
            List<Assignment> assignments = _context.Assignments.ToList();
            return Ok(assignments);
        }

        // GET api/assignment/{id}
        [HttpGet("{id}")]
        public ActionResult<Assignment> GetById([FromRoute] int id)
        {
            Assignment? assignment = _context.Assignments.SingleOrDefault(a => a.Id == id);

            if (assignment == null)
                return NotFound("Přiřazení nenalezeno.");

            return Ok(assignment);
        }

        // POST api/assignment
        [HttpPost]
        public ActionResult<Assignment> Create([FromBody] Assignment assignment)
        {
            // zkontrolujeme jestli pocitac existuje
            if (!_context.Computers.Any(c => c.Uuid == assignment.ComputerUuid))
                return NotFound("Počítač s tímto UUID neexistuje.");

            // zkontrolujeme jestli job existuje
            if (!_context.Jobs.Any(j => j.Id == assignment.JobId))
                return NotFound("Úloha s tímto ID neexistuje.");

            // stejny job nesmi byt prirazen stejnemu pocitaci dvakrat
            bool duplicateExists = _context.Assignments.Any(a =>
                a.ComputerUuid == assignment.ComputerUuid && a.JobId == assignment.JobId);

            if (duplicateExists)
                return Conflict("Tato úloha je tomuto počítači už přiřazena.");

            assignment.AssignAt = DateTime.Now;

            _context.Assignments.Add(assignment);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = assignment.Id }, assignment);
        }

        // DELETE api/assignment/{id}
        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            Assignment? assignment = _context.Assignments.SingleOrDefault(a => a.Id == id);

            if (assignment == null)
                return NotFound("Přiřazení nenalezeno.");

            _context.Assignments.Remove(assignment);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
