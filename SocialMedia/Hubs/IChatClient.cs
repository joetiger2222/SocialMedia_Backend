using SocialMedia.DTO;

namespace SocialMedia.Hubs
{
    public interface IChatClient
    {
        Task ReceiveMessage(MessageViewModel message);
        
    }
}
