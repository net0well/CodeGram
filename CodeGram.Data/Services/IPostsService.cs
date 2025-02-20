using CodeGram.Data.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGram.Data.Services
{
    public interface IPostsService
    {

        Task<List<Post>> GetAllPostsAsync(int loggedInUserId);
        Task<Post> CreatePostAsync(Post post, IFormFile Image);
        Task<Post> RemovePostAsync(int postId);
        Task AddPostCommentAsync(Comment comment);
        Task RemovePostCommentAsync(int commentId);
        Task TogglePostLikeAsync(int postId, int loggedInUserId);
        Task TogglePostFavoriteAsync(int postId, int loggedInUserId);
        Task ReportPostAsync(int postId, int loggedInUserId);
        Task TogglePostVisibilityAsync(int postId, int loggedInUserId);  
    }
}
