using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
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
            var munkalapok = await _context.munkalap
                .Include(m => m.hely)
                .Include(m => m.szerelo)
                .ToListAsync();

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

        [HttpGet("{munkalapId:int}")]
        public async Task<ActionResult<IEnumerable<MunkalapDTO>>> Get(int munkalapId)
        {
            var munkalap = await _context.munkalap
                .Include(m => m.hely)
                .Include(m => m.szerelo)
                .FirstOrDefaultAsync(m => m.id == munkalapId);

            if (munkalap is null)
            {
                return NotFound();
            }

            var result = new MunkalapDTO
            {
                Id = munkalap.id,
                Anyagar = munkalap.anyagar,
                beadas_datum = munkalap.beadas_datum,
                javitas_datum = munkalap.javitas_datum,
                Munkaora = munkalap.munkaora,
                Szerelo = munkalap.szerelo.nev,
                Helyszin = $"{munkalap.hely.telepules}, {munkalap.hely.utca}"
            };

            return Ok(result);
        }

        [HttpPost("{evszam:int?}")]
        public async Task<ActionResult<IEnumerable<MunkalapDTO>>> Search(MunkalapKeresesDto request, int? evszam)
        {
            var munkalapQuery = _context.munkalap.AsQueryable();

            if (evszam is not null)
            {
                munkalapQuery = munkalapQuery.Where(m => m.javitas_datum >= new DateTime((int)evszam, 1, 1));
            }

            var munkalap = await munkalapQuery
                .Include(m => m.hely)
                .Include(m => m.szerelo)
                .FirstOrDefaultAsync(m => m.hely_id == request.helyszin_azonosito && m.szerelo_id == request.szerelo_azonosito);

            if (munkalap is null)
            {
                return NotFound();
            }

            var result = new MunkalapDTO
            {
                Id = munkalap.id,
                Anyagar = munkalap.anyagar,
                beadas_datum = munkalap.beadas_datum,
                javitas_datum = munkalap.javitas_datum,
                Munkaora = munkalap.munkaora,
                Szerelo = munkalap.szerelo.nev,
                Helyszin = $"{munkalap.hely.telepules}, {munkalap.hely.utca}"
            };

            return Ok(result);
        }
    }
}
