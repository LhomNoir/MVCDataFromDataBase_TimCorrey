using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Helpers
{
    public static class Helper
    {
        /// <summary>
        /// Converts a DataTable to a list with generic objects
        /// </summary>
        /// <typeparam name="T">Generic object</typeparam>
        /// <param name="table">DataTable</param>
        /// <returns>List with generic objects</returns>
        public static List<T> DataTableToList<T>(this DataTable table) where T : class, new()
        {
            try
            {
                List<T> list = new List<T>();
                list.Clear();

                foreach (var row in table.AsEnumerable())
                {
                    T obj = new T();

                    foreach (var prop in obj.GetType().GetProperties())
                    {
                        try
                        {
                            PropertyInfo propertyInfo = obj.GetType().GetProperty(prop.Name);
                            propertyInfo.SetValue(obj, Convert.ChangeType(row[prop.Name], propertyInfo.PropertyType), null);
                        }
                        catch
                        {
                            continue;
                        }
                    }

                    list.Add(obj);
                }

                return list;
            }
            catch
            {
                return null;
            }
        }


        private static readonly IDictionary<Type, ICollection<PropertyInfo>> _Properties =
        new Dictionary<Type, ICollection<PropertyInfo>>();

        /// <summary>
        /// Converts a DataTable to a list with generic objects
        /// </summary>
        /// <typeparam name="T">Generic object</typeparam>
        /// <param name="table">DataTable</param>
        /// <returns>List with generic objects</returns>
        public static IEnumerable<T> DataTableToList2<T>(this DataTable table) where T : class, new()
        {
            try
            {
                var objType = typeof(T);
                ICollection<PropertyInfo> properties;

                lock (_Properties)
                {
                    if (!_Properties.TryGetValue(objType, out properties))
                    {
                        properties = objType.GetProperties().Where(property => property.CanWrite).ToList();
                        _Properties.Add(objType, properties);
                    }
                }

                var list = new List<T>(table.Rows.Count);

                foreach (var row in table.AsEnumerable().Skip(1))
                {
                    var obj = new T();

                    foreach (var prop in properties)
                    {
                        try
                        {
                            var propType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                            var safeValue = row[prop.Name] == null ? null : Convert.ChangeType(row[prop.Name], propType);

                            prop.SetValue(obj, safeValue, null);
                        }
                        catch
                        {
                            // ignored
                        }
                    }

                    list.Add(obj);
                }

                return list;
            }
            catch
            {
                return Enumerable.Empty<T>();
            }
        }


        #region -- Generic Insert --
        /*
        public static InsertRow()
        {
            DataTable dataTable = new DataTable();

            string columns = string.Join(",", dataTable.Columns.Cast<DataColumn>().Select(c => c.ColumnName));
            string values = string.Join(","
                , dataTable.Columns.Cast<DataColumn>().Select(c => string.Format("@{0}", c.ColumnName)));
            String sqlCommandInsert = string.Format("INSERT INTO dbo.RAW_DATA({0}) VALUES ({1})", columns, values);

            using (var con = new SqlConnection("ConnectionString"))
            using (var cmd = new SqlCommand(sqlCommandInsert, con))
            {
                con.Open();
                foreach (DataRow row in dataTable.Rows)
                {
                    cmd.Parameters.Clear();
                    foreach (DataColumn col in dataTable.Columns)
                        cmd.Parameters.AddWithValue("@" + col.ColumnName, row[col]);
                    int inserted = cmd.ExecuteNonQuery();
                }
            }
        }


        public static string UpdateExecute(DataTable dataTable, string TableName)
        {

            NpgsqlCommand cmd = null;
            string Result = String.Empty;

            try
            {

                if (dataTable.Columns.Contains("skinData")) dataTable.Columns.Remove("skinData");
                string columns = string.Join(",", dataTable.Columns.Cast<DataColumn>().Select(c => c.ColumnName));

                string values = string.Join(",", dataTable.Columns.Cast<DataColumn>().Select(c => string.Format("@{0}", c.ColumnName)));

                StringBuilder sqlCommandInsert = new StringBuilder();
                sqlCommandInsert.Append("Update " + TableName + " Set ");

                string[] TabCol = columns.Split(',');
                string[] TabVal = values.Split(',');

                for (int i = 0; i < TabCol.Length; i++)
                {
                    for (int j = 0; j < TabVal.Length; j++)
                    {
                        sqlCommandInsert.Append(TabCol[i] + " = " + TabVal[i] + ",");
                        break;
                    }
                }
                string NpgsqlCommandUpdate = sqlCommandInsert.ToString().TrimEnd(',');
                NpgsqlCommandUpdate += (" where " + TabCol[0] + "=" + TabVal[0]);


                using (var con = new NpgsqlConnection("Server=localhost;Port=5432;uid=uapp;pwd=Password;database=Test;"))
                {
                    con.Open();
                    foreach (DataRow row in dataTable.Rows)
                    {
                        cmd = new NpgsqlCommand(NpgsqlCommandUpdate.ToString(), con);
                        cmd.Parameters.Clear();
                        foreach (DataColumn col in dataTable.Columns)
                            cmd.Parameters.AddWithValue("@" + col.ColumnName, row[col]);

                        Result = cmd.ExecuteNonQuery().ToString();
                    }
                }
            }
            catch (Exception)
            {
                Result = "-1";
            }
            return Result;
        }
        */
        #endregion
    }
}
