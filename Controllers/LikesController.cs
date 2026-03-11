using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BloggingApi.Models;
using BloggingApi.Services;

namespace BloggingApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LikesController : ControllerBase
    {
        private readonly ILikeService _likeService;

        public LikesController(ILikeService likeService)
        {
            _likeService = likeService;
        }

        // GET: api/likes/post/{postId}
        [HttpGet("post/{postId}")]
        public async Task<IActionResult> GetLikesForPost(int postId)
        {
            var likes = await _likeService.GetLikesForPostAsync(postId);
            return Ok(likes);
        }

        // POST: api/likes
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> LikePost([FromBody] Like like)
        {
            var result = await _likeService.LikePostAsync(like);
            if (result == null) return BadRequest("Already liked");
            return Ok(result);
        }

        // DELETE: api/likes/{id}
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> UnlikePost(int id)
        {
            var success = await _likeService.UnlikePostAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
