using BloggingApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BloggingApi.Services
{
    public class LikeService : ILikeService
    {
        private readonly BloggingDbContext _context;
        public LikeService(BloggingDbContext context)
        {
            _context = context;
        }

        public async Task<List<Like>> GetLikesForPostAsync(int postId)
        {
            return await _context.Likes.Include(l => l.User).Where(l => l.PostId == postId).ToListAsync();
        }

        public async Task<Like> LikePostAsync(Like like)
        {
            var exists = await _context.Likes.AnyAsync(l => l.UserId == like.UserId && l.PostId == like.PostId);
            if (exists) return null;
            _context.Likes.Add(like);
            await _context.SaveChangesAsync();
            return like;
        }

        public async Task<bool> UnlikePostAsync(int likeId)
        {
            var like = await _context.Likes.FindAsync(likeId);
            if (like == null) return false;
            _context.Likes.Remove(like);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}