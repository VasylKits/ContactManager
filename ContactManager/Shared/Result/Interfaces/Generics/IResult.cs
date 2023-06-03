namespace ContactManager.Shared.Result.Interfaces.Generics;

public interface IResult<out TData> : IResult
{
    TData Data { get; }
}