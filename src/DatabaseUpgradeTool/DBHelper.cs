using System;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;


namespace DatabaseUpgradeTool
{
    public class DBHelper
    {
        private readonly string _connectionString;


        public DBHelper(string connectionString)
        {
            _connectionString = connectionString;
        }


        public void ExecuteSchemaUpdate(string commandText)
        {
            Regex regex = new Regex("^GO", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            string[] subCommands = regex.Split(commandText);

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();
                using (SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.Connection = connection;
                    cmd.Transaction = transaction;

                    foreach (string command in subCommands)
                    {
                        if (command.Length <= 0)
                            continue;

                        cmd.CommandText = command;
                        cmd.CommandType = CommandType.Text;

                        try
                        {
                            cmd.ExecuteNonQuery();
                        }
                        catch (SqlException)
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }

                transaction.Commit();
            }
        }


        public void ExecuteNonQuery(string commandText, params SqlParameter[] parameters)
        {
            using (SqlConnection cnn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand(commandText, cnn)
                {
                    CommandType = CommandType.Text
                };
                foreach (SqlParameter parameter in parameters)
                {
                    cmd.Parameters.Add(parameter);
                }

                cnn.Open();
                cmd.ExecuteNonQuery();
            }
        }


        public T ExecuteScalar<T>(string commandText, params SqlParameter[] parameters)
        {
            using (SqlConnection cnn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand(commandText, cnn)
                {
                    CommandType = CommandType.Text
                };
                foreach (SqlParameter parameter in parameters)
                {
                    cmd.Parameters.Add(parameter);
                }

                cnn.Open();

                return (T)cmd.ExecuteScalar();
            }
        }
    }
}
