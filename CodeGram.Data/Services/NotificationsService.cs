using CodeGram.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using CodeGram.Data.Hubs;
using CodeGram.Data.Helpers.Constants;

namespace CodeGram.Data.Services
{
    public class NotificationsService : INotificationsService
    {
        private readonly AppDbContext _context;
        private readonly IHubContext<NotificationHub> _hubContext;
        public NotificationsService(AppDbContext context, IHubContext<NotificationHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }
        public async Task AddNewNotificationAsync(int userId, string notificationType, string userFullName, int? postId)
        {
            var newNotification = new Notification
            {
                UserId = userId,
                Message = GetPostMessage(notificationType, userFullName),
                Type = notificationType,
                IsRead = false,
                PostId = postId,
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now
            };

            await _context.Notifications.AddAsync(newNotification);
            await _context.SaveChangesAsync();

            var notificationNumber = await GetUnreadNotificationsCountAsync(userId);

            await _hubContext.Clients.User(userId.ToString())
                .SendAsync("ReceiveNotification", notificationNumber);
        }

        public async Task<List<Notification>> GetNotifications(int userId)
        {
            var allNotifications = await _context.Notifications
                .Where(n => n.UserId == userId)
                .OrderBy(n => n.IsRead)
                .ThenByDescending(n => n.DateCreated)
                .ToListAsync();

            return allNotifications;
        }

        public async Task<int> GetUnreadNotificationsCountAsync(int userId)
        {
            var count = await _context.Notifications
                .Where(n => n.UserId == userId && !n.IsRead)
                .CountAsync();

            return count;

        }

        private string GetPostMessage(string notificationType, string userFullName)
        {
            var message = "";

            switch(notificationType)
            {
                case NotificationType.Like:
                    message = $"{userFullName} curtiu o seu post.";
                    break;

                case NotificationType.Favorite:
                    message = $"{userFullName} favoritou o seu post.";
                    break;

                case NotificationType.Comment:
                    message = $"{userFullName} comentou o seu post.";
                    break;

                case NotificationType.FriendRequest:
                    message = $"{userFullName} te adicionou como amigo.";
                    break;

                case NotificationType.FriendRequestApproved:
                    message = $"{userFullName} te aceitou como amigo.";
                    break;

                default:
                    message = "";
                    break;
            }

            return message;
        }
    }
}
