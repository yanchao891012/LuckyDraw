using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LuckyDraw
{
    /// <summary>
    /// Setting.xaml 的交互逻辑
    /// </summary>
    public partial class Setting : Window
    {
        public Setting()
        {
            InitializeComponent();
            GetAwardList();
        }
        /// <summary>
        /// 添加公司名、活动名
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tb_Company.Text) && string.IsNullOrWhiteSpace(tb_Activ.Text))
            {
                return;
            }
            else
            {
                DB.DBHelper.DeleteToTable("TB_Title");
                DB.DBHelper.InsertToTable(tb_Company.Text, tb_Activ.Text);
                MessageBox.Show("添加成功");
            }
        }
        private void GetAwardList()
        {
            lb_Award.ItemsSource = DB.DBHelper.SelectAward();
            lb_Award.DisplayMemberPath = "Name";
        }
        /// <summary>
        /// 添加奖品信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_Add_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(tb_Name.Text) && !string.IsNullOrWhiteSpace(tb_Num.Text) && !string.IsNullOrWhiteSpace(tb_Content.Text))
            {
                if (DB.DBHelper.SelectCount(tb_Name.Text))
                {
                    DB.DBHelper.InsertToTable(tb_Name.Text, tb_Num.Text, tb_Content.Text);
                    MessageBox.Show("添加成功");
                    GetAwardList();
                    tbClear();
                }
                else
                {
                    MessageBox.Show("已包含此奖项");
                    return;
                }
            }
            else
                MessageBox.Show("相关奖项信息不能为空");
        }

        private void tbClear()
        {
            tb_Name.Clear();
            tb_Num.Clear();
            tb_Content.Clear();
        }

        /// <summary>
        /// 删除奖品信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Del_Click(object sender, RoutedEventArgs e)
        {
            if (lb_Award.SelectedItem != null)
            {
                DB.DBHelper.DeleteAwardToTable((lb_Award.SelectedItem as Award).Name);
                tbClear();
            }
            GetAwardList();
        }

        private void tb_Num_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) ||
                (e.Key >= Key.D0 && e.Key <= Key.D9) ||
                e.Key == Key.Back ||
                e.Key == Key.Left || e.Key == Key.Right)
            {
                if (e.KeyboardDevice.Modifiers != ModifierKeys.None)
                {
                    e.Handled = true;
                }
            }
            else
            {
                e.Handled = true;
            }
        }

        private void Btn_Update_Click(object sender, RoutedEventArgs e)
        {
            if (lb_Award.SelectedItem != null && !string.IsNullOrWhiteSpace(tb_Name.Text) && !string.IsNullOrWhiteSpace(tb_Num.Text) && !string.IsNullOrWhiteSpace(tb_Content.Text))
            {
                DB.DBHelper.UpdateAwardToTable((lb_Award.SelectedItem as Award).Name, Int32.Parse(tb_Num.Text), tb_Content.Text);
                MessageBox.Show("更新成功");
                tbClear();
            }
            GetAwardList();
        }

        private void lb_Award_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Award aw = lb_Award.SelectedItem as Award;
            if (aw != null)
            {
                tb_Name.Text = aw.Name;
                tb_Num.Text = aw.Number.ToString();
                tb_Content.Text = aw.Content;
            }
        }

        private void Btn_UpdateSpeed_Click(object sender, RoutedEventArgs e)
        {
            StaticEntity.TimeSpeed = Int32.Parse(tb_speed.Text.ToString());
            MessageBox.Show("更新成功");
        }
    }
}
