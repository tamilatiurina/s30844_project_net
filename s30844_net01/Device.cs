namespace s30844_net01;

public abstract class Device
{
    public string Id { get; set; }
    public string Name { get; set; }
    public bool IsTurnedOn { get; set; }
    public abstract void TurnOn();
}