using Microservices.Answers.Entities.Models;
using Microservices.Answers.Models.InputModels;
using Microservices.Answers.Services;
using Microservices.Models.AnswerModels;
using Microservices.Models.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Answers.Controllers
{
    [Route("api/microservices/[controller]")]
    [ApiController]
    public class AnswersController : ControllerBase
    {
        private readonly AnswerService _answerService;

        public AnswersController(AnswerService answerService)
        {
            _answerService = answerService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            Answer result = await _answerService.Get(id);

            if(result != null)
            {
                return Ok(new GetAnswerModel
                {
                    Text = result.Text,
                    PostId = result.PostId,
                    AnswerId = result.AnswerId,
                    AuthorId = result.AuthorId,
                    LastEditedAt = result.LastEditedAt,
                    PublishedAt = result.PublishedAt
                });
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateInputModel input)
        {
            Status result = await _answerService.Create(input.Text, DateTime.UtcNow,
                                                          input.AuthorId, input.AuthorName,
                                                          input.PostId, input.AnswerId);

            if(result == Status.Ok)
            {
                return Ok(new Response
                {
                    Status = result,
                    Error = ""
                });
            }
            else if(result == Status.InvalidData)
            {
                return BadRequest(new Response
                {
                    Status = result,
                    Error = ""
                });
            }
            else
            {
                return StatusCode(500, new Response
                {
                    Status = result,
                    Error = ""
                });
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateInputModel input)
        {
            Guid guidAnswerId = input.AnswerId != null ? Guid.Parse(input.AnswerId) : Guid.Empty;

            var result = await _answerService.Update(guidAnswerId, input.Text, DateTime.UtcNow);

            if (result == Status.Ok)
            {
                return Ok(new Response
                {
                    Status = result,
                    Error = ""
                });
            }
            else if (result == Status.InvalidData)
            {
                return BadRequest(new Response
                {
                    Status = result,
                    Error = ""
                });
            }
            else
            {
                return StatusCode(500, new Response
                {
                    Status = result,
                    Error = ""
                });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            Status result = await _answerService.Delete(id);

            if (result == Status.Ok)
            {
                return Ok(new Response
                {
                    Status = result,
                    Error = ""
                });
            }
            else if (result == Status.InvalidData)
            {
                return BadRequest(new Response
                {
                    Status = result,
                    Error = ""
                });
            }
            else
            {
                return StatusCode(500, new Response
                {
                    Status = result,
                    Error = ""
                });
            }
        }
    }
}
