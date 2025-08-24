using Microsoft.AspNetCore.Http;

namespace Misc;

public class ResultOptional<T> : Result
{
    private readonly T? _value;

    public T? Value
    {
        get
        {
            if (!IsSuccess) throw new InvalidOperationException("The result is not success");
            return _value;
        }
    }

    protected ResultOptional(T value) : base()
    {
        _value = value;
    }
    protected ResultOptional(Error error) : base(error)
    {
        _value = default;
    }
    
    public static ResultOptional<T> Success(T value)
    {
        return new ResultOptional<T>(value);
    }

    public new static ResultOptional<T> Failure(Error error)
    {
        return new ResultOptional<T>(error);
    }
}