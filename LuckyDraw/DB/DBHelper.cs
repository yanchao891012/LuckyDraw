using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;

namespace LuckyDraw.DB
{
    class DBHelper
    {
        static SQLiteConnection db_Connection;
        /// <summary>
        /// 连接数据库
        /// </summary>
        static bool ConnectionToDataBase()
        {
            db_Connection = new SQLiteConnection("Data Source=" + AppDomain.CurrentDomain.BaseDirectory + "\\DB\\LuckyDrawDB.db;Version=3");

            if (db_Connection.State != System.Data.ConnectionState.Open)
            {
                db_Connection.Open();
            }
            return true;
        }
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="sql"></param>
        static void CommandToTable(string sql)
        {
            try
            {
                if (ConnectionToDataBase())
                {
                    SQLiteCommand db_Command = new SQLiteCommand(sql, db_Connection);
                    db_Command.ExecuteNonQuery();
                    db_Command.Dispose();
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                db_Connection.Close();
                db_Connection.Dispose();
            }
        }
        /// <summary>
        /// 插入标题表
        /// </summary>
        /// <param name="company">公司名</param>
        /// <param name="desc">描述</param>
        public static void InsertToTable(string company, string desc)
        {
            string sql = "insert into TB_Title(Company,Desc) values ('" + company + "','" + desc + "')";
            CommandToTable(sql);
        }
        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="name">奖品等级</param>
        /// <param name="number">奖品数量</param>
        /// <param name="content">奖品内容</param>
        public static void InsertToTable(string name, string number, string content)
        {

            string sql = "insert into TB_Award(Name,Number,Content) values ('" + name + "','" + number + "','" + content + "')";
            CommandToTable(sql);
        }

        public static bool SelectCount(string name)
        {
            try
            {
                ConnectionToDataBase();
                string sql = "select count(*) from TB_Award where Name='" + name + "'";
                SQLiteCommand command = new SQLiteCommand(sql, db_Connection);
                SQLiteDataReader reader = command.ExecuteReader();
                reader.Read();
                if (reader["count(*)"].ToString().Equals("0"))
                {
                    command.Dispose();
                    reader.Close();
                    return true;
                }
                command.Dispose();
                reader.Close();
                return false;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                db_Connection.Close();
                db_Connection.Dispose();
            }
        }

        /// <summary>
        /// 清空表
        /// </summary>
        /// <param name="tableName"></param>
        public static void DeleteToTable(string tableName)
        {
            string sql = "delete from " + tableName + "";
            CommandToTable(sql);
        }
        /// <summary>
        /// 删除一行奖品
        /// </summary>
        /// <param name="name"></param>
        public static void DeleteAwardToTable(string name)
        {
            string sql = "delete from TB_Award where Name='" + name + "'";
            CommandToTable(sql);
        }

        public static void UpdateAwardToTable(string name,int number,string content)
        {
            string sql = "update TB_Award set Number=" + number + ",Content='" + content + "' where Name='" + name + "'";
            CommandToTable(sql);
        }

        public static List<Award> SelectAward()
        {
            try
            {
                ConnectionToDataBase();
                List<Award> list = new List<Award>();
                string sql = "select * from TB_Award";
                SQLiteCommand command = new SQLiteCommand(sql, db_Connection);
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Award award = new Award();
                    award.Name = reader["Name"].ToString();
                    award.Number = int.Parse(reader["Number"].ToString());
                    award.Content = reader["Content"].ToString();
                    list.Add(award);
                }
                command.Dispose();
                reader.Close();
                return list;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                db_Connection.Close();
                db_Connection.Dispose();
            }
        }

        public static ComDesc SelectComDesc()
        {
            try
            {
                ConnectionToDataBase();
                ComDesc item = new ComDesc();
                string sql = "select * from TB_Title";
                SQLiteCommand command = new SQLiteCommand(sql, db_Connection);
                SQLiteDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    item.Company = reader["Company"].ToString();
                    item.Desc = reader["Desc"].ToString();
                }             
                command.Dispose();
                reader.Close();
                return item;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                db_Connection.Close();
                db_Connection.Dispose();
            }
        }
    }
}
