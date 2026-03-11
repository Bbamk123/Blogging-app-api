using BloggingApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BloggingApi.Services
{
    public interface ILikeService
    {
        Task<List<Like>> GetLikesForPostAsync(int postId);
        Task<Like> LikePostAsync(Like like);
        Task<bool> UnlikePostAsync(int likeId);
    }
}