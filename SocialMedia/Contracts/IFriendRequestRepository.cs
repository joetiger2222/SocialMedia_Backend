using SocialMedia.Models;

namespace SocialMedia.Contracts
{
    public interface IFriendRequestRepository
    {
        Task<FriendRequest>AddFriend(FriendRequest friendRequest);
        Task<List<FriendRequest>> GetAllFriendsRequests(string recieverId);
        Task<bool> AcceptFriendRequest(string userId, string otherPersonId);
        Task<bool> RejectFriendRequest(string userId, string otherPersonId);
        Task<List<Friends>> GetUserFriends(string userId);
        Task<bool> CheckFriendsOrNot(string userId, string OtherPersonId);
        Task<string> CheckFriendRequestSenderOrRecieverOrNo(string userId, string otherPersonId);
    }
}
