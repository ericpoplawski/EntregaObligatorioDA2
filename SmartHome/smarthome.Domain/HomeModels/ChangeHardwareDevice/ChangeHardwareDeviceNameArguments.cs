namespace Domain.HomeModels;

public class ChangeHardwareDeviceNameArguments
{
    public string NewName { get; set; }
    
    public ChangeHardwareDeviceNameArguments(string newName)
    {
        NewName = newName;
    }
}