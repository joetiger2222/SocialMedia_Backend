using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Contracts;
using SocialMedia.DTO;
using SocialMedia.Models;
using SocialMedia.Repositories;

namespace SocialMedia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly ICommentRepository commentRepository;

        public CommentController(IMapper mapper,ICommentRepository commentRepository)
        {
            this.mapper = mapper;
            this.commentRepository = commentRepository;
        }

        [HttpPost]
        [Route("{PostId}")]
        public async Task<IActionResult> CreateNewComment([FromRoute]Guid PostId,[FromForm] CreateCommentDto createCommentDto)
        {
            if (createCommentDto.File != null)
            {
                var commentModel = new Comment()
                {
                    Text = createCommentDto.Text,
                    UserId = createCommentDto.UserId,
                    File = createCommentDto.File,
                    FileExtention = Path.GetExtension(createCommentDto.File.FileName),
                    FileName = createCommentDto.FileName,
                    PostId = createCommentDto.PostId,

                };
                var commentToReturn = await commentRepository.CreateNewComment(PostId, commentModel);
                return Ok(commentToReturn);
            }
            else
            {
                var commentModel = new Comment()
                {
                    Text = createCommentDto.Text,
                    UserId = createCommentDto.UserId,
                    PostId=createCommentDto.PostId,
                };
                var commentToReturn = await commentRepository.CreateNewComment(PostId,commentModel);
                return Ok(commentToReturn);
            }
            
        }

        [HttpGet]
        [Route("{PostId}")]
        public async Task<IActionResult> GetAllComments([FromRoute]Guid PostId)
        {
            var comments = await commentRepository.GetAllComments(PostId);
            return Ok(comments);
        }

        [HttpPut]
        [Route("{CommentId}")]
        public async Task<IActionResult> EditComment([FromRoute]Guid CommentId, [FromBody]EditCommentDto editCommentDto)
        {
            var edited=await commentRepository.EditComment(CommentId, mapper.Map<Comment>(editCommentDto));
            if(edited is null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<CommentDto>(edited));

        }

        [HttpDelete]
        [Route("{CommentId}")]
        public async Task<IActionResult> DeleteComment([FromRoute]Guid CommentId)
        {
            var deleted=await commentRepository.DeleteComment(CommentId);
            if(deleted is null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<CommentDto>(deleted));
        }
    }
}
