using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Zac.Direct
{
    [DataContract(Name = "DataResultOf{0}")]
    public class DataResult<T>
    {
        [DataMember]
        public bool IsSuccessful { get; set; }
        [DataMember]
        public string FriendlyErrorMessage { get; set; }
        [DataMember]
        public DataAccessError ErrorDetails { get; set; }

        /// <summary>
        /// If a subset of data is requested RowCount can contain the rows available at the datasource.
        /// </summary>
        [DataMember]
        public long? RowCount { get; set; }
        [DataMember]
        public virtual List<T> ResultCollection { get; set; }
        [DataMember]
        public T ResultObject { get; set; }

        /// <summary>
        /// Throws the exception from ErrorDetails if applicable.
        /// </summary>
        /// <returns>The list or throws a hopefully useful error</returns>
        public List<T> ToSuccessfulList()
        {
            return !IsSuccessful ? throw ErrorDetails.UnderlyingException : ResultCollection;
        }

        /// <summary>
        /// Throws the exception from ErrorDetails if applicable.
        /// </summary>
        /// <returns>The object or throws a hopefully useful error</returns>
        public T ToSuccessfulObject()
        {
            return !IsSuccessful ? throw ErrorDetails.UnderlyingException : ResultObject;
        }

    }
}
