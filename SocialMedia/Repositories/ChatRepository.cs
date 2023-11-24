using Microsoft.EntityFrameworkCore;
using SocialMedia.Contracts;
using SocialMedia.Data;
using SocialMedia.Models;

namespace SocialMedia.Repositories
{
    public class ChatRepository : IChatRepository
    {
        private readonly DataDbContext dbContext;

        public ChatRepository(DataDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<Chat> CreateNewMessage(Chat chat)
        {
            await dbContext.AddAsync(chat);
            await dbContext.SaveChangesAsync();
            return chat;
        }

        public async Task<List<Chat>> GetAllChats(string userId, string otherPersonId)
        {
           var messages=await dbContext.Chats.Where(x=>(x.SenderId==userId&&x.RecieverId==otherPersonId)||(x.SenderId==otherPersonId&&x.RecieverId==userId)).ToListAsync();
            return messages;
        }
    }
}
