using System.Reflection;
using System.Text.RegularExpressions;

namespace s30844_net01;

public class DeviceManager
{
    private const int MaxDevices = 15;
    private List<Device> devices = new List<Device>();
    public string FilePath { get; set; }

    public DeviceManager(string filePath)
    {
        FilePath = filePath;
    }

    public void readFile()
    {
        if (!File.Exists(FilePath))
        {
            Console.WriteLine("File not found");
            return;
        }

        string[] lines = File.ReadAllLines(FilePath);

        foreach (string line in lines)
        {
            if (string.IsNullOrWhiteSpace(line)) continue; 
            
            if (devices.Count >= MaxDevices)
            {
                Console.WriteLine("Storage limit reached");
                break;
            }
            
            string[] parts = line.Split(',');

            if (line.StartsWith("SW", StringComparison.OrdinalIgnoreCase))
            {
                if (parts.Length == 4)
                {
                    Smartwatch device = new Smartwatch();
                    device.Id = parts[0];
                    device.Name = parts[1];
                    device.IsTurnedOn = Boolean.Parse(parts[2]);
                    device.BatteryPercentage = Double.Parse(parts[3].Replace("%", ""));
                    
                    devices.Add(device);
                }
            }else if (line.StartsWith("P", StringComparison.OrdinalIgnoreCase))
            {
                if (parts.Length == 4)
                {
                    PersonalComputer device = new PersonalComputer();
                    device.Id = parts[0];
                    device.Name = parts[1];
                    device.IsTurnedOn = Boolean.Parse(parts[2]);
                    device.OS = parts[3];
                    
                    devices.Add(device);
                }
            }else if (line.StartsWith("ED", StringComparison.OrdinalIgnoreCase))
            {
                if (parts.Length == 5)
                {
                    EmbeddedDevice device = new EmbeddedDevice();
                    device.Id = parts[0];
                    device.Name = parts[1];
                    device.IsTurnedOn = Boolean.Parse(parts[2]);
                    device._ipAddress = parts[3];
                    device.Network = parts[4];
                    
                    devices.Add(device);
                }
            }
        }
    }


    public void AddDevice(Device device)
    {
        if (devices.Any(d => d.Id == device.Id))
        {
            Console.WriteLine($"Error: A device with ID '{device.Id}' already exists");
            return;
        }
        
        if (device is EmbeddedDevice embeddedDevice)
        {
            if (!IsValidIPAddress(embeddedDevice.IP))
            {
                Console.WriteLine($"Error: Invalid IP address '{embeddedDevice.IP}' for device {device.Id}");
                return;
            }
        }

        devices.Add(device);
        SaveData();
    }

    public void RemoveDevice(Device device)
    {
        devices.Remove(device);
    }

    public void EditDevice(string deviceId, string newName, int? newBatteryPercentage, string newOS, string newIp, string newNetwork)//don't edit id and tutnOn
    {
        Device device = devices.FirstOrDefault(d => d.Id == deviceId);
    
        if (device == null)
        {
            Console.WriteLine("Device not found");
            return;
        }
        
        if (!string.IsNullOrEmpty(newName))
        {
            device.Name = newName;
        }
        
        if (device is Smartwatch smartwatch && newBatteryPercentage.HasValue)
        {
            smartwatch.BatteryPercentage = newBatteryPercentage.Value;
        }
        else if (device is PersonalComputer computer && !string.IsNullOrEmpty(newOS))
        {
            computer.OS = newOS;
        }
        else if (device is EmbeddedDevice ed)
        {
            if (!string.IsNullOrEmpty(newIp)) ed._ipAddress = newIp;
            if (!string.IsNullOrEmpty(newNetwork)) ed.Network = newNetwork;
        }
        
        SaveData();
    }

    public void TurnOnDevice(object boxedDevice)
    {
        if (IsExistDevice(boxedDevice))
        {
            Device device = (Device)boxedDevice;
            device.TurnOn();
        }
        else
        {
            Console.WriteLine("Device not found");
        }
        
        SaveData();
    }
    
    public void TurnOffDevice(object boxedDevice)
    {
        if (IsExistDevice(boxedDevice))
        {
            Device device = (Device)boxedDevice;
            device.IsTurnedOn = false;
        }else
        {
            Console.WriteLine("Device not found");
        }
        
        SaveData();
    }
    
    public  void ShowAllDevices()
    {
        foreach (var device in devices)
        {
            Console.WriteLine($"ID: {device.Id}, Name: {device.Name}, Power State: {(device.IsTurnedOn ? "On" : "Off")}");
            
            if (device is Smartwatch smartwatch)
            {
                Console.WriteLine($"Battery: {smartwatch.BatteryPercentage}%");
            }
            else if (device is PersonalComputer computer)
            {
                Console.WriteLine($"OS: {computer.OS}");
            }else if (device is EmbeddedDevice ed)
            {
                Console.WriteLine($"IP: {ed.IP}, Network: {ed.Network}");
            }

            Console.WriteLine();
        }
    }
    
    public void SaveData()
    {
        using (StreamWriter writer = new StreamWriter(FilePath))
        {
            foreach (var device in devices)
            {
                string deviceData = $"{device.Id},{device.Name},{(device.IsTurnedOn ? "true" : "false")}";
                
                if (device is Smartwatch smartwatch)
                {
                    deviceData += $",{smartwatch.BatteryPercentage}%";
                }
                else if (device is PersonalComputer computer)
                {
                    deviceData += $",{computer.OS}";
                }
                else if (device is EmbeddedDevice ed)
                {
                    deviceData += $",{ed.IP},{ed.Network}";
                }

                writer.WriteLine(deviceData);
            }
        }
    }



    public bool IsExistDevice(object boxedDevice)
    {
        if (boxedDevice is Device device)
        {
            return devices.Any(existingDevice => AreDevicesEqual(existingDevice, device));
        }

        return false;
    }
    
    
    private bool AreDevicesEqual(Device device1, Device device2)
    {
        if (device1 == null || device2 == null)
            return false;

        if (device1.GetType() != device2.GetType()) 
            return false;
        
        PropertyInfo[] properties = device1.GetType().GetProperties();
        foreach (PropertyInfo property in properties)
        {
            var value1 = property.GetValue(device1);
            var value2 = property.GetValue(device2);
            
            if (!Equals(value1, value2))
                return false;
        }

        return true;
    }
    
    public List<Device> GetDevices()
    {
        return devices;
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

