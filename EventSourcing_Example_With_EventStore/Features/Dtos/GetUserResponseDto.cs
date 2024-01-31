namespace EventSourcing_Example_With_EventStore.Features.Dtos
{
  public class GetUserResponseDto
  {
    public string UserId { get; set; }
    public string Name { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public bool EmailApprove { get; set; }
  }
}
