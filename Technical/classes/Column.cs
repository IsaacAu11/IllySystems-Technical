namespace Illy_Technical_Task;

public class Column
{
    public string Name { get; }
    public Table Table { get; }
    public string? Alias { get; }

    public Column(string name, Table table, string? alias = null)
    {
        Name = name;
        Table = table;
        Alias = alias;
    }
}
