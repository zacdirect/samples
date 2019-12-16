using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using Zac.Direct.Data.Repository;

namespace Zac.Direct.Data.SqlClient
{
    public abstract class BaseSqlRepository<T, I> : BaseRepository<T, I>
    {
		protected readonly string ConnectionString;
		public BaseSqlRepository(string connectionString)
		{
			ConnectionString = connectionString;
		}

		/// <summary>
		/// Automatically Adds a paramater for every singular property of whatever object is passed in.
		/// Use only if your sql statement and object model are perfectly coupled
		/// </summary>
		protected virtual void TransformToParameters(SqlCommand cmd, object obj)
		{
			TransformToParameters(cmd, obj, false);
		}

		/// <summary>
		/// Automatically Adds a paramater for every singular property of whatever object is passed in.
		/// Use only if your sql statement and object model are perfectly coupled
		/// </summary>
		/// <param name="cmd"></param>
		/// <param name="obj"></param>
		/// <param name="cleanDefaults">Will cleanup and avoid filling basic default values such as empty strings, Guid.Empty, DateTime.MinValue. Use if you prefer defaulting values in your stored procedures.</param>
		protected virtual void TransformToParameters(SqlCommand cmd, object obj, bool cleanDefaults)
		{
			TransformToParameters(cmd, obj, cleanDefaults, false);
		}

		/// <summary>
		/// Automatically Adds a paramater for every singular property of whatever object is passed in.
		/// Use only if your sql statement and object model are perfectly coupled
		/// </summary>
		/// <param name="cmd"></param>
		/// <param name="obj"></param>
		/// <param name="cleanDefaults">Will cleanup and avoid filling basic default values such as empty strings, Guid.Empty, DateTime.MinValue. Use if you prefer defaulting values in your stored procedures.</param>
		/// <param name="cleanZeroInts">Will regard zeros as null</param>
		protected virtual void TransformToParameters(SqlCommand cmd, object obj, bool cleanDefaults, bool cleanZeroInts)
		{
			TransformToParameters(cmd, obj, cleanDefaults, cleanZeroInts, false);
		}

		/// <summary>
		/// Automatically Adds a paramater for every singular property of whatever object is passed in.
		/// Use only if your sql statement and object model are perfectly coupled
		/// </summary>
		/// <param name="cmd"></param>
		/// <param name="obj"></param>
		/// <param name="cleanDefaults">Will cleanup and avoid filling basic default values such as empty strings, Guid.Empty, DateTime.MinValue. Use if you prefer defaulting values in your stored procedures.</param>
		/// <param name="cleanZeroInts">Will regard zeros as null</param>
		/// <param name="detectIdAsOutput">Converts a case insensitive property of 'id' or 'guid' to an output parameter</param>
		protected virtual void TransformToParameters(SqlCommand cmd, object obj, bool cleanDefaults, bool cleanZeroInts, bool detectIdAsOutput)
		{
			TransformToParameters(cmd, obj, cleanDefaults, cleanZeroInts, detectIdAsOutput, "");
		}

		/// <summary>
		/// Automatically Adds a paramater for every singular property of whatever object is passed in.
		/// Use only if your sql statement and object model are perfectly coupled
		/// </summary>
		/// <param name="cmd"></param>
		/// <param name="obj"></param>
		/// <param name="cleanDefaults">Will cleanup and avoid filling basic default values such as empty strings, Guid.Empty, DateTime.MinValue. Use if you prefer defaulting values in your stored procedures.</param>
		/// <param name="cleanZeroInts">Will regard zeros as null</param>
		/// <param name="detectIdAsOutput">Converts a case insensitive property of 'id' or 'guid' to an output parameter</param>
		/// <param name="paramPrefix">Will prefix to every converted parameter</param>
		protected virtual void TransformToParameters(SqlCommand cmd, object obj, bool cleanDefaults, bool cleanZeroInts, bool detectIdAsOutput, string paramPrefix)
		{
			TransformToParameters(cmd, obj, cleanDefaults, cleanZeroInts, detectIdAsOutput, paramPrefix, new string[0]);
		}

