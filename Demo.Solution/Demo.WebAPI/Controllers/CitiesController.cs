using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Demo.WebAPI.DatabaseContext;
using Demo.WebAPI.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Demo.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CitiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Cities
        [HttpGet]
        public async Task<ActionResult<IEnumerable<City>>> GetCities()
        {
            //return await _context.Cities.ToListAsync();
            var cities = await _context.Cities
                .OrderBy(c => c.CityName).ToListAsync();

            return cities;
        }

        // GET: api/Cities/5
        [HttpGet("{id}")]
        public async Task<ActionResult<City>> GetCity(Guid cityID)
        {
            //var city = await _context.Cities.FindAsync(id);

            //if (city == null)
            //{
            //    return NotFound();
            //}

            //return city;

            var city = await _context.Cities.FirstOrDefaultAsync(c => c.CityID == cityID);
            if (city == null)
            {
                return NotFound();
            }

            return city;
        }

        // PUT: api/Cities/5
        [HttpPut("{cityID}")]
        public async Task<IActionResult> PutCity(Guid cityID, City city)
        {
            if (cityID != city.CityID)
            {
                return BadRequest(); //HTTP 400 
            }

            //_context.Entry(city).State = EntityState.Modified;

            var existingCity = await _context.Cities.FindAsync(cityID);
            if (existingCity == null)
            {
                return NotFound(); //HTTP 404
            }

            existingCity.CityName = city.CityName;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CityExists(cityID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Cities
        [HttpPost]
        public async Task<ActionResult<City>> PostCity(City city)
        {
            //if(ModelState.IsValid == false)
            //{
            //    return ValidationProblem(ModelState);
            //}

            if(_context.Cities == null)
            {
                return Problem("Entity set 'AplicationDBContext.Cities' is null");
            }

            _context.Cities.Add(city);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCity", new { id = city.CityID }, city);
        }

        // DELETE: api/Cities/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCity(Guid id)
        {
            var city = await _context.Cities.FindAsync(id);
            if (city == null)
            {
                return NotFound();
            }

            _context.Cities.Remove(city);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CityExists(Guid id)
        {
            return _context.Cities.Any(e => e.CityID == id);
        }
    }
}
