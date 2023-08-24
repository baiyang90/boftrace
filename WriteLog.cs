using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace boftrace
{
    internal class WriteLog:Form1
    {
        public static void writeLog5(string LogMessage)
        {
            try
            {
                string filename = System.DateTime.Now.ToString("yyyy-MM-dd")+"_5#炉";
                System.IO.Directory.CreateDirectory(".\\Log\\");
                StreamWriter w = File.AppendText(".\\Log\\" + filename + ".Log");
                w.WriteLine("{0}{1}", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":  ", LogMessage);
                w.Flush();
                w.Close();
            }
            catch (System.Exception err)
            {
                MessageBox.Show("日志错误" + err.Message);
            }
        }
        public static void writeLog6(string LogMessage)
        {
            try
            {
                string filename = System.DateTime.Now.ToString("yyyy-MM-dd") + "_6#炉";
                System.IO.Directory.CreateDirectory(".\\Log\\");
                StreamWriter w = File.AppendText(".\\Log\\" + filename + ".Log");
                w.WriteLine("{0}{1}", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":  ", LogMessage);
                w.Flush();
                w.Close();
            }
            catch (System.Exception err)
            {
                MessageBox.Show("日志错误" + err.Message);
            }
        }
        public static void writeLog7(string LogMessage)
        {
            try
            {
                string filename = System.DateTime.Now.ToString("yyyy-MM-dd") + "_7#炉";
                System.IO.Directory.CreateDirectory(".\\Log\\");
                StreamWriter w = File.AppendText(".\\Log\\" + filename + ".Log");
                w.WriteLine("{0}{1}", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":  ", LogMessage);
                w.Flush();
                w.Close();
            }
            catch (System.Exception err)
            {
                MessageBox.Show("日志错误" + err.Message);
            }
        }

    }
}
