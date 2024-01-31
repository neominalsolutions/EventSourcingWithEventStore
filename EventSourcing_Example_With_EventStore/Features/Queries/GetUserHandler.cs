using EventSourcing_Example_With_EventStore.Aggregates.Repositories;
using EventSourcing_Example_With_EventStore.Features.Dtos;
using EventSourcing_Example_With_EventStore.Models;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EventSourcing_Example_With_EventStore.Features.Queries
{
  public class GetUserHandler : IRequestHandler<GetUserRequestDto, GetUserResponseDto>
  {

    readonly UserStreamRepository _userStreamRepository;

    public GetUserHandler(UserStreamRepository userStreamRepository)
    {
      _userStreamRepository = userStreamRepository;
    }

    public async Task<GetUserResponseDto> Handle(GetUserRequestDto request, CancellationToken cancellationToken)
    {
      User user = await _userStreamRepository.GetUserProduction($"user-{request.UserId}");

      return await Task.FromResult(new GetUserResponseDto
      {
        Email = user.Email,
        EmailApprove = user.EmailApprove,
        Name = user.Name,
        UserId = user.Id,
        UserName = user.UserName
      });


    }
  }
}
