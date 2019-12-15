using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zac.Direct
{
    public interface IBaseError : IBaseStatus
    {
        string ErrorCode { get; set; }
        string StackTrace { get; set; }
    }
}
