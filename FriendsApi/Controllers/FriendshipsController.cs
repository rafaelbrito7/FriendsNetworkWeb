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
    public class FriendshipsController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public FriendshipsController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: api/Friendships
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Friendship>>> GetFriendship()
        {
            return await _context.Friendship.Include(x => x.PersonOrFriend).ToListAsync();
        }

        // GET: api/Friendships/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Friendship>>> GetFriendship(Guid id)
        {
            var friendShip = await _context.Friendship.Where(x => x.PersonId == id).ToListAsync();

            if (friendShip == null)
            {
                return NotFound();
            }

            List<Friendship> friendships = new List<Friendship>();

            foreach (Friendship u in friendships)
            {
                try
                {
                    Person person = await _context.People.Include(x => x.Country).Include(x => x.State).FirstOrDefaultAsync(x => x.Id == u.FriendId);
                    friendships.Add(new Friendship
                    {
                        PersonId = u.PersonId,
                        FriendId = u.FriendId,
                        PersonOrFriend = person
                    });
                }
                catch (Exception e) {
                    Console.WriteLine(e);
                }
            }

            return friendships;
        }

        // PUT: api/Friendships/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFriendship(Guid id, Friendship friendShip)
        {
            if (id != friendShip.PersonId)
            {
                return BadRequest();
            }

            _context.Entry(friendShip).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FriendshipExists(id))
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

        // POST: api/Friendships
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Friendship>> PostFriendship(Friendship friendship)
        {
            var friendship2 = new Friendship
            {
                PersonId = friendship.FriendId,
                PersonOrFriend = friendship.PersonOrFriend,
                FriendId = friendship.PersonId
            };

            _context.Friendship.Add(friendship);
            _context.Friendship.Add(friendship2);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (FriendshipExists(friendship.PersonId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetFriendship", new { id = friendship.PersonId }, friendship);
        }

        // DELETE: api/Friendships/5
        [HttpDelete("{id}/{id2}")]
        public async Task<ActionResult<Friendship>> DeleteFriendship(Guid id, Guid id2)
        {
            var friendShip = await _context.Friendship.FirstOrDefaultAsync(x => x.PersonId == id && x.FriendId == id2);
            var friendShip1 = await _context.Friendship.FirstOrDefaultAsync(x => x.FriendId == id && x.PersonId == id2);
            if (friendShip == null)
            {
                return NotFound();
            }

            _context.Friendship.Remove(friendShip);
            _context.Friendship.Remove(friendShip1);
            await _context.SaveChangesAsync();

            return friendShip;
        }

        private bool FriendshipExists(Guid id)
        {
            return _context.Friendship.Any(e => e.PersonId == id);
        }
    }
}
