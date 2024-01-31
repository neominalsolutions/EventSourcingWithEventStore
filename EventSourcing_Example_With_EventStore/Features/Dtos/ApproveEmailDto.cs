using MediatR;

namespace EventSourcing_Example_With_EventStore.Features.Dtos
{
  public class ApproveEmailDto:IRequest
  {
    public string UserId { get; set; }

  }
}
