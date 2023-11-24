using SocialMedia.Models;

namespace SocialMedia.Contracts
{
    public interface IChatRepository
    {
        Task<Chat>CreateNewMessage(Chat chat);
        Task<List<Chat>> GetAllChats(string userId,string otherPersonId);
    }
}
