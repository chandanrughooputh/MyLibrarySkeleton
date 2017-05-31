using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace Persistence
{
    public class Repository : IRepository
    {
        private static readonly string connString = ConfigurationManager.ConnectionStrings["bookstore"].ConnectionString;

        public IList<T> RunQuery<T>(string query)
        {

            var results = new List<T>();

            using (var connection = new SqlConnection(connString))
            {
                connection.Open();
                using (var sqlCommand = new SqlCommand())
                {
                    sqlCommand.CommandText = query;
                    sqlCommand.Connection = connection;

                    using (var reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var item = Activator.CreateInstance<T>();
                            foreach (var property in typeof(T).GetProperties())
                            {
                                if (!reader.IsDBNull(reader.GetOrdinal(property.Name)))
                                {
                                    var convertTo = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                                    property.SetValue(item, Convert.ChangeType(reader[property.Name], convertTo), null);
                                }
                            }
                            results.Add(item);
                        }
                    }
                }
            }

            return results;
        }
    } 
}
