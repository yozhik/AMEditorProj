using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DBManager
{
    public class ComboBoxAssembler : BaseAssembler
    {
        public static DataTable Fill()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT * FROM Realizations";
            return GetData(cmd);
        }
    }
}
