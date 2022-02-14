namespace Csrs.Interfaces.Dynamics.Models;

public class OptionSetMetadata
{
    public OptionSet? OptionSet { get; set; }
}

public class PicklistOptionSetMetadata
{
    public List<OptionSetMetadata>? Value { get; set; }
}

public class OptionSet
{
    public List<Option>? Options { get; set; }
}

public class Option
{
    public int Value { get; set; }
    public Label? Label { get; set; }
}

public class Label
{
    public UserLocalizedLabel? UserLocalizedLabel { get; set; }
}

public class UserLocalizedLabel
{
    public string? Label { get; set; }
}
