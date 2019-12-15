using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Zac.Direct
{
    public interface IHierarchalItem<T>
    {
        /// <summary>
        /// Identifier - can be GUID, Int, or Other depending on object.
        /// </summary>
        T ObjectId { get; set; }

        /// <summary>
        /// Identifier - can be GUID, Int, or Other depending on object.
        /// </summary>
        T ParentId { get; set; }
    }
}
