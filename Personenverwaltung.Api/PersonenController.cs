using System.Threading.Tasks;
using System.Web.Http;
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
    }
}