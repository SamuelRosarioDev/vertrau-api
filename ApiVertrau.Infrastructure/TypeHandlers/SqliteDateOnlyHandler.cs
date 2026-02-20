using System.Data;
using Dapper;

namespace ApiVertrau.Infrastructure.TypeHandlers;

public class SqliteDateOnlyHandler<T> : SqlMapper.TypeHandler<T>
{
    public override void SetValue(IDbDataParameter parameter, T? value)
    {
        parameter.Value = value?.ToString();
    }

    public override T Parse(object value)
    {
        if (typeof(T) == typeof(DateOnly?))
        {
            if (value == null || value == DBNull.Value)
                return default!;
            return (T)(object)DateOnly.Parse(value.ToString()!);
        }
        return (T)(object)DateOnly.Parse(value.ToString()!);
    }
}
