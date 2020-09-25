using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FriendsNetwork.Repository.Models;
using FriendsNetwork.Repository;

namespace FriendsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeopleController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public PeopleController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: api/People
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Person>>> GetPeople()
        {
            return await _context.People.Include(x => x.Country).Include(x => x.State).Include(x => x.Friendships).ToListAsync();
        }

        // GET: api/People/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Person>> GetPerson(Guid id)
        {
            var friend = await _context.People.Include(x => x.Country).Include(x => x.State).Include(x => x.Friendships).FirstOrDefaultAsync(x => x.Id == id);

            if (friend == null)
            {
                return NotFound();
            }

            return friend;
        }

        // GET: api/People/Person/5
        [HttpGet("Person/{id}")]
        public async Task<ActionResult<IEnumerable<Person>>> GetPeople(Guid id)
        {
            var friend = await _context.People.Include(x => x.Country).Include(x => x.State).Include(x => x.Friendships).Where(x => x.Id != id).ToListAsync();

            if (friend == null)
            {
                return NotFound();
            }

            return friend;
        }

        // PUT: api/People/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPerson(Guid id, Person person)
        {
            if (id != person.Id)
            {
                return BadRequest();
            }

            _context.Entry(person).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonExists(id))
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

        // POST: api/People
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Person>> PostPerson(Person person)
        {
            _context.People.Add(person);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPerson", new { id = person.Id }, person);
        }

        /* [HttpPut]
        public async Task<ActionResult<Friendship>> AddFriendshipToPerson(Guid Id, Friendship friendship)
        {
            var Person = await GetPerson(Id).Result;
            if (Person == null)
            {
                return NotFound();
            }

            Person.
        } */

        // DELETE: api/Friends/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Person>> DeletePerson(Guid id)
        {
            var friend = await _context.People.FindAsync(id);
            if (friend == null)
            {
                return NotFound();
            }

            _context.People.Remove(friend);
            await _context.SaveChangesAsync();

            return friend;
        }

        private bool PersonExists(Guid id)
        {
            return _context.People.Any(e => e.Id == id);
        }
    }
}
