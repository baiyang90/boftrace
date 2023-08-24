using System;
using System.Collections.Generic;
using System.Text;
//using System.Data.OracleClient;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System.Configuration;

namespace boftrace
{
    public class DataAccess
    {
        public string ConnectString;
        private OracleConnection dConnection;
        //private OracleCommand dCommand; 
        //private OracleDataReader dReader;
        //private DataTable dtable;
        //private OracleDataAdapter dAdapter;

        public DataAccess()
        {
          ConnectString = "User Id=lgbof;Password=123456;Data Source=(DESCRIPTION =(ADDRESS = (PROTOCOL = TCP)(HOST = 172.16.24.1)(PORT = 1521))(CONNECT_DATA =(SERVER = DEDICATED)(SERVICE_NAME = orcl)))";
            dConnection = new OracleConnection(ConnectString);
            dConnection.Open();
        }

        public OracleDataReader SqlToDataReader(string sql)
        {
            OracleCommand dCommand;
            OracleDataReader dReader;
            //OracleCommand dCommand = new OracleCommand(sql, dConnection);
            dCommand = new OracleCommand(sql, dConnection);
            dReader = dCommand.ExecuteReader();
            dCommand.Dispose();
            return dReader;
            
        }

        public void ExeSql(string sql)
        {
            OracleCommand dCommand;
            //if (dConnection.State == ConnectionState.Open)
            {
                dCommand = new OracleCommand(sql, dConnection);
                dCommand.ExecuteNonQuery();
                dCommand.Dispose();
            }
        }

        public DataTable SqlToDataTable(string sql, string tableName)
        {
            DataTable dtable;
            OracleDataAdapter dAdapter;
            DataSet dSet = new DataSet();

            dAdapter = new OracleDataAdapter(sql, dConnection);

            dAdapter.Fill(dSet, tableName);
            dtable = dSet.Tables[tableName];
            dAdapter.Dispose();
            dSet.Dispose();
            return dtable;


        }

    }

}
