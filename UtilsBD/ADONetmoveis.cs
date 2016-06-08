using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace WebCrawlerNetImoveis.UtilsBD
{
    class ADONetmoveis
    {
        string connectionString = @"Data Source=ALAN-PC;Initial Catalog=BDNetMoveis;Integrated Security=True";

        public void Execute(string sql, Dictionary<string, object> parameters)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand command = new SqlCommand();
                command.CommandText = sql;

                foreach (var param in parameters)
                {
                    command.Parameters.Add(new SqlParameter(param.Key, param.Value));
                }

                command.Connection = conn;
                command.ExecuteNonQuery();
            }
        }

        public bool ExistImovel(string sql, Dictionary<string, object> parameters)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand command = new SqlCommand();
                command.CommandText = sql;

                foreach (var param in parameters)
                {
                    command.Parameters.Add(new SqlParameter(param.Key, param.Value));
                }

                command.Connection = conn;

                IDataReader reader = command.ExecuteReader();

                if (reader.Read())
                    return true;
                else
                    return false;
            }
        }
    }
}
