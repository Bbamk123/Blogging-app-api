using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BloggingApi.Models;

namespace BloggingApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FollowsController : ControllerBase
    {
        private readonly BloggingDbContext _context;

        public FollowsController(BloggingDbContext context)
        {
            _context = context;
        }

        // GET: api/follows/user/{userId}
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetFollowers(int userId)
        {
            var followers = await _context.Follows.Include(f => f.Follower).Where(f => f.FollowingId == userId).ToListAsync();
            var following = await _context.Follows.Include(f => f.Following).Where(f => f.FollowerId == userId).ToListAsync();
            return Ok(new { followers, following });
        }

        // POST: api/follows
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> FollowUser([FromBody] Follow follow)
        {
            var exists = await _context.Follows.AnyAsync(f => f.FollowerId == follow.FollowerId && f.FollowingId == follow.FollowingId);
            if (exists) return BadRequest("Already following");
            _context.Follows.Add(follow);
            await _context.SaveChangesAsync();
            return Ok(follow);
        }

        // DELETE: api/follows/{id}
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> UnfollowUser(int id)
        {
            var follow = await _context.Follows.FindAsync(id);
            if (follow == null) return NotFound();
            _context.Follows.Remove(follow);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
