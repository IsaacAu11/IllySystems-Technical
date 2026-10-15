namespace Illy_Technical_Task;

public enum JoinType
{
    Inner,
    Left,
    Right,
}

public class Join 
{
    public JoinType Type { get; }
    public Table Table { get; }
    public Compare OnCondition { get; }

    public Join(JoinType type, Table table, Compare onCondition)
    {
        Type = type;
        Table = table;
        OnCondition = onCondition;
    }
}