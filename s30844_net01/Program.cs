using s30844_net01;

var manager = new DeviceManager("C:\\Users\\Home\\RiderProjects\\s30844_net01\\s30844_net01\\input.txt");
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

manager.EditDevice(sw1);

manager.TurnOnDevice(sw1);

manager.ShowAllDevices();

manager.SaveData();

/*manager.RemoveDevice(sw1);

manager.SaveData();*/ 