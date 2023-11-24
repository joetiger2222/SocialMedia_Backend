using Microsoft.EntityFrameworkCore;
using SocialMedia.Contracts;
using SocialMedia.Data;
using SocialMedia.Models;

namespace SocialMedia.Repositories
{
    public class FriendRequestRepository : IFriendRequestRepository
    {
        private readonly DataDbContext dbContext;

        public FriendRequestRepository(DataDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<bool> AcceptFriendRequest(string userId,string otherPersonId)
        {
            var request = await dbContext.FriendRequests.FirstOrDefaultAsync(x =>x.RecieverId==userId && x.SenderId==otherPersonId);
            if (request is null)
            {
                return false;
            }
            var friend = new Friends()
            {
                FirstId = request.SenderId,
                SecondId = request.RecieverId
            };
            await dbContext.Friends.AddAsync(friend);
            await dbContext.SaveChangesAsync();
            dbContext.FriendRequests.Remove(request);
            await dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<FriendRequest> AddFriend(FriendRequest friendRequest)
        {
            var checkRequest = await dbContext.FriendRequests.FirstOrDefaultAsync(x => (friendRequest.SenderId == x.SenderId&&friendRequest.RecieverId==x.RecieverId) || (friendRequest.SenderId == x.RecieverId&&friendRequest.RecieverId==x.SenderId));
            var checkFriends = await dbContext.Friends.FirstOrDefaultAsync(x => (friendRequest.SenderId == x.FirstId && friendRequest.RecieverId == x.SecondId) || (friendRequest.SenderId == x.SecondId && friendRequest.RecieverId == x.FirstId));
            if (checkRequest == null&&checkFriends==null)
            {
                await dbContext.FriendRequests.AddAsync(friendRequest);
                await dbContext.SaveChangesAsync();
                return friendRequest;
            }
            return null;
        }

        public async Task<string> CheckFriendRequestSenderOrRecieverOrNo(string userId, string otherPersonId)
        {
            var isSender = dbContext.FriendRequests.Any(x => x.SenderId == userId && x.RecieverId == otherPersonId);
            if (isSender)
            {
                return "Sender";
            }
            var isReciever=dbContext.FriendRequests.Any(x=>x.RecieverId==userId&&x.SenderId==otherPersonId);
            if (isReciever)
            {
                return "Reciever";
            }
            return "No";
        }

        public async Task<bool> CheckFriendsOrNot(string userId, string OtherPersonId)
        {
            var isFriends=  dbContext.Friends.Any(x=>x.FirstId==userId&&x.SecondId==OtherPersonId);
            if (isFriends == false)
            {
                isFriends =  dbContext.Friends.Any(x => x.FirstId == OtherPersonId && x.SecondId == userId);
            }
            return isFriends;
        }

        public async Task<List<FriendRequest>> GetAllFriendsRequests(string recieverId)
        {
            
            var requests = await dbContext.FriendRequests.Include("User").Where(x => x.RecieverId == recieverId).ToListAsync();
            return requests;
        }

        public async Task<List<Friends>> GetUserFriends(string userId)
        {
            var friends = await dbContext.Friends.Where(x=>x.FirstId==userId||x.SecondId==userId).Include(x=>x.First).Include(x=>x.Second).ToListAsync();
            return friends;
        }

        public async Task<bool> RejectFriendRequest(string userId, string otherPersonId)
        {
            var request = await dbContext.FriendRequests.FirstOrDefaultAsync(x => (x.SenderId == userId && x.RecieverId == otherPersonId) || (x.RecieverId == userId && x.SenderId == otherPersonId));
            if (request is null)
            {
                return false;
            }
            dbContext.FriendRequests.Remove(request);
            await dbContext.SaveChangesAsync();
            return true;
        }
        //public async Task<List<FriendRequest>> GetAllFriendsRequests(string recieverId)
        //{
        //    var requests = await dbContext.FriendRequests
        //        .Where(x => x.RecieverId == recieverId)
        //        .Select(request => new FriendRequest
        //        {

        //            Id = request.Id,
        //            // Other FriendRequest properties...

        //            // Project specific User properties
        //            User = new User
        //            {
        //                Id = request.User.Id,
        //                UserName = request.User.UserName,
        //                FirstName= request.User.FirstName,
        //                LastName= request.User.LastName,
        //                // Add other User properties you want to include
        //            }
        //        })
        //        .ToListAsync();

        //    return requests;
        //}



    }
}
