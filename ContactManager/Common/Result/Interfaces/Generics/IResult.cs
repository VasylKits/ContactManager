namespace ContactManager.Common.Result.Interfaces.Generic;

public interface IResult<out TData> : IResult
{
    TData Data { get; }
}