		/// <summary>
		/// Automatically Adds a paramater for every singular property of whatever object is passed in.
		/// Use only if your sql statement and object model are perfectly coupled
		/// </summary>
		/// <param name="cmd"></param>
		/// <param name="obj"></param>
		/// <param name="cleanDefaults">Will cleanup and avoid filling basic default values such as empty strings, Guid.Empty, DateTime.MinValue. Use if you prefer defaulting values in your stored procedures.</param>
		/// <param name="cleanZeroInts">Will regard zeros as null</param>
		/// <param name="detectIdAsOutput">Converts a case insensitive property of 'id' or 'guid' to an output parameter</param>
		/// <param name="paramPrefix">Will prefix to every converted parameter</param>
		/// <param name="excludedParameters">Will not attempt any conversions on these case insensitive properties</param>
		protected virtual void TransformToParameters(SqlCommand cmd, object obj, bool cleanDefaults, bool cleanZeroInts, bool detectIdAsOutput, string paramPrefix, string[] excludedParameters)
		{
			if (paramPrefix == null)
			{
				paramPrefix = "";
			}
			if (excludedParameters == null)
			{
				excludedParameters = new string[0];
			}

			PropertyInfo[] properties = obj.GetType().GetProperties();
			for (int i = 0; i <= properties.Length - 1; i++)
			{
				var name = properties[i].Name;

				if (excludedParameters.Length > 0 && excludedParameters.Where(p => String.Compare(p, name) == 0).Count() > 0)
				{
					continue;
				}

				var sqlName = "@" + paramPrefix + name;
				var val = properties[i].GetValue(obj, null);
				var type = properties[i].PropertyType.ToDataSafeType();
				if (type != typeof(string) && typeof(IEnumerable).IsAssignableFrom(type))
				{
					//transforming arrays and other things is a bit too tricky to satisfy everybody
					//if desired, these properties need to be handled at the respective child repo
					continue;
				}

				if (detectIdAsOutput && (name.ToLower() == "id" || name.ToLower() == "guid" || name.ToLower() == "rowid"))
				{
					SqlDbType supportedIdType;
					if (type == typeof(Guid))
					{
						supportedIdType = SqlDbType.UniqueIdentifier;
					}
					else if (type == typeof(long))
					{
						supportedIdType = SqlDbType.BigInt;
					}
					else if (type == typeof(string))
					{
						supportedIdType = SqlDbType.VarChar;
					}
					else
					{
						supportedIdType = SqlDbType.Int;
					}
					cmd.Parameters.Add(new SqlParameter(sqlName, supportedIdType) { Direction = ParameterDirection.Output });

					continue;
				}

				if (val == null)
				{
					if (cleanDefaults)
					{
						continue;
					}
					else
					{
						cmd.Parameters.AddWithValue(sqlName, null);
					}
				}
				else
				{
					if (cleanZeroInts && (type == typeof(int) || type == typeof(long)))
					{
						if (Convert.ToInt64(val) == 0)
						{
							continue;
						}
						else
						{
							cmd.Parameters.AddWithValue(sqlName, val);
							continue;
						}
					}

					if (cleanDefaults)
					{
						if (type == typeof(Guid) && (Guid)val == Guid.Empty)
						{
							continue;
						}
						else if (type == typeof(string))
						{
							if (String.IsNullOrWhiteSpace((string)val))
							{
								continue;
							}
							else
							{
								cmd.Parameters.AddWithValue(sqlName, ((string)val).Trim());
							}
						}
						else if (type == typeof(DateTime) && (DateTime)val == DateTime.MinValue)
						{
							continue;
						}
						else
						{
							cmd.Parameters.AddWithValue(sqlName, val);
						}
					}
					else
					{
						cmd.Parameters.AddWithValue(sqlName, val);
					}
				}
			}
		}
	}
}
