using ContactManager.Shared.Result.Interfaces.Implementations;

namespace ContactManager.Shared.Result.Interfaces;

public interface IResult
{
    bool Success { get; }

    ErrorInfo ErrorInfo { get; }
}