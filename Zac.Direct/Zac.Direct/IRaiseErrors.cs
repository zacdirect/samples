using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zac.Direct
{
    public interface IRaiseErrors
    {
        event EventHandler<ErrorEventArgs> OnError;
        void RaiseErrorEvent(ErrorEventArgs args);
    }
}
