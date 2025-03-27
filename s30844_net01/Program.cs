using s30844_net01;

var manager = new DeviceManager("input.txt");
manager.readFile();

Console.WriteLine("Initial devices from .txt");
manager.ShowAllDevices();

var sw1 = new Smartwatch();
sw1.Id = "SW100";
sw1.Name = "something";
sw1.IsTurnedOn = true;
sw1.BatteryPercentage = 57;

manager.AddDevice(sw1);

manager.TurnOffDevice(sw1);

//editing device
Console.WriteLine("Enter device ID to edit:");
string deviceId = Console.ReadLine();
Device device = manager.GetDevices().FirstOrDefault(d => d.Id == deviceId);
if (device == null)
{
    Console.WriteLine("Device not found.");
    return;
}
Console.Write("Enter new name (or press Enter to keep current): ");
string newName = Console.ReadLine();
int? newBatteryPercentage = null;
if (device is Smartwatch)
{
    Console.Write("Enter new battery percentage (0-100): ");
    if (int.TryParse(Console.ReadLine(), out int batteryValue))
    {
        newBatteryPercentage = batteryValue;
    }
}
string newOS = null;
if (device is PersonalComputer)
{
    Console.Write("Enter new OS: ");
    newOS = Console.ReadLine();
}
string newIp = null, newNetwork = null;
if (device is EmbeddedDevice)
{
    Console.Write("Enter new IP: ");
    newIp = Console.ReadLine();
    Console.Write("Enter new network: ");
    newNetwork = Console.ReadLine();
}
manager.EditDevice(deviceId, newName, newBatteryPercentage, newOS, newIp, newNetwork);

manager.TurnOnDevice(sw1);

manager.ShowAllDevices();


/*manager.RemoveDevice(sw1);

manager.SaveData();*/ 