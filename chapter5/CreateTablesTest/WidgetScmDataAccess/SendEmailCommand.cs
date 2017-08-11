namespace WidgetScmDataAccess
{
  public class SendEmailCommand
  {
    public int Id { get; set; }
    public string To { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
  }
}
