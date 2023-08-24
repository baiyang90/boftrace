using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;

namespace boftrace
{
    internal class DBconn
    {
        public static string ConnectStringnew = "User Id=cgbof;Password=123456;Data Source=172.16.24.5/orcl;";
        public static string ConnectStringold = "User Id=lgbof;Password=123456;Data Source=172.16.24.3/orcl;";
        //public static string ConnectString = "Data Source = (DESCRIPTION = (ADDRESS = (PROTOCOL = TCP)(HOST = 127.0.0.1)(PORT = 1521))(CONNECT_DATA = (SID = LGBOF))); User Id = lgbof; Password=123456;";
        static OracleConnection dConnection1;
        static OracleConnection dConnection2;

        public static void OpenConnect()
        {
            dConnection1 = new OracleConnection(ConnectStringnew);
            dConnection1.Open();
            dConnection2 = new OracleConnection(ConnectStringnew);
            dConnection2.Open();

        }
        public static void CloseConnect()
        {
            dConnection1.Close();
            dConnection2.Close();
        }

        public static void writeNew(string sql)
        {
            OracleCommand dCommand;
            //if (dConnection.State == ConnectionState.Open)
            {
                dCommand = new OracleCommand(sql, dConnection1);
                dCommand.ExecuteNonQuery();
                dCommand.Dispose();
            }

        }

        public static DataTable readNew(string sql)
        {
            DataTable dtable;
            OracleDataAdapter dAdapter;
            DataSet dSet = new DataSet();

            dAdapter = new OracleDataAdapter(sql, dConnection1);

            dAdapter.Fill(dSet);
            dtable = dSet.Tables[0];
            dAdapter.Dispose();
            dSet.Dispose();
            return dtable;

        }
        public static void writeOld(string sql)
        {
            OracleCommand dCommand;
            //if (dConnection.State == ConnectionState.Open)
            {
                dCommand = new OracleCommand(sql, dConnection2);
                dCommand.ExecuteNonQuery();
                dCommand.Dispose();
            }

        }

        public static DataTable readOld(string sql)
        {
            DataTable dtable;
            OracleDataAdapter dAdapter;
            DataSet dSet = new DataSet();

            dAdapter = new OracleDataAdapter(sql, dConnection2);

            dAdapter.Fill(dSet);
            dtable = dSet.Tables[0];
            dAdapter.Dispose();
            dSet.Dispose();
            return dtable;

        }
    }
}
