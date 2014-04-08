using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DBManager
{
    public class GridViewAssembler: BaseAssembler
    {
        public static DataTable UpdateOperators()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT ID, Name, Description, AlgorithmModel, OperatorModel FROM Operators";
            return GetData(cmd);
        }

        public static DataTable UpdateOperations()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT * FROM Operations";
            return GetData(cmd);
        }
    }
}
