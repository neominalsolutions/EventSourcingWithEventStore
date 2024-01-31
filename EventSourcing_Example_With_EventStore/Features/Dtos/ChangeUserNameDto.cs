using MediatR;

namespace EventSourcing_Example_With_EventStore.Features.Dtos
{
  public class ChangeUserNameDto:IRequest
  {
    public string UserName { get; set; }
    public string UserId { get; set; }


  }
}
