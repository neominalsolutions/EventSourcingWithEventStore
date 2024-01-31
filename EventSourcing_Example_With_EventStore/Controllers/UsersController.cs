using EventSourcing_Example_With_EventStore.Aggregates;
using EventSourcing_Example_With_EventStore.Aggregates.Repositories;
using EventSourcing_Example_With_EventStore.Features.Dtos;
using EventSourcing_Example_With_EventStore.Models;
using EventSourcing_Example_With_EventStore.ViewModels;
using EventStore.ClientAPI;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EventSourcing_Example_With_EventStore.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class UsersController : ControllerBase
  {
    

    readonly UserStreamRepository _userStreamRepository;
    readonly IMediator _mediator;
 
    public UsersController(UserStreamRepository userStreamRepository, IMediator mediator)
    {
      _userStreamRepository = userStreamRepository;
      _mediator = mediator;
    }
    [HttpPost("[action]")]
    public async Task<IActionResult> Create(CreateUserDto request)
    {

      await _mediator.Send(request);
      return StatusCode((int)HttpStatusCode.Created);
    }
    [HttpPut("[action]")]
    public async Task<IActionResult> UpdateUserName(ChangeUserNameDto request)
    {

      await _mediator.Send(request);
      return StatusCode((int)HttpStatusCode.OK);

    }

    [HttpPut("[action]")]
    public async Task<IActionResult> EmailApprove(ApproveEmailDto request)
    {

      await _mediator.Send(request);
      return StatusCode((int)HttpStatusCode.OK);

    }
    [HttpGet("[action]/{streamName}")]
    public async Task<IActionResult> GetEvents(string streamName)
    {
      dynamic events = await _userStreamRepository.GetEvents($"user-{streamName}");
      return Ok(events);
    }

    [HttpGet("[action]/{userId}")]
    public async Task<IActionResult> GetUser(string userId)
    {
      var response = await _mediator.Send(new GetUserRequestDto { UserId = userId });

      return Ok(response);
    }
  }
}
