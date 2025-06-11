﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGram.Data.Services
{
    public interface INotificationsService
    {
        Task AddNewNotificationAsync(int userId, string message, string notificationType);
        Task<int> GetUnreadNotificationsCountAsync(int userId);
    }
}
