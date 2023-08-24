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
        public static string ConnectStringnew = "User Id=lgbof;Password=123456;Data Source=172.16.24.3/orcl;Connection Timeout=3;";
        public static string ConnectStringold = "User Id=cgbof;Password=123456;Data Source=172.16.24.5/orcl;Connection Timeout=3;";
        //public static string ConnectString = "Data Source = (DESCRIPTION = (ADDRESS = (PROTOCOL = TCP)(HOST = 127.0.0.1)(PORT = 1521))(CONNECT_DATA = (SID = LGBOF))); User Id = lgbof; Password=123456;";
        static OracleConnection dConnection1;
        static OracleConnection dConnection2;

        public static void OpenConnect()
        {
            try {

                dConnection1 = new OracleConnection(ConnectStringnew);
                dConnection1.Open();
                dConnection2 = new OracleConnection(ConnectStringold);
                dConnection2.Open();
            }
            catch (Exception err)
            {
                
            }

        }
        public static void OpenConnect1()
        {
            
            

                dConnection1 = new OracleConnection(ConnectStringnew);
                dConnection1.Open();
                
            

        }
        public static void OpenConnect2()
        {


                
                dConnection2 = new OracleConnection(ConnectStringold);
                dConnection2.Open();

        }
        public static void CloseConnect()
        {
            try
            {
                dConnection1.Close();
                dConnection2.Close();
            }
            catch (Exception err)
            {

            }
        }

        public static void writeNew(string sql)
        {
            OracleCommand dCommand;
            dCommand = new OracleCommand(sql, dConnection1);
                dCommand.ExecuteNonQuery();
                dCommand.Dispose();


        }
        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="sql">存储过程名</param>
        public static void run_Produce_new(string sql)
        {
            OracleCommand dCommandpd;
            dCommandpd = new OracleCommand(sql, dConnection1);
            dCommandpd.CommandType = CommandType.StoredProcedure;
            dCommandpd.ExecuteNonQuery();
            dCommandpd.Dispose();

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
