namespace Illy_Technical_Task;


// just a generic class to show tables for now
public class Table
{
    public string Name { get; }
    public string? Alias { get; }

    public Table(string name, string? alias = null)
    {
        Name = name;
        Alias = alias;
    }
}
