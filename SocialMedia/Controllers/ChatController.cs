using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SocialMedia.Contracts;
using SocialMedia.DTO;
using SocialMedia.Hubs;
using SocialMedia.Models;

namespace SocialMedia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IHubContext<ChatHub, IChatClient> _chatHub;
        private readonly IChatRepository chatRepository;
        private readonly IMapper mapper;

        public ChatController(IHubContext<ChatHub, IChatClient> chatHub,IChatRepository chatRepository,IMapper mapper)
        {
            _chatHub= chatHub;
            this.chatRepository = chatRepository;
            this.mapper = mapper;
        }
        [HttpPost]
        public async Task<IActionResult> SendPrivateMessage([FromBody]MessageViewModel messageViewModel)
        {
            var sentMessage = await chatRepository.CreateNewMessage(mapper.Map<Chat>(messageViewModel));
            await _chatHub.Clients.All.ReceiveMessage(messageViewModel);
            return Ok(messageViewModel);
        }

        [HttpGet]
        [Route("{userId}/{otherPersonId}")]
        public async Task<IActionResult> GetMessages([FromRoute]string userId, [FromRoute]string otherPersonId)
        {
            var messages=await chatRepository.GetAllChats(userId, otherPersonId);
            return Ok(messages);
        }

    }
}
