using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zac.Direct
{
    public interface IBaseStatus
    {
        string Message { get; set; }
        string FriendlyMessage { get; set; }
        string Source { get; set; }
        ArgStatusType Severity { get; set; }
    }
}
