using EventSourcing_Example_With_EventStore.Aggregates.Repositories;
using EventSourcing_Example_With_EventStore.Aggregates;
using EventSourcing_Example_With_EventStore.Features.Dtos;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace EventSourcing_Example_With_EventStore.Features.Commands
{
  public class ApproveEmailHandler : IRequestHandler<ApproveEmailDto>
  {
    readonly UserStreamAggregate _userAggregate;
    readonly UserStreamRepository _userStreamRepository;

    public ApproveEmailHandler(UserStreamAggregate userAggregate, UserStreamRepository userStreamRepository)
    {
      _userAggregate = userAggregate;
      _userStreamRepository = userStreamRepository;
    }

    public async Task Handle(ApproveEmailDto request, CancellationToken cancellationToken)
    {
      _userAggregate.EmailApproved(request.UserId);
      await _userStreamRepository.SaveAsync(_userAggregate);
      
    }
  }
}
