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
    public class PostController : ControllerBase
    {
        private readonly IPostRepository postRepository;
        private readonly IMapper mapper;

        public PostController(IPostRepository postRepository,IMapper mapper)
        {
            this.postRepository = postRepository;
            this.mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult>CreateNewPost([FromForm]CreatePostDto createPostDto)
        {
            //var created=await postRepository.CreateNewPost(mapper.Map<Post>(createPostDto));
            //return Ok(created);
            if (createPostDto.File != null)
            {
                var postModel = new Post()
                {
                    Text = createPostDto.Text,
                    UserId = createPostDto.UserId,
                    File = createPostDto.File,
                    FileExtention = Path.GetExtension(createPostDto.File.FileName),
                    FileName = createPostDto.FileName

                };
                await postRepository.CreateNewPost(postModel);
                return Ok(postModel);
            }
            else
            {
                var postModel = new Post()
                {
                    Text = createPostDto.Text,
                    UserId = createPostDto.UserId,
                };
                await postRepository.CreateNewPost(postModel);
                return Ok(postModel);
            }
            
            

        }

        [HttpGet]
        [Route("{userId}/{profileId}")]
        public async Task<IActionResult>GetUserPosts([FromRoute]string userId, [FromRoute]string profileId)
        {
            var posts=await postRepository.GetUserPosts(userId,profileId);
            return Ok(mapper.Map<List<PostDto>>(posts));
        }

        [HttpPut]
        [Route("PostId")]
        public async Task<IActionResult>EditPost([FromRoute]Guid PostId, EditPostDto editPostDto)
        {
            var edited = await postRepository.EditPost(PostId, mapper.Map<Post>(editPostDto));
            if(edited is null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<PostDto>(edited));
        }

        [HttpGet]
        [Route("UserFeed/{userId}")]
        public async Task<IActionResult> GetUserFeed([FromRoute]string userId)
        {
            var posts= await postRepository.GetUserFeed(userId);
            return Ok(posts);
        }

        [HttpDelete]
        [Route("{PostId}")]
        public async Task<IActionResult> DeletePost([FromRoute]Guid PostId)
        {
            var deletedPost=await postRepository.DeletePost(PostId);
            if(deletedPost is null)
            {
                return NotFound(); 
            }
            return Ok(mapper.Map<PostDto>(deletedPost));
        }

        [HttpPut]
        [Route("AddOrRemoveLike")]
        public async Task<IActionResult> AddOrRemoveLike(AddLikeDto addLikeDto)
        {
            var like=await postRepository.AddOrRemoveLikeToPost(mapper.Map<PostLikes>(addLikeDto));
            return Ok(like);
        }
    }
}
