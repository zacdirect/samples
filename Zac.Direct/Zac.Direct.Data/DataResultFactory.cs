using System;
using System.Collections.Generic;

namespace Zac.Direct.Data
{
    public static class DataResultFactory<T>
    {

        public static DataResult<T> Create()
        {
            return Create(true);
        }


        public static DataResult<T> Create(bool IsSuccessful)
        {
            DataResult<T> result = new DataResult<T>
            {
                ResultCollection = new List<T>(),
                IsSuccessful = IsSuccessful,
                RowCount = 0
            };
            return result;
        }

        public static DataResult<T> Create(bool IsSuccessful, T resultObject)
        {
            DataResult<T> result = new DataResult<T>
            {
                ResultCollection = new List<T>(),
                IsSuccessful = IsSuccessful,
                ResultObject = resultObject,
                RowCount = 1
            };
            return result;
        }

        public static DataResult<T> Create(bool IsSuccessful, string FriendlyErrorMessage, T resultObject)
        {
            DataResult<T> result = new DataResult<T>
            {
                ResultCollection = new List<T>(),
                IsSuccessful = IsSuccessful,
                ResultObject = resultObject
            };
            if (!IsSuccessful)
            {
                result.FriendlyErrorMessage = FriendlyErrorMessage;
                result.ErrorDetails = new DataAccessError(FriendlyErrorMessage);
            }
            result.RowCount = 1;
            return result;
        }

        public static DataResult<T> Create(string FriendlyErrorMessage)
        {
            DataResult<T> result = new DataResult<T>
            {
                ResultCollection = new List<T>(),
                FriendlyErrorMessage = FriendlyErrorMessage,
                ErrorDetails = new DataAccessError(FriendlyErrorMessage),
                IsSuccessful = false,
                RowCount = 0
            };
            return result;
        }

        public static DataResult<T> Create(Exception ex)
        {
            DataResult<T> result = new DataResult<T>
            {
                ResultCollection = new List<T>(),
                FriendlyErrorMessage = ex.Message,
                ErrorDetails = new DataAccessError(ex),
                IsSuccessful = false,
                RowCount = 0
            };
            return result;
        }

        public static DataResult<T> Create(DataAccessError dae)
        {
            return new DataResult<T>
            {
                ResultCollection = new List<T>(),
                FriendlyErrorMessage = dae.FriendlyMessage,
                ErrorDetails = dae,
                IsSuccessful = false,
                RowCount = 0
            };
        }

        public static DataResult<T> Create(string FriendlyErrorMessage, Exception ex)
        {
            return new DataResult<T>
            {
                ResultCollection = new List<T>(),
                FriendlyErrorMessage = FriendlyErrorMessage,
                ErrorDetails = new DataAccessError(ex, FriendlyErrorMessage),
                IsSuccessful = false,
                RowCount = 0
            };
        }

        public static DataResult<T> Create(T resultObject)
        {
            DataResult<T> result = new DataResult<T>
            {
                ResultCollection = new List<T>(),
                ResultObject = resultObject
            };
            result.ResultCollection.Add(resultObject);
            result.IsSuccessful = true;
            result.RowCount = 1;
            return result;

        }

        public static DataResult<T> Create(T[] ResultCollection)
        {
            DataResult<T> result = new DataResult<T>
            {
                ResultCollection = new List<T>()
            };
            if (ResultCollection.Length > 0)
            {
                result.ResultObject = ResultCollection[0];
                foreach (T obj in ResultCollection)
                {
                    result.ResultCollection.Add(obj);
                }
                result.RowCount = result.ResultCollection.Count;
            }
            else
            {
                result.ResultObject = default(T);
                result.ResultCollection = new List<T>();
                result.RowCount = 0;
            }
            result.IsSuccessful = true;
            return result;

        }

        public static DataResult<T> Create(List<T> list)
        {
            DataResult<T> result = new DataResult<T>
            {
                ResultCollection = new List<T>()
            };
            if (list != null && list.Count > 0)
            {
                result.ResultObject = list[0];
                result.ResultCollection = list;
                result.RowCount = list.Count;
            }
            else
            {
                result.ResultObject = default(T);
                result.ResultCollection = new List<T>();
                result.RowCount = 0;
            }
            result.IsSuccessful = true;
            return result;

        }

        public static DataResult<T> Create(List<T> ResultCollection, Int64 totalRecords)
        {
            DataResult<T> result = new DataResult<T>
            {
                ResultCollection = new List<T>()
            };
            if (ResultCollection.Count > 0)
            {
                result.ResultObject = ResultCollection[0];
                result.ResultCollection = ResultCollection;
                result.RowCount = totalRecords > 0 ? totalRecords : ResultCollection.Count;
            }
            else
            {
                result.ResultObject = default(T);
                result.ResultCollection = new List<T>();
                result.RowCount = 0;
            }
            result.IsSuccessful = true;
            return result;

        }
    }
}
