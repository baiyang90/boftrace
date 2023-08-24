using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace boftrace
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }
        int bof5newheatid;
        int bof5thisheatid;
        int bof6newheatid;
        int bof6thisheatid;
        int bof7newheatid;
        int bof7thisheatid;
        int bof5ledstatus=0,bof6ledstatus=0,bof7ledstatus=0;
        DataTable dt_bofstatus;
        DataTable dt_by_shoudong;
        DataTable dt_by_bofstatus;
        string bof5start_l1;
        string bof6start_l1;
        string bof7start_l1;
        string bof5o2start_l1;
        string bof5o2startwait = "0";
        string bof6o2startwait = "0";
        string bof7o2startwait = "0";
        string bof6o2start_l1;
        string bof7o2start_l1;
        string bof5o2end_l1;
        string bof5o2endwait = "0";
        string bof6o2endwait = "0";
        string bof7o2endwait = "0";
        string bof6o2end_l1;
        string bof7o2end_l1;
        string bof5cgstart_l1;
        string bof5cgstartwait = "0";
        string bof6cgstartwait = "0";
        string bof7cgstartwait = "0";
        string bof6cgstart_l1;
        string bof7cgstart_l1;
        string bof5cgend_l1;
        string bof6cgend_l1;
        string bof7cgend_l1;
        string bof5end_l1;
        string bof6end_l1;
        string bof7end_l1;
        string bof5_o2start_again_L1, bof6_o2start_again_L1, bof7_o2start_again_L1, bof5_o2end_again_L1, bof6_o2end_again_L1, bof7_o2end_again_L1;
        string bof_no_history;
        string sd_bof5start, sd_bof5o2start, sd_bof5o2end, sd_bof5cgstart, sd_bof5cgend, sd_bof5end;
        string sd_bof6start, sd_bof6o2start, sd_bof6o2end, sd_bof6cgstart, sd_bof6cgend, sd_bof6end;
        string sd_bof7start, sd_bof7o2start, sd_bof7o2end, sd_bof7cgstart, sd_bof7cgend, sd_bof7end;
        int run_once=0;
        int[] bofstartcount = { 0, 0, 0 };
        
        private void Form1_Load(object sender, EventArgs e)
        {
            DBconn.OpenConnect();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = DBconn.readNew("select to_char(t.create_time,'yyyy/mm/dd hh24:mi:ss') as create_time,t.info from BY_INFOMATION t order by t.create_time desc");
            dataGridView2.DataSource = DBconn.readNew("select to_char(t.create_time,'yyyy/mm/dd hh24:mi:ss') as create_time,t.message from ERR_LOG t order by t.create_time desc");
        }

        int sd_bof5o2start_again = 0, sd_bof5o2end_again = 0;
        int sd_bof6o2start_again = 0, sd_bof6o2end_again = 0;
        /// <summary>
        /// 手动计算吹氧时间
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            if (tB_cytime_heatid.Text.Length == 8)
            {
                if (tB_cytime_heatid.Text.Substring(2, 1) == "5")
                {
                    o2jisuan_sd("5", tB_cytime_heatid.Text);
                    MessageBox.Show(tB_cytime_heatid.Text+"计算完成");
                }
                else if(tB_cytime_heatid.Text.Substring(2, 1) == "6")
                {
                    o2jisuan_sd("6", tB_cytime_heatid.Text);
                    MessageBox.Show(tB_cytime_heatid.Text + "计算完成");
                }
                else if(tB_cytime_heatid.Text.Substring(2, 1) == "7")
                {
                    o2jisuan_sd("7", tB_cytime_heatid.Text);
                    MessageBox.Show(tB_cytime_heatid.Text + "计算完成");
                }
            }
            else
            {
                MessageBox.Show("输入的炉号长度不为8");
            }
        }

        int sd_bof7o2start_again = 0, sd_bof7o2end_again = 0;

        private void button22_Click(object sender, EventArgs e)
        {
            sj_duquhisDB();
        }

        private void button23_Click(object sender, EventArgs e)
        {
            write_hisDB();
        }

        string bof5_trace_enable_l1, bof6_trace_enable_l1, bof7_trace_enable_l1;
        string endheat_no5 = "";
        string endheat_no6 = "";
        string endheat_no7 = "";
        public void WritelogAuto(string LogMessage)
        {
            try
            {
                string filename = System.DateTime.Now.ToString("yyyy-MM-dd") + "_错误日志";
                System.IO.Directory.CreateDirectory(".\\Log\\");
                StreamWriter w = File.AppendText(".\\Log\\" + filename + ".Log");
                w.WriteLine("{0}{1}", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":  ", LogMessage);
                w.Flush();
                w.Close();
                TB_all.AppendText("|_" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + LogMessage+"_|");
            }
            catch (System.Exception err)
            {
                MessageBox.Show("日志错误" + err.Message);
            }
        }
        /// <summary>
        /// 日志功能
        /// </summary>
        /// <param name="BSNo">炉座号</param>
        /// <param name="LogMessage">日志内容</param>
        public void WritelogAuto(string BSNo, string LogMessage)
        {
            try
            {
                string filename = System.DateTime.Now.ToString("yyyy-MM-dd") + "_" + BSNo + "#炉";
                System.IO.Directory.CreateDirectory(".\\Log\\");
                StreamWriter w = File.AppendText(".\\Log\\" + filename + ".Log");
                w.WriteLine("{0}{1}", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":  ", LogMessage);
                w.Flush();
                w.Close();
                if (BSNo == "5")
                    by_TB5_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + LogMessage);
                if (BSNo == "6")
                    by_TB6_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + LogMessage);
                if (BSNo == "7")
                    by_TB7_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + LogMessage);
            }
            catch (System.Exception err)
            {
                MessageBox.Show("日志错误" + err.Message);
            }
        }
        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="BSNo">炉座号</param>
        /// <param name="LogMessage">日志内容</param>
        /// <param name="level">日志等级</param>
        public void WritelogAuto(string BSNo, string LogMessage,int level)
        {
            try
            {
                string filename = System.DateTime.Now.ToString("yyyy-MM-dd") + "_" + BSNo + "#炉";
                System.IO.Directory.CreateDirectory(".\\Log\\");
                StreamWriter w = File.AppendText(".\\Log\\" + filename + ".Log");
                w.WriteLine("{0}{1}", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":  ", LogMessage);
                w.Flush();
                w.Close();
                if (BSNo == "5")
                {
                    by_TB5_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + LogMessage);
                    DBconn.writeNew("insert into TRACE_INFO(create_time,info_level,info_1,bof_station) values(sysdate,'"+level+"','"+ LogMessage + "','"+ BSNo + "')");
                }
                if (BSNo == "6")
                {
                    by_TB6_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + LogMessage);
                    DBconn.writeNew("insert into TRACE_INFO(create_time,info_level,info_1,bof_station) values(sysdate,'" + level + "','" + LogMessage + "','" + BSNo + "')");
                }
                if (BSNo == "7")
                {
                    by_TB7_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + LogMessage);
                    DBconn.writeNew("insert into TRACE_INFO(create_time,info_level,info_1,bof_station) values(sysdate,'" + level + "','" + LogMessage + "','" + BSNo + "')");
                }
            }
            catch (System.Exception err)
            {
                MessageBox.Show("日志错误" + err.Message);
            }
        }
        public void ChangeTextBox(string BSNo, string bofstatus, string bof_no)
        {
            if (BSNo == "5")
            {
                TB_bof5status.Text = bofstatus;
                TB_bof5heatid.Text = bof_no;
            }
            if (BSNo == "6")
            {
                TB_bof6status.Text = bofstatus;
                TB_bof6heatid.Text = bof_no;
            }
            if (BSNo == "7")
            {
                TB_bof7status.Text = bofstatus;
                TB_bof7heatid.Text = bof_no;
            }
        }

        /// <summary>
        /// 定时发送炉次实际
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        int delete_count = 0;
        private void timer2_Tick(object sender, EventArgs e)
        {
            try
            {
                
                //DBconn.OpenConnect();
                DBconn.writeNew("call by_send_history()");
                delete_count++;
                if (delete_count >= 2000)//清理过多数据
                {
                    delete_count = 0;
                    DBconn.writeNew("call delete_so_much_data()");
                }
                //DBconn.CloseConnect();
            }
            catch (Exception err)
            {
                TB_all.AppendText("存储过程call by_send_history()执行错误" + err.Message+"|");

            }
        }



        #region 旧跟踪程序弃用
        /// <summary>
        /// 5#炉跟踪
        /// </summary>
        /// 

        private void bof5trace()
        {

            string heat_id = "";
            string heat_id_L1;
            DataTable dt;
            DataTable dt_zlsj_temp_5;
            DataTable dt_cgjh_5;
            string pono, plan_no, st_no, bof_no;
            dt_zlsj_temp_5 = DBconn.readNew("select * from zlsj_temp t  where t.by_station_no=5");
            bof_no_history = DBconn.readNew("select max(heat_no) from YZZT t  where t.station_no=5").Rows[0][0].ToString();
            if (dt_zlsj_temp_5.Rows.Count > 0)
            {
                heat_id = dt_zlsj_temp_5.Rows[0]["HEAT_NO"].ToString();
            }

            if (bof5start_l1 == "1" || sd_bof5start == "1")//-------------------------------------------------------炉次开始

            {
                #region 炉次开始
                try
                {
                    WriteLog.writeLog5("------炉次开始------");
                    by_TB5_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": ------炉次开始------");

                    dt_zlsj_temp_5 = DBconn.readNew(" select * from zlsj_temp where BY_STATION_NO ='5'");
                    if (Convert.ToInt32(dt_zlsj_temp_5.Rows[0]["STATUS"]) > 4 && heat_id != "")          //强制结束上一炉
                    {

                        pono = dt_zlsj_temp_5.Rows[0]["pono"].ToString();
                        plan_no = dt_zlsj_temp_5.Rows[0]["sm_plan_no"].ToString();
                        st_no = dt_zlsj_temp_5.Rows[0]["st_no"].ToString();
                        heat_id = dt_zlsj_temp_5.Rows[0]["HEAT_NO"].ToString();
                        WriteLog.writeLog5("上炉未正常结束强制结束" + heat_id);
                        by_TB5_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": 上炉未正常结束强制结束" + heat_id);

                        DBconn.writeNew("insert into yzzt(sm_plan_no, proc_no, station_id, station_no, heat_no, pono, st_no, run_signal, run_time)values('" + plan_no + "'," + heat_id + ",'B','5'," + heat_id + ",'" + pono + "','" + st_no + "','356','" + DateTime.Now.ToString("yyyyMMddHHmmss") + "')");
                        WriteLog.writeLog5("写入356运转状态成功" + heat_id + "|" + pono + "|" + plan_no);
                        by_TB5_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": 写入356运转状态成功" + heat_id + "|" + pono + "|" + plan_no);

                        DBconn.writeNew("update zlsj_temp set STATUS ='6',STATUS_NEW ='1'  where BY_STATION_NO ='5'");
                        WriteLog.writeLog5("更新转炉实际状态为6成功");
                        by_TB5_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": 更新转炉实际状态为6成功");

                        DBconn.writeNew("update zlsj_temp set STATUS ='0',HEAT_NO ='' where BY_STATION_NO ='5'");
                        DBconn.writeNew("update BY_BOFSTATUS set bof5_status_now=7");
                        WriteLog.writeLog5("强制结束完成");
                        by_TB5_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": 强制结束完成");

                    }
                    //--------------------------------------
                    heat_id_L1 = DBconn.readOld("select heatid from bof5status").Rows[0][0].ToString();//从一级取当前炉号
                    WriteLog.writeLog5(":从L1取炉号" + heat_id_L1);
                    by_TB5_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": 从L1取炉号" + heat_id_L1);
                    //--------------------------------------
                    if (Convert.ToInt32(heat_id_L1) > Convert.ToInt32(bof_no_history))//炉号正常加一
                    {
                        dt_cgjh_5 = DBconn.readNew("select * from cgjh where bof_no='5' and status is null order by to_number(plan_no)");  //读取最近一个未用的计划
                        if (dt_cgjh_5.Rows.Count > 0)
                        {
                            bof_no = dt_cgjh_5.Rows[0]["bof_no"].ToString();
                            pono = dt_cgjh_5.Rows[0]["pono"].ToString();
                            plan_no = dt_cgjh_5.Rows[0]["plan_no"].ToString();
                            st_no = dt_cgjh_5.Rows[0]["st_no"].ToString();
                            WriteLog.writeLog5("炉次开始读取计划成功" + bof_no + "|" + pono + "|" + plan_no);
                            by_TB5_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": 炉次开始读取计划成功" + bof_no + "|" + pono + "|" + plan_no);

                            DBconn.writeNew("insert into yzzt(sm_plan_no, proc_no, station_id, station_no, heat_no, pono, st_no, run_signal, run_time)values('" + plan_no + "'," + heat_id_L1 + ",'B','5'," + heat_id_L1 + ",'" + pono + "','" + st_no + "','351','" + DateTime.Now.ToString("yyyyMMddHHmmss") + "')");
                            WriteLog.writeLog5("写入运转状态：" + plan_no + "|" + heat_id_L1 + "|" + pono + "成功");
                            by_TB5_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": 写入351运转状态：" + plan_no + "|" + heat_id_L1 + "|" + pono + "成功");

                            DBconn.writeNew("update cgjh set status ='1',heat_no='" + heat_id_L1 + "' where plan_no ='" + plan_no + "'");
                            WriteLog.writeLog5("更新出钢计划标志位成功");
                            by_TB5_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": 更新出钢计划标志位成功");

                            DBconn.writeNew("update zlsj_temp set SM_PLAN_NO ='" + plan_no + "',pono ='" + pono + "',HEAT_NO ='" + heat_id_L1 + "', ST_NO ='" + st_no + "',STATION_NO='" + bof_no + "',PROD_DATE='" + DateTime.Now.ToString("yyyyMMdd") + "', STATUS ='1' where BY_STATION_NO ='5'");
                            WriteLog.writeLog5("更新转炉实际1成功");
                            by_TB5_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": 更新转炉实际1成功");

                            TB_bof5status.Text = "炉次开始";
                            TB_bof5heatid.Text = heat_id_L1.ToString();
                            DBconn.writeNew("update by_bofstatus set bof5_start=0,bof5_o2start=0,bof5_o2end=0,bof5_cgstart=0,bof5_cgend=0");//炉次开始初始化状态位
                            DBconn.writeNew("update by_shoudong set sd_bof5start=0");
                            DBconn.writeNew("update BY_BOFSTATUS set bof5_status_now=1");
                            WriteLog.writeLog5(heat_id_L1 + "------炉次开始完成------");
                            by_TB5_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + heat_id_L1 + "------炉次开始完成------");
                        }
                        else
                        {
                            DBconn.writeNew("update by_bofstatus set bof5_start=0");
                            DBconn.writeNew("update by_shoudong set sd_bof5start=0");
                            WriteLog.writeLog5("炉次开始失败，没有计划，bof5_start已置0");
                            by_TB5_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": 炉次开始失败，没有计划，bof5_start已置0");
                        }

                    }
                    else
                    {
                        DBconn.writeNew("update by_bofstatus set bof5_start=0");
                        DBconn.writeNew("update by_shoudong set sd_bof5start=0");
                        WriteLog.writeLog5("L1炉号" + heat_id_L1 + "不大于运转状态最大炉号" + bof_no_history + "炉次开始失败");
                        by_TB5_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "L1炉号" + heat_id_L1 + "不大于运转状态最大炉号" + bof_no_history + "炉次开始失败");
                    }
                }
                catch (Exception err)
                {
                    WriteLog.writeLog5("炉次开始运行失败" + err.Message);
                    DBconn.writeNew("update by_bofstatus set bof5_start=0");
                    DBconn.writeNew("update by_shoudong set sd_bof5start=0");
                    by_TB5_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + err.Message + "炉次开始运行失败" + heat_id);
                }
                #endregion
            }
            if (bof5o2start_l1 == "1" || sd_bof5o2start == "1")//-------------------------------------------------------吹氧开始

            {
                #region 吹氧开始
                try
                {
                    WriteLog.writeLog5("------吹氧开始------");
                    by_TB5_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": ------吹氧开始------");
                    dt_zlsj_temp_5 = DBconn.readNew("select * from zlsj_temp where by_station_no ='5'");
                    if (Convert.ToInt32(dt_zlsj_temp_5.Rows[0]["STATUS"]) < 3 && heat_id != "")//正常炉次开始
                    {
                        WriteLog.writeLog5(heat_id + "正常吹氧开始");
                        by_TB5_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + heat_id + "正常吹氧开始");
                        pono = dt_zlsj_temp_5.Rows[0]["pono"].ToString();
                        plan_no = dt_zlsj_temp_5.Rows[0]["SM_PLAN_NO"].ToString();
                        st_no = dt_zlsj_temp_5.Rows[0]["st_no"].ToString();

                        DBconn.writeNew("insert into yzzt(sm_plan_no, proc_no, station_id, station_no, heat_no, pono, st_no, run_signal, run_time)values('" + plan_no + "'," + heat_id + ",'B','5'," + heat_id + ",'" + pono + "','" + st_no + "','352','" + DateTime.Now.ToString("yyyyMMddHHmmss") + "')");
                        DBconn.writeNew("insert into yzzt(sm_plan_no, proc_no, station_id, station_no, heat_no, pono, st_no, run_signal, run_time)values('" + plan_no + "'," + heat_id + ",'B','5'," + heat_id + ",'" + pono + "','" + st_no + "','353','" + DateTime.Now.ToString("yyyyMMddHHmmss") + "')");
                        WriteLog.writeLog5("写入运转状态：" + plan_no + "|" + heat_id + "|" + pono + "成功");
                        by_TB5_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": 写入352，353运转状态：" + plan_no + "|" + heat_id + "|" + pono + "成功");

                        DBconn.writeNew("update zlsj_temp set STATUS ='3' where by_station_no ='5'");
                        WriteLog.writeLog5(heat_id + ":更新炉次实际3成功");
                        by_TB5_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + heat_id + "更新炉次实际3成功");

                        TB_bof5status.Text = "吹氧开始";
                        DBconn.writeNew("update BY_BOFSTATUS set bof5_status_now=3");
                        DBconn.writeNew("update by_bofstatus set bof5_start=0");
                        DBconn.writeNew("update by_bofstatus set bof5_o2start=0");
                        DBconn.writeNew("update by_shoudong set sd_bof5o2start=0");
                        WriteLog.writeLog5("------吹氧开始完成------");
                        by_TB5_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": ------吹氧开始完成------");

                    }
                    else//不正常开始 炉次开始信号没有来
                    {
                        WriteLog.writeLog5(":吹氧异常开始");
                        by_TB5_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": 吹氧异常开始");

                        dt_zlsj_temp_5 = DBconn.readNew(" select * from zlsj_temp where by_station_no ='5'");

                        WriteLog.writeLog5(":判断上炉是否结束");
                        by_TB5_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": 判断上炉是否结束");
                        if (Convert.ToInt32(dt_zlsj_temp_5.Rows[0]["STATUS"]) > 4)          //强制结束上一炉
                        {
                            WriteLog.writeLog5(heat_id + ":上炉未结束");
                            by_TB5_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + heat_id + "上一炉未结束");
                            pono = dt_zlsj_temp_5.Rows[0]["pono"].ToString();
                            plan_no = dt_zlsj_temp_5.Rows[0]["sm_plan_no"].ToString();
                            st_no = dt_zlsj_temp_5.Rows[0]["st_no"].ToString();
                            heat_id = dt_zlsj_temp_5.Rows[0]["HEAT_NO"].ToString();
                            DBconn.writeNew("insert into yzzt(sm_plan_no, proc_no, station_id, station_no, heat_no, pono, st_no, run_signal, run_time)values('" + plan_no + "'," + heat_id + ",'B','5'," + heat_id + ",'" + pono + "','" + st_no + "','356','" + DateTime.Now.ToString("yyyyMMddHHmmss") + "')");
                            WriteLog.writeLog5("写入356运转状态成功" + heat_id + "|" + pono + "|" + plan_no);
                            by_TB5_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": 写入356运转状态成功" + heat_id + "|" + pono + "|" + plan_no);

                            DBconn.writeNew("update zlsj_temp set STATUS ='6',STATUS_NEW ='1'  where BY_STATION_NO ='5'");
                            DBconn.writeNew("update BY_BOFSTATUS set bof5_status_now=7");
                            WriteLog.writeLog5("更新转炉实际状态为6成功");
                            by_TB5_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": 更新转炉实际状态为6成功");

                            DBconn.writeNew("update zlsj_temp set STATUS ='0',HEAT_NO ='' where BY_STATION_NO ='5'");
                            WriteLog.writeLog5("强制结束完成");
                            by_TB5_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": 强制结束完成");
                        }
                        else
                        {
                            WriteLog.writeLog5(heat_id + ":上炉成功结束，炉次开始信号没来，模拟炉次开始");
                            by_TB5_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + heat_id + "上炉成功结束，炉次开始信号没来，模拟炉次开始");
                            //---------------------------------------
                            heat_id_L1 = DBconn.readOld("select heatid from bof5status").Rows[0][0].ToString();//从一级取当前炉号
                            WriteLog.writeLog5(":从L1取炉号" + heat_id_L1);
                            by_TB5_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": 从L1取炉号" + heat_id_L1);
                            //---------------------------------------
                            if (Convert.ToInt32(heat_id_L1) > Convert.ToInt32(bof_no_history))//炉号正常加一
                            {

                                dt_cgjh_5 = DBconn.readNew("select * from cgjh where bof_no='5' and status is null order by to_number(plan_no)");  //读取最近一个未用的计划
                                if (dt_cgjh_5.Rows.Count > 0)
                                {
                                    bof_no = dt_cgjh_5.Rows[0]["bof_no"].ToString();
                                    pono = dt_cgjh_5.Rows[0]["pono"].ToString();
                                    plan_no = dt_cgjh_5.Rows[0]["plan_no"].ToString();
                                    st_no = dt_cgjh_5.Rows[0]["st_no"].ToString();
                                    WriteLog.writeLog5("炉次在吹氧开始读取计划成功" + bof_no + "|" + pono + "|" + plan_no);
                                    by_TB5_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": 炉次在吹氧开始读取计划成功" + bof_no + "|" + pono + "|" + plan_no);

                                    DBconn.writeNew("insert into yzzt(sm_plan_no, proc_no, station_id, station_no, heat_no, pono, st_no, run_signal, run_time)values('" + plan_no + "'," + heat_id_L1 + ",'B','5'," + heat_id_L1 + ",'" + pono + "','" + st_no + "','351','" + DateTime.Now.ToString("yyyyMMddHHmmss") + "')");
                                    WriteLog.writeLog5("写入运转状态：" + plan_no + "|" + heat_id_L1 + "|" + pono + "成功");
                                    by_TB5_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": 写入351运转状态：" + plan_no + "|" + heat_id_L1 + "|" + pono + "成功");

                                    DBconn.writeNew("update cgjh set status ='1',heat_no='" + heat_id_L1 + "' where plan_no ='" + plan_no + "'");
                                    WriteLog.writeLog5("更新出钢计划1成功");
                                    by_TB5_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": 更新出钢计划1成功");

                                    DBconn.writeNew("update zlsj_temp set SM_PLAN_NO ='" + plan_no + "',pono ='" + pono + "',HEAT_NO ='" + heat_id_L1 + "', ST_NO ='" + st_no + "',STATION_NO='" + bof_no + "',PROD_DATE='" + DateTime.Now.ToString("yyyyMMdd") + "', STATUS ='1' where BY_STATION_NO ='5'");
                                    WriteLog.writeLog5("更新转炉实际1成功");
                                    by_TB5_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": 更新转炉实际1成功");

                                    TB_bof5status.Text = "炉次在吹氧开始";
                                    TB_bof5heatid.Text = heat_id_L1.ToString();
                                    DBconn.writeNew("update by_bofstatus set bof5_start=0,bof5_o2end=0,bof5_cgstart=0,bof5_cgend=0");//炉次开始初始化状态位
                                    DBconn.writeNew("update by_shoudong set sd_bof5start=0");
                                    DBconn.writeNew("update BY_BOFSTATUS set bof5_status_now=1");
                                    WriteLog.writeLog5(heat_id_L1 + "炉次在吹氧开始并重置标志位");
                                    by_TB5_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + heat_id_L1 + "炉次在吹氧开始并重置标志位");
                                }
                                else
                                {
                                    DBconn.writeNew("update by_bofstatus set bof5_start=0");
                                    DBconn.writeNew("update by_shoudong set sd_bof5start=0");
                                    DBconn.writeNew("update by_bofstatus set bof5_o2start=0");
                                    DBconn.writeNew("update by_shoudong set sd_bof5o2start=0");
                                    WriteLog.writeLog5("炉次在吹氧开始失败，没有计划，bof5_start,bof5_o2start已置0");
                                    by_TB5_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": 炉次在吹氧开始失败，没有计划，bof5_start,bof5_o2start已置0");
                                }
                            }
                            else
                            {
                                DBconn.writeNew("update by_bofstatus set bof5_o2start=0");
                                DBconn.writeNew("update by_shoudong set sd_bof5o2start=0");
                                DBconn.writeNew("update by_bofstatus set bof5_start=0");
                                DBconn.writeNew("update by_shoudong set sd_bof5start=0");
                                WriteLog.writeLog5("L1炉号" + heat_id_L1 + "不大于运转状态最大炉号" + bof_no_history + "炉次在吹氧开始失败标志位已置0");
                                by_TB5_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "L1炉号" + heat_id_L1 + "不大于运转状态最大炉号" + bof_no_history + "炉次在吹氧开始失败标志位已置0");
                            }
                        }
                    }


                }
                catch (Exception err)
                {
                    WriteLog.writeLog5("吹氧开始运行失败标志位已置0" + err.Message);
                    DBconn.writeNew("update by_bofstatus set bof5_o2start=0");
                    DBconn.writeNew("update by_shoudong set sd_bof5o2start=0");
                    DBconn.writeNew("update by_bofstatus set bof5_start=0");
                    DBconn.writeNew("update by_shoudong set sd_bof5start=0");
                    by_TB5_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + err.Message + "吹氧开始运行失败标志位已置0" + heat_id);
                }
                #endregion

            }
            if (bof5o2end_l1 == "1" && heat_id != "" || sd_bof5o2end == "1")//-------------------------------------------------------吹氧结束

            {
                #region 吹氧结束
                try
                {
                    WriteLog.writeLog5(heat_id + "------吹氧结束开始------");
                    by_TB5_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + heat_id + "------吹氧结束开始------");
                    dt_zlsj_temp_5 = DBconn.readNew("select * from zlsj_temp where by_station_no=5");
                    if (Convert.ToInt32(dt_zlsj_temp_5.Rows[0]["STATUS"]) < 4)//正常炉次开始
                    {
                        WriteLog.writeLog5(heat_id + "吹氧结束正常开始");
                        by_TB5_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + heat_id + "吹氧结束正常开始");

                        pono = dt_zlsj_temp_5.Rows[0]["pono"].ToString();
                        plan_no = dt_zlsj_temp_5.Rows[0]["SM_PLAN_NO"].ToString();
                        st_no = dt_zlsj_temp_5.Rows[0]["st_no"].ToString();

                        DBconn.writeNew("insert into yzzt(sm_plan_no, proc_no, station_id, station_no, heat_no, pono, st_no, run_signal, run_time)values('" + plan_no + "'," + heat_id + ",'B','5'," + heat_id + ",'" + pono + "','" + st_no + "','354','" + DateTime.Now.ToString("yyyyMMddHHmmss") + "')");
                        WriteLog.writeLog5(heat_id + "插入运转状态354成功" + plan_no + "|" + pono);
                        by_TB5_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + heat_id + "插入运转状态354成功" + plan_no + "|" + pono);

                        DBconn.writeNew("update zlsj_temp set STATUS ='4' where by_station_no ='5'");
                        WriteLog.writeLog5(heat_id + "更新转炉实际标志位4成功");
                        by_TB5_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + heat_id + "更新转炉实际标志位4成功");

                        TB_bof5status.Text = "吹氧结束";
                        DBconn.writeNew("update by_bofstatus set bof5_o2end=0");
                        DBconn.writeNew("update by_shoudong set sd_bof5o2end=0");
                        DBconn.writeNew("update BY_BOFSTATUS set bof5_status_now=4");
                        WriteLog.writeLog5(heat_id + "------吹氧结束成功------");
                        by_TB5_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + heat_id + "------吹氧结束成功------");

                    }
                    else
                    {
                        DBconn.writeNew("update by_bofstatus set bof5_o2end=0");
                        DBconn.writeNew("update by_shoudong set sd_bof5o2end=0");
                        WriteLog.writeLog5(heat_id + "吹氧结束开始失败status必须小于4，重置标志位");
                        by_TB5_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + heat_id + "吹氧结束开始失败status必须小于4，重置标志位");
                    }
                }
                catch (Exception err)
                {
                    WriteLog.writeLog5("吹氧结束失败" + err.Message);
                    by_TB5_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + err.Message + "吹氧结束失败" + heat_id);
                    DBconn.writeNew("update by_bofstatus set bof5_o2end=0");
                    DBconn.writeNew("update by_shoudong set sd_bof5o2end=0");
                }
                #endregion
            }

            if (bof5cgstart_l1 == "1" && heat_id != "" || sd_bof5cgstart == "1")//-------------------------------------------------------出钢开始

            {
                #region 出钢开始
                try
                {
                    WriteLog.writeLog5(heat_id + "------出钢开始------");
                    by_TB5_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + heat_id + "------出钢开始------");
                    dt = DBconn.readNew("select * from zlsj_temp where by_station_no=5");
                    if (Convert.ToInt32(dt.Rows[0]["STATUS"]) < 5)//正常炉次开始
                    {
                        WriteLog.writeLog5(heat_id + "正常出钢开始");
                        by_TB5_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + heat_id + "正常出钢开始");
                        pono = dt.Rows[0]["pono"].ToString();
                        plan_no = dt.Rows[0]["SM_PLAN_NO"].ToString();
                        st_no = dt.Rows[0]["st_no"].ToString();
                        DBconn.writeNew("insert into yzzt(sm_plan_no, proc_no, station_id, station_no, heat_no, pono, st_no, run_signal, run_time)values('" + plan_no + "'," + heat_id + ",'B','5'," + heat_id + ",'" + pono + "','" + st_no + "','355','" + DateTime.Now.ToString("yyyyMMddHHmmss") + "')");
                        WriteLog.writeLog5(heat_id + "插入运转状态355成功" + plan_no + "|" + pono);
                        by_TB5_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + heat_id + "插入运转状态355成功" + plan_no + "|" + pono);
                        try
                        {
                            DBconn.writeOld("call NEW_ZLJLSJ5_SEND_PRO('" + heat_id + "')");//传送转炉加料实绩
                        }
                        catch (Exception err)
                        {
                            WriteLog.writeLog5(heat_id + "传送转炉加料实绩失败" + err.Message);
                            by_TB5_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + heat_id + "传送转炉加料实绩失败" + err.Message);
                        }
                        try//更新吹氧数据
                        {
                            int i = 0;

                            DataTable dt_cuiyang = DBconn.readOld("select round((t.blowendtime-t.blowbegintime)*24*60*60),t.blowbegintime,t.blowendtime,round(t.blowo2amount) from BOF5BLOWO2DATA t where t.heatid='" + heat_id + "' order by t.blowo2times");
                            if (dt_cuiyang.Rows.Count > 0)
                            {
                                DBconn.writeNew("update zlsj_temp set reblow_num='" + dt_cuiyang.Rows.Count + "' where by_station_no=5");
                                string starttime;//开始时间
                                string endtime;//结束时间
                                int t1;//单个吹氧时间
                                int tall = 0;//总吹氧时间
                                int EXT_ITEM3 = 0;//补吹次数
                                int EXT_ITEM4_temp = 0;//第一次吹氧量
                                int EXT_ITEM4 = 0;//补吹氧量
                                int REBLOW_DURATION = 0;//补吹持续时间
                                foreach (DataRow dr in dt_cuiyang.Rows)
                                {
                                    i++;
                                    if (i <= 5)
                                    {
                                        starttime = Convert.ToDateTime(dr[1]).ToString("yyyyMMddHHmmss");
                                        endtime = Convert.ToDateTime(dr[2]).ToString("yyyyMMddHHmmss");
                                        t1 = Convert.ToInt32(dr[0].ToString());
                                        tall = tall + t1;
                                        DBconn.writeNew("update zlsj_temp set blow_time" + i + "='" + t1 + "',blow_start_time" + i + "='" + starttime + "',blow_end_time" + i + "='" + endtime + "',o2_sum_comsume='" + dr[3].ToString() + "',blow_time='" + tall + "' where by_station_no=5");
                                        if (i < 2)
                                        {
                                            EXT_ITEM4_temp = Convert.ToInt32(dr[3].ToString());
                                        }
                                        if (i > 1)
                                        {
                                            EXT_ITEM4 = 0;
                                            REBLOW_DURATION = REBLOW_DURATION + t1;
                                            EXT_ITEM3++;
                                            EXT_ITEM4 = Convert.ToInt32(dr[3].ToString()) - EXT_ITEM4_temp;
                                        }
                                    }
                                }
                                DBconn.writeNew("update zlsj_temp set EXT_ITEM3='" + EXT_ITEM3 + "',EXT_ITEM4='" + EXT_ITEM4 + "',REBLOW_DURATION='" + REBLOW_DURATION + "'  where by_station_no=5");
                            }
                        }
                        catch (Exception err)
                        {
                            WriteLog.writeLog5(heat_id + "吹氧数据更新失败" + err.Message);
                            by_TB5_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + heat_id + "吹氧数据更新失败" + err.Message);
                        }
                        DBconn.writeNew("update zlsj_temp set STATUS ='5' where by_station_no ='5'");
                        WriteLog.writeLog5(heat_id + "更新转炉实际标志位成功");
                        by_TB5_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + heat_id + "更新转炉实际标志位成功");
                        TB_bof5status.Text = "出钢开始";
                        DBconn.writeNew("update BY_BOFSTATUS set bof5_status_now=5");
                        DBconn.writeNew("update by_bofstatus set bof5_cgstart=0");
                        DBconn.writeNew("update by_shoudong set sd_bof5cgstart=0");
                        WriteLog.writeLog5(heat_id + "------出钢开始完成------");
                        by_TB5_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + heat_id + "------出钢开始完成------");
                    }
                    else
                    {
                        DBconn.writeNew("update by_bofstatus set bof5_cgstart=0");
                        DBconn.writeNew("update by_shoudong set sd_bof5cgstart=0");
                        WriteLog.writeLog5(heat_id + "出钢开始失败，标志位不小于5");
                        by_TB5_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + heat_id + "出钢开始失败，标志位不小于5");
                    }

                }
                catch (Exception err)
                {
                    WriteLog.writeLog5("出钢开始失败" + err.Message);
                    by_TB5_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + err.Message + "出钢开始失败" + heat_id);
                    DBconn.writeNew("update by_bofstatus set bof5_cgstart=0");
                    DBconn.writeNew("update by_shoudong set sd_bof5cgstart=0");
                }
                #endregion


                //  bof5thisheatid++;//把当炉炉号加1
                // DBconn.writeNew("update by_bofstatus set bof5_heatid_now=" + DateTime.Now.ToString("yy")   + bof5thisheatid);//更新数据库炉号


            }
            if (bof5cgend_l1 == "1" && DBconn.readNew("select heat_no from zlsj_temp where by_station_no=5").Rows[0][0].ToString() != "" || sd_bof5cgend == "1")//-------------------------------------------------------出钢结束

            {
                #region 出钢结束
                try
                {
                    WriteLog.writeLog5(heat_id + "------出钢结束开始------");
                    by_TB5_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + heat_id + "------出钢结束开始------");
                    dt = DBconn.readNew("select * from zlsj_temp where by_station_no=5");
                    if (Convert.ToInt32(dt.Rows[0]["STATUS"]) < 6)//正常炉次开始
                    {
                        WriteLog.writeLog5(heat_id + "出钢结束正常开始");
                        by_TB5_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + heat_id + "出钢结束正常开始");
                        pono = dt.Rows[0]["pono"].ToString();
                        plan_no = dt.Rows[0]["SM_PLAN_NO"].ToString();
                        st_no = dt.Rows[0]["st_no"].ToString();

                        DBconn.writeNew("insert into yzzt(sm_plan_no, proc_no, station_id, station_no, heat_no, pono, st_no, run_signal, run_time)values('" + plan_no + "'," + heat_id + ",'B','5'," + heat_id + ",'" + pono + "','" + st_no + "','356','" + DateTime.Now.ToString("yyyyMMddHHmmss") + "')");
                        WriteLog.writeLog5(heat_id + "插入运转状态356成功" + plan_no + "|" + pono);
                        by_TB5_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + heat_id + "插入运转状态356成功" + plan_no + "|" + pono);

                        DBconn.writeNew("call zlsj_send_pro('" + heat_id + "')");//传送转炉实际

                        DBconn.writeNew("update zlsj_temp set tap_end_time='" + DateTime.Now.ToString("yyyyMMddHHmmss") + "',STATUS ='6',status_new='1' where by_station_no ='5'");
                        WriteLog.writeLog5(heat_id + "更新转炉实际成功");
                        by_TB5_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + heat_id + "更新转炉实际成功");
                        endheat_no5 = heat_id;
                        DBconn.writeNew("update zlsj_temp set STATUS ='0',HEAT_NO ='' where by_station_no ='5'");
                        DBconn.writeNew("update by_bofstatus set bof5_start=0,bof5_o2start=0,bof5_o2end=0,bof5_cgstart=0,bof5_cgend=0");
                        DBconn.writeNew("update by_shoudong set sd_bof5cgend=0");
                        DBconn.writeNew("update BY_BOFSTATUS set bof5_status_now=6");
                        TB_bof5status.Text = "出钢结束";
                        WriteLog.writeLog5(heat_id + "------出钢结束完成------");
                        by_TB5_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + heat_id + "------出钢结束完成------");
                    }
                    else
                    {
                        DBconn.writeNew("update by_bofstatus set bof5_start=0,bof5_o2start=0,bof5_o2end=0,bof5_cgstart=0,bof5_cgend=0");
                        DBconn.writeNew("update by_shoudong set sd_bof5cgend=0");
                        WriteLog.writeLog5(heat_id + "出钢结束失败，状态位不小于6");
                        by_TB5_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + heat_id + "出钢结束失败，状态位不小于6");
                    }



                }
                catch (Exception err)
                {
                    WriteLog.writeLog5("出钢结束失败" + err.Message);
                    by_TB5_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + err.Message + "出钢结束失败" + heat_id);
                    DBconn.writeNew("update by_bofstatus set bof5_cgend=0");
                    DBconn.writeNew("update by_shoudong set sd_bof5cgend=0");
                }
                #endregion

            }
            if (bof5end_l1 == "1")//-------------------------------------------------------炉次结束
            {
                #region 炉次结束动作
                try
                {

                    WriteLog.writeLog5(heat_id + "------炉次结束------");
                    by_TB5_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + heat_id + "------炉次结束------");

                    if (heat_id != "")
                    {
                        WriteLog.writeLog5(heat_id + "炉次结束信号来了，有炉号，可能上一炉未结束或者炉次开始和结束信号一起来");
                        by_TB5_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + heat_id + "炉次结束信号来了，有炉号，可能上一炉未结束或者炉次开始和结束信号一起来");
                        //DBconn.writeNew("update zlsj_temp set tap_end_time='" + DateTime.Now.ToString("yyyyMMddHHmmss") + "',STATUS ='6',status_new='1' where by_station_no ='5'");
                    }
                    else if (endheat_no5 != "")
                    {
                        WriteLog.writeLog5(endheat_no5 + "开始结束炉次写历史炉次标志位");
                        by_TB5_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + endheat_no5 + "开始结束炉次写历史炉次标志位");

                        DBconn.readNew("update zlsj_history set status=8 where heat_no='" + endheat_no5 + "'");

                        WriteLog.writeLog5(endheat_no5 + "炉次结束_历史炉次标志位置9");
                        by_TB5_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + endheat_no5 + "炉次结束_历史炉次标志位置9");
                    }

                    DBconn.writeNew("update BY_BOFSTATUS set bof5_status_now=7");
                    DBconn.writeNew("update by_bofstatus set bof5_end=0");
                    WriteLog.writeLog5(heat_id + "炉次结束");
                    by_TB5_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + heat_id + "炉次5#结束");
                }
                catch (Exception err)
                {
                    DBconn.writeNew("update by_bofstatus set bof5_end=0");
                    WriteLog.writeLog5("炉次结束失败" + err.Message);
                    by_TB5_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + err.Message + "炉次结束失败" + heat_id);
                }
                #endregion
            }

        }
        /// <summary>
        /// 6#炉跟踪
        /// </summary>
        /// 

        private void bof6trace()
        {

            string heat_id = "";
            string heat_id_L1;
            DataTable dt;
            DataTable dt_zlsj_temp_6;
            DataTable dt_cgjh_6;
            string pono, plan_no, st_no, bof_no;
            dt_zlsj_temp_6 = DBconn.readNew("select * from zlsj_temp t  where t.by_station_no=6");
            bof_no_history = DBconn.readNew("select max(heat_no) from YZZT t  where t.station_no=6").Rows[0][0].ToString();
            if (dt_zlsj_temp_6.Rows.Count > 0)
            {
                heat_id = dt_zlsj_temp_6.Rows[0]["HEAT_NO"].ToString();
            }

            if (bof6start_l1 == "1" || sd_bof6start == "1")//-------------------------------------------------------炉次开始

            {
                #region 炉次开始
                try
                {
                    WriteLog.writeLog6("------炉次开始------");
                    by_TB6_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": ------炉次开始------");

                    dt_zlsj_temp_6 = DBconn.readNew(" select * from zlsj_temp where BY_STATION_NO ='6'");
                    if (Convert.ToInt32(dt_zlsj_temp_6.Rows[0]["STATUS"]) > 4 && heat_id != "")          //强制结束上一炉
                    {

                        pono = dt_zlsj_temp_6.Rows[0]["pono"].ToString();
                        plan_no = dt_zlsj_temp_6.Rows[0]["sm_plan_no"].ToString();
                        st_no = dt_zlsj_temp_6.Rows[0]["st_no"].ToString();
                        heat_id = dt_zlsj_temp_6.Rows[0]["HEAT_NO"].ToString();
                        WriteLog.writeLog6("上炉未正常结束强制结束" + heat_id);
                        by_TB6_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": 上炉未正常结束强制结束" + heat_id);

                        DBconn.writeNew("insert into yzzt(sm_plan_no, proc_no, station_id, station_no, heat_no, pono, st_no, run_signal, run_time)values('" + plan_no + "'," + heat_id + ",'B','6'," + heat_id + ",'" + pono + "','" + st_no + "','366','" + DateTime.Now.ToString("yyyyMMddHHmmss") + "')");
                        WriteLog.writeLog6("写入366运转状态成功" + heat_id + "|" + pono + "|" + plan_no);
                        by_TB6_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": 写入366运转状态成功" + heat_id + "|" + pono + "|" + plan_no);

                        DBconn.writeNew("update zlsj_temp set STATUS ='6',STATUS_NEW ='1'  where BY_STATION_NO ='6'");      //zlsj_temp状态位STATUS=6代表出钢结束，触发by_send_zlsj触发器写入转炉实绩表；STATUS_NEW=1，往zlsj_history写入数据，写完之后STATUS_NEW=0；
                        WriteLog.writeLog6("更新转炉实际状态为6成功");
                        by_TB6_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": 更新转炉实际状态为6成功");
                        DBconn.writeNew("update BY_BOFSTATUS set bof6_status_now=7");     //BY_BOFSTATUS状态位“bof6_status_now”为7控制画面全亮绿灯
                        DBconn.writeNew("update zlsj_temp set STATUS ='0',HEAT_NO ='' where BY_STATION_NO ='6'");    //zlsj_temp状态位STATUS=0代表等待炉次开始，
                        WriteLog.writeLog6("强制结束完成");
                        by_TB6_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": 强制结束完成");

                    }
                    //--------------------------------------
                    heat_id_L1 = DBconn.readOld("select heatid from bof6status").Rows[0][0].ToString();//从一级取当前炉号
                    WriteLog.writeLog6(":从L1取炉号" + heat_id_L1);
                    by_TB6_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": 从L1取炉号" + heat_id_L1);
                    //--------------------------------------
                    if (Convert.ToInt32(heat_id_L1) > Convert.ToInt32(bof_no_history))//炉号正常加一      bof_no_history为yzzt表最大炉号
                    {
                        dt_cgjh_6 = DBconn.readNew("select * from cgjh where bof_no='6' and status is null order by to_number(plan_no)");  //读取最近一个未用的计划
                        if (dt_cgjh_6.Rows.Count > 0)
                        {
                            bof_no = dt_cgjh_6.Rows[0]["bof_no"].ToString();
                            pono = dt_cgjh_6.Rows[0]["pono"].ToString();
                            plan_no = dt_cgjh_6.Rows[0]["plan_no"].ToString();
                            st_no = dt_cgjh_6.Rows[0]["st_no"].ToString();
                            WriteLog.writeLog6("炉次开始读取计划成功" + bof_no + "|" + pono + "|" + plan_no);
                            by_TB6_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": 炉次开始读取计划成功" + bof_no + "|" + pono + "|" + plan_no);

                            DBconn.writeNew("insert into yzzt(sm_plan_no, proc_no, station_id, station_no, heat_no, pono, st_no, run_signal, run_time)values('" + plan_no + "'," + heat_id_L1 + ",'B','6'," + heat_id_L1 + ",'" + pono + "','" + st_no + "','361','" + DateTime.Now.ToString("yyyyMMddHHmmss") + "')");
                            WriteLog.writeLog6("写入运转状态：" + plan_no + "|" + heat_id_L1 + "|" + pono + "成功");
                            by_TB6_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": 写入361运转状态：" + plan_no + "|" + heat_id_L1 + "|" + pono + "成功");

                            DBconn.writeNew("update cgjh set status ='1',heat_no='" + heat_id_L1 + "' where plan_no ='" + plan_no + "'");         //炉次开始cgjh表status ='1'，代表计划已用
                            WriteLog.writeLog6("更新出钢计划标志位成功");
                            by_TB6_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": 更新出钢计划标志位成功");

                            DBconn.writeNew("update zlsj_temp set SM_PLAN_NO ='" + plan_no + "',pono ='" + pono + "',HEAT_NO ='" + heat_id_L1 + "', ST_NO ='" + st_no + "',STATION_NO='" + bof_no + "',PROD_DATE='" + DateTime.Now.ToString("yyyyMMdd") + "', STATUS ='1' where BY_STATION_NO ='6'");   //把当前使用计划信息写入zlsj_temp表，status=1代表炉次开始状态完成；
                            WriteLog.writeLog6("更新转炉实际1成功");
                            by_TB6_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": 更新转炉实际1成功");

                            TB_bof6status.Text = "炉次开始";
                            TB_bof6heatid.Text = heat_id_L1.ToString();
                            DBconn.writeNew("update BY_BOFSTATUS set bof6_status_now=1");   // BY_BOFSTATUS状态位“bof6_status_now”为1，画面“炉次开始”灯亮
                            DBconn.writeNew("update by_bofstatus set bof6_start=0,bof6_o2start=0,bof6_o2end=0,bof6_cgstart=0,bof6_cgend=0");//炉次开始初始化状态位    //自动状态位全部初始化
                            DBconn.writeNew("update by_shoudong set sd_bof6start=0");            //手动状态位初始化
                            WriteLog.writeLog6(heat_id_L1 + "------炉次开始完成------");
                            by_TB6_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + heat_id_L1 + "------炉次开始完成------");
                        }
                        else
                        {
                            DBconn.writeNew("update by_bofstatus set bof6_start=0");
                            DBconn.writeNew("update by_shoudong set sd_bof6start=0");
                            WriteLog.writeLog6("炉次开始失败，没有计划，bof6_start已置0");
                            by_TB6_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": 炉次开始失败，没有计划，bof6_start已置0");
                        }

                    }
                    else
                    {
                        DBconn.writeNew("update by_bofstatus set bof6_start=0");
                        DBconn.writeNew("update by_shoudong set sd_bof6start=0");
                        WriteLog.writeLog6("L1炉号" + heat_id_L1 + "不大于运转状态最大炉号" + bof_no_history + "炉次开始失败");
                        by_TB6_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "L1炉号" + heat_id_L1 + "不大于运转状态最大炉号" + bof_no_history + "炉次开始失败");
                    }
                }
                catch (Exception err)
                {
                    WriteLog.writeLog6("炉次开始运行失败" + err.Message);
                    DBconn.writeNew("update by_bofstatus set bof6_start=0");
                    DBconn.writeNew("update by_shoudong set sd_bof6start=0");
                    by_TB6_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + err.Message + "炉次开始运行失败" + heat_id);
                }
                #endregion
            }
            if (bof6o2start_l1 == "1" || sd_bof6o2start == "1")//-------------------------------------------------------吹氧开始

            {
                #region 吹氧开始
                try
                {
                    WriteLog.writeLog6("------吹氧开始------");
                    by_TB6_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": ------吹氧开始------");
                    dt_zlsj_temp_6 = DBconn.readNew("select * from zlsj_temp where by_station_no ='6'");
                    if (Convert.ToInt32(dt_zlsj_temp_6.Rows[0]["STATUS"]) < 3 && heat_id != "")//正常炉次开始
                    {
                        WriteLog.writeLog6(heat_id + "正常吹氧开始");
                        by_TB6_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + heat_id + "正常吹氧开始");
                        pono = dt_zlsj_temp_6.Rows[0]["pono"].ToString();
                        plan_no = dt_zlsj_temp_6.Rows[0]["SM_PLAN_NO"].ToString();
                        st_no = dt_zlsj_temp_6.Rows[0]["st_no"].ToString();

                        DBconn.writeNew("insert into yzzt(sm_plan_no, proc_no, station_id, station_no, heat_no, pono, st_no, run_signal, run_time)values('" + plan_no + "'," + heat_id + ",'B','6'," + heat_id + ",'" + pono + "','" + st_no + "','362','" + DateTime.Now.ToString("yyyyMMddHHmmss") + "')");
                        DBconn.writeNew("insert into yzzt(sm_plan_no, proc_no, station_id, station_no, heat_no, pono, st_no, run_signal, run_time)values('" + plan_no + "'," + heat_id + ",'B','6'," + heat_id + ",'" + pono + "','" + st_no + "','363','" + DateTime.Now.ToString("yyyyMMddHHmmss") + "')");
                        WriteLog.writeLog6("写入运转状态：" + plan_no + "|" + heat_id + "|" + pono + "成功");
                        by_TB6_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": 写入362，363运转状态：" + plan_no + "|" + heat_id + "|" + pono + "成功");

                        DBconn.writeNew("update zlsj_temp set STATUS ='3' where by_station_no ='6'");      // STATUS ='3' 代表吹氧开始状态
                        DBconn.writeNew("update BY_BOFSTATUS set bof6_status_now=3");
                        WriteLog.writeLog6(heat_id + ":更新炉次实际3成功");
                        by_TB6_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + heat_id + "更新炉次实际3成功");

                        TB_bof6status.Text = "吹氧开始";

                        DBconn.writeNew("update by_bofstatus set bof6_start=0");
                        DBconn.writeNew("update by_bofstatus set bof6_o2start=0");             //初始化吹氧开始状态位bof6_start=0，等待下一次炉次吹氧使用；
                        DBconn.writeNew("update by_shoudong set sd_bof6o2start=0");                //初始化吹氧开始状态位bof6_start=0，等待下一次炉次吹氧使（手动模式）
                        WriteLog.writeLog6("------吹氧开始完成------");
                        by_TB6_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": ------吹氧开始完成------");

                    }
                    else//不正常开始 炉次开始信号没有来
                    {
                        WriteLog.writeLog6(":吹氧异常开始");
                        by_TB6_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": 吹氧异常开始");

                        dt_zlsj_temp_6 = DBconn.readNew(" select * from zlsj_temp where by_station_no ='6'");

                        WriteLog.writeLog6(":判断上炉是否结束");
                        by_TB6_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": 判断上炉是否结束");
                        if (Convert.ToInt32(dt_zlsj_temp_6.Rows[0]["STATUS"]) > 4)          //强制结束上一炉
                        {
                            WriteLog.writeLog6(heat_id + ":上炉未结束");
                            by_TB6_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + heat_id + "上一炉未结束");
                            pono = dt_zlsj_temp_6.Rows[0]["pono"].ToString();
                            plan_no = dt_zlsj_temp_6.Rows[0]["sm_plan_no"].ToString();
                            st_no = dt_zlsj_temp_6.Rows[0]["st_no"].ToString();
                            heat_id = dt_zlsj_temp_6.Rows[0]["HEAT_NO"].ToString();
                            DBconn.writeNew("insert into yzzt(sm_plan_no, proc_no, station_id, station_no, heat_no, pono, st_no, run_signal, run_time)values('" + plan_no + "'," + heat_id + ",'B','6'," + heat_id + ",'" + pono + "','" + st_no + "','366','" + DateTime.Now.ToString("yyyyMMddHHmmss") + "')");
                            WriteLog.writeLog6("写入366运转状态成功" + heat_id + "|" + pono + "|" + plan_no);
                            by_TB6_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": 写入366运转状态成功" + heat_id + "|" + pono + "|" + plan_no);

                            DBconn.writeNew("update zlsj_temp set STATUS ='6',STATUS_NEW ='1'  where BY_STATION_NO ='6'");
                            DBconn.writeNew("update BY_BOFSTATUS set bof6_status_now=7");
                            WriteLog.writeLog6("更新转炉实际状态为6成功");
                            by_TB6_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": 更新转炉实际状态为6成功");

                            DBconn.writeNew("update zlsj_temp set STATUS ='0',HEAT_NO ='' where BY_STATION_NO ='6'");
                            WriteLog.writeLog6("强制结束完成");
                            by_TB6_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": 强制结束完成");
                        }
                        else
                        {
                            WriteLog.writeLog6(heat_id + ":上炉成功结束，炉次开始信号没来，模拟炉次开始");
                            by_TB6_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + heat_id + "上炉成功结束，炉次开始信号没来，模拟炉次开始");
                            //---------------------------------------
                            heat_id_L1 = DBconn.readOld("select heatid from bof6status").Rows[0][0].ToString();//从一级取当前炉号
                            WriteLog.writeLog6(":从L1取炉号" + heat_id_L1);
                            by_TB6_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": 从L1取炉号" + heat_id_L1);
                            //---------------------------------------
                            if (Convert.ToInt32(heat_id_L1) > Convert.ToInt32(bof_no_history))//炉号正常加一
                            {

                                dt_cgjh_6 = DBconn.readNew("select * from cgjh where bof_no='6' and status is null order by to_number(plan_no)");  //读取最近一个未用的计划
                                if (dt_cgjh_6.Rows.Count > 0)
                                {
                                    bof_no = dt_cgjh_6.Rows[0]["bof_no"].ToString();
                                    pono = dt_cgjh_6.Rows[0]["pono"].ToString();
                                    plan_no = dt_cgjh_6.Rows[0]["plan_no"].ToString();
                                    st_no = dt_cgjh_6.Rows[0]["st_no"].ToString();
                                    WriteLog.writeLog6("炉次在吹氧开始读取计划成功" + bof_no + "|" + pono + "|" + plan_no);
                                    by_TB6_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": 炉次在吹氧开始读取计划成功" + bof_no + "|" + pono + "|" + plan_no);

                                    DBconn.writeNew("insert into yzzt(sm_plan_no, proc_no, station_id, station_no, heat_no, pono, st_no, run_signal, run_time)values('" + plan_no + "'," + heat_id_L1 + ",'B','6'," + heat_id_L1 + ",'" + pono + "','" + st_no + "','361','" + DateTime.Now.ToString("yyyyMMddHHmmss") + "')");
                                    WriteLog.writeLog6("写入运转状态：" + plan_no + "|" + heat_id_L1 + "|" + pono + "成功");
                                    by_TB6_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": 写入361运转状态：" + plan_no + "|" + heat_id_L1 + "|" + pono + "成功");

                                    DBconn.writeNew("update cgjh set status ='1',heat_no='" + heat_id_L1 + "' where plan_no ='" + plan_no + "'");
                                    WriteLog.writeLog6("更新出钢计划1成功");
                                    by_TB6_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": 更新出钢计划1成功");

                                    DBconn.writeNew("update zlsj_temp set SM_PLAN_NO ='" + plan_no + "',pono ='" + pono + "',HEAT_NO ='" + heat_id_L1 + "', ST_NO ='" + st_no + "',STATION_NO='" + bof_no + "',PROD_DATE='" + DateTime.Now.ToString("yyyyMMdd") + "', STATUS ='1' where BY_STATION_NO ='6'");
                                    WriteLog.writeLog6("更新转炉实际1成功");
                                    by_TB6_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": 更新转炉实际1成功");

                                    TB_bof6status.Text = "炉次在吹氧开始";
                                    TB_bof6heatid.Text = heat_id_L1.ToString();
                                    DBconn.writeNew("update BY_BOFSTATUS set bof6_status_now=1");
                                    DBconn.writeNew("update by_bofstatus set bof6_start=0,bof6_o2end=0,bof6_cgstart=0,bof6_cgend=0");//炉次开始初始化状态位
                                    DBconn.writeNew("update by_shoudong set sd_bof6start=0");
                                    WriteLog.writeLog6(heat_id_L1 + "炉次在吹氧开始并重置标志位");
                                    by_TB6_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + heat_id_L1 + "炉次在吹氧开始并重置标志位");
                                }
                                else
                                {
                                    DBconn.writeNew("update by_bofstatus set bof6_start=0");
                                    DBconn.writeNew("update by_shoudong set sd_bof6start=0");
                                    DBconn.writeNew("update by_bofstatus set bof6_o2start=0");
                                    DBconn.writeNew("update by_shoudong set sd_bof6o2start=0");
                                    WriteLog.writeLog6("炉次在吹氧开始失败，没有计划，bof6_start, bof6_o2start已置0");
                                    by_TB6_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": 炉次在吹氧开始失败，没有计划，bof6_start, bof6_o2start已置0");
                                }
                            }
                            else
                            {
                                DBconn.writeNew("update by_bofstatus set bof6_o2start=0");
                                DBconn.writeNew("update by_shoudong set sd_bof6o2start=0");
                                DBconn.writeNew("update by_bofstatus set bof6_start=0");
                                DBconn.writeNew("update by_shoudong set sd_bof6start=0");
                                WriteLog.writeLog6("L1炉号" + heat_id_L1 + "不大于运转状态最大炉号" + bof_no_history + "炉次在吹氧开始失败标志位已置0");
                                by_TB6_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "L1炉号" + heat_id_L1 + "不大于运转状态最大炉号" + bof_no_history + "炉次在吹氧开始失败标志位已置0");
                            }
                        }
                    }


                }
                catch (Exception err)
                {
                    WriteLog.writeLog6("吹氧开始运行失败标志位已置0" + err.Message);
                    DBconn.writeNew("update by_bofstatus set bof6_o2start=0");
                    DBconn.writeNew("update by_shoudong set sd_bof6o2start=0");
                    DBconn.writeNew("update by_bofstatus set bof6_start=0");
                    DBconn.writeNew("update by_shoudong set sd_bof6start=0");
                    by_TB6_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + err.Message + "吹氧开始运行失败标志位已置0" + heat_id);
                }
                #endregion

            }
            if (bof6o2end_l1 == "1" && heat_id != "" || sd_bof6o2end == "1")//-------------------------------------------------------吹氧结束

            {
                try
                {
                    WriteLog.writeLog6(heat_id + "------吹氧结束开始------");
                    by_TB6_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + heat_id + "------吹氧结束开始------");
                    dt_zlsj_temp_6 = DBconn.readNew("select * from zlsj_temp where by_station_no=6");
                    if (Convert.ToInt32(dt_zlsj_temp_6.Rows[0]["STATUS"]) < 4)//正常炉次开始
                    {
                        WriteLog.writeLog6(heat_id + "吹氧结束正常开始");
                        by_TB6_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + heat_id + "吹氧结束正常开始");

                        pono = dt_zlsj_temp_6.Rows[0]["pono"].ToString();
                        plan_no = dt_zlsj_temp_6.Rows[0]["SM_PLAN_NO"].ToString();
                        st_no = dt_zlsj_temp_6.Rows[0]["st_no"].ToString();

                        DBconn.writeNew("insert into yzzt(sm_plan_no, proc_no, station_id, station_no, heat_no, pono, st_no, run_signal, run_time)values('" + plan_no + "'," + heat_id + ",'B','6'," + heat_id + ",'" + pono + "','" + st_no + "','364','" + DateTime.Now.ToString("yyyyMMddHHmmss") + "')");
                        WriteLog.writeLog6(heat_id + "插入运转状态364成功" + plan_no + "|" + pono);
                        by_TB6_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + heat_id + "插入运转状态364成功" + plan_no + "|" + pono);

                        DBconn.writeNew("update zlsj_temp set STATUS ='4' where by_station_no ='6'");
                        WriteLog.writeLog6(heat_id + "更新转炉实际标志位4成功");
                        by_TB6_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + heat_id + "更新转炉实际标志位4成功");

                        TB_bof6status.Text = "吹氧结束";
                        DBconn.writeNew("update by_bofstatus set bof6_o2end=0");
                        DBconn.writeNew("update by_shoudong set sd_bof6o2end=0");
                        DBconn.writeNew("update BY_BOFSTATUS set bof6_status_now=4");
                        WriteLog.writeLog6(heat_id + "------吹氧结束成功------");
                        by_TB6_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + heat_id + "------吹氧结束成功------");

                    }
                    else
                    {
                        DBconn.writeNew("update by_bofstatus set bof6_o2end=0");
                        DBconn.writeNew("update by_shoudong set sd_bof6o2end=0");
                        WriteLog.writeLog6(heat_id + "吹氧结束开始失败status必须小于4，重置标志位");
                        by_TB6_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + heat_id + "吹氧结束开始失败status必须小于4，重置标志位");
                    }
                }
                catch (Exception err)
                {
                    WriteLog.writeLog6("吹氧结束失败" + err.Message);
                    by_TB6_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + err.Message + "吹氧结束失败" + heat_id);
                    DBconn.writeNew("update by_bofstatus set bof6_o2end=0");
                    DBconn.writeNew("update by_shoudong set sd_bof6o2end=0");
                }
            }

            if (bof6cgstart_l1 == "1" && heat_id != "" || sd_bof6cgstart == "1")//-------------------------------------------------------出钢开始

            {
                try
                {
                    WriteLog.writeLog6(heat_id + "------出钢开始------");
                    by_TB6_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + heat_id + "------出钢开始------");
                    dt = DBconn.readNew("select * from zlsj_temp where by_station_no=6");
                    if (Convert.ToInt32(dt.Rows[0]["STATUS"]) < 5)//正常炉次开始
                    {
                        WriteLog.writeLog6(heat_id + "正常出钢开始");
                        by_TB6_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + heat_id + "正常出钢开始");
                        pono = dt.Rows[0]["pono"].ToString();
                        plan_no = dt.Rows[0]["SM_PLAN_NO"].ToString();
                        st_no = dt.Rows[0]["st_no"].ToString();
                        DBconn.writeNew("insert into yzzt(sm_plan_no, proc_no, station_id, station_no, heat_no, pono, st_no, run_signal, run_time)values('" + plan_no + "'," + heat_id + ",'B','6'," + heat_id + ",'" + pono + "','" + st_no + "','365','" + DateTime.Now.ToString("yyyyMMddHHmmss") + "')");
                        WriteLog.writeLog6(heat_id + "插入运转状态365成功" + plan_no + "|" + pono);
                        by_TB6_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + heat_id + "插入运转状态365成功" + plan_no + "|" + pono);
                        try
                        {
                            DBconn.writeOld("call NEW_ZLJLSJ6_SEND_PRO('" + heat_id + "')");//传送转炉加料实绩
                        }
                        catch (Exception err)
                        {
                            WriteLog.writeLog6(heat_id + "传送转炉加料实绩失败" + err.Message);
                            by_TB6_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + heat_id + "传送转炉加料实绩失败" + err.Message);
                        }
                        try//更新吹氧数据
                        {
                            int i = 0;

                            DataTable dt_cuiyang = DBconn.readOld("select round((t.blowendtime-t.blowbegintime)*24*60*60),t.blowbegintime,t.blowendtime,round(t.blowo2amount) from BOF6BLOWO2DATA t where t.heatid='" + heat_id + "' order by t.blowo2times");
                            if (dt_cuiyang.Rows.Count > 0)
                            {
                                DBconn.writeNew("update zlsj_temp set reblow_num='" + dt_cuiyang.Rows.Count + "' where by_station_no=6");
                                string starttime;//开始时间
                                string endtime;//结束时间
                                int t1;//单个吹氧时间
                                int tall = 0;//总吹氧时间
                                int EXT_ITEM3 = 0;//补吹次数
                                int EXT_ITEM4_temp = 0;//第一次吹氧量
                                int EXT_ITEM4 = 0;//补吹氧量
                                int REBLOW_DURATION = 0;//补吹持续时间
                                foreach (DataRow dr in dt_cuiyang.Rows)
                                {
                                    i++;
                                    if (i <= 5)
                                    {
                                        starttime = Convert.ToDateTime(dr[1]).ToString("yyyyMMddHHmmss");
                                        endtime = Convert.ToDateTime(dr[2]).ToString("yyyyMMddHHmmss");
                                        t1 = Convert.ToInt32(dr[0].ToString());
                                        tall = tall + t1;
                                        DBconn.writeNew("update zlsj_temp set blow_time" + i + "='" + t1 + "',blow_start_time" + i + "='" + starttime + "',blow_end_time" + i + "='" + endtime + "',o2_sum_comsume='" + dr[3].ToString() + "',blow_time='" + tall + "' where by_station_no=6");
                                        if (i < 2)
                                        {
                                            EXT_ITEM4_temp = Convert.ToInt32(dr[3].ToString());
                                        }
                                        if (i > 1)
                                        {
                                            EXT_ITEM4 = 0;
                                            REBLOW_DURATION = REBLOW_DURATION + t1;
                                            EXT_ITEM3++;
                                            EXT_ITEM4 = Convert.ToInt32(dr[3].ToString()) - EXT_ITEM4_temp;
                                        }
                                    }
                                }
                                DBconn.writeNew("update zlsj_temp set EXT_ITEM3='" + EXT_ITEM3 + "',EXT_ITEM4='" + EXT_ITEM4 + "',REBLOW_DURATION='" + REBLOW_DURATION + "'  where by_station_no=6");
                            }
                        }
                        catch (Exception err)
                        {
                            WriteLog.writeLog6(heat_id + "吹氧数据更新失败" + err.Message);
                            by_TB6_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + heat_id + "吹氧数据更新失败" + err.Message);
                        }
                        DBconn.writeNew("update zlsj_temp set STATUS ='5' where by_station_no ='6'");
                        WriteLog.writeLog6(heat_id + "更新转炉实际标志位成功");
                        by_TB6_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + heat_id + "更新转炉实际标志位成功");
                        TB_bof6status.Text = "出钢开始";
                        DBconn.writeNew("update by_bofstatus set bof6_cgstart=0");
                        DBconn.writeNew("update by_shoudong set sd_bof6cgstart=0");
                        DBconn.writeNew("update BY_BOFSTATUS set bof6_status_now=5");
                        WriteLog.writeLog6(heat_id + "------出钢开始完成------");
                        by_TB6_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + heat_id + "------出钢开始完成------");
                    }
                    else
                    {
                        DBconn.writeNew("update by_bofstatus set bof6_cgstart=0");
                        DBconn.writeNew("update by_shoudong set sd_bof6cgstart=0");
                        WriteLog.writeLog6(heat_id + "出钢开始失败，标志位不小于5");
                        by_TB6_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + heat_id + "出钢开始失败，标志位不小于5");
                    }

                }
                catch (Exception err)
                {
                    WriteLog.writeLog6("出钢开始失败" + err.Message);
                    by_TB6_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + err.Message + "出钢开始失败" + heat_id);
                    DBconn.writeNew("update by_bofstatus set bof6_cgstart=0");
                    DBconn.writeNew("update by_shoudong set sd_bof6cgstart=0");
                }


                //  bof6thisheatid++;//把当炉炉号加1
                // DBconn.writeNew("update by_bofstatus set bof6_heatid_now=" + DateTime.Now.ToString("yy")   + bof6thisheatid);//更新数据库炉号


            }
            if (bof6cgend_l1 == "1" && DBconn.readNew("select heat_no from zlsj_temp where by_station_no=6").Rows[0][0].ToString() != "" || sd_bof6cgend == "1")//-------------------------------------------------------出钢结束

            {
                try
                {
                    WriteLog.writeLog6(heat_id + "------出钢结束开始------");
                    by_TB6_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + heat_id + "------出钢结束开始------");
                    dt = DBconn.readNew("select * from zlsj_temp where by_station_no=6");
                    if (Convert.ToInt32(dt.Rows[0]["STATUS"]) < 6)//正常炉次开始
                    {
                        WriteLog.writeLog6(heat_id + "出钢结束正常开始");
                        by_TB6_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + heat_id + "出钢结束正常开始");
                        pono = dt.Rows[0]["pono"].ToString();
                        plan_no = dt.Rows[0]["SM_PLAN_NO"].ToString();
                        st_no = dt.Rows[0]["st_no"].ToString();

                        DBconn.writeNew("insert into yzzt(sm_plan_no, proc_no, station_id, station_no, heat_no, pono, st_no, run_signal, run_time)values('" + plan_no + "'," + heat_id + ",'B','6'," + heat_id + ",'" + pono + "','" + st_no + "','366','" + DateTime.Now.ToString("yyyyMMddHHmmss") + "')");
                        WriteLog.writeLog6(heat_id + "插入运转状态366成功" + plan_no + "|" + pono);
                        by_TB6_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + heat_id + "插入运转状态366成功" + plan_no + "|" + pono);

                        DBconn.writeNew("call zlsj_send_pro('" + heat_id + "')");//传送转炉实绩

                        DBconn.writeNew("update zlsj_temp set tap_end_time='" + DateTime.Now.ToString("yyyyMMddHHmmss") + "',STATUS ='6',status_new='1' where by_station_no ='6'");
                        WriteLog.writeLog6(heat_id + "更新转炉实际成功");
                        by_TB6_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + heat_id + "更新转炉实际成功");
                        endheat_no6 = heat_id;

                        DBconn.writeNew("update zlsj_temp set STATUS ='0',HEAT_NO ='' where by_station_no ='6'");
                        DBconn.writeNew("update by_bofstatus set bof6_start=0,bof6_o2start=0,bof6_o2end=0,bof6_cgstart=0,bof6_cgend=0");
                        DBconn.writeNew("update by_shoudong set sd_bof6cgend=0");
                        DBconn.writeNew("update BY_BOFSTATUS set bof6_status_now=6");
                        TB_bof6status.Text = "出钢结束";
                        WriteLog.writeLog6(heat_id + "------出钢结束完成------");
                        by_TB6_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + heat_id + "------出钢结束完成------");
                    }
                    else
                    {
                        DBconn.writeNew("update by_bofstatus set bof6_start=0,bof6_o2start=0,bof6_o2end=0,bof6_cgstart=0,bof6_cgend=0");
                        DBconn.writeNew("update by_shoudong set sd_bof6cgend=0");
                        WriteLog.writeLog6(heat_id + "出钢结束失败，状态位不小于6");
                        by_TB6_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + heat_id + "出钢结束失败，状态位不小于6");
                    }



                }
                catch (Exception err)
                {
                    WriteLog.writeLog6("出钢结束失败" + err.Message);
                    by_TB6_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + err.Message + "出钢结束失败" + heat_id);
                    DBconn.writeNew("update by_bofstatus set bof6_cgend=0");
                    DBconn.writeNew("update by_shoudong set sd_bof6cgend=0");
                }


            }
            if (bof6end_l1 == "1")//-------------------------------------------------------炉次结束
            {
                try
                {
                    WriteLog.writeLog6(heat_id + "------炉次结束------");
                    by_TB6_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + heat_id + "------炉次结束------");
                    #region 炉次结束动作
                    if (heat_id != "")
                    {
                        WriteLog.writeLog6(heat_id + "炉次结束信号来了，有炉号，可能上一炉未结束或者炉次开始和结束信号一起来");
                        by_TB6_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + heat_id + "炉次结束信号来了，有炉号，可能上一炉未结束或者炉次开始和结束信号一起来");
                        // DBconn.writeNew("update zlsj_temp set tap_end_time='" + DateTime.Now.ToString("yyyyMMddHHmmss") + "',STATUS ='6',status_new='1' where by_station_no ='6'");
                    }
                    else if (endheat_no6 != "")
                    {
                        WriteLog.writeLog6(endheat_no6 + "开始结束炉次写历史炉次标志位");
                        by_TB6_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + endheat_no6 + "开始结束炉次写历史炉次标志位");
                        DBconn.readNew("update zlsj_history set status=8 where heat_no='" + endheat_no6 + "'");
                        WriteLog.writeLog6(endheat_no6 + "炉次结束_历史炉次标志位置9");
                        by_TB6_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + endheat_no6 + "炉次结束_历史炉次标志位置9");
                    }
                    #endregion
                    DBconn.writeNew("update BY_BOFSTATUS set bof6_status_now=7");
                    DBconn.writeNew("update by_bofstatus set bof6_end=0");
                    WriteLog.writeLog6(heat_id + "炉次结束");
                    by_TB6_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + heat_id + "炉次6#结束");
                }
                catch (Exception err)
                {
                    DBconn.writeNew("update by_bofstatus set bof6_end=0");
                    WriteLog.writeLog6("炉次结束失败" + err.Message);
                    by_TB6_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + err.Message + "炉次结束失败" + heat_id);
                }

            }
        }

        /// <summary>
        /// 7#炉跟踪
        /// </summary>

        private void bof7trace()
        {

            string heat_id = "";
            string heat_id_L1;
            DataTable dt;
            DataTable dt_zlsj_temp_7;
            DataTable dt_cgjh_7;
            string pono, plan_no, st_no, bof_no;
            dt_zlsj_temp_7 = DBconn.readNew("select * from zlsj_temp t  where t.by_station_no=7");
            bof_no_history = DBconn.readNew("select max(heat_no) from YZZT t  where t.station_no=7").Rows[0][0].ToString();
            if (dt_zlsj_temp_7.Rows.Count > 0)
            {
                heat_id = dt_zlsj_temp_7.Rows[0]["HEAT_NO"].ToString();
            }

            if (bof7start_l1 == "1" || sd_bof7start == "1")//-------------------------------------------------------炉次开始

            {
                #region 炉次开始
                try
                {
                    WriteLog.writeLog7("------炉次开始------");
                    by_TB7_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": ------炉次开始------");

                    dt_zlsj_temp_7 = DBconn.readNew(" select * from zlsj_temp where BY_STATION_NO ='7'");
                    if (Convert.ToInt32(dt_zlsj_temp_7.Rows[0]["STATUS"]) > 4 && heat_id != "")          //强制结束上一炉
                    {

                        pono = dt_zlsj_temp_7.Rows[0]["pono"].ToString();
                        plan_no = dt_zlsj_temp_7.Rows[0]["sm_plan_no"].ToString();
                        st_no = dt_zlsj_temp_7.Rows[0]["st_no"].ToString();
                        heat_id = dt_zlsj_temp_7.Rows[0]["HEAT_NO"].ToString();
                        WriteLog.writeLog7("上炉未正常结束强制结束" + heat_id);
                        by_TB7_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": 上炉未正常结束强制结束" + heat_id);

                        DBconn.writeNew("insert into yzzt(sm_plan_no, proc_no, station_id, station_no, heat_no, pono, st_no, run_signal, run_time)values('" + plan_no + "'," + heat_id + ",'B','7'," + heat_id + ",'" + pono + "','" + st_no + "','376','" + DateTime.Now.ToString("yyyyMMddHHmmss") + "')");
                        WriteLog.writeLog7("写入376运转状态成功" + heat_id + "|" + pono + "|" + plan_no);
                        by_TB7_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": 写入376运转状态成功" + heat_id + "|" + pono + "|" + plan_no);

                        DBconn.writeNew("update zlsj_temp set STATUS ='6',STATUS_NEW ='1'  where BY_STATION_NO ='7'");
                        WriteLog.writeLog7("更新转炉实际状态为6成功");
                        by_TB7_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": 更新转炉实际状态为6成功");

                        DBconn.writeNew("update BY_BOFSTATUS set bof7_status_now=7");
                        DBconn.writeNew("update zlsj_temp set STATUS ='0',HEAT_NO ='' where BY_STATION_NO ='7'");
                        WriteLog.writeLog7("强制结束完成");
                        by_TB7_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": 强制结束完成");

                    }
                    //--------------------------------------
                    heat_id_L1 = DBconn.readOld("select heatid from bof7status").Rows[0][0].ToString();//从一级取当前炉号
                    WriteLog.writeLog7(":从L1取炉号" + heat_id_L1);
                    by_TB7_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": 从L1取炉号" + heat_id_L1);
                    //--------------------------------------
                    if (Convert.ToInt32(heat_id_L1) > Convert.ToInt32(bof_no_history))//炉号正常加一
                    {
                        dt_cgjh_7 = DBconn.readNew("select * from cgjh where bof_no='7' and status is null order by to_number(plan_no)");  //读取最近一个未用的计划
                        if (dt_cgjh_7.Rows.Count > 0)
                        {
                            bof_no = dt_cgjh_7.Rows[0]["bof_no"].ToString();
                            pono = dt_cgjh_7.Rows[0]["pono"].ToString();
                            plan_no = dt_cgjh_7.Rows[0]["plan_no"].ToString();
                            st_no = dt_cgjh_7.Rows[0]["st_no"].ToString();
                            WriteLog.writeLog7("炉次开始读取计划成功" + bof_no + "|" + pono + "|" + plan_no);
                            by_TB7_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": 炉次开始读取计划成功" + bof_no + "|" + pono + "|" + plan_no);

                            DBconn.writeNew("insert into yzzt(sm_plan_no, proc_no, station_id, station_no, heat_no, pono, st_no, run_signal, run_time)values('" + plan_no + "'," + heat_id_L1 + ",'B','7'," + heat_id_L1 + ",'" + pono + "','" + st_no + "','371','" + DateTime.Now.ToString("yyyyMMddHHmmss") + "')");
                            WriteLog.writeLog7("写入运转状态：" + plan_no + "|" + heat_id_L1 + "|" + pono + "成功");
                            by_TB7_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": 写入371运转状态：" + plan_no + "|" + heat_id_L1 + "|" + pono + "成功");

                            DBconn.writeNew("update cgjh set status ='1',heat_no='" + heat_id_L1 + "' where plan_no ='" + plan_no + "'");
                            WriteLog.writeLog7("更新出钢计划标志位成功");
                            by_TB7_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": 更新出钢计划标志位成功");

                            DBconn.writeNew("update zlsj_temp set SM_PLAN_NO ='" + plan_no + "',pono ='" + pono + "',HEAT_NO ='" + heat_id_L1 + "', ST_NO ='" + st_no + "',STATION_NO='" + bof_no + "',PROD_DATE='" + DateTime.Now.ToString("yyyyMMdd") + "', STATUS ='1' where BY_STATION_NO ='7'");
                            WriteLog.writeLog7("更新转炉实际1成功");
                            by_TB7_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": 更新转炉实际1成功");

                            TB_bof7status.Text = "炉次开始";
                            TB_bof7heatid.Text = heat_id_L1.ToString();
                            DBconn.writeNew("update by_bofstatus set bof7_start=0,bof7_o2start=0,bof7_o2end=0,bof7_cgstart=0,bof7_cgend=0");//炉次开始初始化状态位
                            DBconn.writeNew("update by_shoudong set sd_bof7start=0");
                            DBconn.writeNew("update BY_BOFSTATUS set bof7_status_now=1");
                            WriteLog.writeLog7(heat_id_L1 + "------炉次开始完成------");
                            by_TB7_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + heat_id_L1 + "------炉次开始完成------");
                        }
                        else
                        {
                            DBconn.writeNew("update by_bofstatus set bof7_start=0");
                            DBconn.writeNew("update by_shoudong set sd_bof7start=0");
                            WriteLog.writeLog7("炉次开始失败，没有计划，bof7_start已置0");
                            by_TB7_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": 炉次开始失败，没有计划，bof7_start已置0");
                        }

                    }
                    else
                    {
                        DBconn.writeNew("update by_bofstatus set bof7_start=0");
                        DBconn.writeNew("update by_shoudong set sd_bof7start=0");
                        WriteLog.writeLog7("L1炉号" + heat_id_L1 + "不大于运转状态最大炉号" + bof_no_history + "炉次开始失败");
                        by_TB7_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "L1炉号" + heat_id_L1 + "不大于运转状态最大炉号" + bof_no_history + "炉次开始失败");
                    }
                }
                catch (Exception err)
                {
                    WriteLog.writeLog7("炉次开始运行失败" + err.Message);
                    DBconn.writeNew("update by_bofstatus set bof7_start=0");
                    DBconn.writeNew("update by_shoudong set sd_bof7start=0");
                    by_TB7_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + err.Message + "炉次开始运行失败" + heat_id);
                }
                #endregion
            }
            if (bof7o2start_l1 == "1" || sd_bof7o2start == "1")//-------------------------------------------------------吹氧开始

            {
                #region 吹氧开始
                try
                {
                    WriteLog.writeLog7("------吹氧开始------");
                    by_TB7_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": ------吹氧开始------");
                    dt_zlsj_temp_7 = DBconn.readNew("select * from zlsj_temp where by_station_no ='7'");
                    if (Convert.ToInt32(dt_zlsj_temp_7.Rows[0]["STATUS"]) < 3 && heat_id != "")//正常炉次开始
                    {
                        WriteLog.writeLog7(heat_id + "正常吹氧开始");
                        by_TB7_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + heat_id + "正常吹氧开始");
                        pono = dt_zlsj_temp_7.Rows[0]["pono"].ToString();
                        plan_no = dt_zlsj_temp_7.Rows[0]["SM_PLAN_NO"].ToString();
                        st_no = dt_zlsj_temp_7.Rows[0]["st_no"].ToString();

                        DBconn.writeNew("insert into yzzt(sm_plan_no, proc_no, station_id, station_no, heat_no, pono, st_no, run_signal, run_time)values('" + plan_no + "'," + heat_id + ",'B','7'," + heat_id + ",'" + pono + "','" + st_no + "','372','" + DateTime.Now.ToString("yyyyMMddHHmmss") + "')");
                        DBconn.writeNew("insert into yzzt(sm_plan_no, proc_no, station_id, station_no, heat_no, pono, st_no, run_signal, run_time)values('" + plan_no + "'," + heat_id + ",'B','7'," + heat_id + ",'" + pono + "','" + st_no + "','373','" + DateTime.Now.ToString("yyyyMMddHHmmss") + "')");
                        WriteLog.writeLog7("写入运转状态：" + plan_no + "|" + heat_id + "|" + pono + "成功");
                        by_TB7_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": 写入372,373运转状态：" + plan_no + "|" + heat_id + "|" + pono + "成功");

                        DBconn.writeNew("update zlsj_temp set STATUS ='3' where by_station_no ='7'");
                        WriteLog.writeLog7(heat_id + ":更新炉次实际3成功");
                        by_TB7_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + heat_id + "更新炉次实际3成功");

                        TB_bof7status.Text = "吹氧开始";

                        DBconn.writeNew("update BY_BOFSTATUS set bof7_status_now=3");
                        DBconn.writeNew("update by_bofstatus set bof7_start=0");
                        DBconn.writeNew("update by_bofstatus set bof7_o2start=0");
                        DBconn.writeNew("update by_shoudong set sd_bof7o2start=0");
                        WriteLog.writeLog7("------吹氧开始完成------");
                        by_TB7_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": ------吹氧开始完成------");

                    }
                    else//不正常开始 炉次开始信号没有来
                    {
                        WriteLog.writeLog7(":吹氧异常开始");
                        by_TB7_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": 吹氧异常开始");

                        dt_zlsj_temp_7 = DBconn.readNew(" select * from zlsj_temp where by_station_no ='7'");

                        WriteLog.writeLog7(":判断上炉是否结束");
                        by_TB7_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": 判断上炉是否结束");
                        if (Convert.ToInt32(dt_zlsj_temp_7.Rows[0]["STATUS"]) > 4)          //强制结束上一炉
                        {
                            WriteLog.writeLog7(heat_id + ":上炉未结束");
                            by_TB7_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + heat_id + "上一炉未结束");
                            pono = dt_zlsj_temp_7.Rows[0]["pono"].ToString();
                            plan_no = dt_zlsj_temp_7.Rows[0]["sm_plan_no"].ToString();
                            st_no = dt_zlsj_temp_7.Rows[0]["st_no"].ToString();
                            heat_id = dt_zlsj_temp_7.Rows[0]["HEAT_NO"].ToString();
                            DBconn.writeNew("insert into yzzt(sm_plan_no, proc_no, station_id, station_no, heat_no, pono, st_no, run_signal, run_time)values('" + plan_no + "'," + heat_id + ",'B','7'," + heat_id + ",'" + pono + "','" + st_no + "','376','" + DateTime.Now.ToString("yyyyMMddHHmmss") + "')");
                            WriteLog.writeLog7("写入376运转状态成功" + heat_id + "|" + pono + "|" + plan_no);
                            by_TB7_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": 写入376运转状态成功" + heat_id + "|" + pono + "|" + plan_no);

                            DBconn.writeNew("update zlsj_temp set STATUS ='6',STATUS_NEW ='1'  where BY_STATION_NO ='7'");
                            DBconn.writeNew("update BY_BOFSTATUS set bof7_status_now=7");
                            WriteLog.writeLog7("更新转炉实际状态为6成功");
                            by_TB7_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": 更新转炉实际状态为6成功");

                            DBconn.writeNew("update zlsj_temp set STATUS ='0',HEAT_NO ='' where BY_STATION_NO ='7'");
                            WriteLog.writeLog7("强制结束完成");
                            by_TB7_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": 强制结束完成");
                        }
                        else
                        {
                            WriteLog.writeLog7(heat_id + ":上炉成功结束，炉次开始信号没来，模拟炉次开始");
                            by_TB7_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + heat_id + "上炉成功结束，炉次开始信号没来，模拟炉次开始");
                            //---------------------------------------
                            heat_id_L1 = DBconn.readOld("select heatid from bof7status").Rows[0][0].ToString();//从一级取当前炉号
                            WriteLog.writeLog7(":从L1取炉号" + heat_id_L1);
                            by_TB7_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": 从L1取炉号" + heat_id_L1);
                            //---------------------------------------
                            if (Convert.ToInt32(heat_id_L1) > Convert.ToInt32(bof_no_history))//炉号正常加一
                            {

                                dt_cgjh_7 = DBconn.readNew("select * from cgjh where bof_no='7' and status is null order by to_number(plan_no)");  //读取最近一个未用的计划
                                if (dt_cgjh_7.Rows.Count > 0)
                                {
                                    bof_no = dt_cgjh_7.Rows[0]["bof_no"].ToString();
                                    pono = dt_cgjh_7.Rows[0]["pono"].ToString();
                                    plan_no = dt_cgjh_7.Rows[0]["plan_no"].ToString();
                                    st_no = dt_cgjh_7.Rows[0]["st_no"].ToString();
                                    WriteLog.writeLog7("炉次在吹氧开始读取计划成功" + bof_no + "|" + pono + "|" + plan_no);
                                    by_TB7_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": 炉次在吹氧开始读取计划成功" + bof_no + "|" + pono + "|" + plan_no);

                                    DBconn.writeNew("insert into yzzt(sm_plan_no, proc_no, station_id, station_no, heat_no, pono, st_no, run_signal, run_time)values('" + plan_no + "'," + heat_id_L1 + ",'B','7'," + heat_id_L1 + ",'" + pono + "','" + st_no + "','371','" + DateTime.Now.ToString("yyyyMMddHHmmss") + "')");
                                    WriteLog.writeLog7("写入运转状态：" + plan_no + "|" + heat_id_L1 + "|" + pono + "成功");
                                    by_TB7_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": 写入371运转状态：" + plan_no + "|" + heat_id_L1 + "|" + pono + "成功");

                                    DBconn.writeNew("update cgjh set status ='1',heat_no='" + heat_id_L1 + "' where plan_no ='" + plan_no + "'");
                                    WriteLog.writeLog7("更新出钢计划1成功");
                                    by_TB7_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": 更新出钢计划1成功");

                                    DBconn.writeNew("update zlsj_temp set SM_PLAN_NO ='" + plan_no + "',pono ='" + pono + "',HEAT_NO ='" + heat_id_L1 + "', ST_NO ='" + st_no + "',STATION_NO='" + bof_no + "',PROD_DATE='" + DateTime.Now.ToString("yyyyMMdd") + "', STATUS ='1' where BY_STATION_NO ='7'");
                                    WriteLog.writeLog7("更新转炉实际1成功");
                                    by_TB7_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": 更新转炉实际1成功");

                                    TB_bof7status.Text = "炉次在吹氧开始";
                                    TB_bof7heatid.Text = heat_id_L1.ToString();
                                    DBconn.writeNew("update by_bofstatus set bof7_start=0,bof7_o2end=0,bof7_cgstart=0,bof7_cgend=0");//炉次开始初始化状态位
                                    DBconn.writeNew("update by_shoudong set sd_bof7start=0");
                                    DBconn.writeNew("update BY_BOFSTATUS set bof7_status_now=1");
                                    WriteLog.writeLog7(heat_id_L1 + "炉次在吹氧开始");
                                    by_TB7_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + heat_id_L1 + "炉次在吹氧开始");
                                }
                                else
                                {
                                    DBconn.writeNew("update by_bofstatus set bof7_start=0");
                                    DBconn.writeNew("update by_shoudong set sd_bof7start=0");
                                    DBconn.writeNew("update by_bofstatus set bof7_o2start=0");
                                    DBconn.writeNew("update by_shoudong set sd_bof7o2start=0");
                                    WriteLog.writeLog7("炉次在吹氧开始失败，没有计划，bof7_start,bof7_o2start已置0");
                                    by_TB7_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": 炉次在吹氧开始失败，没有计划，bof7_start,bof7_o2start已置0");
                                }
                            }
                            else
                            {
                                DBconn.writeNew("update by_bofstatus set bof7_o2start=0");
                                DBconn.writeNew("update by_shoudong set sd_bof7o2start=0");
                                DBconn.writeNew("update by_bofstatus set bof7_start=0");
                                DBconn.writeNew("update by_shoudong set sd_bof7start=0");
                                WriteLog.writeLog7("L1炉号" + heat_id_L1 + "不大于运转状态最大炉号" + bof_no_history + "炉次在吹氧开始失败标志位已置0");
                                by_TB7_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "L1炉号" + heat_id_L1 + "不大于运转状态最大炉号" + bof_no_history + "炉次在吹氧开始失败标志位已置0");
                            }
                        }
                    }


                }
                catch (Exception err)
                {
                    WriteLog.writeLog7("吹氧开始运行失败标志位已置0" + err.Message);
                    DBconn.writeNew("update by_bofstatus set bof7_o2start=0");
                    DBconn.writeNew("update by_shoudong set sd_bof7o2start=0");
                    DBconn.writeNew("update by_bofstatus set bof7_start=0");
                    DBconn.writeNew("update by_shoudong set sd_bof7start=0");
                    by_TB7_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + err.Message + "吹氧开始运行失败标志位已置0" + heat_id);
                }
                #endregion

            }
            if (bof7o2end_l1 == "1" && heat_id != "" || sd_bof7o2end == "1")//-------------------------------------------------------吹氧结束

            {
                try
                {
                    WriteLog.writeLog7(heat_id + "------吹氧结束开始------");
                    by_TB7_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + heat_id + "------吹氧结束开始------");
                    dt_zlsj_temp_7 = DBconn.readNew("select * from zlsj_temp where by_station_no=7");
                    if (Convert.ToInt32(dt_zlsj_temp_7.Rows[0]["STATUS"]) < 4)//正常炉次开始
                    {
                        WriteLog.writeLog7(heat_id + "吹氧结束正常开始");
                        by_TB7_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + heat_id + "吹氧结束正常开始");

                        pono = dt_zlsj_temp_7.Rows[0]["pono"].ToString();
                        plan_no = dt_zlsj_temp_7.Rows[0]["SM_PLAN_NO"].ToString();
                        st_no = dt_zlsj_temp_7.Rows[0]["st_no"].ToString();

                        DBconn.writeNew("insert into yzzt(sm_plan_no, proc_no, station_id, station_no, heat_no, pono, st_no, run_signal, run_time)values('" + plan_no + "'," + heat_id + ",'B','7'," + heat_id + ",'" + pono + "','" + st_no + "','374','" + DateTime.Now.ToString("yyyyMMddHHmmss") + "')");
                        WriteLog.writeLog7(heat_id + "插入运转状态374成功" + plan_no + "|" + pono);
                        by_TB7_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + heat_id + "插入运转状态374成功" + plan_no + "|" + pono);

                        DBconn.writeNew("update zlsj_temp set STATUS ='4' where by_station_no ='7'");
                        WriteLog.writeLog7(heat_id + "更新转炉实际标志位4成功");
                        by_TB7_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + heat_id + "更新转炉实际标志位4成功");

                        TB_bof7status.Text = "吹氧结束";
                        DBconn.writeNew("update by_bofstatus set bof7_o2end=0");
                        DBconn.writeNew("update by_shoudong set sd_bof7o2end=0");
                        DBconn.writeNew("update BY_BOFSTATUS set bof7_status_now=4");
                        WriteLog.writeLog7(heat_id + "------吹氧结束成功------");
                        by_TB7_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + heat_id + "------吹氧结束成功------");

                    }
                    else
                    {
                        DBconn.writeNew("update by_bofstatus set bof7_o2end=0");
                        DBconn.writeNew("update by_shoudong set sd_bof7o2end=0");
                        WriteLog.writeLog7(heat_id + "吹氧结束开始失败status必须小于4，重置标志位");
                        by_TB7_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + heat_id + "吹氧结束开始失败status必须小于4，重置标志位");
                    }
                }
                catch (Exception err)
                {
                    WriteLog.writeLog7("吹氧结束失败" + err.Message);
                    by_TB7_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + err.Message + "吹氧结束失败" + heat_id);
                    DBconn.writeNew("update by_bofstatus set bof7_o2end=0");
                    DBconn.writeNew("update by_shoudong set sd_bof7o2end=0");
                }
            }

            if (bof7cgstart_l1 == "1" && heat_id != "" || sd_bof7cgstart == "1")//-------------------------------------------------------出钢开始

            {
                try
                {
                    WriteLog.writeLog7(heat_id + "------出钢开始------");
                    by_TB7_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + heat_id + "------出钢开始------");
                    dt = DBconn.readNew("select * from zlsj_temp where by_station_no=7");
                    if (Convert.ToInt32(dt.Rows[0]["STATUS"]) < 5)//正常炉次开始
                    {
                        WriteLog.writeLog7(heat_id + "正常出钢开始");
                        by_TB7_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + heat_id + "正常出钢开始");
                        pono = dt.Rows[0]["pono"].ToString();
                        plan_no = dt.Rows[0]["SM_PLAN_NO"].ToString();
                        st_no = dt.Rows[0]["st_no"].ToString();
                        DBconn.writeNew("insert into yzzt(sm_plan_no, proc_no, station_id, station_no, heat_no, pono, st_no, run_signal, run_time)values('" + plan_no + "'," + heat_id + ",'B','7'," + heat_id + ",'" + pono + "','" + st_no + "','375','" + DateTime.Now.ToString("yyyyMMddHHmmss") + "')");
                        WriteLog.writeLog7(heat_id + "插入运转状态375成功" + plan_no + "|" + pono);
                        by_TB7_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + heat_id + "插入运转状态375成功" + plan_no + "|" + pono);
                        try
                        {
                            DBconn.writeOld("call NEW_ZLJLSJ7_SEND_PRO('" + heat_id + "')");//传送转炉加料实绩
                        }
                        catch (Exception err)
                        {
                            WriteLog.writeLog7(heat_id + "传送转炉加料实绩失败" + err.Message);
                            by_TB7_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + heat_id + "传送转炉加料实绩失败" + err.Message);
                        }
                        try//更新吹氧数据
                        {
                            int i = 0;

                            DataTable dt_cuiyang = DBconn.readOld("select round((t.blowendtime-t.blowbegintime)*24*60*60),t.blowbegintime,t.blowendtime,round(t.blowo2amount) from BOF7BLOWO2DATA t where t.heatid='" + heat_id + "' order by t.blowo2times");
                            if (dt_cuiyang.Rows.Count > 0)
                            {
                                DBconn.writeNew("update zlsj_temp set reblow_num='" + dt_cuiyang.Rows.Count + "' where by_station_no=7");
                                string starttime;//开始时间
                                string endtime;//结束时间
                                int t1;//单个吹氧时间
                                int tall = 0;//总吹氧时间
                                int EXT_ITEM3 = 0;//补吹次数
                                int EXT_ITEM4_temp = 0;//第一次吹氧量
                                int EXT_ITEM4 = 0;//补吹氧量
                                int REBLOW_DURATION = 0;//补吹持续时间
                                foreach (DataRow dr in dt_cuiyang.Rows)
                                {
                                    i++;
                                    if (i <= 5)
                                    {
                                        starttime = Convert.ToDateTime(dr[1]).ToString("yyyyMMddHHmmss");
                                        endtime = Convert.ToDateTime(dr[2]).ToString("yyyyMMddHHmmss");
                                        t1 = Convert.ToInt32(dr[0].ToString());
                                        tall = tall + t1;
                                        DBconn.writeNew("update zlsj_temp set blow_time" + i + "='" + t1 + "',blow_start_time" + i + "='" + starttime + "',blow_end_time" + i + "='" + endtime + "',o2_sum_comsume='" + dr[3].ToString() + "',blow_time='" + tall + "' where by_station_no=7");
                                        if (i < 2)
                                        {
                                            EXT_ITEM4_temp = Convert.ToInt32(dr[3].ToString());
                                        }
                                        if (i > 1)
                                        {
                                            EXT_ITEM4 = 0;
                                            REBLOW_DURATION = REBLOW_DURATION + t1;
                                            EXT_ITEM3++;
                                            EXT_ITEM4 = Convert.ToInt32(dr[3].ToString()) - EXT_ITEM4_temp;
                                        }
                                    }
                                }
                                DBconn.writeNew("update zlsj_temp set EXT_ITEM3='" + EXT_ITEM3 + "',EXT_ITEM4='" + EXT_ITEM4 + "',REBLOW_DURATION='" + REBLOW_DURATION + "'  where by_station_no=7");
                            }
                        }
                        catch (Exception err)
                        {
                            WriteLog.writeLog7(heat_id + "吹氧数据更新失败" + err.Message);
                            by_TB7_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + heat_id + "吹氧数据更新失败" + err.Message);
                        }
                        DBconn.writeNew("update zlsj_temp set STATUS ='5' where by_station_no ='7'");
                        WriteLog.writeLog7(heat_id + "更新转炉实际标志位成功");
                        by_TB7_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + heat_id + "更新转炉实际标志位成功");
                        TB_bof7status.Text = "出钢开始";
                        DBconn.writeNew("update by_bofstatus set bof7_cgstart=0");
                        DBconn.writeNew("update by_shoudong set sd_bof7cgstart=0");
                        DBconn.writeNew("update BY_BOFSTATUS set bof7_status_now=5");
                        WriteLog.writeLog7(heat_id + "------出钢开始完成------");
                        by_TB7_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + heat_id + "------出钢开始完成------");
                    }
                    else
                    {
                        DBconn.writeNew("update by_bofstatus set bof7_cgstart=0");
                        DBconn.writeNew("update by_shoudong set sd_bof7cgstart=0");
                        WriteLog.writeLog7(heat_id + "出钢开始失败，标志位不小于5");
                        by_TB7_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + heat_id + "出钢开始失败，标志位不小于5");
                    }

                }
                catch (Exception err)
                {
                    WriteLog.writeLog7("出钢开始失败" + err.Message);
                    by_TB7_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + err.Message + "出钢开始失败" + heat_id);
                    DBconn.writeNew("update by_bofstatus set bof7_cgstart=0");
                    DBconn.writeNew("update by_shoudong set sd_bof7cgstart=0");
                }


                //  bof7thisheatid++;//把当炉炉号加1
                // DBconn.writeNew("update by_bofstatus set bof7_heatid_now=" + DateTime.Now.ToString("yy")   + bof7thisheatid);//更新数据库炉号


            }
            if (bof7cgend_l1 == "1" && DBconn.readNew("select heat_no from zlsj_temp where by_station_no=7").Rows[0][0].ToString() != "" || sd_bof7cgend == "1")//-------------------------------------------------------出钢结束

            {
                try
                {
                    WriteLog.writeLog7(heat_id + "------出钢结束开始------");
                    by_TB7_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + heat_id + "------出钢结束开始------");
                    dt = DBconn.readNew("select * from zlsj_temp where by_station_no=7");
                    if (Convert.ToInt32(dt.Rows[0]["STATUS"]) < 6)//正常炉次开始
                    {
                        WriteLog.writeLog7(heat_id + "出钢结束正常开始");
                        by_TB7_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + heat_id + "出钢结束正常开始");
                        pono = dt.Rows[0]["pono"].ToString();
                        plan_no = dt.Rows[0]["SM_PLAN_NO"].ToString();
                        st_no = dt.Rows[0]["st_no"].ToString();

                        DBconn.writeNew("insert into yzzt(sm_plan_no, proc_no, station_id, station_no, heat_no, pono, st_no, run_signal, run_time)values('" + plan_no + "'," + heat_id + ",'B','7'," + heat_id + ",'" + pono + "','" + st_no + "','376','" + DateTime.Now.ToString("yyyyMMddHHmmss") + "')");
                        WriteLog.writeLog7(heat_id + "插入运转状态376成功" + plan_no + "|" + pono);
                        by_TB7_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + heat_id + "插入运转状态376成功" + plan_no + "|" + pono);

                        DBconn.writeNew("call zlsj_send_pro('" + heat_id + "')");//传送转炉实绩

                        DBconn.writeNew("update zlsj_temp set tap_end_time='" + DateTime.Now.ToString("yyyyMMddHHmmss") + "',STATUS ='6',status_new='1' where by_station_no ='7'");

                        WriteLog.writeLog7(heat_id + "更新转炉实际成功");
                        by_TB7_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + heat_id + "更新转炉实际成功");
                        endheat_no7 = heat_id;
                        DBconn.writeNew("update zlsj_temp set STATUS ='0',HEAT_NO ='' where by_station_no ='7'");
                        DBconn.writeNew("update by_bofstatus set bof7_start=0,bof7_o2start=0,bof7_o2end=0,bof7_cgstart=0,bof7_cgend=0");
                        DBconn.writeNew("update by_shoudong set sd_bof7cgend=0");
                        DBconn.writeNew("update BY_BOFSTATUS set bof7_status_now=6");
                        TB_bof7status.Text = "出钢结束";
                        WriteLog.writeLog7(heat_id + "------出钢结束完成------");
                        by_TB7_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + heat_id + "------出钢结束完成------");
                    }
                    else
                    {
                        DBconn.writeNew("update by_bofstatus set bof7_start=0,bof7_o2start=0,bof7_o2end=0,bof7_cgstart=0,bof7_cgend=0");
                        DBconn.writeNew("update by_shoudong set sd_bof7cgend=0");
                        WriteLog.writeLog7(heat_id + "出钢结束失败，状态位不小于6");
                        by_TB7_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + heat_id + "出钢结束失败，状态位不小于6");
                    }



                }
                catch (Exception err)
                {
                    WriteLog.writeLog7("出钢结束失败" + err.Message);
                    by_TB7_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + err.Message + "出钢结束失败" + heat_id);
                    DBconn.writeNew("update by_bofstatus set bof7_cgend=0");
                    DBconn.writeNew("update by_shoudong set sd_bof7cgend=0");
                }


            }
            if (bof7end_l1 == "1")//-------------------------------------------------------炉次结束
            {

                try
                {
                    if (heat_id != "")
                    {
                        WriteLog.writeLog7(heat_id + "炉次结束信号来有炉号，炉次未结束或结束信号和开始信号一起来");
                        by_TB7_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + heat_id + "炉次结束信号来有炉号，炉次未结束或结束信号和开始信号一起来");
                        //DBconn.writeNew("update zlsj_temp set tap_end_time='" + DateTime.Now.ToString("yyyyMMddHHmmss") + "',STATUS ='6',status_new='1' where by_station_no ='7'");
                        //WriteLog.writeLog7(heat_id + "更新炉次结束状态位成功");
                        //by_TB7_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + heat_id + "更新炉次结束状态位成功");
                    }
                    else if (endheat_no7 != "")
                    {
                        WriteLog.writeLog7(endheat_no7 + "开始结束炉次写历史炉次标志位");
                        by_TB7_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + endheat_no7 + "开始结束炉次写历史炉次标志位");

                        DBconn.readNew("update zlsj_history set status=8 where heat_no='" + endheat_no7 + "'");

                        WriteLog.writeLog7(endheat_no7 + "炉次结束_历史炉次标志位置9");
                        by_TB7_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + endheat_no7 + "炉次结束_历史炉次标志位置9");
                    }
                    DBconn.writeNew("update BY_BOFSTATUS set bof7_status_now=7");
                    DBconn.writeNew("update BY_BOFSTATUS set bof7_end=0");
                    WriteLog.writeLog7(heat_id + "炉次结束");
                    by_TB7_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + heat_id + "炉次7#结束");
                }
                catch (Exception err)
                {
                    DBconn.writeNew("update by_bofstatus set bof7_end=0");
                    WriteLog.writeLog7("炉次结束失败" + err.Message);
                    by_TB7_message.AppendText("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + err.Message + "炉次结束失败" + heat_id);
                }

            }
        }
        #endregion


        /// <summary>
        /// 新跟踪程序
        /// </summary>
        /// <param name="BSNo">炉座号</param>
        /// <param name="vbofstart_l1">炉次开始</param>
        /// <param name="vsd_bofstart">炉次手动开始</param>
        /// <param name="vbofo2start_l1">吹氧开始</param>
        /// <param name="vsd_bofo2start">吹氧手动开始</param>
        /// <param name="vbofo2end_l1">吹氧结束</param>
        /// <param name="vsd_bofo2end">吹氧手动结束</param>
        /// <param name="vbofcgstart_l1">出钢开始</param>
        /// <param name="vsd_bofcgstart">出钢手动开始</param>
        /// <param name="vbofcgend_l1">出钢结束</param>
        /// <param name="vsd_bofcgend">出钢手动结束</param>
        /// <param name="vbofend_l1">炉次结束</param>
        /// <param name="vbof_o2start_again">补吹开始</param>
        /// <param name="vbof_o2end_again">补吹结束</param>
        /// <param name="vsd_bof_o2start_again">补吹手动开始</param>
        /// <param name="vsd_bof_o2end_again">补吹手动结束</param>
        private void boftraceAuto(string BSNo, string vbofstart_l1, string vsd_bofstart, string vbofo2start_l1, string vsd_bofo2start, string vbofo2end_l1, string vsd_bofo2end, string vbofcgstart_l1, string vsd_bofcgstart, string vbofcgend_l1, string vsd_bofcgend, string vbofend_l1,string vbof_o2start_again,string vbof_o2end_again,int vsd_bof_o2start_again,int vsd_bof_o2end_again)
        {
            try {
                
                //read_by_bofstatus();
                string heat_id = "";
                string heat_id_L1;
                string heat_id_L2;
                DataTable dt;
                DataTable dt_zlsj_temp;
                DataTable dt_cgjh;
                string pono, plan_no, st_no, bof_no;
                dt_zlsj_temp = DBconn.readNew("select * from zlsj_temp t  where t.by_station_no=" + BSNo);
                bof_no_history = DBconn.readNew("select max(heat_no) from YZZT t  where t.station_no=" + BSNo).Rows[0][0].ToString();
                if (bof_no_history == "")
                {
                    bof_no_history = "1";
                }
                if (dt_zlsj_temp.Rows.Count > 0)
                {
                    heat_id = dt_zlsj_temp.Rows[0]["HEAT_NO"].ToString();
                }

                if (vbofstart_l1 == "1" || vsd_bofstart == "1")//-------------------------------------------------------炉次开始

                {
                    #region 炉次开始
                    try
                    {
                        WritelogAuto(BSNo, "------炉次开始------");

                        dt_zlsj_temp = DBconn.readNew(" select * from zlsj_temp where BY_STATION_NO ='" + BSNo + "'");
                        if (Convert.ToInt32(dt_zlsj_temp.Rows[0]["STATUS"]) > 4 && heat_id != "")          //强制结束上一炉
                        {

                            //pono = dt_zlsj_temp.Rows[0]["pono"].ToString();
                            //plan_no = dt_zlsj_temp.Rows[0]["sm_plan_no"].ToString();
                            //st_no = dt_zlsj_temp.Rows[0]["st_no"].ToString();
                            //heat_id = dt_zlsj_temp.Rows[0]["HEAT_NO"].ToString();
                            //WritelogAuto(BSNo, "上炉未正常结束强制结束" + heat_id);

                            //DBconn.writeNew("insert into yzzt(sm_plan_no, proc_no, station_id, station_no, heat_no, pono, st_no, run_signal, run_time)values('" + plan_no + "'," + heat_id + ",'B','" + BSNo + "'," + heat_id + ",'" + pono + "','" + st_no + "','3" + BSNo + "6','" + DateTime.Now.ToString("yyyyMMddHHmmss") + "')");
                            //WritelogAuto(BSNo, "写入3" + BSNo + "6运转状态成功" + heat_id + "|" + pono + "|" + plan_no);


                            //DBconn.writeNew("update zlsj_temp set STATUS ='6',STATUS_NEW ='1'  where BY_STATION_NO ='" + BSNo + "'");
                            //WritelogAuto(BSNo, "更新转炉实际状态为6成功");

                            //DBconn.writeNew("update zlsj_temp set STATUS ='0',HEAT_NO ='' where BY_STATION_NO ='" + BSNo + "'");
                            //DBconn.writeNew("update BY_BOFSTATUS set bof" + BSNo + "_status_now=7");
                            //WritelogAuto(BSNo, "强制结束完成");
                            #region 强制结束上一炉
                            try
                            {
                                WritelogAuto(BSNo, heat_id + "------炉次开始检测到上炉未结束，模拟出钢结束------");
                                if (DBconn.readNew("select heat_no from zlsj_temp where by_station_no=" + BSNo).Rows[0][0].ToString() != "")
                                {

                                    dt = DBconn.readNew("select * from zlsj_temp where by_station_no=" + BSNo);
                                    if (Convert.ToInt32(dt.Rows[0]["STATUS"]) < 6)//正常炉次开始
                                    {
                                        //WritelogAuto(BSNo, heat_id + "出钢结束正常开始");

                                        pono = dt.Rows[0]["pono"].ToString();
                                        plan_no = dt.Rows[0]["SM_PLAN_NO"].ToString();
                                        st_no = dt.Rows[0]["st_no"].ToString();

                                        DBconn.writeNew("insert into yzzt(sm_plan_no, proc_no, station_id, station_no, heat_no, pono, st_no, run_signal, run_time)values('" + plan_no + "'," + heat_id + ",'B','" + BSNo + "'," + heat_id + ",'" + pono + "','" + st_no + "','3" + BSNo + "6','" + DateTime.Now.ToString("yyyyMMddHHmmss") + "')");
                                        WritelogAuto(BSNo, heat_id + "插入运转状态3" + BSNo + "6成功;计划号=" + plan_no + ",制造命令号=" + pono);

                                        o2jisuan(BSNo, heat_id);//吹氧数据计算
                                        Thread.Sleep(1100);

                                        DBconn.writeNew("update zlsj_temp set tap_end_time='" + DateTime.Now.ToString("yyyyMMddHHmmss") + "',STATUS ='6',status_new='1' where by_station_no ='" + BSNo + "'");
                                        WritelogAuto(BSNo, heat_id + "更新转炉实际成功");
                                        
                                        if (BSNo == "5")
                                            endheat_no5 = heat_id;
                                        if (BSNo == "6")
                                            endheat_no6 = heat_id;
                                        if (BSNo == "7")
                                            endheat_no7 = heat_id;
                                        try
                                        {
                                            int nextheatid = heatid_add1(heat_id);
                                            DBconn.writeOld("UPDATE BOF" + BSNo + "CURRENTID SET HeatID='" + nextheatid + "'");
                                            WritelogAuto(BSNo, heat_id + "更新BOF" + BSNo + "CURRENTID_HeatID=" + nextheatid + "成功");
                                        }
                                        catch (Exception err)
                                        {
                                            WritelogAuto(BSNo, heat_id + "更新BOF" + BSNo + "CURRENTID_HeatID失败" + err.Message);
                                        }
                                        zljlsj_time(heat_id, BSNo);//合金时间计算
                                        DBconn.writeNew("update zlsj_temp set STATUS ='0',HEAT_NO ='' where by_station_no ='" + BSNo + "'");
                                        DBconn.writeNew("update by_bofstatus set bof" + BSNo + "_start=0,bof" + BSNo + "_o2start=0,bof" + BSNo + "_o2end=0,bof" + BSNo + "_cgstart=0,bof" + BSNo + "_cgend=0");
                                        DBconn.writeNew("update by_shoudong set sd_bof" + BSNo + "cgend=0");
                                        DBconn.writeNew("update BY_BOFSTATUS set bof" + BSNo + "_status_now=6");
                                        ChangeTextBox(BSNo, "出钢结束", heat_id);

                                        WritelogAuto(BSNo, heat_id + "模拟出钢结束成功", 1);

                                    }
                                    else
                                    {
                                        DBconn.writeNew("update by_bofstatus set bof" + BSNo + "_start=0,bof" + BSNo + "_o2start=0,bof" + BSNo + "_o2end=0,bof" + BSNo + "_cgstart=0,bof" + BSNo + "_cgend=0");
                                        DBconn.writeNew("update by_shoudong set sd_bof" + BSNo + "cgend=0");
                                        WritelogAuto(BSNo, heat_id + "模拟出钢结束失败，状态位不小于6", 2);

                                    }
                                }
                                else
                                {
                                    DBconn.writeNew("update by_bofstatus set bof" + BSNo + "_cgend=0");
                                    DBconn.writeNew("update by_shoudong set sd_bof" + BSNo + "cgend=0");
                                    WritelogAuto(BSNo, "模拟出钢结束失败，没有炉号", 2);
                                }



                            }
                            catch (Exception err)
                            {
                                WritelogAuto(BSNo, "出钢结束失败" + err.Message, 3);

                                DBconn.writeNew("update by_bofstatus set bof" + BSNo + "_cgend=0");
                                DBconn.writeNew("update by_shoudong set sd_bof" + BSNo + "cgend=0");
                            }
                            #endregion

                        }
                        //--------------------------------------
                        heat_id_L1 = DBconn.readOld("select heatid from bof" + BSNo + "status").Rows[0][0].ToString();//从一级取当前炉号
                        heat_id_L2 = DBconn.readNew("select t.heat_no from HEATNO_REALTIME t where t.bofno=" + BSNo).Rows[0][0].ToString();//实时炉号
                        WritelogAuto(BSNo, ":从L1取炉号" + heat_id_L1 + ";从实时表取炉号" + heat_id_L2);
                        if(Convert.ToInt32(heat_id_L1)> Convert.ToInt32(heat_id_L2)&&bofstartcount[(Convert.ToInt32(BSNo)-5)]<=3)
                        {
                            WritelogAuto(BSNo, ":L1炉号" + heat_id_L1 + "与实时表炉号" + heat_id_L2+"不相等下一循环重试，重试次数="+ bofstartcount[(Convert.ToInt32(BSNo) - 5)], 1);
                            bofstartcount[(Convert.ToInt32(BSNo) - 5)]++;
                            return;
                        }else if (Convert.ToInt32(heat_id_L1) > Convert.ToInt32(heat_id_L2) && bofstartcount[(Convert.ToInt32(BSNo) - 5)] > 3)
                        {
                            WritelogAuto(BSNo, ":L1炉号" + heat_id_L1 + "与实时表炉号" + heat_id_L2 + "不相等，重试次数>3放弃炉次开始",2);
                            DBconn.writeNew("update by_bofstatus set bof" + BSNo + "_start=0,bof" + BSNo + "_o2start=0,bof" + BSNo + "_o2end=0,bof" + BSNo + "_cgstart=0,bof" + BSNo + "_cgend=0,BOF" + BSNo + "_O2START_AGAIN=0,BOF" + BSNo + "_O2END_AGAIN=0,BOF" + BSNo + "_END=0");
                            DBconn.writeNew("update by_shoudong t set t.sd_bof" + BSNo + "start=0,t.sd_bof" + BSNo + "o2start=0,t.sd_bof" + BSNo + "o2end=0,t.sd_bof" + BSNo + "cgstart=0,t.sd_bof" + BSNo + "cgend=0");
                            bofstartcount[(Convert.ToInt32(BSNo) - 5)] = 0;
                            return;
                        }
                        bofstartcount[(Convert.ToInt32(BSNo) - 5)] = 0;
                        //--------------------------------------
                        if (heat_id_L1!="" && Convert.ToInt32(heat_id_L1) > Convert.ToInt32(bof_no_history))//一级炉号正常加一
                        {
                            dt_cgjh = DBconn.readNew("select * from cgjh where bof_no='" + BSNo + "' and status is null and trim(heat_no) is null order by bof_blow_time");  //读取最近一个未用的计划
                            if (dt_cgjh.Rows.Count > 0)
                            {
                                bof_no = dt_cgjh.Rows[0]["bof_no"].ToString();
                                pono = dt_cgjh.Rows[0]["pono"].ToString();
                                plan_no = dt_cgjh.Rows[0]["plan_no"].ToString();
                                st_no = dt_cgjh.Rows[0]["st_no"].ToString();
                                string bof_blow_time= dt_cgjh.Rows[0]["bof_blow_time"].ToString();
                                WritelogAuto(BSNo, "绑定L1炉号=" + heat_id_L1 + ",制造命令号=" + pono + ",计划号=" + plan_no + ",计划开始时间=" + bof_blow_time);

                                DataTable dt_huitui = new DataTable();
                                dt_huitui = DBconn.readNew("select * from huitui_time t where t.status='0' and t.huitui_heat_no='" + heat_id_L1 + "'");
                                if (dt_huitui.Rows.Count > 0)
                                {
                                    string huitui_time = dt_huitui.Rows[0]["huitui_bof_start_time"].ToString();
                                    DBconn.writeNew("insert into yzzt(sm_plan_no, proc_no, station_id, station_no, heat_no, pono, st_no, run_signal, run_time)values('" + plan_no + "'," + heat_id_L1 + ",'B','" + BSNo + "'," + heat_id_L1 + ",'" + pono + "','" + st_no + "','3" + BSNo + "1','" + huitui_time + "')");
                                    WritelogAuto(BSNo, heat_id_L1 + "回退炉次,写入运转状态3" + BSNo + "1:计划号=" + plan_no + ",炉号=" + heat_id_L1 + ",制造命令号=" + pono + ",开始时间=" + huitui_time);
                                    DBconn.writeNew("update huitui_time t set t.status='1' where t.huitui_heat_no='" + heat_id_L1 + "'");
                                }
                                else
                                {
                                    DBconn.writeNew("insert into yzzt(sm_plan_no, proc_no, station_id, station_no, heat_no, pono, st_no, run_signal, run_time)values('" + plan_no + "'," + heat_id_L1 + ",'B','" + BSNo + "'," + heat_id_L1 + ",'" + pono + "','" + st_no + "','3" + BSNo + "1','" + DateTime.Now.ToString("yyyyMMddHHmmss") + "')");
                                    WritelogAuto(BSNo, heat_id_L1 + "写入运转状态3" + BSNo + "1:计划号=" + plan_no + ",炉号=" + heat_id_L1 + ",制造命令号=" + pono);
                                }



                                DBconn.writeNew("update cgjh set status ='1',heat_no='" + heat_id_L1 + "' where plan_no ='" + plan_no + "'");
                                WritelogAuto(BSNo, heat_id_L1 + "更新出钢计划标志位成功");


                                DBconn.writeNew("update zlsj_temp set SM_PLAN_NO ='" + plan_no + "',pono ='" + pono + "',HEAT_NO ='" + heat_id_L1 + "', ST_NO ='" + st_no + "',STATION_NO='" + bof_no + "',PROD_DATE='" + DateTime.Now.ToString("yyyyMMdd") + "', STATUS ='1' where BY_STATION_NO ='" + BSNo + "'");
                                WritelogAuto(BSNo, heat_id_L1 + "更新转炉实际1成功");
                                DBconn.writeOld("delete from BOF"+BSNo+ "BLOWO2DATA where heatid='" + heat_id_L1 + "'");
                                WritelogAuto(BSNo, heat_id_L1 + "清空该炉吹氧数据成功");
                                ChangeTextBox(BSNo, "炉次开始", heat_id_L1);//跟踪界面显示

                                DBconn.writeNew("update by_bofstatus set bof" + BSNo + "_start=0,bof" + BSNo + "_o2start=0,bof" + BSNo + "_o2end=0,bof" + BSNo + "_cgstart=0,bof" + BSNo + "_cgend=0,BOF" + BSNo + "_O2START_AGAIN=0,BOF" + BSNo + "_O2END_AGAIN=0,BOF" + BSNo + "_END=0");//炉次开始初始化状态位
                                DBconn.writeNew("update by_shoudong set sd_bof" + BSNo + "start=0");
                                DBconn.writeNew("update BY_BOFSTATUS set bof" + BSNo + "_status_now=1");
                                run_once = 0;
                                WritelogAuto(BSNo, heat_id_L1 + "炉次开始完成", 1);

                            }
                            else
                            {
                                DBconn.writeNew("update by_bofstatus set bof" + BSNo + "_start=0,bof" + BSNo + "_o2start=0,bof" + BSNo + "_o2end=0,bof" + BSNo + "_cgstart=0,bof" + BSNo + "_cgend=0,BOF" + BSNo + "_O2START_AGAIN=0,BOF" + BSNo + "_O2END_AGAIN=0");
                                DBconn.writeNew("update by_shoudong set sd_bof" + BSNo + "start=0");
                                WritelogAuto(BSNo, "炉次开始失败，没有计划，bof" + BSNo + "_start已置0", 2);

                            }

                        }
                        else if (Convert.ToInt32(heat_id_L2) > Convert.ToInt32(bof_no_history))//实时炉号正常加一
                        {
                            dt_cgjh = DBconn.readNew("select * from cgjh where bof_no='" + BSNo + "' and status is null and trim(heat_no) is null order by bof_blow_time");  //读取最近一个未用的计划
                            if (dt_cgjh.Rows.Count > 0)
                            {
                                bof_no = dt_cgjh.Rows[0]["bof_no"].ToString();
                                pono = dt_cgjh.Rows[0]["pono"].ToString();
                                plan_no = dt_cgjh.Rows[0]["plan_no"].ToString();
                                st_no = dt_cgjh.Rows[0]["st_no"].ToString();
                                WritelogAuto(BSNo, "绑定实时炉号=" + heat_id_L2 + ",制造命令号=" + pono + ",计划号=" + plan_no);

                                DBconn.writeOld("update bof" + BSNo + "status t set t.heatid='" + heat_id_L2 + "'");
                                WritelogAuto(BSNo, "更新bof" + BSNo + "status表heatid=" + heat_id_L2);

                                DataTable dt_huitui = new DataTable();
                                dt_huitui = DBconn.readNew("select * from huitui_time t where t.status='0' and t.huitui_heat_no='" + heat_id_L2 + "'");
                                if (dt_huitui.Rows.Count > 0)
                                {
                                    string huitui_time = dt_huitui.Rows[0]["huitui_bof_start_time"].ToString();
                                    DBconn.writeNew("insert into yzzt(sm_plan_no, proc_no, station_id, station_no, heat_no, pono, st_no, run_signal, run_time)values('" + plan_no + "'," + heat_id_L2 + ",'B','" + BSNo + "'," + heat_id_L2 + ",'" + pono + "','" + st_no + "','3" + BSNo + "1','" + huitui_time + "')");
                                    WritelogAuto(BSNo, heat_id_L2 + "回退炉次,写入运转状态3" + BSNo + "1:计划号=" + plan_no + ",炉号=" + heat_id_L2 + ",制造命令号=" + pono + ",开始时间=" + huitui_time);
                                    DBconn.writeNew("update huitui_time t set t.status='1' where t.huitui_heat_no='" + heat_id_L2 + "'");
                                }
                                else
                                {
                                    DBconn.writeNew("insert into yzzt(sm_plan_no, proc_no, station_id, station_no, heat_no, pono, st_no, run_signal, run_time)values('" + plan_no + "'," + heat_id_L2 + ",'B','" + BSNo + "'," + heat_id_L2 + ",'" + pono + "','" + st_no + "','3" + BSNo + "1','" + DateTime.Now.ToString("yyyyMMddHHmmss") + "')");
                                    WritelogAuto(BSNo, heat_id_L2 + "写入运转状态3" + BSNo + "1:计划号=" + plan_no + ",炉号=" + heat_id_L2 + ",制造命令号=" + pono);
                                }




                                DBconn.writeNew("update cgjh set status ='1',heat_no='" + heat_id_L2 + "' where plan_no ='" + plan_no + "'");
                                WritelogAuto(BSNo, heat_id_L2 + "更新出钢计划标志位成功");


                                DBconn.writeNew("update zlsj_temp set SM_PLAN_NO ='" + plan_no + "',pono ='" + pono + "',HEAT_NO ='" + heat_id_L2 + "', ST_NO ='" + st_no + "',STATION_NO='" + bof_no + "',PROD_DATE='" + DateTime.Now.ToString("yyyyMMdd") + "', STATUS ='1' where BY_STATION_NO ='" + BSNo + "'");
                                WritelogAuto(BSNo, heat_id_L2 + "更新转炉实际1成功");
                                DBconn.writeOld("delete from BOF" + BSNo + "BLOWO2DATA where heatid='" + heat_id_L2 + "'");
                                WritelogAuto(BSNo, heat_id_L1 + "清空该炉吹氧数据成功");
                                ChangeTextBox(BSNo, "炉次开始", heat_id_L2);//跟踪界面显示

                                DBconn.writeNew("update by_bofstatus set bof" + BSNo + "_start=0,bof" + BSNo + "_o2start=0,bof" + BSNo + "_o2end=0,bof" + BSNo + "_cgstart=0,bof" + BSNo + "_cgend=0,BOF" + BSNo + "_O2START_AGAIN=0,BOF" + BSNo + "_O2END_AGAIN=0");//炉次开始初始化状态位
                                DBconn.writeNew("update by_shoudong set sd_bof" + BSNo + "start=0");
                                DBconn.writeNew("update BY_BOFSTATUS set bof" + BSNo + "_status_now=1");
                                run_once = 0;
                                WritelogAuto(BSNo, heat_id_L2 + "炉次开始完成", 1);

                            }
                            else
                            {
                                DBconn.writeNew("update by_bofstatus set bof" + BSNo + "_start=0,bof" + BSNo + "_o2start=0,bof" + BSNo + "_o2end=0,bof" + BSNo + "_cgstart=0,bof" + BSNo + "_cgend=0,BOF" + BSNo + "_O2START_AGAIN=0,BOF" + BSNo + "_O2END_AGAIN=0");
                                DBconn.writeNew("update by_shoudong set sd_bof" + BSNo + "start=0");
                                WritelogAuto(BSNo, "炉次开始失败，没有计划，bof" + BSNo + "_start已置0", 2);

                            }
                        }
                        else
                        {
                            DBconn.writeNew("update by_bofstatus set bof" + BSNo + "_start=0,bof" + BSNo + "_o2start=0,bof" + BSNo + "_o2end=0,bof" + BSNo + "_cgstart=0,bof" + BSNo + "_cgend=0,BOF" + BSNo + "_O2START_AGAIN=0,BOF" + BSNo + "_O2END_AGAIN=0");
                            DBconn.writeNew("update by_shoudong set sd_bof" + BSNo + "start=0");
                            WritelogAuto(BSNo, "炉号" + heat_id_L2 + "不大于运转状态炉号" + bof_no_history + "炉次开始失败", 2);

                        }
                    }
                    catch (Exception err)
                    {
                        WritelogAuto(BSNo, "炉次开始运行失败" + err.Message);
                        DBconn.writeNew("update by_bofstatus set bof" + BSNo + "_start=0,bof" + BSNo + "_o2start=0,bof" + BSNo + "_o2end=0,bof" + BSNo + "_cgstart=0,bof" + BSNo + "_cgend=0,BOF" + BSNo + "_O2START_AGAIN=0,BOF" + BSNo + "_O2END_AGAIN=0");
                        DBconn.writeNew("update by_shoudong set sd_bof" + BSNo + "start=0");
                        WritelogAuto(BSNo, err.Message + ";炉次开始运行失败", 3);
                    }
                    #endregion
                }
                else if (vbofo2start_l1 == "1" || vsd_bofo2start == "1")//-------------------------------------------------------吹氧开始

                {
                    #region 吹氧开始
                    try
                    {
                        WritelogAuto(BSNo, heat_id + "------吹氧开始------");
                        dt_zlsj_temp = DBconn.readNew("select * from zlsj_temp where by_station_no ='" + BSNo + "'");
                        dt_by_bofstatus = DBconn.readNew("select * from BY_BOFSTATUS t");
                        if (heat_id == "" && run_once == 0)//检测炉次是否开始,如果炉次没开始 开始炉次
                        {
                            WritelogAuto(BSNo, "吹氧开始检测到炉次未开始，先开始炉次", 2);
                            DBconn.writeNew("update BY_BOFSTATUS set bof" + BSNo + "_start=1 ");//模拟炉次开始
                            DBconn.writeNew("update by_shoudong set sd_bof" + BSNo + "o2start=1");//手动吹氧开始1
                            run_once++;
                            return;
                        }
                        else if (Convert.ToInt32(dt_zlsj_temp.Rows[0]["STATUS"]) < 3 && heat_id != "")//正常吹氧开始
                        {
                            WritelogAuto(BSNo, heat_id + "正常吹氧开始");

                            pono = dt_zlsj_temp.Rows[0]["pono"].ToString();
                            plan_no = dt_zlsj_temp.Rows[0]["SM_PLAN_NO"].ToString();
                            st_no = dt_zlsj_temp.Rows[0]["st_no"].ToString();

                            DBconn.writeNew("insert into yzzt(sm_plan_no, proc_no, station_id, station_no, heat_no, pono, st_no, run_signal, run_time)values('" + plan_no + "'," + heat_id + ",'B','" + BSNo + "'," + heat_id + ",'" + pono + "','" + st_no + "','3" + BSNo + "2','" + DateTime.Now.ToString("yyyyMMddHHmmss") + "')");
                            WritelogAuto(BSNo, heat_id + "写入运转状态3" + BSNo + "2:计划号=" + plan_no + ",炉号=" + heat_id + ",制造命令号=" + pono + "成功");
                            Thread.Sleep(1100);
                            DBconn.writeNew("insert into yzzt(sm_plan_no, proc_no, station_id, station_no, heat_no, pono, st_no, run_signal, run_time)values('" + plan_no + "'," + heat_id + ",'B','" + BSNo + "'," + heat_id + ",'" + pono + "','" + st_no + "','3" + BSNo + "3','" + DateTime.Now.ToString("yyyyMMddHHmmss") + "')");
                            WritelogAuto(BSNo, heat_id + "写入运转状态3" + BSNo + "3:计划号=" + plan_no + ",炉号=" + heat_id + ",制造命令号=" + pono + "成功");



                            DBconn.writeNew("update zlsj_temp set STATUS ='3' where by_station_no ='" + BSNo + "'");
                            WritelogAuto(BSNo, heat_id + ":更新炉次实际3成功");
                            try
                            {
                                DBconn.writeNew("call BY_BANCI('"+BSNo+"')");//计算班次
                                WritelogAuto(BSNo, heat_id + ":更新班次成功");
                            }
                            catch (Exception err)
                            {
                                WritelogAuto(BSNo, heat_id + ":更新班次失败", 2);
                            }
                            stop_heat(heat_id, BSNo);//停炉时间判断

                            ChangeTextBox(BSNo, "吹氧开始", heat_id);

                            DBconn.writeNew("update BY_BOFSTATUS set bof" + BSNo + "_status_now=3");
                            DBconn.writeNew("update by_bofstatus set bof" + BSNo + "_start=0,bof" + BSNo + "_o2start=0");
                            DBconn.writeNew("update by_shoudong set sd_bof" + BSNo + "o2start=0");
                            run_once = 0;
                            WritelogAuto(BSNo, heat_id + "吹氧开始完成", 1);

                        }
                        else//吹氧开始异常
                        {
                            DBconn.writeNew("update by_bofstatus set bof" + BSNo + "_o2start=0");
                            DBconn.writeNew("update by_shoudong set sd_bof" + BSNo + "o2start=0");
                            WritelogAuto(BSNo, heat_id + "吹氧开始失败,可能没有炉号或者计划", 2);
                            run_once = 0;
                        }


                    }
                    catch (Exception err)
                    {
                        WritelogAuto(BSNo, "吹氧开始运行失败标志位已置0" + err.Message, 3);
                        DBconn.writeNew("update by_bofstatus set bof" + BSNo + "_o2start=0");
                        DBconn.writeNew("update by_shoudong set sd_bof" + BSNo + "o2start=0");
                        run_once = 0;
                    }
                    #endregion

                }
                else if (vbofo2end_l1 == "1" || vsd_bofo2end == "1")//-------------------------------------------------------吹氧结束

                {
                    #region 吹氧结束
                    try
                    {
                        WritelogAuto(BSNo, heat_id + "------吹氧结束开始------");
                        dt_by_bofstatus = DBconn.readNew("select * from BY_BOFSTATUS t");
                        dt_zlsj_temp = DBconn.readNew("select * from zlsj_temp where by_station_no=" + BSNo);
                        if (heat_id == "" && run_once == 0)//检测炉次是否开始,如果炉次没开始 开始炉次
                        {
                            WritelogAuto(BSNo, "吹氧结束检测到炉次未开始，先开始炉次", 2);
                            DBconn.writeNew("update BY_BOFSTATUS set bof" + BSNo + "_start=1 ");//模拟炉次开始
                            DBconn.writeNew("update by_shoudong set sd_bof" + BSNo + "o2end=1");//手动吹氧结束1
                            run_once++;
                            return;
                        }
                        else if (dt_by_bofstatus.Rows[0]["bof" + BSNo + "_status_now"].ToString() == "1" && run_once == 0)//炉次开始了 但是吹氧开始没有来
                        {
                            WritelogAuto(BSNo, heat_id + "吹氧结束检测到吹氧未开始，先开始吹氧", 2);
                            DBconn.writeNew("update BY_BOFSTATUS set bof" + BSNo + "_o2start=1 ");//模拟吹氧开始
                            run_once++;
                            return;
                        }
                        else if (Convert.ToInt32(dt_zlsj_temp.Rows[0]["STATUS"]) < 4 && heat_id != "")//正常吹氧结束
                        {
                            WritelogAuto(BSNo, heat_id + "吹氧结束正常开始");

                            pono = dt_zlsj_temp.Rows[0]["pono"].ToString();
                            plan_no = dt_zlsj_temp.Rows[0]["SM_PLAN_NO"].ToString();
                            st_no = dt_zlsj_temp.Rows[0]["st_no"].ToString();

                            DBconn.writeNew("insert into yzzt(sm_plan_no, proc_no, station_id, station_no, heat_no, pono, st_no, run_signal, run_time)values('" + plan_no + "'," + heat_id + ",'B','" + BSNo + "'," + heat_id + ",'" + pono + "','" + st_no + "','3" + BSNo + "4','" + DateTime.Now.ToString("yyyyMMddHHmmss") + "')");
                            WritelogAuto(BSNo, heat_id + "插入运转状态3" + BSNo + "4成功,计划号=" + plan_no + ",制造命令号=" + pono);

                            DBconn.writeNew("update zlsj_temp set STATUS ='4' where by_station_no ='" + BSNo + "'");
                            WritelogAuto(BSNo, heat_id + "更新转炉实际标志位4成功");

                            ChangeTextBox(BSNo, "吹氧结束", heat_id);

                            DBconn.writeNew("update by_bofstatus set bof" + BSNo + "_o2end=0");
                            DBconn.writeNew("update by_shoudong set sd_bof" + BSNo + "o2end=0");
                            DBconn.writeNew("update BY_BOFSTATUS set bof" + BSNo + "_status_now=4");
                            run_once = 0;
                            WritelogAuto(BSNo, heat_id + "吹氧成功结束", 1);


                        }
                        else
                        {
                            DBconn.writeNew("update by_bofstatus set bof" + BSNo + "_o2end=0");
                            DBconn.writeNew("update by_shoudong set sd_bof" + BSNo + "o2end=0");
                            WritelogAuto(BSNo, heat_id + "吹氧结束开始失败,可能没有炉号", 2);
                            run_once = 0;

                        }
                    }
                    catch (Exception err)
                    {
                        WritelogAuto(BSNo, "吹氧结束失败" + err.Message, 3);
                        DBconn.writeNew("update by_bofstatus set bof" + BSNo + "_o2end=0");
                        DBconn.writeNew("update by_shoudong set sd_bof" + BSNo + "o2end=0");
                        run_once = 0;
                    }
                    #endregion
                }
                else if (vbof_o2start_again == "1" || vsd_bof_o2start_again == 1)//------------------------------------------------------------补吹开始
                {
                    #region 补吹开始
                    try
                    {
                        dt_by_bofstatus = DBconn.readNew("select * from BY_BOFSTATUS t");
                        if (dt_by_bofstatus.Rows[0]["bof" + BSNo + "_status_now"].ToString() != "6")
                        {
                            if (heat_id == "" && run_once == 0)//检测炉次是否开始,如果炉次没开始 开始炉次
                            {
                                WritelogAuto(BSNo, heat_id + "补吹开始检测到炉次未开始，先开始炉次", 2);
                                DBconn.writeNew("update BY_BOFSTATUS set bof" + BSNo + "_start=1 ");//模拟炉次开始
                                if (BSNo == "5")
                                    sd_bof5o2start_again = 1;
                                if (BSNo == "6")
                                    sd_bof6o2start_again = 1;
                                if (BSNo == "7")
                                    sd_bof7o2start_again = 1;
                                run_once++;
                                return;
                            }
                            else if (dt_by_bofstatus.Rows[0]["bof" + BSNo + "_status_now"].ToString() == "1" && run_once == 0)//炉次开始了 但是吹氧开始没有来
                            {
                                WritelogAuto(BSNo, heat_id + "补吹开始检测到吹氧未开始，先开始吹氧", 2);
                                DBconn.writeNew("update BY_BOFSTATUS set bof" + BSNo + "_o2start=1 ");//模拟吹氧开始
                                run_once++;
                                return;
                            }
                            else if (dt_by_bofstatus.Rows[0]["bof" + BSNo + "_status_now"].ToString() == "3" && run_once == 0)//吹氧开始来了 但是吹氧结束没来
                            {
                                WritelogAuto(BSNo, heat_id + "补吹开始检测到吹氧未结束，先结束吹氧", 2);
                                DBconn.writeNew("update BY_BOFSTATUS set bof" + BSNo + "_o2end=1 ");//模拟吹氧结束
                                run_once++;
                                return;
                            }
                            DBconn.writeNew("update by_bofstatus set bof" + BSNo + "_o2start_again=0");
                            run_once = 0;
                            if (BSNo == "5")
                                sd_bof5o2start_again = 0;
                            if (BSNo == "6")
                                sd_bof6o2start_again = 0;
                            if (BSNo == "7")
                                sd_bof7o2start_again = 0;
                            WritelogAuto(BSNo, heat_id + "补吹开始");
                            //run_once = 0;
                        }
                        else
                        {
                            WritelogAuto(BSNo, heat_id + "出钢结束补吹开始");
                            DBconn.writeNew("update by_bofstatus set bof" + BSNo + "_o2start_again=0");
                        }
                    }
                    catch (Exception err)
                    {
                        DBconn.writeNew("update by_bofstatus set bof" + BSNo + "_o2start_again=0");
                        WritelogAuto(BSNo, heat_id + "补吹开始执行错误" + err.Message);
                    }




                    #endregion
                }
                else if (vbof_o2end_again == "1" || vsd_bof_o2end_again == 1)//--------------------------------------------------------------补吹结束
                {
                    #region 补吹结束
                    try
                    {

                        dt_by_bofstatus = DBconn.readNew("select * from BY_BOFSTATUS t");
                        if (dt_by_bofstatus.Rows[0]["bof" + BSNo + "_status_now"].ToString() != "6")
                        {
                            if (heat_id == "" && run_once == 0)//检测炉次是否开始,如果炉次没开始 开始炉次
                            {
                                WritelogAuto(BSNo, heat_id + "补吹结束检测到炉次未开始，先开始炉次", 2);
                                DBconn.writeNew("update BY_BOFSTATUS set bof" + BSNo + "_start=1 ");//模拟炉次开始
                                if (BSNo == "5")
                                    sd_bof5o2end_again = 1;
                                if (BSNo == "6")
                                    sd_bof6o2end_again = 1;
                                if (BSNo == "7")
                                    sd_bof7o2end_again = 1;
                                run_once++;
                                return;
                            }
                            else if (dt_by_bofstatus.Rows[0]["bof" + BSNo + "_status_now"].ToString() == "1" && run_once == 0)//炉次开始了 但是吹氧开始没有来
                            {
                                WritelogAuto(BSNo, heat_id + "补吹结束检测到吹氧未开始，先开始吹氧", 2);
                                DBconn.writeNew("update BY_BOFSTATUS set bof" + BSNo + "_o2start=1 ");//模拟吹氧开始
                                run_once++;
                                return;
                            }
                            else if (dt_by_bofstatus.Rows[0]["bof" + BSNo + "_status_now"].ToString() == "3" && run_once == 0)//吹氧开始来了 但是吹氧结束没来
                            {
                                WritelogAuto(BSNo, heat_id + "补吹结束检测到吹氧未结束，先结束吹氧", 2);
                                DBconn.writeNew("update BY_BOFSTATUS set bof" + BSNo + "_o2end=1 ");//模拟吹氧结束
                                run_once++;
                                return;
                            }
                            DBconn.writeNew("update by_bofstatus set bof" + BSNo + "_o2end_again=0");
                            run_once = 0;
                            if (BSNo == "5")
                                sd_bof5o2end_again = 0;
                            if (BSNo == "6")
                                sd_bof6o2end_again = 0;
                            if (BSNo == "7")
                                sd_bof7o2end_again = 0;
                            WritelogAuto(BSNo, heat_id + "补吹结束");
                        }
                        else
                        {
                            WritelogAuto(BSNo, heat_id + "出钢结束补吹结束");
                            DBconn.writeNew("update by_bofstatus set bof" + BSNo + "_o2end_again=0");
                        }
                    }
                    catch (Exception err)
                    {
                        DBconn.writeNew("update by_bofstatus set bof" + BSNo + "_o2end_again=0");
                        WritelogAuto(BSNo, heat_id + "补吹结束执行错误" + err.Message);
                    }



                    #endregion
                }

                else if (vbofcgstart_l1 == "1" || vsd_bofcgstart == "1")//-------------------------------------------------------出钢开始
                {
                    #region 出钢开始
                    try
                    {
                        WritelogAuto(BSNo, heat_id + "------出钢开始------");
                        dt = DBconn.readNew("select * from zlsj_temp where by_station_no=" + BSNo);
                        dt_by_bofstatus = DBconn.readNew("select * from BY_BOFSTATUS t");
                        if (heat_id == "" && run_once == 0)//检测炉次是否开始,如果炉次没开始 开始炉次
                        {
                            WritelogAuto(BSNo, heat_id + "出钢开始检测到炉次未开始，先开始炉次", 2);
                            DBconn.writeNew("update BY_BOFSTATUS set bof" + BSNo + "_start=1 ");//模拟炉次开始
                            DBconn.writeNew("update by_shoudong set sd_bof" + BSNo + "cgstart=1");//手动出钢1
                            run_once++;
                            return;
                        }
                        else if (dt_by_bofstatus.Rows[0]["bof" + BSNo + "_status_now"].ToString() == "1" && run_once == 0)//炉次开始了 但是吹氧开始没有来
                        {
                            WritelogAuto(BSNo, heat_id + "出钢开始检测到吹氧未开始，先开始吹氧", 2);
                            DBconn.writeNew("update BY_BOFSTATUS set bof" + BSNo + "_o2start=1 ");//模拟吹氧开始
                            run_once++;
                            return;
                        }
                        else if (dt_by_bofstatus.Rows[0]["bof" + BSNo + "_status_now"].ToString() == "3" && run_once == 0)//吹氧开始来了 但是吹氧结束没来
                        {
                            WritelogAuto(BSNo, heat_id + "出钢开始检测到吹氧未结束，先结束吹氧", 2);
                            DBconn.writeNew("update BY_BOFSTATUS set bof" + BSNo + "_o2end=1 ");//模拟吹氧结束
                            run_once++;
                            return;
                        }
                        else if (Convert.ToInt32(dt.Rows[0]["STATUS"]) < 5 && heat_id != "")//正常出钢开始
                        {
                            WritelogAuto(BSNo, heat_id + "正常出钢开始");

                            pono = dt.Rows[0]["pono"].ToString();
                            plan_no = dt.Rows[0]["SM_PLAN_NO"].ToString();
                            st_no = dt.Rows[0]["st_no"].ToString();
                            DBconn.writeNew("insert into yzzt(sm_plan_no, proc_no, station_id, station_no, heat_no, pono, st_no, run_signal, run_time)values('" + plan_no + "'," + heat_id + ",'B','" + BSNo + "'," + heat_id + ",'" + pono + "','" + st_no + "','3" + BSNo + "5','" + DateTime.Now.ToString("yyyyMMddHHmmss") + "')");
                            WritelogAuto(BSNo, heat_id + "插入运转状态3" + BSNo + "5成功;计划号=" + plan_no + ",制造命令号=" + pono);
                            o2jisuan(BSNo, heat_id);//吹氧数据计算
                            try
                            {
                                DBconn.writeOld("call NEW_ZLJLSJ" + BSNo + "_SEND_PRO('" + heat_id + "')");//传送转炉加料实绩
                                WritelogAuto(BSNo, heat_id + "传送转炉加料实绩成功");
                                aux_alloy_fenpijiliang(heat_id, BSNo);
                            }
                            catch (Exception err)
                            {
                                WritelogAuto(BSNo, heat_id + "传送转炉加料实绩失败" + err.Message, 2);

                            }


                            

                            DBconn.writeNew("update zlsj_temp set STATUS ='5' where by_station_no ='" + BSNo + "'");
                            WritelogAuto(BSNo, heat_id + "更新转炉实际标志位成功");

                            ChangeTextBox(BSNo, "出刚开始", heat_id);

                            DBconn.writeNew("update BY_BOFSTATUS set bof" + BSNo + "_status_now=5");
                            DBconn.writeNew("update by_bofstatus set bof" + BSNo + "_cgstart=0");
                            DBconn.writeNew("update by_shoudong set sd_bof" + BSNo + "cgstart=0");
                            run_once = 0;
                            WritelogAuto(BSNo, heat_id + "出钢开始", 1);

                        }
                        else
                        {
                            DBconn.writeNew("update by_bofstatus set bof" + BSNo + "_cgstart=0");
                            DBconn.writeNew("update by_shoudong set sd_bof" + BSNo + "cgstart=0");
                            WritelogAuto(BSNo, heat_id + "出钢开始失败,可能没有炉号", 2);
                            run_once = 0;

                        }

                    }
                    catch (Exception err)
                    {
                        WritelogAuto(BSNo, "出钢开始失败" + err.Message, 3);
                        DBconn.writeNew("update by_bofstatus set bof" + BSNo + "_cgstart=0");
                        DBconn.writeNew("update by_shoudong set sd_bof" + BSNo + "cgstart=0");
                        run_once = 0;
                    }
                    #endregion

                }
                else if (vbofcgend_l1 == "1" || vsd_bofcgend == "1")//-------------------------------------------------------出钢结束

                {
                    #region 出钢结束
                    try
                    {
                        WritelogAuto(BSNo, heat_id + "------出钢结束开始------");
                        if (DBconn.readNew("select heat_no from zlsj_temp where by_station_no=" + BSNo).Rows[0][0].ToString() != "")
                        {

                            dt = DBconn.readNew("select * from zlsj_temp where by_station_no=" + BSNo);
                            if (Convert.ToInt32(dt.Rows[0]["STATUS"]) < 6)//正常炉次开始
                            {
                                WritelogAuto(BSNo, heat_id + "出钢结束正常开始");

                                pono = dt.Rows[0]["pono"].ToString();
                                plan_no = dt.Rows[0]["SM_PLAN_NO"].ToString();
                                st_no = dt.Rows[0]["st_no"].ToString();

                                DBconn.writeNew("insert into yzzt(sm_plan_no, proc_no, station_id, station_no, heat_no, pono, st_no, run_signal, run_time)values('" + plan_no + "'," + heat_id + ",'B','" + BSNo + "'," + heat_id + ",'" + pono + "','" + st_no + "','3" + BSNo + "6','" + DateTime.Now.ToString("yyyyMMddHHmmss") + "')");
                                WritelogAuto(BSNo, heat_id + "插入运转状态3" + BSNo + "6成功;计划号=" + plan_no + ",制造命令号=" + pono);
                                o2jisuan(BSNo, heat_id);//吹氧数据计算
                                Thread.Sleep(1100);

                                DBconn.writeNew("update zlsj_temp set tap_end_time='" + DateTime.Now.ToString("yyyyMMddHHmmss") + "',STATUS ='6',status_new='1' where by_station_no ='" + BSNo + "'");
                                WritelogAuto(BSNo, heat_id + "更新转炉实际成功");
                                
                                if (BSNo == "5")
                                    endheat_no5 = heat_id;
                                if (BSNo == "6")
                                    endheat_no6 = heat_id;
                                if (BSNo == "7")
                                    endheat_no7 = heat_id;
                                try
                                {
                                    int nextheatid = heatid_add1(heat_id);
                                    DBconn.writeOld("UPDATE BOF" + BSNo + "CURRENTID SET HeatID='" + nextheatid + "'");
                                    WritelogAuto(BSNo, heat_id + "更新BOF" + BSNo + "CURRENTID_HeatID=" + nextheatid + "成功");
                                }
                                catch (Exception err)
                                {
                                    WritelogAuto(BSNo, heat_id + "更新BOF" + BSNo + "CURRENTID_HeatID失败" + err.Message);
                                }
                                zljlsj_time(heat_id, BSNo);//合金时间计算
                                DBconn.writeNew("update zlsj_temp set STATUS ='0',HEAT_NO ='' where by_station_no ='" + BSNo + "'");
                                DBconn.writeNew("update by_bofstatus set bof" + BSNo + "_start=0,bof" + BSNo + "_o2start=0,bof" + BSNo + "_o2end=0,bof" + BSNo + "_cgstart=0,bof" + BSNo + "_cgend=0");
                                DBconn.writeNew("update by_shoudong set sd_bof" + BSNo + "cgend=0");
                                DBconn.writeNew("update BY_BOFSTATUS set bof" + BSNo + "_status_now=6");
                                ChangeTextBox(BSNo, "出钢结束", heat_id);

                                WritelogAuto(BSNo, heat_id + "出钢成功结束", 1);

                            }
                            else
                            {
                                DBconn.writeNew("update by_bofstatus set bof" + BSNo + "_start=0,bof" + BSNo + "_o2start=0,bof" + BSNo + "_o2end=0,bof" + BSNo + "_cgstart=0,bof" + BSNo + "_cgend=0");
                                DBconn.writeNew("update by_shoudong set sd_bof" + BSNo + "cgend=0");
                                WritelogAuto(BSNo, heat_id + "出钢结束失败，状态位不小于6", 2);

                            }
                        }
                        else
                        {
                            DBconn.writeNew("update by_bofstatus set bof" + BSNo + "_cgend=0");
                            DBconn.writeNew("update by_shoudong set sd_bof" + BSNo + "cgend=0");
                            WritelogAuto(BSNo, "出钢结束失败，没有炉号", 2);
                        }



                    }
                    catch (Exception err)
                    {
                        WritelogAuto(BSNo, "出钢结束失败" + err.Message, 3);

                        DBconn.writeNew("update by_bofstatus set bof" + BSNo + "_cgend=0");
                        DBconn.writeNew("update by_shoudong set sd_bof" + BSNo + "cgend=0");
                    }
                    #endregion

                }
                else if (vbofend_l1 == "1")//-------------------------------------------------------炉次结束
                {
                    #region 炉次结束动作
                    try
                    {



                        if (heat_id != "")
                        {
                            WritelogAuto(BSNo, heat_id + "炉次结束信号来了，有炉号，可能上一炉未结束或者炉次开始和结束信号一起来");
                            DBconn.writeNew("update by_bofstatus set bof" + BSNo + "_end=0");
                            if (Convert.ToInt32(dt_zlsj_temp.Rows[0]["STATUS"]) >= 5 && heat_id != "")          //强制结束上一炉
                            {
                                DBconn.writeNew("update zlsj_temp t set t.end_of_heat=to_char(sysdate,'yyyymmddhh24miss') where t.by_station_no='"+BSNo+"' ");
                                WritelogAuto(BSNo, heat_id + "炉次结束检测到出钢结束没来，模拟出钢结束", 2);
                                DBconn.writeNew("update BY_BOFSTATUS set bof" + BSNo + "_cgend=1 ");//模拟出钢结束
                                run_once++;

                            }
                            return;

                            //DBconn.writeNew("update zlsj_temp set tap_end_time='" + DateTime.Now.ToString("yyyyMMddHHmmss") + "',STATUS ='6',status_new='1' where by_station_no ='5'");
                        }
                        else if (endheat_no5 != "" && BSNo == "5")
                        {
                            WritelogAuto(BSNo, endheat_no5 + "------炉次结束------");
                            WritelogAuto(BSNo, endheat_no5 + "开始结束炉次写历史炉次标志位");

                            DBconn.writeNew("update zlsj_history set status=9 where heat_no='" + endheat_no5 + "'");
                            WritelogAuto(BSNo, endheat_no5 + "炉次结束", 1);

                            WritelogAuto(BSNo, endheat_no5 + "炉次结束_历史炉次标志位置8");

                        }
                        else if (endheat_no6 != "" && BSNo == "6")
                        {
                            WritelogAuto(BSNo, endheat_no6 + "------炉次结束------");
                            WritelogAuto(BSNo, endheat_no6 + "开始结束炉次写历史炉次标志位");

                            DBconn.writeNew("update zlsj_history set status=9 where heat_no='" + endheat_no6 + "'");
                            WritelogAuto(BSNo, endheat_no6 + "炉次结束", 1);

                            WritelogAuto(BSNo, endheat_no6 + "炉次结束_历史炉次标志位置8");

                        }
                        else if (endheat_no7 != "" && BSNo == "7")
                        {
                            WritelogAuto(BSNo, endheat_no7 + "------炉次结束------");
                            WritelogAuto(BSNo, endheat_no7 + "开始结束炉次写历史炉次标志位");

                            DBconn.writeNew("update zlsj_history set status=9 where heat_no='" + endheat_no7 + "'");
                            WritelogAuto(BSNo, endheat_no7 + "炉次结束", 1);

                            WritelogAuto(BSNo, endheat_no7 + "炉次结束_历史炉次标志位置8");

                        }
                        DBconn.writeNew("update BY_BOFSTATUS set bof" + BSNo + "_status_now=7");
                        DBconn.writeNew("update by_bofstatus set bof" + BSNo + "_end=0");


                    }
                    catch (Exception err)
                    {
                        DBconn.writeNew("update by_bofstatus set bof" + BSNo + "_end=0");
                        WritelogAuto(BSNo, "炉次结束失败" + err.Message);

                    }
                    #endregion
                }

                stop_heat_time(BSNo);//实时停炉判断

                #region LED灯
                if (BSNo == "5")
                {
                    bof5ledstatus += 1;
                    if (bof5ledstatus > 3)
                        bof5ledstatus = 1;
                    
                }
                if (BSNo == "6")
                {
                    bof6ledstatus += 1;
                    if (bof6ledstatus > 3)
                        bof6ledstatus = 1;
                    
                }
                if (BSNo == "7")
                {
                    bof7ledstatus += 1;
                    if (bof7ledstatus > 3)
                        bof7ledstatus = 1;
                    
                }
                #endregion

                
            }catch(Exception err)
            {
                TB_all.AppendText(err.Message);
            }

        }

        /// <summary>
        /// 读取数据库一级状态
        /// </summary>
        private void read_by_bofstatus()
        {
            try
            {
                dt_by_shoudong = DBconn.readNew("select * from by_shoudong");
                dt_bofstatus = DBconn.readNew("select * from by_bofstatus");
                bof5start_l1 = dt_bofstatus.Rows[0]["bof5_start"].ToString();
                bof6start_l1 = dt_bofstatus.Rows[0]["bof6_start"].ToString();
                bof7start_l1 = dt_bofstatus.Rows[0]["bof7_start"].ToString();
                bof5o2start_l1 = dt_bofstatus.Rows[0]["bof5_o2start"].ToString();
                bof6o2start_l1 = dt_bofstatus.Rows[0]["bof6_o2start"].ToString();
                bof7o2start_l1 = dt_bofstatus.Rows[0]["bof7_o2start"].ToString();
                bof5o2end_l1 = dt_bofstatus.Rows[0]["bof5_o2end"].ToString();
                bof6o2end_l1 = dt_bofstatus.Rows[0]["bof6_o2end"].ToString();
                bof7o2end_l1 = dt_bofstatus.Rows[0]["bof7_o2end"].ToString();
                bof5cgstart_l1 = dt_bofstatus.Rows[0]["bof5_cgstart"].ToString();
                bof6cgstart_l1 = dt_bofstatus.Rows[0]["bof6_cgstart"].ToString();
                bof7cgstart_l1 = dt_bofstatus.Rows[0]["bof7_cgstart"].ToString();
                bof5cgend_l1 = dt_bofstatus.Rows[0]["bof5_cgend"].ToString();
                bof6cgend_l1 = dt_bofstatus.Rows[0]["bof6_cgend"].ToString();
                bof7cgend_l1 = dt_bofstatus.Rows[0]["bof7_cgend"].ToString();
                bof5end_l1 = dt_bofstatus.Rows[0]["bof5_end"].ToString();
                bof6end_l1 = dt_bofstatus.Rows[0]["bof6_end"].ToString();
                bof7end_l1 = dt_bofstatus.Rows[0]["bof7_end"].ToString();
                bof5_o2start_again_L1 = dt_bofstatus.Rows[0]["bof5_o2start_again"].ToString();
                bof6_o2start_again_L1 = dt_bofstatus.Rows[0]["bof6_o2start_again"].ToString();
                bof7_o2start_again_L1 = dt_bofstatus.Rows[0]["bof7_o2start_again"].ToString();
                bof5_o2end_again_L1 = dt_bofstatus.Rows[0]["bof5_o2end_again"].ToString();
                bof6_o2end_again_L1 = dt_bofstatus.Rows[0]["bof6_o2end_again"].ToString();
                bof7_o2end_again_L1 = dt_bofstatus.Rows[0]["bof7_o2end_again"].ToString();
                bof5_trace_enable_l1 = dt_bofstatus.Rows[0]["BOF5_TRACE_ENABLE"].ToString();
                bof6_trace_enable_l1 = dt_bofstatus.Rows[0]["BOF6_TRACE_ENABLE"].ToString();
                bof7_trace_enable_l1 = dt_bofstatus.Rows[0]["BOF7_TRACE_ENABLE"].ToString();
                ///////////////////
                bof5thisheatid = Convert.ToInt32(dt_bofstatus.Rows[0]["bof5_heatid_now"].ToString().Substring(2, 6));
                bof5newheatid = Convert.ToInt32(dt_bofstatus.Rows[0]["bof5_heatid_next"].ToString().Substring(2, 6));
                bof6thisheatid = Convert.ToInt32(dt_bofstatus.Rows[0]["bof6_heatid_now"].ToString().Substring(2, 6));
                bof6newheatid = Convert.ToInt32(dt_bofstatus.Rows[0]["bof6_heatid_next"].ToString().Substring(2, 6));
                bof7thisheatid = Convert.ToInt32(dt_bofstatus.Rows[0]["bof7_heatid_now"].ToString().Substring(2, 6));
                bof7newheatid = Convert.ToInt32(dt_bofstatus.Rows[0]["bof7_heatid_next"].ToString().Substring(2, 6));
                ///////////////////
                sd_bof5start = dt_by_shoudong.Rows[0]["sd_bof5start"].ToString();
                sd_bof5o2start = dt_by_shoudong.Rows[0]["sd_bof5o2start"].ToString();
                sd_bof5o2end = dt_by_shoudong.Rows[0]["sd_bof5o2end"].ToString();
                sd_bof5cgstart = dt_by_shoudong.Rows[0]["sd_bof5cgstart"].ToString();
                sd_bof5cgend = dt_by_shoudong.Rows[0]["sd_bof5cgend"].ToString();
                sd_bof5end = dt_by_shoudong.Rows[0]["sd_bof5end"].ToString();
                sd_bof6start = dt_by_shoudong.Rows[0]["sd_bof6start"].ToString();
                sd_bof6o2start = dt_by_shoudong.Rows[0]["sd_bof6o2start"].ToString();
                sd_bof6o2end = dt_by_shoudong.Rows[0]["sd_bof6o2end"].ToString();
                sd_bof6cgstart = dt_by_shoudong.Rows[0]["sd_bof6cgstart"].ToString();
                sd_bof6cgend = dt_by_shoudong.Rows[0]["sd_bof6cgend"].ToString();
                sd_bof6end = dt_by_shoudong.Rows[0]["sd_bof6end"].ToString();
                sd_bof7start = dt_by_shoudong.Rows[0]["sd_bof7start"].ToString();
                sd_bof7o2start = dt_by_shoudong.Rows[0]["sd_bof7o2start"].ToString();
                sd_bof7o2end = dt_by_shoudong.Rows[0]["sd_bof7o2end"].ToString();
                sd_bof7cgstart = dt_by_shoudong.Rows[0]["sd_bof7cgstart"].ToString();
                sd_bof7cgend = dt_by_shoudong.Rows[0]["sd_bof7cgend"].ToString();
                sd_bof7end = dt_by_shoudong.Rows[0]["sd_bof7end"].ToString();
                tb_new7status.Text = DBconn.readOld("select bofstatus from bof7status").Rows[0][0].ToString();
                TB_bof7heatid.Text = DBconn.readOld("select heatid from bof7status").Rows[0][0].ToString();
                tb_new6status.Text = DBconn.readOld("select bofstatus from bof6status").Rows[0][0].ToString();
                TB_bof6heatid.Text = DBconn.readOld("select heatid from bof6status").Rows[0][0].ToString();
                tb_new5status.Text = DBconn.readOld("select bofstatus from bof5status").Rows[0][0].ToString();
                TB_bof5heatid.Text = DBconn.readOld("select heatid from bof5status").Rows[0][0].ToString();
            }
            catch (Exception err)
            {

            }
        }
        /// <summary>
        /// 吹氧数据计算
        /// </summary>
        /// <param name="BSNo">炉座号</param>
        /// <param name="heat_id">炉号</param>
        private void o2jisuan(string BSNo, string heat_id)
        {
            try//更新吹氧数据
            {


                int i = 0;
                //int[] CT = null;
                //DataTable dt_cuiyang = DBconn.readOld("select round((t.blowendtime-t.blowbegintime)*24*60*60),t.blowbegintime,t.blowendtime,round(t.blowo2amount),t.blowo2times from BOF" + BSNo + "BLOWO2DATA t where t.heatid='" + heat_id + "' order by t.blowo2times");
                DataTable dt_cuiyang = DBconn.readOld("select round((t1.blowendtime - t1.blowbegintime) * 24 * 60 * 60),t1.blowbegintime,t1.blowendtime,round(t1.blowo2amount) from BOF" + BSNo + "BLOWO2DATA t1 inner join (select max(t.blowbegintime) as maxo2begintime from bof" + BSNo + "blowo2data t where t.heatid = '" + heat_id + "' group by t.blowo2times) a on t1.blowbegintime = a.maxo2begintime order by t1.blowo2times");
                if (dt_cuiyang.Rows.Count > 0)
                {

                    if (Convert.ToInt32(dt_cuiyang.Rows[0][0].ToString()) >= 600)/////////////////////////////////////////////////////////////////第一次吹氧时间大于600秒
                    {
                        DBconn.writeNew("update zlsj_temp set reblow_num='" + (dt_cuiyang.Rows.Count-1) + "' where by_station_no=" + BSNo);
                        string starttime;//开始时间
                        string endtime;//结束时间
                        int t1;//单个吹氧时间
                        int t_all = 0;//总吹氧时间
                        int EXT_ITEM3 = 0;//补吹次数
                        int EXT_ITEM4_temp = 0;//第一次吹氧量
                        int EXT_ITEM4 = 0;//补吹氧量
                        int REBLOW_DURATION = 0;//补吹持续时间
                        foreach (DataRow dr in dt_cuiyang.Rows)
                        {
                            i++;
                            if (i <= 5)
                            {
                                starttime = Convert.ToDateTime(dr[1]).ToString("yyyyMMddHHmmss");
                                endtime = Convert.ToDateTime(dr[2]).ToString("yyyyMMddHHmmss");
                                t1 = Convert.ToInt32(dr[0].ToString());
                                t_all = t_all + t1;
                                DBconn.writeNew("update zlsj_temp set blow_time" + i + "='" + t1 + "',blow_start_time" + i + "='" + starttime + "',blow_end_time" + i + "='" + endtime + "',o2_sum_comsume='" + dr[3].ToString() + "',blow_time='" + t_all + "' where by_station_no=" + BSNo);
                                if (i < 2)
                                {
                                    EXT_ITEM4_temp = Convert.ToInt32(dr[3].ToString());
                                }
                                if (i > 1)
                                {
                                    EXT_ITEM4 = 0;
                                    REBLOW_DURATION = REBLOW_DURATION + t1;
                                    EXT_ITEM3++;
                                    EXT_ITEM4 = Convert.ToInt32(dr[3].ToString()) - EXT_ITEM4_temp;
                                }
                            }
                        }
                        DBconn.writeNew("update zlsj_temp set EXT_ITEM3='" + EXT_ITEM3 + "',EXT_ITEM4='" + EXT_ITEM4 + "',REBLOW_DURATION='" + REBLOW_DURATION + "'  where by_station_no=" + BSNo);
                    }
                    else if (dt_cuiyang.Rows.Count > 1&&Convert.ToInt32(dt_cuiyang.Rows[0][0].ToString()) + Convert.ToInt32(dt_cuiyang.Rows[1][0].ToString()) >= 600)//////////////////////////////////////////////////////////第一次加第二次大于600秒
                    {
                            DBconn.writeNew("update zlsj_temp set reblow_num='" + (dt_cuiyang.Rows.Count - 2) + "' where by_station_no=" + BSNo);
                            string starttime;//开始时间
                            string endtime;//结束时间
                            int t1;//单个吹氧时间
                            int t_all = 0;//总吹氧时间
                            int EXT_ITEM3 = 0;//补吹次数
                            int EXT_ITEM4_temp = 0;//第一次吹氧量
                            int EXT_ITEM4 = 0;//补吹氧量
                            int REBLOW_DURATION = 0;//补吹持续时间
                            foreach (DataRow dr in dt_cuiyang.Rows)
                            {
                                i++;
                                if (i == 2)
                                {
                                    starttime = Convert.ToDateTime(dt_cuiyang.Rows[0][1]).ToString("yyyyMMddHHmmss");
                                    endtime = Convert.ToDateTime(dr[2]).ToString("yyyyMMddHHmmss");
                                    t1 = Convert.ToInt32(dt_cuiyang.Rows[0][0].ToString()) + Convert.ToInt32(dt_cuiyang.Rows[1][0].ToString());
                                    t_all = t_all + t1;
                                    EXT_ITEM4_temp = Convert.ToInt32(dr[3].ToString());
                                    DBconn.writeNew("update zlsj_temp set blow_time" + (i - 1) + "='" + t1 + "',blow_start_time" + (i - 1) + "='" + starttime + "',blow_end_time" + (i - 1) + "='" + endtime + "',o2_sum_comsume='" + dr[3].ToString() + "',blow_time='" + t_all + "' where by_station_no=" + BSNo);
                                }
                                else if (i > 2 && i <= 6)
                                {
                                    starttime = Convert.ToDateTime(dt_cuiyang.Rows[(i-1)][1]).ToString("yyyyMMddHHmmss");
                                    endtime = Convert.ToDateTime(dr[2]).ToString("yyyyMMddHHmmss");
                                    t1 = Convert.ToInt32(dr[0].ToString());
                                    t_all = t_all + t1;
                                    EXT_ITEM4 = 0;
                                    REBLOW_DURATION = REBLOW_DURATION + Convert.ToInt32(dr[0].ToString());
                                    EXT_ITEM3++;
                                    EXT_ITEM4 = Convert.ToInt32(dr[3].ToString()) - EXT_ITEM4_temp;
                                    DBconn.writeNew("update zlsj_temp set blow_time" + (i - 1) + "='" + t1 + "',blow_start_time" + (i - 1) + "='" + starttime + "',blow_end_time" + (i - 1) + "='" + endtime + "',o2_sum_comsume='" + dr[3].ToString() + "',blow_time='" + t_all + "' where by_station_no=" + BSNo);
                                }
                            }
                            DBconn.writeNew("update zlsj_temp set EXT_ITEM3='" + EXT_ITEM3 + "',EXT_ITEM4='" + EXT_ITEM4 + "',REBLOW_DURATION='" + REBLOW_DURATION + "'  where by_station_no=" + BSNo);
                        }
                    
                    else if (dt_cuiyang.Rows.Count > 2&&Convert.ToInt32(dt_cuiyang.Rows[0][0].ToString()) + Convert.ToInt32(dt_cuiyang.Rows[1][0].ToString()) + Convert.ToInt32(dt_cuiyang.Rows[2][0].ToString()) >= 600)//////////////////////////////////////////////////////////第一次加第二加第三次次大于600秒
                    {
                            DBconn.writeNew("update zlsj_temp set reblow_num='" + (dt_cuiyang.Rows.Count - 3) + "' where by_station_no=" + BSNo);
                            string starttime;//开始时间
                            string endtime;//结束时间
                            int t1;//单个吹氧时间
                            int t_all = 0;//总吹氧时间
                            int EXT_ITEM3 = 0;//补吹次数
                            int EXT_ITEM4_temp = 0;//第一次吹氧量
                            int EXT_ITEM4 = 0;//补吹氧量
                            int REBLOW_DURATION = 0;//补吹持续时间
                            foreach (DataRow dr in dt_cuiyang.Rows)
                            {
                                i++;
                                if (i == 3)
                                {
                                    starttime = Convert.ToDateTime(dt_cuiyang.Rows[0][1]).ToString("yyyyMMddHHmmss");
                                    endtime = Convert.ToDateTime(dr[2]).ToString("yyyyMMddHHmmss");
                                    t1 = Convert.ToInt32(dt_cuiyang.Rows[0][0].ToString()) + Convert.ToInt32(dt_cuiyang.Rows[1][0].ToString()) + Convert.ToInt32(dt_cuiyang.Rows[2][0].ToString());
                                    t_all = t_all + t1;
                                    EXT_ITEM4_temp = Convert.ToInt32(dr[3].ToString());
                                    DBconn.writeNew("update zlsj_temp set blow_time" + (i - 2) + "='" + t1 + "',blow_start_time" + (i - 2) + "='" + starttime + "',blow_end_time" + (i - 2) + "='" + endtime + "',o2_sum_comsume='" + dr[3].ToString() + "',blow_time='" + t_all + "' where by_station_no=" + BSNo);
                                }
                                else if (i > 3 && i <= 7)
                                {
                                    starttime = Convert.ToDateTime(dt_cuiyang.Rows[(i-2)][1]).ToString("yyyyMMddHHmmss");
                                    endtime = Convert.ToDateTime(dr[2]).ToString("yyyyMMddHHmmss");
                                    t1 = Convert.ToInt32(dr[0].ToString());
                                    t_all = t_all + t1;
                                    EXT_ITEM4 = 0;
                                    REBLOW_DURATION = REBLOW_DURATION + Convert.ToInt32(dr[0].ToString());
                                    EXT_ITEM3++;
                                    EXT_ITEM4 = Convert.ToInt32(dr[3].ToString()) - EXT_ITEM4_temp;
                                    DBconn.writeNew("update zlsj_temp set blow_time" + (i - 2) + "='" + t1 + "',blow_start_time" + (i - 2) + "='" + starttime + "',blow_end_time" + (i - 2) + "='" + endtime + "',o2_sum_comsume='" + dr[3].ToString() + "',blow_time='" + t_all + "' where by_station_no=" + BSNo);
                                }
                            }
                            DBconn.writeNew("update zlsj_temp set EXT_ITEM3='" + EXT_ITEM3 + "',EXT_ITEM4='" + EXT_ITEM4 + "',REBLOW_DURATION='" + REBLOW_DURATION + "'  where by_station_no=" + BSNo);
                        }
                    
                    else
                    {
                        DBconn.writeNew("update zlsj_temp set reblow_num='" + (dt_cuiyang.Rows.Count-1) + "' where by_station_no=" + BSNo);
                        string starttime;//开始时间
                        string endtime;//结束时间
                        int t1;//单个吹氧时间
                        int t_all = 0;//总吹氧时间
                        int EXT_ITEM3 = 0;//补吹次数
                        int EXT_ITEM4_temp = 0;//第一次吹氧量
                        int EXT_ITEM4 = 0;//补吹氧量
                        int REBLOW_DURATION = 0;//补吹持续时间
                        foreach (DataRow dr in dt_cuiyang.Rows)
                        {
                            i++;
                            if (i <= 5)
                            {
                                starttime = Convert.ToDateTime(dr[1]).ToString("yyyyMMddHHmmss");
                                endtime = Convert.ToDateTime(dr[2]).ToString("yyyyMMddHHmmss");
                                t1 = Convert.ToInt32(dr[0].ToString());
                                t_all = t_all + t1;
                                DBconn.writeNew("update zlsj_temp set blow_time" + i + "='" + t1 + "',blow_start_time" + i + "='" + starttime + "',blow_end_time" + i + "='" + endtime + "',o2_sum_comsume='" + dr[3].ToString() + "',blow_time='" + t_all + "' where by_station_no=" + BSNo);
                                if (i < 2)
                                {
                                    EXT_ITEM4_temp = Convert.ToInt32(dr[3].ToString());
                                }
                                if (i > 1)
                                {
                                    EXT_ITEM4 = 0;
                                    REBLOW_DURATION = REBLOW_DURATION + t1;
                                    EXT_ITEM3++;
                                    EXT_ITEM4 = Convert.ToInt32(dr[3].ToString()) - EXT_ITEM4_temp;
                                }
                            }
                        }
                        DBconn.writeNew("update zlsj_temp set EXT_ITEM3='" + EXT_ITEM3 + "',EXT_ITEM4='" + EXT_ITEM4 + "',REBLOW_DURATION='" + REBLOW_DURATION + "'  where by_station_no=" + BSNo);
                    }
                }
                WritelogAuto(BSNo, heat_id + "吹氧数据计算成功");
            }
            catch (Exception err)
            {
                WritelogAuto(BSNo, heat_id + "吹氧数据更新失败" + err.Message);

            }
        }
        private void o2jisuan_sd(string BSNo, string heat_id)
        {
            try//更新吹氧数据
            {


                int i = 0;
                //int[] CT = null;
                //DataTable dt_cuiyang = DBconn.readOld("select round((t.blowendtime-t.blowbegintime)*24*60*60),t.blowbegintime,t.blowendtime,round(t.blowo2amount),t.blowo2times from BOF" + BSNo + "BLOWO2DATA t where t.heatid='" + heat_id + "' order by t.blowo2times");
                DataTable dt_cuiyang = DBconn.readOld("select round((t1.blowendtime - t1.blowbegintime) * 24 * 60 * 60),t1.blowbegintime,t1.blowendtime,round(t1.blowo2amount) from BOF" + BSNo + "BLOWO2DATA t1 inner join (select max(t.blowbegintime) as maxo2begintime from bof" + BSNo + "blowo2data t where t.heatid = '" + heat_id + "' group by t.blowo2times) a on t1.blowbegintime = a.maxo2begintime order by t1.blowo2times");
                if (dt_cuiyang.Rows.Count > 0)
                {

                    if (Convert.ToInt32(dt_cuiyang.Rows[0][0].ToString()) >= 600)/////////////////////////////////////////////////////////////////第一次吹氧时间大于600秒
                    {
                        DBconn.writeNew("update zlsj_history set reblow_num='" + (dt_cuiyang.Rows.Count - 1) + "' where station_no=" + BSNo);
                        string starttime;//开始时间
                        string endtime;//结束时间
                        int t1;//单个吹氧时间
                        int t_all = 0;//总吹氧时间
                        int EXT_ITEM3 = 0;//补吹次数
                        int EXT_ITEM4_temp = 0;//第一次吹氧量
                        int EXT_ITEM4 = 0;//补吹氧量
                        int REBLOW_DURATION = 0;//补吹持续时间
                        foreach (DataRow dr in dt_cuiyang.Rows)
                        {
                            i++;
                            if (i <= 5)
                            {
                                starttime = Convert.ToDateTime(dr[1]).ToString("yyyyMMddHHmmss");
                                endtime = Convert.ToDateTime(dr[2]).ToString("yyyyMMddHHmmss");
                                t1 = Convert.ToInt32(dr[0].ToString());
                                t_all = t_all + t1;
                                DBconn.writeNew("update zlsj_history set blow_time" + i + "='" + t1 + "',blow_start_time" + i + "='" + starttime + "',blow_end_time" + i + "='" + endtime + "',o2_sum_comsume='" + dr[3].ToString() + "',blow_time='" + t_all + "' where station_no=" + BSNo);
                                if (i < 2)
                                {
                                    EXT_ITEM4_temp = Convert.ToInt32(dr[3].ToString());
                                }
                                if (i > 1)
                                {
                                    EXT_ITEM4 = 0;
                                    REBLOW_DURATION = REBLOW_DURATION + t1;
                                    EXT_ITEM3++;
                                    EXT_ITEM4 = Convert.ToInt32(dr[3].ToString()) - EXT_ITEM4_temp;
                                }
                            }
                        }
                        DBconn.writeNew("update zlsj_history set EXT_ITEM3='" + EXT_ITEM3 + "',EXT_ITEM4='" + EXT_ITEM4 + "',REBLOW_DURATION='" + REBLOW_DURATION + "'  where station_no=" + BSNo);
                    }
                    else if (dt_cuiyang.Rows.Count > 1 && Convert.ToInt32(dt_cuiyang.Rows[0][0].ToString()) + Convert.ToInt32(dt_cuiyang.Rows[1][0].ToString()) >= 600)//////////////////////////////////////////////////////////第一次加第二次大于600秒
                    {
                        DBconn.writeNew("update zlsj_history set reblow_num='" + (dt_cuiyang.Rows.Count - 2) + "' where station_no=" + BSNo);
                        string starttime;//开始时间
                        string endtime;//结束时间
                        int t1;//单个吹氧时间
                        int t_all = 0;//总吹氧时间
                        int EXT_ITEM3 = 0;//补吹次数
                        int EXT_ITEM4_temp = 0;//第一次吹氧量
                        int EXT_ITEM4 = 0;//补吹氧量
                        int REBLOW_DURATION = 0;//补吹持续时间
                        foreach (DataRow dr in dt_cuiyang.Rows)
                        {
                            i++;
                            if (i == 2)
                            {
                                starttime = Convert.ToDateTime(dt_cuiyang.Rows[0][1]).ToString("yyyyMMddHHmmss");
                                endtime = Convert.ToDateTime(dr[2]).ToString("yyyyMMddHHmmss");
                                t1 = Convert.ToInt32(dt_cuiyang.Rows[0][0].ToString()) + Convert.ToInt32(dt_cuiyang.Rows[1][0].ToString());
                                t_all = t_all + t1;
                                EXT_ITEM4_temp = Convert.ToInt32(dr[3].ToString());
                                DBconn.writeNew("update zlsj_history set blow_time" + (i - 1) + "='" + t1 + "',blow_start_time" + (i - 1) + "='" + starttime + "',blow_end_time" + (i - 1) + "='" + endtime + "',o2_sum_comsume='" + dr[3].ToString() + "',blow_time='" + t_all + "' where station_no=" + BSNo);
                            }
                            else if (i > 2 && i <= 6)
                            {
                                starttime = Convert.ToDateTime(dt_cuiyang.Rows[(i - 1)][1]).ToString("yyyyMMddHHmmss");
                                endtime = Convert.ToDateTime(dr[2]).ToString("yyyyMMddHHmmss");
                                t1 = Convert.ToInt32(dr[0].ToString());
                                t_all = t_all + t1;
                                EXT_ITEM4 = 0;
                                REBLOW_DURATION = REBLOW_DURATION + Convert.ToInt32(dr[0].ToString());
                                EXT_ITEM3++;
                                EXT_ITEM4 = Convert.ToInt32(dr[3].ToString()) - EXT_ITEM4_temp;
                                DBconn.writeNew("update zlsj_history set blow_time" + (i - 1) + "='" + t1 + "',blow_start_time" + (i - 1) + "='" + starttime + "',blow_end_time" + (i - 1) + "='" + endtime + "',o2_sum_comsume='" + dr[3].ToString() + "',blow_time='" + t_all + "' where station_no=" + BSNo);
                            }
                        }
                        DBconn.writeNew("update zlsj_history set EXT_ITEM3='" + EXT_ITEM3 + "',EXT_ITEM4='" + EXT_ITEM4 + "',REBLOW_DURATION='" + REBLOW_DURATION + "'  where station_no=" + BSNo);
                    }

                    else if (dt_cuiyang.Rows.Count > 2 && Convert.ToInt32(dt_cuiyang.Rows[0][0].ToString()) + Convert.ToInt32(dt_cuiyang.Rows[1][0].ToString()) + Convert.ToInt32(dt_cuiyang.Rows[2][0].ToString()) >= 600)//////////////////////////////////////////////////////////第一次加第二加第三次次大于600秒
                    {
                        DBconn.writeNew("update zlsj_history set reblow_num='" + (dt_cuiyang.Rows.Count - 3) + "' where station_no=" + BSNo);
                        string starttime;//开始时间
                        string endtime;//结束时间
                        int t1;//单个吹氧时间
                        int t_all = 0;//总吹氧时间
                        int EXT_ITEM3 = 0;//补吹次数
                        int EXT_ITEM4_temp = 0;//第一次吹氧量
                        int EXT_ITEM4 = 0;//补吹氧量
                        int REBLOW_DURATION = 0;//补吹持续时间
                        foreach (DataRow dr in dt_cuiyang.Rows)
                        {
                            i++;
                            if (i == 3)
                            {
                                starttime = Convert.ToDateTime(dt_cuiyang.Rows[0][1]).ToString("yyyyMMddHHmmss");
                                endtime = Convert.ToDateTime(dr[2]).ToString("yyyyMMddHHmmss");
                                t1 = Convert.ToInt32(dt_cuiyang.Rows[0][0].ToString()) + Convert.ToInt32(dt_cuiyang.Rows[1][0].ToString()) + Convert.ToInt32(dt_cuiyang.Rows[2][0].ToString());
                                t_all = t_all + t1;
                                EXT_ITEM4_temp = Convert.ToInt32(dr[3].ToString());
                                DBconn.writeNew("update zlsj_history set blow_time" + (i - 2) + "='" + t1 + "',blow_start_time" + (i - 2) + "='" + starttime + "',blow_end_time" + (i - 2) + "='" + endtime + "',o2_sum_comsume='" + dr[3].ToString() + "',blow_time='" + t_all + "' where station_no=" + BSNo);
                            }
                            else if (i > 3 && i <= 7)
                            {
                                starttime = Convert.ToDateTime(dt_cuiyang.Rows[(i - 2)][1]).ToString("yyyyMMddHHmmss");
                                endtime = Convert.ToDateTime(dr[2]).ToString("yyyyMMddHHmmss");
                                t1 = Convert.ToInt32(dr[0].ToString());
                                t_all = t_all + t1;
                                EXT_ITEM4 = 0;
                                REBLOW_DURATION = REBLOW_DURATION + Convert.ToInt32(dr[0].ToString());
                                EXT_ITEM3++;
                                EXT_ITEM4 = Convert.ToInt32(dr[3].ToString()) - EXT_ITEM4_temp;
                                DBconn.writeNew("update zlsj_history set blow_time" + (i - 2) + "='" + t1 + "',blow_start_time" + (i - 2) + "='" + starttime + "',blow_end_time" + (i - 2) + "='" + endtime + "',o2_sum_comsume='" + dr[3].ToString() + "',blow_time='" + t_all + "' where station_no=" + BSNo);
                            }
                        }
                        DBconn.writeNew("update zlsj_history set EXT_ITEM3='" + EXT_ITEM3 + "',EXT_ITEM4='" + EXT_ITEM4 + "',REBLOW_DURATION='" + REBLOW_DURATION + "'  where station_no=" + BSNo);
                    }

                    else
                    {
                        DBconn.writeNew("update zlsj_history set reblow_num='" + (dt_cuiyang.Rows.Count - 1) + "' where station_no=" + BSNo);
                        string starttime;//开始时间
                        string endtime;//结束时间
                        int t1;//单个吹氧时间
                        int t_all = 0;//总吹氧时间
                        int EXT_ITEM3 = 0;//补吹次数
                        int EXT_ITEM4_temp = 0;//第一次吹氧量
                        int EXT_ITEM4 = 0;//补吹氧量
                        int REBLOW_DURATION = 0;//补吹持续时间
                        foreach (DataRow dr in dt_cuiyang.Rows)
                        {
                            i++;
                            if (i <= 5)
                            {
                                starttime = Convert.ToDateTime(dr[1]).ToString("yyyyMMddHHmmss");
                                endtime = Convert.ToDateTime(dr[2]).ToString("yyyyMMddHHmmss");
                                t1 = Convert.ToInt32(dr[0].ToString());
                                t_all = t_all + t1;
                                DBconn.writeNew("update zlsj_history set blow_time" + i + "='" + t1 + "',blow_start_time" + i + "='" + starttime + "',blow_end_time" + i + "='" + endtime + "',o2_sum_comsume='" + dr[3].ToString() + "',blow_time='" + t_all + "' where station_no=" + BSNo);
                                if (i < 2)
                                {
                                    EXT_ITEM4_temp = Convert.ToInt32(dr[3].ToString());
                                }
                                if (i > 1)
                                {
                                    EXT_ITEM4 = 0;
                                    REBLOW_DURATION = REBLOW_DURATION + t1;
                                    EXT_ITEM3++;
                                    EXT_ITEM4 = Convert.ToInt32(dr[3].ToString()) - EXT_ITEM4_temp;
                                }
                            }
                        }
                        DBconn.writeNew("update zlsj_history set EXT_ITEM3='" + EXT_ITEM3 + "',EXT_ITEM4='" + EXT_ITEM4 + "',REBLOW_DURATION='" + REBLOW_DURATION + "'  where station_no=" + BSNo);
                    }
                }
                WritelogAuto(BSNo, heat_id + "吹氧数据history计算成功");
            }
            catch (Exception err)
            {
                WritelogAuto(BSNo, heat_id + "吹氧数据history更新失败" + err.Message);

            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            DateTime date1 = DateTime.Now;
            try
            {
                DBconn.readNew("select * from zlsj_temp t");
            }
            catch(Exception err)
            {
                WritelogAuto("新数据库连接错误:" + err.Message + "尝试重新连接");
                try
                {
                    DBconn.OpenConnect1();
                    WritelogAuto("新数据库连接成功");
                }
                catch (Exception er)
                {
                    WritelogAuto(er.Message + "新数据库重连失败");
                    return;
                }
            }
            try
            {
                DBconn.readOld("select heatid from bof5status");
            }
            catch (Exception err)
            {
                WritelogAuto("旧数据库连接错误:" + err.Message + "尝试重新连接");
                try
                {
                    DBconn.OpenConnect2();
                    WritelogAuto("旧数据库连接成功");
                }
                catch (Exception er)
                {
                    WritelogAuto(er.Message + "旧据库新重连失败");
                    return;
                }
            }
            read_by_bofstatus();
            //TB_all.AppendText(DateTime.Now.Subtract(date1).TotalSeconds.ToString().Substring(0, 4)+"|");
            //DBconn.CloseConnect();
            if (bof5_trace_enable_l1 == "1")
            {
                boftraceAuto("5", bof5start_l1, sd_bof5start, bof5o2start_l1, sd_bof5o2start, bof5o2end_l1, sd_bof5o2end, bof5cgstart_l1, sd_bof5cgstart, bof5cgend_l1, sd_bof5cgend, bof5end_l1, bof5_o2start_again_L1, bof5_o2end_again_L1, sd_bof5o2start_again, sd_bof5o2end_again);
            }
            //TB_all.AppendText(DateTime.Now.Subtract(date1).TotalSeconds.ToString().Substring(0, 4) + "|");
            if (bof6_trace_enable_l1 == "1")
            {
                boftraceAuto("6", bof6start_l1, sd_bof6start, bof6o2start_l1, sd_bof6o2start, bof6o2end_l1, sd_bof6o2end, bof6cgstart_l1, sd_bof6cgstart, bof6cgend_l1, sd_bof6cgend, bof6end_l1, bof6_o2start_again_L1, bof6_o2end_again_L1, sd_bof6o2start_again, sd_bof6o2end_again);
            }
            //TB_all.AppendText(DateTime.Now.Subtract(date1).TotalSeconds.ToString().Substring(0, 4) + "|");
            if (bof7_trace_enable_l1 == "1")
            {
                boftraceAuto("7", bof7start_l1, sd_bof7start, bof7o2start_l1, sd_bof7o2start, bof7o2end_l1, sd_bof7o2end, bof7cgstart_l1, sd_bof7cgstart, bof7cgend_l1, sd_bof7cgend, bof7end_l1, bof7_o2start_again_L1, bof7_o2end_again_L1, sd_bof7o2start_again, sd_bof7o2end_again);
            }
            //TB_all.AppendText(DateTime.Now.Subtract(date1).TotalSeconds.ToString().Substring(0, 4)+"|\r\n");
            try
            {
                
                if (by_TB5_message.TextLength > 10000)
                {
                    by_TB5_message.Clear();
                }
                if (by_TB6_message.TextLength > 10000)
                {
                    by_TB6_message.Clear();
                }
                if (by_TB7_message.TextLength > 10000)
                {
                    by_TB7_message.Clear();
                }
                if (TB_all.TextLength>10000)
                {
                    TB_all.Clear();
                }
                by_LB_endheatno5.Text = endheat_no5;
                by_LB_endheatno6.Text = endheat_no6;
                by_LB_endheatno7.Text = endheat_no7;
                Ledstatus();

            }
            catch (Exception err)
            {

                
            }
            DateTime date2 = DateTime.Now;
            label17.Text = "周期耗时:" + date2.Subtract(date1).TotalSeconds.ToString() + "秒";
        }
        private int heatid_add1(string heatid)
        {
            int iheatid = Convert.ToInt32(heatid);
            iheatid++;
            return iheatid;
        }
       
        #region 分批加料
        private void aux_alloy_fenpijiliang(string heatid,string BSNo)
        {
            int loop_count = 0, loop_count_aux = 0, loop_count_alloy = 0;
            DataTable dt_local = new DataTable();
            DataTable dt = new DataTable();
            DataTable dt_code = new DataTable();


            try
            {
                //基础信息
                dt_local = DBconn.readNew("SELECT prod_date, station_no, sm_plan_no, pono, heat_no, PROD_SHIFT_NO, PROD_SHIFT_GROUP FROM zlsj_temp WHERE heat_no = '"+heatid+"'");
                string heat_no = heatid;

                //合金数据写入数据准备表
                dt = DBconn.readOld("select heatid,allbincode01,allbindata01,allbincode02,allbindata02,allbincode03,allbindata03, allbincode04,allbindata04,allbincode05,allbindata05,allbincode06,allbindata06,allbincode07,allbindata07,allbincode08,allbindata08, allbincode09,allbindata09,allbincode10,allbindata10,allbincode11,allbindata11,allbincode12,allbindata12,allbincode13,allbindata13, allbincode14,allbindata14,allbincode15,allbindata15,allbincode16,allbindata16,allbincode17,allbindata17,allbincode18,allbindata18, allbincode19,allbindata19,allbincode20,allbindata20 from bof" + BSNo + "status where heatid ='" + heat_no + "'");
                // string heat_no = dt.Rows[0]["heatid"].ToString();

                //合金编码写入数据准备表
                dt_code = DBconn.readOld("select * from BOFBINCODE order by sampletime desc");
                for (int i = 0; i < 20; i++)
                {
                    int j = i + 1;
                    if (i < 9)
                    {
                        if (dt.Rows[0]["allbindata0" + j + ""].ToString() != "0" && dt.Rows[0]["allbindata0" + j + ""].ToString() != "")
                        {
                            string hejinSql = "insert into ZLJLSJ_34CANG(mat_name,mat_type1,SALE_SEQ1,station_no,heat_no,MAT_CODE1,MAT_SIMPLE_ENAME1,DEVO_WT1,DEVO_TIME1) values('合金','alloy','"+BSNo+"20"+j+"','" + BSNo + "','" + heat_no + "','" + dt_code.Rows[0]["allbinid" + j + ""] + "','" + dt.Rows[0]["allbincode0" + j + ""] + "','" + dt.Rows[0]["allbindata0" + j + ""] + "',sysdate )";
                            DBconn.writeNew(hejinSql);
                            loop_count_alloy++;
                        }

                    }
                    else
                    {
                        if (dt.Rows[0]["allbindata" + j + ""].ToString() != "0" && dt.Rows[0]["allbindata" + j + ""].ToString() != "")
                        {
                            string hejinSql = "insert into ZLJLSJ_34CANG(mat_name,mat_type1,SALE_SEQ1,station_no,heat_no,MAT_CODE1,MAT_SIMPLE_ENAME1,DEVO_WT1,DEVO_TIME1) values('合金','alloy','" + BSNo + "2" + j + "','" + BSNo+"','" + heat_no + "','" + dt_code.Rows[0]["allbinid" + j + ""] + "','" + dt.Rows[0]["allbincode" + j + ""] + "','" + dt.Rows[0]["allbindata" + j + ""] + "',sysdate )";
                            DBconn.writeNew(hejinSql);
                            loop_count_alloy++;
                        }
                    }
                }
                WritelogAuto(BSNo, heatid + "写入ZLJLSJ_34CANG写入成功");
                //辅料+合金
                
                string outsql = "", insql = "";
                string ziduan = "", douhao = ",", ziduansql = "";


                dt = DBconn.readNew("select * from ZLJLSJ_34CANG where heat_no ='" + heat_no + "'");
                int count = dt.Rows.Count; ;
                //for( int i=0; i < dt.Rows.Count; i ++)
                for (int i = 0; i < 1; i++)
                {
                    int j = i + 1;
                    outsql = "select wm_concat('''' || MAT_CODE" + j + " || '''' || ',' || '''' || MAT_SIMPLE_ENAME" + j + " || '''' || ' ,' || '''' || DEVO_WT" + j + " || '''' || ' ,' || '''' || MANUAL_FLAG" + j + " || '''' || ' ,' || '''' || MAT_TYPE" + j + " || '''' || ' ,' || '''' || to_char(DEVO_TIME" + j + ",'yyyymmddhh24miss')|| '''' || ' ,' || '''' || SALE_SEQ" + j + " ||'''') as message from ZLJLSJ_34CANG where heat_no ='" + heat_no + "' ";
                    dt = DBconn.readNew(outsql);
                    insql += dt.Rows[0]["message"].ToString();
                    loop_count_aux++;

                }
                
                for (int i = 0; i < count; i++)
                {
                    int j = i + 1;
                    ziduan = "MAT_CODE" + j + ",MAT_SIMPLE_ENAME" + j + ",DEVO_WT" + j + ",MANUAL_FLAG" + j + ",MAT_TYPE" + j + ",DEVO_TIME" + j + ",SALE_SEQ" + j + "";

                    ziduansql += ziduan + douhao;
                }
                ziduansql = ziduansql.Substring(0, ziduansql.Length - 1);
                //WritelogAuto(BSNo, heatid + "读取合金辅料数据成功");
                //写入转炉加料实际
                loop_count = count;
                
                DBconn.writeNew("insert into zljlsj_fenpi(station_id,station_no,prod_time, sm_plan_no, pono, heat_no,proc_no, PROD_SHIFT_NO, PROD_SHIFT_GROUP,loop_count," + ziduansql + ") values('B','" + dt_local.Rows[0]["station_no"] + "',to_char(sysdate,'yyyymmddhh24miss'),'" + dt_local.Rows[0]["sm_plan_no"] + "','" + dt_local.Rows[0]["pono"] + "','" + dt_local.Rows[0]["heat_no"] + "','" + dt_local.Rows[0]["heat_no"] + "','" + dt_local.Rows[0]["PROD_SHIFT_NO"] + "','" + dt_local.Rows[0]["PROD_SHIFT_GROUP"] + "','" + loop_count + "', " + insql + ")");
                WritelogAuto(BSNo, heatid + "写入zljlsj_fenpi分次转炉加料实际成功");
                
            }
            catch (Exception err)
            {
                WritelogAuto(BSNo, heatid + "分批加料更新失败" + err.Message);
            }

        }

        #endregion
        
        #region 合金时间
        private void zljlsj_time(string heat_no,string BSNo)
        {
            try
            {
                DataTable by_zljlsj_t = DBconn.readNew("select * from ZLJLSJ_34CANG t where t.heat_no='"+heat_no+"' order by t.devo_time1 desc");
                DataTable by_zljlsj = DBconn.readNew("select * from ZLJLSJ t where t.heat_no='" + heat_no + "'");
                int loop_count = Convert.ToInt32(by_zljlsj.Rows[0]["loop_count"].ToString());
                string jl_time_name;
                string jl_time ;
                string sql = "update zljlsj set ";
                string ii="";
                for (int i = 0; i < loop_count; i++)
                {

                    ii = (i+1).ToString();
                    jl_time_name = by_zljlsj.Rows[0]["MAT_SIMPLE_ENAME" + ii].ToString();
                    if (by_zljlsj_t.Select("MAT_SIMPLE_ENAME1='" + jl_time_name + "'").Length > 0)
                    {
                        if (by_zljlsj.Rows[0]["DEVO_WT" + ii].ToString() != "" && by_zljlsj.Rows[0]["DEVO_WT" + ii].ToString() != "0")
                        {
                            jl_time = by_zljlsj_t.Select("MAT_SIMPLE_ENAME1='" + jl_time_name + "'")[0]["DEVO_TIME1"].ToString();
                            jl_time = Convert.ToDateTime(jl_time).ToString("yyyyMMddHHmmss");
                            sql += "devo_time" + ii + "='" + jl_time + "',";
                        }
                    }


                }
                sql = sql.Substring(0, sql.Length - 1);//去最后一个逗号
                sql += " where heat_no='" + heat_no + "'";
                DBconn.writeNew(sql);
                WritelogAuto(BSNo, heat_no+"合金时间写入成功");
            }
            catch (Exception err)
            {
                WritelogAuto(BSNo, heat_no+"合金时间写入失败" + err.Message);
            }
        }
        #endregion
        
        #region 停炉判断
        private void stop_heat(string heat_no,string BSNO)
        {
            try {
                DateTime last_o2_end;//上炉吹氧结束时刻
                DateTime this_o2_start;//本炉吹氧开始时刻
                TimeSpan chazhi;//差
                string s_lo2e;//string格式的上炉吹氧结束时刻
                string s_to2s;//string格式的本炉吹氧开始时刻
                string lasto2;//verchar14格式的上炉吹氧结束时刻
                string thiso2;//verchar14格式的本炉吹氧开始时刻
                DataTable zlsj_his = DBconn.readNew("select * from (select t.heat_no,t.prod_shift_no,t.prod_shift_group from ZLSJ_HISTORY t where t.station_no='" + BSNO + "' order by t.create_time desc) where rownum=1");
                string last_heat_no = zlsj_his.Rows[0][0].ToString();
                string banci = zlsj_his.Rows[0][1].ToString();
                string banzhu = zlsj_his.Rows[0][2].ToString();
                DataTable o2_end_time = DBconn.readOld("select to_char(t.blowendtime,'yyyymmddhh24miss') from BOF" + BSNO + "BLOWO2DATA t where t.heatid='" + last_heat_no + "' order by t.blowendtime desc");
                if (o2_end_time.Rows.Count > 0)//转格式
                {
                    s_lo2e = o2_end_time.Rows[0][0].ToString();
                    lasto2 = s_lo2e;
                    s_lo2e = s_lo2e.Substring(0, 4) + "/" + s_lo2e.Substring(4, 2) + "/" + s_lo2e.Substring(6, 2) + " " + s_lo2e.Substring(8, 2) + ":" + s_lo2e.Substring(10, 2) + ":" + s_lo2e.Substring(12, 2);
                }
                else
                {
                    WritelogAuto(BSNO, heat_no+"未查询到上炉吹氧信息,停炉判断失败");
                    return;
                }
                last_o2_end = Convert.ToDateTime(s_lo2e);//上炉吹氧结束时刻
                DataTable o2_begin_time = DBconn.readOld("select to_char(t.blowo2begintime,'yyyymmddhh24miss') from BOF" + BSNO + "STATUS t");
                if (o2_begin_time.Rows[0][0].ToString() != "")//转格式
                {
                    s_to2s = o2_begin_time.Rows[0][0].ToString();

                    thiso2 = s_to2s;
                    s_to2s = s_to2s.Substring(0, 4) + "/" + s_to2s.Substring(4, 2) + "/" + s_to2s.Substring(6, 2) + " " + s_to2s.Substring(8, 2) + ":" + s_to2s.Substring(10, 2) + ":" + s_to2s.Substring(12, 2);
                    this_o2_start= Convert.ToDateTime(s_to2s);//本炉吹氧开始时刻
                }
                else
                {
                    this_o2_start = DateTime.Now;//本炉吹氧开始时刻
                    thiso2 = DateTime.Now.ToString("yyyyMMddHHmmss");
                }
                chazhi = this_o2_start.Subtract(last_o2_end);//差值计算
                if ((int)chazhi.TotalSeconds > 1500)
                {
                    WritelogAuto(BSNO, heat_no+"检测到停炉时间大于25分钟写入转炉停机表,时间:" + (int)chazhi.TotalSeconds + "秒");
                    DBconn.writeNew("insert into zltj values('B" + BSNO + "','A2','" + banci + "','"+ banzhu + "','"+ lasto2 + "','"+ (int)chazhi.TotalSeconds + "','"+ last_heat_no + "','"+ thiso2 + "','"+ heat_no + "','0',sysdate)");
                }
                else
                {
                    WritelogAuto(BSNO, heat_no+"停炉检测,时间:" + (int)chazhi.TotalSeconds+"秒");
                }

            }catch(Exception err)
            {
                WritelogAuto(BSNO, heat_no+"停炉判断错误:" + err.Message);
            }
        }
        #endregion
        
        #region 实时停炉判断
        private void stop_heat_time(string BSNO)
        {
            try
            {
                DataTable dt_zlsj_tj = DBconn.readNew("select t.heat_no from ZLSJ_TEMP t where t.by_station_no='"+BSNO+"'");
                if (dt_zlsj_tj.Rows[0][0].ToString().Length > 2)
                {
                    return;
                }
                DateTime last_o2_end;//上炉吹氧结束时刻
                TimeSpan chazhi;//差
                string s_lo2e;//string格式的上炉吹氧结束时刻
                string lasto2;//verchar14格式的上炉吹氧结束时刻
                string thiso2;//verchar14格式的本炉吹氧开始时刻
                DataTable zlsj_his = DBconn.readNew("select * from (select t.heat_no,t.prod_shift_no,t.prod_shift_group from ZLSJ_HISTORY t where t.station_no='" + BSNO + "' order by t.create_time desc) where rownum=1");
                if (zlsj_his.Rows.Count < 1)
                {
                    return;
                }
                string banci = zlsj_his.Rows[0][1].ToString();
                string banzhu = zlsj_his.Rows[0][2].ToString();
                string last_heat_no = zlsj_his.Rows[0][0].ToString();
                
                //DataTable dt_max_o2_heatno = DBconn.readOld("select max(heatid) from BOF" + BSNO + "BLOWO2DATA");
                //if (dt_max_o2_heatno.Rows.Count > 0)
                //{
                //    string max_o2_heatno = dt_max_o2_heatno.Rows[0][0].ToString();
                //    int i_last_heat_no = Convert.ToInt32(last_heat_no);
                //    int i_max_o2_heatno = Convert.ToInt32(max_o2_heatno);
                //    if (i_max_o2_heatno > i_last_heat_no)//比较吹氧数据炉号与历史表炉号
                //    {
                //        return;
                //    }
                //}
                DataTable dt_zltj = DBconn.readNew("select t.bak1 from ZLTJ t where t.bak1='" + last_heat_no + "'");
                if (dt_zltj.Rows.Count > 0)//如果停炉表有该炉炉号则退出(判断停炉表有没有该炉炉号)
                {
                    return;
                }
                
                DataTable o2_end_time = DBconn.readOld("select to_char(t.blowendtime,'yyyymmddhh24miss') from BOF" + BSNO + "BLOWO2DATA t where t.heatid='" + last_heat_no + "' order by t.blowendtime desc");

                if (o2_end_time.Rows.Count > 0)//转格式
                {
                    s_lo2e = o2_end_time.Rows[0][0].ToString();
                    lasto2 = s_lo2e;
                    s_lo2e = s_lo2e.Substring(0, 4) + "/" + s_lo2e.Substring(4, 2) + "/" + s_lo2e.Substring(6, 2) + " " + s_lo2e.Substring(8, 2) + ":" + s_lo2e.Substring(10, 2) + ":" + s_lo2e.Substring(12, 2);
                }
                else
                {
                    return;
                }
                last_o2_end = Convert.ToDateTime(s_lo2e);//上炉吹氧结束时刻
                DataTable o2_begin_time = DBconn.readOld("select to_char(t.blowo2begintime,'yyyymmddhh24miss') from BOF" + BSNO + "STATUS t");
                thiso2= o2_begin_time.Rows[0][0].ToString();
                DateTime datetime_now = DateTime.Now;
                chazhi = datetime_now.Subtract(last_o2_end);//差值计算
                if ((int)chazhi.TotalSeconds > 1500&& thiso2=="")
                {
                    WritelogAuto(BSNO, "实时检测到停炉时间大于25分钟写入转炉停机表,停炉炉号" + last_heat_no);
                    DBconn.writeNew("insert into zltj values('B" + BSNO + "','A2','" + banci + "','" + banzhu + "','" + lasto2 + "','','" + last_heat_no + "','','','0',sysdate)");
                }
            }
            catch (Exception err)
            {
                WritelogAuto(BSNO, "实时停炉判断错误:" + err.Message);
            }


        }



        #endregion

        #region LED灯

        private void Ledstatus()
        {
            if (bof5ledstatus == 1)
            {
                LB_5status1.BackColor = Color.LightGreen;
                LB_5status2.BackColor = Color.DeepSkyBlue;
                LB_5status3.BackColor = Color.DeepSkyBlue;
            }
            else if(bof5ledstatus == 2)
            {
                LB_5status1.BackColor = Color.DeepSkyBlue;
                LB_5status2.BackColor = Color.LightGreen;
                LB_5status3.BackColor = Color.DeepSkyBlue;
            }
            else if (bof5ledstatus == 3)
            {
                LB_5status1.BackColor = Color.DeepSkyBlue;
                LB_5status2.BackColor = Color.DeepSkyBlue;
                LB_5status3.BackColor = Color.LightGreen;
            }
            else
            {
                LB_5status1.BackColor = Color.Red;
                LB_5status2.BackColor = Color.Red;
                LB_5status3.BackColor = Color.Red;
            }
            //-------------------------------------------------
            if (bof6ledstatus == 1)
            {
                LB_6status1.BackColor = Color.LightGreen;
                LB_6status2.BackColor = Color.DeepSkyBlue;
                LB_6status3.BackColor = Color.DeepSkyBlue;
            }
            else if (bof6ledstatus == 2)
            {
                LB_6status1.BackColor = Color.DeepSkyBlue;
                LB_6status2.BackColor = Color.LightGreen;
                LB_6status3.BackColor = Color.DeepSkyBlue;
            }
            else if (bof6ledstatus == 3)
            {
                LB_6status1.BackColor = Color.DeepSkyBlue;
                LB_6status2.BackColor = Color.DeepSkyBlue;
                LB_6status3.BackColor = Color.LightGreen;
            }
            else
            {
                LB_6status1.BackColor = Color.Red;
                LB_6status2.BackColor = Color.Red;
                LB_6status3.BackColor = Color.Red;
            }
            //-------------------------------------------------
            if (bof7ledstatus == 1)
            {
                LB_7status1.BackColor = Color.LightGreen;
                LB_7status2.BackColor = Color.DeepSkyBlue;
                LB_7status3.BackColor = Color.DeepSkyBlue;
            }
            else if (bof7ledstatus == 2)
            {
                LB_7status1.BackColor = Color.DeepSkyBlue;
                LB_7status2.BackColor = Color.LightGreen;
                LB_7status3.BackColor = Color.DeepSkyBlue;
            }
            else if (bof7ledstatus == 3)
            {
                LB_7status1.BackColor = Color.DeepSkyBlue;
                LB_7status2.BackColor = Color.DeepSkyBlue;
                LB_7status3.BackColor = Color.LightGreen;
            }
            else
            {
                LB_7status1.BackColor = Color.Red;
                LB_7status2.BackColor = Color.Red;
                LB_7status3.BackColor = Color.Red;
            }
        }

        #endregion

        #region 手动
        private void sj_duquhisDB()//读取历史数据
        {
            try
            {
                DataTable by_hisDT;
                by_hisDT = DBconn.readNew("select * from ZLSJ_history t where t.heat_no=" + by_heatno_TB.Text + " ");
                if (by_hisDT.Rows.Count > 0)
                {
                    foreach (Control cc in tabPage3.Controls)//获取tabpage1的所有控件
                    {
                        if (cc is TextBox && cc.Name.Substring(0, 6) == "by_his")
                        {
                            cc.Text = by_hisDT.Rows[0][Convert.ToInt32(cc.Name.Substring(6)) - 1].ToString();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("没有查到炉号为" + by_heatno_TB.Text + "的历史数据");
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);

            }
        }

        private void write_hisDB()//写入历史数据
        {
            try
            {

                if (DBconn.readNew("select count(*) from ZLSJ_HISTORY t where t.heat_no='" + by_his005.Text + "'").Rows[0][0].ToString() == "0")
                {
                    DBconn.writeNew("insert into zlsj_history(heat_no) values('" + by_his005.Text + "')");
                    DBconn.writeNew("insert into by_infomation values(sysdate,'插入历史实绩 heat_no=" + by_his005.Text + " station=跟踪程序')");
                    Thread.Sleep(1000);
                }
                DBconn.writeNew("insert into by_infomation values(sysdate,'历史实际修改 heat_no=" + by_his005.Text + " station=跟踪程序')");
                by_his109.Text = "8";
                string str_genxin = "update ZLSJ_history t set ";
                foreach (Control cc in tabPage3.Controls)//获取tabpage3的所有控件
                {
                    if (cc is TextBox && cc.Name.Substring(0, 6) == "by_his" && cc.Name != "by_his108")
                    {
                        str_genxin += by_DBTBhis.Lines[Convert.ToInt32(cc.Name.Substring(6)) - 1] + " = '" + cc.Text + "',";
                    }
                }
                str_genxin = str_genxin.Substring(0, str_genxin.Length - 1);
                str_genxin += " where t.heat_no='" + by_his005.Text + "'";
                DBconn.writeNew(str_genxin);
                MessageBox.Show("写入历史数据成功!，重新发送历史数据到三级有1-2分钟左右延迟");
                sj_duquhisDB();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message + "/n写入数据失败!");
            }
        }
        #endregion
    }
}
