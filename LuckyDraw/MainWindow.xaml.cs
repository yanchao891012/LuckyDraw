﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LuckyDraw
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            SelectInfo();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Setting win = new Setting();
            win.ShowInTaskbar = false;
            win.Owner = this;
            win.ShowDialog();            
        }

        private void SelectInfo()
        {
            StaticEntity.AwardNameList.Clear();
            ComDesc comDesc= DB.DBHelper.SelectComDesc();
            tb_Company.Text = comDesc.Company;
            tb_Desc.Text = comDesc.Desc;

            List<Award> awardList = DB.DBHelper.SelectAward();
            sp_list1.Children.Clear();
            sp_list2.Children.Clear();
            foreach (var item in awardList)
            {
                TextBlock tb1 = new TextBlock();
                tb1.Text = item.Name +"："+ item.Number + "名";
                tb1.Style = Resources["TextBlockAwardStyle"] as Style;
                tb1.Foreground = Brushes.Gold;
                sp_list1.Children.Add(tb1);

                TextBlock tb2 = new TextBlock();
                tb2.Text = "  奖品：" + item.Content;
                tb2.Style = Resources["TextBlockAwardStyle"] as Style;
                tb2.Foreground = Brushes.Gold;
                sp_list2.Children.Add(tb2);

                for (int i = 0; i < item.Number; i++)
                {
                    StaticEntity.AwardNameList.Add(item.Name);
                }
                StaticEntity.DispalyNameList.Add(item.Name);
            }
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            SelectInfo();
        }

        private void btn_start_Click(object sender, RoutedEventArgs e)
        {
            if (btn_start.Tag.ToString().Equals("start"))
            {
                if (StaticEntity.AwardNameList.Count==0)
                {
                    la_Award.Content = "开奖区";
                    MessageBox.Show("奖池中已无奖项，请添加!");
                    return;
                }
                btn_start.Content = "停  止";
                btn_start.Tag = "end";
                timer_random = new Timer(StaticEntity.TimeSpeed);
                timer_random.AutoReset = true;
                timer_random.Elapsed += Timer_random_Elapsed;
                timer_random.Start();
                return;
            }

            if (btn_start.Tag.ToString().Equals("end"))
            {
                btn_start.Content = "开  始";
                btn_start.Tag = "start";
                timer_random.Stop();
                RandomAward(StaticEntity.AwardNameList.Count, StaticEntity.AwardNameList);
                StaticEntity.AwardNameList.Remove(StaticEntity.AwardNameList[StaticEntity.WinNum]);
                return;
            }
        }

        private void Timer_random_Elapsed(object sender, ElapsedEventArgs e)
        {
            RandomAward(StaticEntity.DispalyNameList.Count, StaticEntity.DispalyNameList);
        }

        Timer timer_random;
        private void RandomAward(int count,List<string> list)
        {
            //int count = StaticEntity.AwardNameList.Count;
            Random random = new Random();
            int ranNum = random.Next(0, count);
            StaticEntity.WinNum = ranNum;
            this.la_Award.Dispatcher.Invoke(new Action(() =>
            {
                la_Award.Content = list[ranNum];
            }));
        }



        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key==Key.Space)
            {
                btn_start_Click(sender, e);
            }
        }
    }
}
