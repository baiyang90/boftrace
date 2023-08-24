using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
namespace boftrace
{
    public class by_dbconnCGBOF
    {
        public static string ConnectString = "User Id=cgbof;Password=123456;Data Source=172.16.24.5/orcl;";
        //public static string ConnectString = "Data Source = (DESCRIPTION = (ADDRESS = (PROTOCOL = TCP)(HOST = 127.0.0.1)(PORT = 1521))(CONNECT_DATA = (SID = LGBOF))); User Id = lgbof; Password=123456;";
        static OracleConnection dConnection;
        //private OracleCommand dCommand; 
        //private OracleDataReader dReader;
        //private DataTable dtable;
        //private OracleDataAdapter dAdapter;

        //public By_Connection()
        //{
        //    ConnectString = "User Id=system;Password=Admin123456;Data Source=192.168.124.128:1521/BYDB;pooling=false;";
        //    //ConnectString = "server=CGCCM7;uid=ccm7;pwd=ccm7";
        //    dConnection = new OracleConnection(ConnectString);
        //    dConnection.Open();
        //}
        public static void OpenConnect()
        {
            dConnection = new OracleConnection(ConnectString);
            dConnection.Open();

        }
        public static void CloseConnect()
        {
            dConnection.Close();
        }
        public static OracleDataReader SqlToDataReader(string sql)
        {
            OracleCommand dCommand;
            OracleDataReader dReader;
            dCommand = new OracleCommand(sql, dConnection);
            dReader = dCommand.ExecuteReader();
            dCommand.Dispose();
            return dReader;
        }

        public static void ExeSql(string sql)
        {
            OracleCommand dCommand;
            //if (dConnection.State == ConnectionState.Open)
            {
                dCommand = new OracleCommand(sql, dConnection);
                dCommand.ExecuteNonQuery();
                dCommand.Dispose();
            }

        }


        public static DataSet SqlToDataSet(string sql)
        {


            OracleDataAdapter dAdapter;
            DataSet dSet = new DataSet();

            dAdapter = new OracleDataAdapter(sql, dConnection);

            dAdapter.Fill(dSet);

            dAdapter.Dispose();
            return dSet;

        }
        public static DataTable SqlToDataTableBy(string sql)
        {
            DataTable dtable;
            OracleDataAdapter dAdapter;
            DataSet dSet = new DataSet();

            dAdapter = new OracleDataAdapter(sql, dConnection);

            dAdapter.Fill(dSet);
            dtable = dSet.Tables[0];
            dAdapter.Dispose();
            dSet.Dispose();
            return dtable;
        }
    }
}
