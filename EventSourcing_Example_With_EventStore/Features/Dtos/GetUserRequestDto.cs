using MediatR;

namespace EventSourcing_Example_With_EventStore.Features.Dtos
{
  public class GetUserRequestDto:IRequest<GetUserResponseDto>
  {
    public string UserId { get; set; }

  }
}
