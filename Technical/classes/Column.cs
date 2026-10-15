namespace Illy_Technical_Task;


//basic class to show columns for now, should link to the table class
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
