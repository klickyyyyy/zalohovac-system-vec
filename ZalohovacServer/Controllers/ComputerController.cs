using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZalohovacServer.Database;
using ZalohovacServer.Entities.DB;

namespace ZalohovacServer.Controllers
{
    [Route("api/computer")]
    [ApiController]
    [Authorize]
    public class ComputerController : ControllerBase
    {
        private DatabaseContext _context;

        public ComputerController(DatabaseContext context)
        {
            _context = context;
        }

        // GET api/computer
        [HttpGet]
        public ActionResult<List<Computer>> GetAll()
        {
            List<Computer> computers = _context.Computers.ToList();
            return Ok(computers);
        }

        // GET api/computer/{uuid}
        [HttpGet("{uuid}")]
        public ActionResult<Computer> GetByUuid([FromRoute] Guid uuid)
        {
            Computer? computer = _context.Computers.SingleOrDefault(c => c.Uuid == uuid);

            if (computer == null)
                return NotFound("Počítač nenalezen.");

            return Ok(computer);
        }

        // POST api/computer
        [HttpPost]
        public ActionResult<Computer> Create([FromBody] Computer computer)
        {
            computer.Uuid = Guid.NewGuid();

            if (string.IsNullOrWhiteSpace(computer.Name))
                return BadRequest("Název počítače nesmí být prázdný.");

            if (_context.Computers.Any(c => c.Uuid == computer.Uuid))
                return Conflict("Počítač s tímto UUID už existuje.");

            _context.Computers.Add(computer);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetByUuid), new { uuid = computer.Uuid }, computer);
        }

        // PUT api/computer/{uuid}
        [HttpPut("{uuid}")]
        public ActionResult Update([FromRoute] Guid uuid, [FromBody] Computer computer)
        {
            Computer? existing = _context.Computers.SingleOrDefault(c => c.Uuid == uuid);

            if (existing == null)
                return NotFound("Počítač nenalezen.");

            existing.Name = computer.Name;
            existing.Enabled = computer.Enabled;

            _context.SaveChanges();

            return NoContent();
        }

        // DELETE api/computer/{uuid}
        [HttpDelete("{uuid}")]
        public ActionResult Delete([FromRoute] Guid uuid)
        {
            Computer? computer = _context.Computers.SingleOrDefault(c => c.Uuid == uuid);

            if (computer == null)
                return NotFound("Počítač nenalezen.");

            _context.Computers.Remove(computer);
            _context.SaveChanges();

            return NoContent();
        }
    }
}