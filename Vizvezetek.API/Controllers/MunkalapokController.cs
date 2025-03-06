using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vizvezetek.API.DTOs;
using Vizvezetek.API.Models;

namespace Vizvezetek.API.Controllers
{
    [Route("api/Munkalapok")]
    [ApiController]
    public class MunkalapokController : ControllerBase
    {
        private readonly vizvezetekContext _context;

        public MunkalapokController(vizvezetekContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MunkalapDTO>>> GetAll()
        {
            var munkalapok = _context.munkalap
                .Include(m => m.hely)
                .Include(m => m.szerelo)
                .ToList();

            var result = munkalapok.Select(munkalap => new MunkalapDTO
            {
                Id = munkalap.id,
                Anyagar = munkalap.anyagar,
                beadas_datum = munkalap.beadas_datum,
                javitas_datum = munkalap.javitas_datum,
                Munkaora = munkalap.munkaora,
                Szerelo = munkalap.szerelo.nev,
                Helyszin = $"{munkalap.hely.telepules}, {munkalap.hely.utca}"
            });

            return Ok(result);
        }
    }
}
