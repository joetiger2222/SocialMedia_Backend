using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Contracts;
using SocialMedia.DTO;
using SocialMedia.Models;

namespace SocialMedia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FriendRequestController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IFriendRequestRepository friendRequestRepository;

        public FriendRequestController(IMapper mapper, IFriendRequestRepository friendRequestRepository)
        {
            this.mapper = mapper;
            this.friendRequestRepository = friendRequestRepository;
        }
        [HttpPost]
        public async Task<IActionResult> SendFriendReques([FromBody] SendFriendRequestDto sendFriendRequestDto)
        {
            var friendRequest = await friendRequestRepository.AddFriend(mapper.Map<FriendRequest>(sendFriendRequestDto));
            if (friendRequest == null)
            {
                return BadRequest("Aleary Sent A Request");
            }
            return Ok(friendRequest);
        }

        [HttpGet]
        [Route("{recieverId}")]
        public async Task<IActionResult> GetAllFriendRequests([FromRoute] string recieverId)
        {
            var requests = await friendRequestRepository.GetAllFriendsRequests(recieverId);

            return Ok(requests);
        }

        [HttpPost]
        [Route("AcceptFriendRequest/{userId}/{otherPersonId}")]
        public async Task<IActionResult> AcceptFriendReques([FromRoute] string userId, [FromRoute] string otherPersonId)
        {
            var result=await friendRequestRepository.AcceptFriendRequest(userId,otherPersonId);
            if (result == false)
            {
                return BadRequest();
            }
            return Ok(new {message="Friend Added Successfully"});
        }

        [HttpDelete]
        [Route("RejectOrCancelFriendRequest/{userId}/{otherPersonId}")]
        public async Task<IActionResult> RejectFriendReques([FromRoute] string userId, [FromRoute] string otherPersonId)
        {
            var result = await friendRequestRepository.RejectFriendRequest(userId, otherPersonId);
            if (result == false)
            {
                return BadRequest();
            }
            return Ok(new { message = "Friend Request Removed Successfully" });
        }

        [HttpGet]
        [Route("Friends/{userId}")]
        public async Task<IActionResult> GetUserFriends([FromRoute]string userId)
        {
            var users=await friendRequestRepository.GetUserFriends(userId);
            return Ok(users);
        }
        [HttpGet]
        [Route("/IsFriends/{userId}/{otherPersonId}")]
        public async Task<IActionResult> CheckFriendsOrNot([FromRoute]string userId, [FromRoute]string otherPersonId)
        {
            var isFriends=await friendRequestRepository.CheckFriendsOrNot(userId, otherPersonId);
            return Ok(new {isFriends=isFriends});
        }

        [HttpGet]
        [Route("/IsFriendReques/{userId}/{otherPersonId}")]
        public async Task<IActionResult> CheckFriendRequest([FromRoute]string userId, [FromRoute]string otherPersonId)
        {
            var result=await friendRequestRepository.CheckFriendRequestSenderOrRecieverOrNo(userId, otherPersonId);
            return Ok(new {isFriendReques=result});
        }
    }
}
