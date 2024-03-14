using EventSourcing_Example_With_EventStore.Aggregates.Repositories;
using EventSourcing_Example_With_EventStore.Aggregates;
using EventSourcing_Example_With_EventStore.Features.Dtos;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace EventSourcing_Example_With_EventStore.Features.Commands
{
  public class ChangeUserNameHandler : IRequestHandler<ChangeUserNameDto>
  {
    readonly UserStreamAggregate _userAggregate;
    readonly UserStreamRepository _userStreamRepository;

    public ChangeUserNameHandler(UserStreamAggregate userAggregate, UserStreamRepository userStreamRepository)
    {
      _userAggregate = userAggregate;
      _userStreamRepository = userStreamRepository;
    }

    public async Task Handle(ChangeUserNameDto request, CancellationToken cancellationToken)
    {
      _userAggregate.UserNameChanged(request.UserName, request.UserId);
      await _userStreamRepository.SaveAsync(_userAggregate);

    }
  }
}
