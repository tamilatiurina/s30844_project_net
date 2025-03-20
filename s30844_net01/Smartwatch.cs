namespace s30844_net01;

public class Smartwatch() : Device, IPowerNotifier
{
    public double BatteryPercentage { get; set; }

    public void CheckBattery()
    {
        if (BatteryPercentage < 20)
        {
            Notify();
        }

        if (BatteryPercentage > 100 || BatteryPercentage < 0)
        {
            Console.WriteLine("Battery is out of range [0,100]");
        }
    }

    public void Notify()
    {
        Console.WriteLine("Low battery percentage");
    }

    public override void TurnOn()
    {
        if (BatteryPercentage < 11)
        {
            throw new EmptyBatteryException("Empty battery percentage");
        }
        else
        {
            IsTurnedOn = true;
            BatteryPercentage -= 10;
        }
    }
}

public class EmptyBatteryException : Exception
{
    public EmptyBatteryException(string emptyBatteryPercentage)
    {
        throw new NotImplementedException();
    }
}