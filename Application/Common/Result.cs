using Application.Common.ErrorTypes;

namespace Application.Common;

public class Result
{
    private readonly Error? _error;
    public bool IsSuccess { get; private set; }

    public bool IsFailure => !IsSuccess;
    
    public Error GetError
    {
        get
        {
            if (IsSuccess) throw new InvalidOperationException("The result is success");
            return _error;
        }
    }
    protected Result()
    {
        IsSuccess = true;
        _error = null;
    }

    protected Result(Error error)
    {
        IsSuccess = false;
        _error = error;
    }

    public static Result Success()
    {
        return new Result();
    }

    public static Result Failure(Error error)
    {
        return new Result(error);
    }
}