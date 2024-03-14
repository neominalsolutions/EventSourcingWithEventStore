using EventSourcing_Example_With_EventStore.Aggregates.Repositories;
using EventSourcing_Example_With_EventStore.Aggregates;
using EventSourcing_Example_With_EventStore.Features.Dtos;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System;

namespace EventSourcing_Example_With_EventStore.Features.Commands
{
  public class CreateUserHandler : IRequestHandler<CreateUserDto>
  {
    readonly UserStreamAggregate _userAggregate;
    readonly UserStreamRepository _userStreamRepository;

    public CreateUserHandler(UserStreamAggregate userAggregate, UserStreamRepository userStreamRepository)
    {
      _userAggregate = userAggregate;
      _userStreamRepository = userStreamRepository;
    }

    public async Task Handle(CreateUserDto request, CancellationToken cancellationToken)
    {

      var userId = Guid.NewGuid().ToString();
 
      _userAggregate.Created(new()
      {
        Id = userId,
        Email = request.Email,
        Name = request.Name,
        UserName = request.UserName
      });

      await _userStreamRepository.SaveAsync(_userAggregate);
    }
  }
}
