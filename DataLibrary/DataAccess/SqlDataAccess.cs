using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Dapper;
using System.Data.SqlClient;
using System.Configuration;
using System.ComponentModel;

namespace DataLibrary.DataAccess
{
    public static class SqlDataAccess
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionName"></param>
        /// <returns></returns>
        public static string GetConnectionString(string connectionName = "MVCDemoDB")
        {
            return ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static List<T>LoadData<T>(string sqlQuery)
        {
            using (IDbConnection dbConnection = new SqlConnection(GetConnectionString()))
            {
                return dbConnection.Query<T>(sqlQuery).ToList();
            }
        }
       
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlQuery"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int SaveData<T>(string sqlQuery, T data)
        {
            using (IDbConnection dbConnection = new SqlConnection(GetConnectionString()))
            {
                return dbConnection.Execute(sqlQuery, data);
            }
        }


        ///
        public static bool SelectOneData<T>(string sqlQuery, int id)
        {
            using (IDbConnection dbConnection = new SqlConnection(GetConnectionString()))
            {
                var exist = dbConnection.Query<bool>(sqlQuery, new { EmployeId = id });
                var val = exist.FirstOrDefault();
                return true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlQuery"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int DeleteData<T>(string sqlQuery, int id)
        {
            using (IDbConnection dbConnection = new SqlConnection(GetConnectionString()))
            {
                return dbConnection.Execute(sqlQuery, new { EmployeId = id });
            }
        }

        #region -- TEST DELETE METHODE !!!!!!!!!!!! --
        public static int Delete<T>(string sqlQuery, int id)
        {
            using (IDbConnection dbConnection = new SqlConnection(GetConnectionString()))
            {
                return 0;
            }
        }

        public static int Delete<T>(string sqlQuery, T entityToDelete)
        {
            return 0;
        }

        public static int DeleteList<T>(string sqlQuery, object whereConditions, 
            IDbTransaction transaction = null, int? commandTimeout = null)
        {
            return 0;
        }

        public static int DeleteList<T>(string sqlQuery, string conditions, 
            object parameters = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            return 0;
        }
        #endregion
        
        #region ---   ---
        // your data table
        private static DataTable dataTable = new DataTable();
        
        // your method to pull data from database to datatable   
        public static DataTable PullDataOnDataTable(string sqlQuery)
        {
            using (var sqlConnection = new SqlConnection(GetConnectionString()))
            using(var slqCommand = new SqlCommand(sqlQuery, sqlConnection))
            {
                sqlConnection.Open();

                dataTable.Clear();
                // create data adapter
                using (var da = new SqlDataAdapter(slqCommand))
                {
                    // this will query your database and return the result to your datatable
                    da.Fill(dataTable);
                }
                return dataTable;                
            }
        }

        /// <summary>
        /// --- !!!!!!!!!!!!!!!!!!!!!!!!!!! ---
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="tableName"></param>
        /// <param name="table"></param>
        public static void BulkInsertDataTable(string tableName, DataTable table)
        {
            using (SqlConnection connection = new SqlConnection(GetConnectionString()))
            {
                SqlBulkCopy bulkCopy =
                    new SqlBulkCopy
                    (
                        connection,
                        SqlBulkCopyOptions.TableLock |
                        SqlBulkCopyOptions.FireTriggers |
                        SqlBulkCopyOptions.UseInternalTransaction,
                        null
                    );

                bulkCopy.DestinationTableName = tableName;
                connection.Open();

                bulkCopy.WriteToServer(table);
                connection.Close();
            }
        }

        /// <summary>
        /// --   --
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="employeeModel"></param>
        public static void InsertListOfObject(string tableName, List<Models.EmployeeModel> employeeModel)
        {
            using (var connection = new SqlConnection(GetConnectionString()))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();

                using (var bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, transaction))
                {
                    bulkCopy.BatchSize = 100;
                    bulkCopy.DestinationTableName = tableName;
                    try
                    {
                        bulkCopy.WriteToServer(employeeModel.AsDataTable());
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine(exception.ToString());
                        transaction.Rollback();
                        connection.Close();
                    }
                }

                transaction.Commit();
            }
        }

        #endregion
    }

    #region ----   --
    /// <summary>
    /// --   --
    /// </summary>
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// --  --
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static DataTable AsDataTable<T>(this IEnumerable<T> data)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            var table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;
        }
    }
    #endregion
}
