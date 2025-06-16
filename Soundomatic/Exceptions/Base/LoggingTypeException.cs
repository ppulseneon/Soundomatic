using System;
using System.Threading.Tasks;

namespace Soundomatic.Exceptions.Base;

public class LoggingTypeException: BaseException
{
    /// <inheritdoc />
    protected LoggingTypeException(string message) : base(message){}

    /// <inheritdoc />
    protected override Task HandleAsync(Exception e)
    {
        throw new NotImplementedException();
    }
}