using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Personenverwaltung.Data;
using Personenverwaltung.Logic;

namespace Personenverwaltung.Api
{
    [RoutePrefix("api/Personen")]
    public class PersonenController : ApiController
    {
        private readonly PersonService _personService = new PersonService();

        [HttpGet, Route("")]
        public async Task<IHttpActionResult> Get(string name = null)
        {
            var personen = await _personService.SuchenAsync(name);
            return Ok(personen);
        }

        [HttpGet, Route("{id:int}")]
        public async Task<IHttpActionResult> GetById(int id)
        {
            var person = await _personService.GetDetailAsync(id);
            
            if (person == null) return NotFound();
            return Ok(person);
        }

        [HttpPut, Route("{id:int}")]
        public async Task<IHttpActionResult> Put(int id, [FromBody] PersonChangeDto personChangeDto)
        {
            if (personChangeDto == null) return BadRequest("Es wurden keine Daten übermittelt");

            try
            {
                bool found = await _personService.ChangeNameAsync(id, personChangeDto.Name, personChangeDto.Vorname);
                
                if (!found) return NotFound();

                return StatusCode(HttpStatusCode.NoContent);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}