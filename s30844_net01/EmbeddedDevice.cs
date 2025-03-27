using System.Text.RegularExpressions;

namespace s30844_net01;

public class EmbeddedDevice : Device
{
    public string _ipAddress;
    public string Network { get; set; }
    public string IP
    {
        get => _ipAddress;
        set
        {
            if (!IsValidIPAddress(value))
            {
                throw new ArgumentException("Invalid IP address format.");
            }
            _ipAddress = value;
        }
    }
    
    public override void TurnOn()
    {
        try
        {
            Connect(IP, Network);
            IsTurnedOn = true;
        }
        catch (ConnectionException ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }

    public void Connect(string IP, string Network)
    {
        if (!Network.Contains("MD Ltd."))
        {
            throw new ConnectionException("Invalid network. Connection failed.");
        }
    }
    
    public bool IsValidIPAddress(string ip)
    {
        string pattern = @"^(25[0-5]|2[0-4][0-9]|1?[0-9][0-9]?)\."
                         + @"(25[0-5]|2[0-4][0-9]|1?[0-9][0-9]?)\."
                         + @"(25[0-5]|2[0-4][0-9]|1?[0-9][0-9]?)\."
                         + @"(25[0-5]|2[0-4][0-9]|1?[0-9][0-9]?)$";

        return Regex.IsMatch(ip, pattern);
    }
}

public class ConnectionException : Exception
{
    public ConnectionException(string invalidNetworkConnectionFailed)
    {
        throw new NotImplementedException();
    }
}