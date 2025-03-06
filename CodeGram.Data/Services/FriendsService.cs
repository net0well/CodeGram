using Azure.Core;
using CodeGram.Data.Dtos;
using CodeGram.Data.Helpers.Constants;
using CodeGram.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGram.Data.Services
{
    public class FriendsService : IFriendsService
    {
        private readonly AppDbContext _context;
        public FriendsService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<UserWithFriendsCountDto>> GetSuggestedFriendsAsync(int userId)
        {
            var existingFriendIds = await _context.Friendships
                .Where(n => n.SenderId == userId || n.ReceiverId == userId)
                .Select(n => n.SenderId == userId ? n.ReceiverId : n.SenderId)
                .ToListAsync();

            //pending requests
            var pendingRequestIds = await _context.FriendRequests
                .Where(n => (n.SenderId == userId || n.ReceiverId == userId) && n.Status == FriendshipStatus.Pending)
                .Select(n => n.SenderId == userId ? n.ReceiverId : n.SenderId)
                .ToListAsync();

            //get suggeted friends
            var suggestedFriends = await _context.Users
                .Where(n => n.Id != userId &&
                !existingFriendIds.Contains(n.Id) &&
                !pendingRequestIds.Contains(n.Id))
                .Select(n => new UserWithFriendsCountDto()
                {
                    User = n,
                    FriendsCount = _context.Friendships.Count(f => f.SenderId == n.Id || f.ReceiverId == n.Id),
                })
                .Take(5)
                .ToListAsync();

            return suggestedFriends;
        }

        public async Task RemoveFriendAsync(int friendshipId)
        {
            var friendship = await _context.Friendships.FirstOrDefaultAsync(n => n.Id == friendshipId);

            if(friendship != null)
            {
                _context.Friendships.Remove(friendship);
                await _context.SaveChangesAsync();
            }
        }

        public async Task SendRequestAsync(int senderId, int receiverId)
        {
            var request = new FriendRequest
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                Status = FriendshipStatus.Pending,
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now
            };

            _context.FriendRequests.Add(request);
            await _context.SaveChangesAsync();
        
        }

        public async Task UpdateRequestAsync(int requestId, string newStatus)
        {
            var requestDb = await _context.FriendRequests.FirstOrDefaultAsync(n => n.Id == requestId);

            if (requestDb != null)
            {
                requestDb.Status = newStatus;
                requestDb.DateUpdated = DateTime.Now;
                _context.Update(requestDb);
                await _context.SaveChangesAsync();
            }

            if(newStatus == FriendshipStatus.Accepted)
            {
                var friendship = new Friendship
                {
                    SenderId = requestDb.SenderId,
                    ReceiverId = requestDb.ReceiverId,
                    DateCreated = DateTime.Now
                };

                await _context.Friendships.AddAsync(friendship);
                await _context.SaveChangesAsync();
            }
        }
    }
}
