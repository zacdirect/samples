using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zac.Direct
{
    public interface IProcessingItem
    {
        bool? IsNew { get; set; }
        bool? IsActive { get; set; }
        bool? IsChanged { get; set; }
        bool? IsDeleted { get; set; }
        bool? IsReadOnly { get; set; }
    }
}
