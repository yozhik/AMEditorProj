using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Configuration;
using System.Data;

namespace DBManager
{
    public class SqlManager
    {
        
        protected static string ConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings["DataCollectionSystem"].ToString(); }
        }


        public static void ExecuteSQL(SqlCommand sqlCommand)
        {
            try
            {
                sqlCommand.Connection = new SqlConnection(ConnectionString);
                sqlCommand.Connection.Open();
                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR: " + ex.Message);
                throw;
            }
            finally
            {
                sqlCommand.Connection.Close();
            }
        }


        public static DataTable GetData(SqlCommand sqlCommand)
        {
            try
            {
                sqlCommand.Connection = new SqlConnection(ConnectionString);
                sqlCommand.Connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand);
                DataTable table = new DataTable();
                adapter.Fill(table);
                return table;
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR: " + ex.Message);
                throw;
            }
            finally
            {
                sqlCommand.Connection.Close();
            }
        }

        public SqlDataReader GetReader(SqlCommand sqlCommand, bool closeConnection)
        {
            try
            {
                sqlCommand.Connection = new SqlConnection(ConnectionString);
                sqlCommand.Connection.Open();
                return sqlCommand.ExecuteReader();
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR: " + ex.Message);
                throw;
            }
            finally
            {
                if (closeConnection)
                    sqlCommand.Connection.Close();
            }
        }
        

    }
}
