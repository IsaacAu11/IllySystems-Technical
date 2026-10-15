namespace Illy_Technical_Task;

public class Query
{
    public Table FromTable { get; }
    public List<Join> Joins { get; } = new();
    public Condition? Where { get; set; }
    public List<Column> Columns { get; } = new();

    public Query(Table table)
    {
        FromTable = table;
    }

    public Query Select(params Column[] columns)
    {
        ArgumentNullException.ThrowIfNull(columns);
        Columns.Clear();
        Columns.AddRange(columns);
        return this;
    }

    public Query AddJoin(Join join)
    {
        ArgumentNullException.ThrowIfNull(join);
        Joins.Add(join);
        return this;
    }

    public Query SetWhere(Condition condition)
    {
        ArgumentNullException.ThrowIfNull(condition);
        Where = condition;
        return this;
    }

    public string ToSql()
    {
        string sql = $"{BuildSelectClause()} {BuildFromClause()}";

        foreach (Join join in Joins)
        {
            sql += $" {BuildJoinClause(join)}";
        }

        if (Where is not null)
        {
            sql += $" WHERE {BuildCondition(Where)}";
        }

        return sql;
    }

    private string BuildSelectClause()
    {
        if (Columns.Count == 0)
        {
            return "SELECT *";
        }

        var selectedColumns = new List<string>();
        foreach (Column column in Columns)
        {
            string columnSql = BuildColumnReference(column);

            if (column.Alias is not null)
            {
                columnSql += $" AS {column.Alias}";
            }

            selectedColumns.Add(columnSql);
        }

        return $"SELECT {string.Join(", ", selectedColumns)}";
    }

    private string BuildFromClause()
    {
        return $"FROM {BuildTableReference(FromTable)}";
    }

    private string BuildJoinClause(Join join)
    {
        string joinType;
        switch (join.Type)
        {
            case JoinType.Inner:
                joinType = "INNER";
                break;
            case JoinType.Left:
                joinType = "LEFT";
                break;
            case JoinType.Right:
                joinType = "RIGHT";
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(join));
        }

        return $"{joinType} JOIN {BuildTableReference(join.Table)} ON {BuildCondition(join.OnCondition)}";
    }

    private string BuildCondition(Condition condition)
    {
        if (condition is Compare compare)
        {
            return $"{BuildColumnReference(compare.Column)} {BuildOperation(compare.Operator)} {SqlValue(compare.Value)}";
        }

        if (condition is ConditionalGroup group)
        {
            if (group.Children.Count == 0)
            {
                throw new ArgumentException("condition group cannot be empty");
            }

            string logic = group.Logic == LogicalOperator.And ? "AND" : "OR";
            var childConditions = new List<string>();

            foreach (Condition child in group.Children)
            {
                childConditions.Add(BuildCondition(child));
            }

            return $"({string.Join($" {logic} ", childConditions)})";
        }

        throw new ArgumentException($"unsupported condition: ");
    }

    private string BuildColumnReference(Column column)
    {
        var tableReference = column.Table.Alias ?? column.Table.Name;
        return $"{tableReference}.{column.Name}";
    }

    private string BuildTableReference(Table table)
    {
        return table.Alias is null
            ? table.Name
            : $"{table.Name} AS {table.Alias}";
    }

    private string BuildOperation(Operation operation)
    {
        switch (operation)
        {
            case Operation.Equal:
                return "=";
            case Operation.NotEqual:
                return "<>";
            case Operation.GreaterThan:
                return ">";
            case Operation.LessThan:
                return "<";
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private string SqlValue(object value)
    {
        if (value is Column column)
        {
            return BuildColumnReference(column);
        }

        if (value is string text)
        {
            return "'" + text.Replace("'", "''") + "'";
        }

        if (value is bool boolean)
        {
            return boolean ? "1" : "0";
        }

        if (value is null)
        {
            return "NULL";
        }

        return value.ToString()!;
    }
}
