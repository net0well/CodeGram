﻿using CodeGram.Data.Dtos;
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
        Task<Post> GetPostByIdAsync(int postId);
        Task<List<Post>> GetAllFavoritedPostAsync(int userId);
        Task<Post> CreatePostAsync(Post post);
        Task<Post> RemovePostAsync(int postId);
        Task AddPostCommentAsync(Comment comment);
        Task RemovePostCommentAsync(int commentId);
        Task<GetNotificationDto> TogglePostLikeAsync(int postId, int loggedInUserId);
        Task<GetNotificationDto> TogglePostFavoriteAsync(int postId, int loggedInUserId);
        Task ReportPostAsync(int postId, int loggedInUserId);
        Task TogglePostVisibilityAsync(int postId, int loggedInUserId);  
    }
}
