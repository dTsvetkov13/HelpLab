using APIGateway.Models.InputModels;
using APIGateway.Services;
using Microservices.Models.UserModels;
using Microservices.Models.AnswerModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microservices.Models.Common;
using APIGateway.Models.InputModels.AnswerInputModels;

namespace APIGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnswersController : ControllerBase
    {
        private static readonly string AnswersRoot = "https://localhost:44301/api/microservices/answers";
        private readonly HttpSender _httpSender;

        public AnswersController(HttpSender httpSender)
        {
            _httpSender = httpSender;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string id)
        {
            GetAnswerModel response = await _httpSender.SendGetAsync<GetAnswerModel>(AnswersRoot + "?id=" + id);

            if (response != null)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest("There is no such post.");
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(CreateAnswerInputModel input)
        {
            string userId = User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;

            var user = await _httpSender.SendGetAsync<GetUserResponse>(UsersController.UsersRoot + "?id=" + userId);

            CreateAnswerModel answer = new CreateAnswerModel
            {
                Text = input.Text,
                AuthorId = userId,
                AuthorName = user.Name,
                AnswerId = input.AnswerId,
                PostId = input.PostId
            };

            Response response = new Response();

            try
            {
                response = await _httpSender.SendPostAsync<Response, CreateAnswerModel>(answer, AnswersRoot);
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }

            if (response.Status == Status.Ok)
            {
                return Ok();
            }
            else if (response.Status == Status.ServerError)
            {
                return StatusCode(500, response.Error);
            }
            else
            {
                return BadRequest(response.Error);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateAnswerInputModel input)
        {
            var result = await _httpSender.SendPutAsync<Response, UpdateAnswerInputModel>(input, AnswersRoot);

            if (result.Status == Status.Ok)
            {
                return Ok(new Response
                {
                    Status = result.Status,
                    Error = result.Error
                });
            }
            else if (result.Status == Status.InvalidData)
            {
                return BadRequest(new Response
                {
                    Status = result.Status,
                    Error = result.Error
                });
            }
            else
            {
                return StatusCode(500, (new Response
                {
                    Status = result.Status,
                    Error = result.Error
                }));
            }
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            string userId = User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;

            Response response = new Response();

            try
            {
                response = await _httpSender.SendDeleteAsync<Response>(AnswersRoot + "?id=" + id + "&userId=" + userId);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }

            if (response.Status == Status.Ok)
            {
                return Ok();
            }
            else if(response.Status == Status.ServerError)
            {
                return StatusCode(500, response.Error);
            }
            else
            {
                return BadRequest(response.Error);
            }
        }
    }
}
