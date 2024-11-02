namespace Domain.DeviceModels;

public sealed record class GetSupportedTypesResponse
{
    public string Type { get; set; }
    
    public GetSupportedTypesResponse(string type)
    {
        Type = type;
    }
}