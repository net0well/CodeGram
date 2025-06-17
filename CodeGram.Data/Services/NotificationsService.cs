﻿using CodeGram.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using CodeGram.Data.Hubs;

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
        public async Task AddNewNotificationAsync(int userId, string message, string notificationType)
        {
            var newNotification = new Notification
            {
                UserId = userId,
                Message = message,
                Type = notificationType,
                IsRead = false,
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now
            };

            await _context.Notifications.AddAsync(newNotification);
            await _context.SaveChangesAsync();

            var notificationNumber = await GetUnreadNotificationsCountAsync(userId);

            await _hubContext.Clients.User(userId.ToString())
                .SendAsync("ReceiveNotification", notificationNumber);
        }

        public async Task<int> GetUnreadNotificationsCountAsync(int userId)
        {
            var count = await _context.Notifications
                .Where(n => n.UserId == userId && !n.IsRead)
                .CountAsync();

            return count;

        }
    }
}
