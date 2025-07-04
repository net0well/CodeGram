﻿using CodeGram.Data.Dtos;
using CodeGram.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGram.Data.Services
{
    public class PostsService : IPostsService
    {
        private readonly AppDbContext _context;
        private readonly INotificationsService _notificationsService;
        public PostsService(AppDbContext context, INotificationsService notificationsService)
        {
           _context = context;
           _notificationsService = notificationsService;
        }

        public async Task<List<Post>> GetAllPostsAsync(int loggedInUserId)
        {
            var allPosts = await _context.Posts
                .Where(n => (!n.IsPrivate || n.UserId == loggedInUserId) && n.Reports.Count < 5 && !n.IsDeleted)
                .Include(n => n.User)
                .Include(n => n.Likes)
                .Include(n => n.Favorites)
                .Include(n => n.Comments).ThenInclude(n => n.User)
                .Include(n => n.Reports)
                .OrderByDescending(n => n.DateCreated)
                .ToListAsync();

            return allPosts;
        }

        public async Task<Post> CreatePostAsync(Post post)
        {

            await _context.Posts.AddAsync(post);
            await _context.SaveChangesAsync();

            return post;

        }

        public async Task<Post> RemovePostAsync(int postId)
        {
            var postDb = await _context.Posts.FirstOrDefaultAsync(p => p.Id == postId);

            if (postDb != null)
            {
                /* _context.Posts.Remove(postDb);*/
                postDb.IsDeleted = true;
                _context.Posts.Update(postDb);
                await _context.SaveChangesAsync();
            }

            return postDb;
        }

        public async Task AddPostCommentAsync(Comment comment)
        {
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
        }

        public async Task RemovePostCommentAsync(int commentId)
        {
            var commentDb = _context.Comments.FirstOrDefault(p => p.Id == commentId);

            if(commentDb != null)
            {
                _context.Comments.Remove(commentDb);
                await _context.SaveChangesAsync();
            }

        }

        public async Task ReportPostAsync(int postId, int loggedInUserId)
        {
            var newReport = new Report()
            {
                PostId = postId,
                UserId = loggedInUserId,
                DateCreated = DateTime.UtcNow
            };


            await _context.Reports.AddAsync(newReport);
            await _context.SaveChangesAsync();

            var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == postId);

            if (post != null)
            {
                post.NrOfReports += 1;
                _context.Posts.Update(post);
                await _context.SaveChangesAsync();

            }
        }

        public async Task<GetNotificationDto> TogglePostFavoriteAsync(int postId, int loggedInUserId)
        {

            var response = new GetNotificationDto()
            {
                Success = true,
                SendNotification = false
            };

            //check if user has already favorited the post
            var favorite = await _context.Favorites
                .Where(l => l.PostId == postId && l.UserId == loggedInUserId)
                .FirstOrDefaultAsync();

            if (favorite != null)
            {
                _context.Favorites.Remove(favorite);
                await _context.SaveChangesAsync();
            }
            else
            {
                var newFavorite = new Favorite()
                {
                    PostId = postId,
                    UserId = loggedInUserId,
                    DateCreated = DateTime.UtcNow
                };

                await _context.Favorites.AddAsync(newFavorite);
                await _context.SaveChangesAsync();

                response.SendNotification = true;
            }

            return response;
        }

        public async Task<GetNotificationDto> TogglePostLikeAsync(int postId, int loggedInUserId)
        {
            var response = new GetNotificationDto()
            {
                Success = true,
                SendNotification = false
            };

            //check if user has already liked the post
            var like = await _context.Likes
                .Where(l => l.PostId == postId && l.UserId == loggedInUserId)
                .FirstOrDefaultAsync();

            if (like != null)
            {
                _context.Likes.Remove(like);
                await _context.SaveChangesAsync();
            }
            else
            {
                var newLike = new Like()
                {
                    PostId = postId,
                    UserId = loggedInUserId
                };
                await _context.Likes.AddAsync(newLike);
                await _context.SaveChangesAsync();

                response.SendNotification = true;
            }
            return response;
        }

        public async Task TogglePostVisibilityAsync(int postId, int loggedInUserId)
        {
            //get post by id and loggedin user id
            var post = await _context.Posts
                .FirstOrDefaultAsync(l => l.Id == postId && l.UserId == loggedInUserId);

            if (post != null)
            {
                post.IsPrivate = !post.IsPrivate;
                _context.Posts.Update(post);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Post>> GetAllFavoritedPostAsync(int userId)
        {
            var allFavoritedPosts = await _context.Favorites
                .Include(f => f.Post.Reports)
                .Include(f => f.Post.User)
                .Include(f => f.Post.Comments)
                    .ThenInclude(c => c.User)
                .Include(f => f.Post.Likes)
                .Include(f => f.Post.Favorites)
                .Where(n => n.UserId == userId &&
                    !n.Post.IsDeleted &&
                    n.Post.Reports.Count < 5)
                .OrderByDescending(f => f.DateCreated)
                .Select(n => n.Post)
                .ToListAsync();

            return allFavoritedPosts;
        }

        public async Task<Post> GetPostByIdAsync(int postId)
        {
            var postDb = await _context.Posts
                .Include(n => n.User)
                .Include(n => n.Likes)
                .Include(n => n.Favorites)
                .Include(n => n.Comments).ThenInclude(n => n.User)
                .FirstOrDefaultAsync(n => n.Id == postId);
            
            return postDb;
        }
    }
}
