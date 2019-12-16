using System;
using System.Collections.Generic;
using System.Text;

namespace Zac.Direct
{
    public static class TypeExtensions
    {
		public static Type ToDataSafeType(this Type type)
		{
			if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
			{
				return type.GetGenericArguments()[0];
			}
			else
			{
				return type;
			}
		}
	}
}
