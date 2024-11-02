using CQ.Utility;

namespace Domain.HomeModels;

public class ChangeHomeAliasArguments
{
    public string Alias { get; set; }
    
    public ChangeHomeAliasArguments(string alias)
    {
        Guard.ThrowIsNullOrEmpty(alias, nameof(alias));
        Alias = alias;
    }
    
}