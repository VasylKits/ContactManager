using ContactManager.Common.Result.Interfaces.Implementations;

namespace ContactManager.Common.Result.Interfaces;

public interface IResult
{
    bool Success { get; }

    ErrorInfo ErrorInfo { get; }
}