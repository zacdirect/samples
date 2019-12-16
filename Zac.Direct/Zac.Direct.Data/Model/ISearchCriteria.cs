using System.Runtime.Serialization;

namespace Zac.Direct.Data.Model
{
    public interface ISearchCriteria
    {
        [DataMember]
        string OrderBy { get; set; }

        [DataMember]
        int? RecordStart { get; set; }

        [DataMember]
        int? RecordLimit { get; set; }
    }
}
