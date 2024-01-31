using MediatR;

namespace EventSourcing_Example_With_EventStore.Features.Dtos
{
  public class CreateUserDto:IRequest
  {
    public string Email { get; set; }
    public string Name { get; set; }
    public string UserName { get; set; }

  }
}
