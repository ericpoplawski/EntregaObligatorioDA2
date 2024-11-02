namespace Domain.HomeModels;

public sealed record class ChangeHomeAliasRequest
{
    public string Alias { get; set; }
}