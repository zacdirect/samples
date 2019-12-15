using System;

namespace Zac.Direct
{
    public class Helper
    {
        public static bool IsValidDate(DateTime? obj)
        {
            return obj.HasValue ? IsValidDate(obj.Value) : false;
        }
        public static bool IsValidDate(DateTime obj)
        {
            return obj != DateTime.MinValue;
        }

        public static bool IsValidGuid(Guid? obj)
        {
            return obj.HasValue ? IsValidGuid(obj.Value) : false;
        }

        public static bool IsValidGuid(Guid obj)
        {
            return obj != Guid.Empty;
        }

        public static bool IsValidGuid(string guid)
        {
            var newGuid = new Guid();
            return IsValidGuid(guid, out newGuid);
        }

        public static bool IsValidGuid(string guid, out Guid newGuid)
        {
            return Guid.TryParse(guid, out newGuid);
        }

        public static bool ReturnValidBoolean(object obj)
        {
            if (obj.GetType() == typeof(bool))
            {
                return (bool)obj;
            }
            else
            {
                return ReturnValidBoolean(obj.ToStringOrEmpty());
            }
        }

        public static bool ReturnValidBoolean(string anyString)
        {
            if (String.IsNullOrEmpty(anyString))
            {
                return false;
            }
            else
            {
                var newString = new string(anyString.ToCharArray());
                if (newString == "true" || newString == "y" || newString == "yes" || newString == "t" || newString == "pass")
                {
                    return true;
                }
                else if (newString == "false" || newString == "n" || newString == "no" || newString == "f" || newString == "fail")
                {
                    return false;
                }

                try
                {
                    var intTest = Convert.ToInt32(newString);
                    var boolTest = Convert.ToBoolean(intTest);

                    return boolTest;
                }
                catch
                {
                    return false;
                }
            }
        }
    }
}
