namespace Domain.DeviceModels;

public class GetAllDevicesArguments
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public string DeviceName { get; set; }
    public string ModelNumber { get; set; }
    public string CompanyName { get; set; }
    public string DeviceType { get; set; }

    public GetAllDevicesArguments(int pageNumber, int pageSize, 
        string deviceName, string modelNumber, string companyName, string deviceType)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        DeviceName = deviceName;
        ModelNumber = modelNumber;
        CompanyName = companyName;
        DeviceType = deviceType;
    }
}