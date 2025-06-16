using System;
using System.Threading.Tasks;

namespace Soundomatic.Exceptions.Base;

public class ViewBoxTypeException: BaseException
{
    /// <inheritdoc />
    protected ViewBoxTypeException(string message) : base(message){}

    /// <inheritdoc />
    protected override Task HandleAsync(Exception e)
    {
        throw new NotImplementedException();
    }
}