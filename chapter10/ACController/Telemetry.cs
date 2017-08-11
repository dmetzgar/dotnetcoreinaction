namespace ACController
{
  public class Telemetry
  {
    public void LogStatus() 
    {
      Controller.Events.ExhaustAirTemp(TempControl.ExhaustAirTemp);
      Controller.Events.CoolantTemp(TempControl.CoolantTemp);
      Controller.Events.OutsideAirTemp(TempControl.OutsideAirTemp);
    }
  }
}
