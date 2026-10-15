using System;

namespace Illy_Technical_Task;

public static class Program
{
    public static void Main()
    {
        Table table = new Table("Users");
        Column col_id = new Column("Id", table);
        Column col_name = new Column("Name", table);

        Join join = new Join(JoinType.Inner, table, new Compare(col_id, Operation.Equal, 1));

        // honestly not sure how to turn this into an sql query yet...
        //think the base is done though


    }
}
