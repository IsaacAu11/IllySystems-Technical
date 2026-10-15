using System.Collections.Generic;
using System.Linq;

namespace Illy_Technical_Task;

public abstract class Condition
{
}

public enum Operation
{
    Equal,
    NotEqual,
    GreaterThan,
    LessThan,
}

public enum LogicalOperator
{
    And,
    Or,
}

public class Compare : Condition
{
    public Column Column { get; }
    public Operation Operator { get; }
    public object Value { get; }

    public Compare(Column column, Operation op, object value)
    {
        Column = column;
        Operator = op;
        Value = value;
    }
}
// realised that i needed AND and OR condiitions to chain together
public class ConditionalGroup : Condition
{
    public LogicalOperator Logic { get; }
    public List<Condition> Children { get; }

    public ConditionalGroup(LogicalOperator logic, params Condition[] children)
    {
        Logic = logic;
        Children = children.ToList();
    }
}
