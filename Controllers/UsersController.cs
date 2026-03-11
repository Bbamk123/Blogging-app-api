using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BloggingApi.Models;

namespace BloggingApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly BloggingDbContext _context;

        public UsersController(BloggingDbContext context)
        {
            _context = context;
        }

        // GET: api/users/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserProfile(int id)
        {
            var user = await _context.Users.Include(u => u.Posts).Include(u => u.Followers).Include(u => u.Following).FirstOrDefaultAsync(u => u.Id == id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        // PUT: api/users/{id}
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUserProfile(int id, [FromBody] User user)
        {
            var existing = await _context.Users.FindAsync(id);
            if (existing == null) return NotFound();
            existing.Bio = user.Bio;
            existing.AvatarUrl = user.AvatarUrl;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // GET: api/users/{id}/feed
        [Authorize]
        [HttpGet("{id}/feed")]
        public async Task<IActionResult> GetUserFeed(int id)
        {
            var followingIds = await _context.Follows.Where(f => f.FollowerId == id).Select(f => f.FollowingId).ToListAsync();
            var feedPosts = await _context.Posts.Include(p => p.User)
                .Where(p => followingIds.Contains(p.UserId) || p.UserId == id)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
            return Ok(feedPosts);
        }
    }
}
