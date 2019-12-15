using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zac.Direct
{
    public interface IServiceResponse
    {
        ServiceResponseCode ResponseCode { get; }
        IBaseError[] Errors { get; }
    }

    public interface IServiceResponse<T> : IServiceResponse
    {
        T Response { get; }
    }
}
