using System.Text;

Console.WriteLine("Hello, World!");

var simpleQuery = SimpleQueryBuilder.Create()
    .Select("Test", "Test2")
    .From("Test_Table t")
    .Join("Test_Table2 t2", "t.id = t2.id")
    .Where("t.name = 'Test'")
    .AndWhere("t2.test = 'Test'")
    .OrWhere("t.type = 'Test'")
    .OrderBy("Test")
    .Build();

Console.WriteLine(simpleQuery);

public class SimpleQueryBuilder : ICanSelect, ICanFrom, ICanJoin, ICanOrderBy, ICanBuild
{
    private StringBuilder _query;
    private SimpleQueryBuilder()
    {
        _query = new StringBuilder();
    }

    public static ICanSelect Create()
    {
        return new SimpleQueryBuilder();
    }

    public ICanFrom Select(params string[] columns)
    {
        this._query.Append($"SELECT {String.Join(", ", columns)} ");
        return this;
    }

    public ICanJoin From(params string[] tables)
    {
        this._query.Append($"FROM {String.Join(" ", tables)} ");
        return this;
    }

    public ICanJoin Join(string table, string onCondition)
    {
        this._query.Append($"JOIN {table} ").Append($"ON {onCondition} ");
        return this;
    }
    
    public ICanOrderBy Where(string condition)
    {
        this._query.Append($"WHERE {condition} ");
        return this;
    }

    public ICanOrderBy AndWhere(string condition)
    {
        this._query.Append($"AND ");
        return Where(condition);
    }

    public ICanOrderBy OrWhere(string condition)
    {
        this._query.Append($"OR ");
        return Where(condition);
    }

    public ICanBuild OrderBy(params string[] columns)
    {
        this._query.Append($"ORDER BY {String.Join(" ", columns)} ");
        return this;
    }

    public string Build()
    {
        return _query.ToString();
    }
}

public interface ICanSelect
{
    ICanFrom Select(params string[] columns);
}

public interface ICanFrom { 
    ICanJoin From (params string[] tables);
}

public interface ICanJoin : ICanBuild
{
    ICanJoin Join(string table, string onCondition);
    ICanOrderBy Where(string condition);
}

public interface ICanOrderBy : ICanBuild
{
    ICanOrderBy AndWhere(string condition);
    ICanOrderBy OrWhere(string condition);
    ICanBuild OrderBy(params string[] columns);
}

public interface ICanBuild
{
    string Build();
}
