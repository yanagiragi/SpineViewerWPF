﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SpineViewerWPF.Views;
using Microsoft.Win32;
using WpfXnaControl;
using System.Text.RegularExpressions;

namespace SpineViewerWPF
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow MasterMain;
        public static ContentControl MasterControl;
        public static Player2_1_08 UC_Player2_1_08;
        public static Player2_1_25 UC_Player2_1_25;
        public static Player3_1_07 UC_Player3_1_07;
        public static Player3_4_02 UC_Player3_4_02;
        public static Player3_5_51 UC_Player3_5_51;
        public static Player3_6_32 UC_Player3_6_32;
        public static Player3_6_39 UC_Player3_6_39;


        public MainWindow()
        {
            InitializeComponent();
            MasterMain = this;
            LoadSetting();
        }

        private void LoadSetting()
        {
            if (App.GV.Scale == 0)
                App.GV.Scale = 1;
            App.LastDir = App.RootDir;

            tb_Fps.SetBinding(TextBox.TextProperty, new Binding() { Source = App.GV, Path = new PropertyPath("Speed") });
            lb_Scale.SetBinding(Label.ContentProperty, new Binding() { Source = App.GV, Path = new PropertyPath("Scale") });
            lb_Width.SetBinding(ContentProperty, new Binding() { Source = App.GV, Path = new PropertyPath("FrameWidth") });
            lb_Height.SetBinding(ContentProperty, new Binding() { Source = App.GV, Path = new PropertyPath("FrameHeight") });

            //lb_PosX.SetBinding(ContentProperty, new Binding() { Source = App.GV, Path = new PropertyPath("PosX") });
            //lb_PosY.SetBinding(ContentProperty, new Binding() { Source = App.GV, Path = new PropertyPath("PosY") });

            tb_PosX.SetBinding(TextBox.TextProperty, new Binding() { Source = App.GV, Path = new PropertyPath("PosX") });
            tb_PosY.SetBinding(TextBox.TextProperty, new Binding() { Source = App.GV, Path = new PropertyPath("PosY") });

            chb_Alpha.SetBinding(CheckBox.IsCheckedProperty, new Binding() { Source = App.GV, Path = new PropertyPath("Alpha") });
            chb_IsLoop.SetBinding(CheckBox.IsCheckedProperty, new Binding() { Source = App.GV, Path = new PropertyPath("IsLoop") });
            chb_PreMultiplyAlpha.SetBinding(CheckBox.IsCheckedProperty, new Binding() { Source = App.GV, Path = new PropertyPath("PreMultiplyAlpha") });
            TextCompositionManager.AddPreviewTextInputStartHandler(tb_Fps, tb_Fps_PreviewTextInput);


        }

        private void cb_AnimeList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cb_AnimeList.SelectedIndex != -1)
            {
                if (cb_AnimeList.SelectedItem.ToString() != "")
                {
                    App.GV.SelectAnimeName = cb_AnimeList.SelectedItem.ToString();
                    App.GV.SetAnime = true;
                }
            }
        }

        public void UpdateSpine()
        {
            switch (App.GV.SpineVersion)
            {
                case "2.1.08":
                    UC_Player2_1_08.ChangeSet();
                    break;
                case "2.1.25":
                    UC_Player2_1_25.ChangeSet();
                    break;
                case "3.1.07":
                    UC_Player3_1_07.ChangeSet();
                    break;
                case "3.4.02":
                    UC_Player3_4_02.ChangeSet();
                    break;
                case "3.5.51":
                    UC_Player3_5_51.ChangeSet();
                    break;
                case "3.6.32":
                    UC_Player3_6_32.ChangeSet();
                    break;
                case "3.6.39":
                    UC_Player3_6_39.ChangeSet();
                    break;
            }
        }

        public static void SetCBAnimeName()
        {
            for (int i = 0; i < App.GV.AnimeList.Count; i++)
            {
                MasterMain.cb_AnimeList.Items.Add(App.GV.AnimeList[i]);
            }

            for (int i = 0; i < App.GV.SkinList.Count; i++)
            {
                MasterMain.cb_SkinList.Items.Add(App.GV.SkinList[i]);
            }
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {

            App.GV.FrameWidth = Math.Round(this.ActualWidth - 200, 2);
            App.GV.FrameHeight = Math.Round(this.ActualHeight - 60, 2);
            Player.Width = App.GV.FrameWidth;
            Player.Height = App.GV.FrameHeight +20;

        }

        private void tb_PosX_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            TextChange[] changesText = new TextChange[e.Changes.Count];
            e.Changes.CopyTo(changesText, 0);

            int offset = changesText[0].Offset;
            int totalCount = changesText[0].AddedLength;

            if (totalCount > 0)
            {
                char[] charArray = new char[textBox.Text.Length];
                textBox.Text.CopyTo(0, charArray, 0, charArray.Length);
                StringBuilder sbu = new StringBuilder(new string(charArray));

                for (int i = sbu.Length - 1; i >= offset; --i)
                {
                    if (charArray[i] < 48 || charArray[i] > 57)
                    {
                        sbu = sbu.Remove(i, 1);
                    }
                }
                textBox.Text = sbu.ToString();
                textBox.Select(offset + totalCount, 0);
            }

            if (totalCount == 0)
            {
                // avoid check when totalCount = 0
                // probably user just deleting all number when trying to type a new number
                App.GV.PosX = 0;
                textBox.Text = "0";
            }
            else
            {
                float posX;
                if (float.TryParse(textBox.Text.Trim(), out posX))
                {
                    App.GV.PosX = posX;
                }
                else
                {
                    App.GV.PosX = 2048;
                    textBox.Text = "2048";
                    MessageBox.Show("Error Number！");
                }
            }
            
        }

        private void tb_PosY_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            TextChange[] changesText = new TextChange[e.Changes.Count];
            e.Changes.CopyTo(changesText, 0);

            int offset = changesText[0].Offset;
            int totalCount = changesText[0].AddedLength;

            if (totalCount > 0)
            {
                char[] charArray = new char[textBox.Text.Length];
                textBox.Text.CopyTo(0, charArray, 0, charArray.Length);
                StringBuilder sbu = new StringBuilder(new string(charArray));

                for (int i = sbu.Length - 1; i >= offset; --i)
                {
                    if (charArray[i] < 48 || charArray[i] > 57)
                    {
                        sbu = sbu.Remove(i, 1);
                    }
                }
                textBox.Text = sbu.ToString();
                textBox.Select(offset + totalCount, 0);
            }

            if (totalCount == 0)
            {
                // avoid check when totalCount = 0
                // probably user just deleting all number when trying to type a new number
                App.GV.PosY = 0;
                textBox.Text = "0";
            }
            else
            {
                float posY;
                if (float.TryParse(textBox.Text.Trim(), out posY))
                {
                    App.GV.PosY = posY;
                }
                else
                {
                    App.GV.PosY = 2048;
                    textBox.Text = "2048";
                    MessageBox.Show("Error Number！");
                }
            }            
        }

        private void tb_Fps_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            TextChange[] changesText = new TextChange[e.Changes.Count];
            e.Changes.CopyTo(changesText, 0);
            
            int offset = changesText[0].Offset;
            int totalCount = changesText[0].AddedLength;
            
            if (totalCount > 0)
            {
                char[] charArray = new char[textBox.Text.Length];
                textBox.Text.CopyTo(0, charArray, 0, charArray.Length);
                StringBuilder sbu = new StringBuilder(new string(charArray));
                
                for (int i = sbu.Length - 1; i >= offset; --i)
                {
                    if (charArray[i] < 48 || charArray[i] > 57)
                    {
                        sbu = sbu.Remove(i, 1);
                    }
                }
                textBox.Text = sbu.ToString();
                textBox.Select(offset + totalCount, 0);
            }

            // Modified By yanagiragi
            // Original: if (int.TryParse(tb_Fps.Text.Trim(), out int fps))
            // I got "CS1525 Invalid expression term 'int'" when using .Net Framework 4.2.6
            // Instead I declare a varaible as workAround.
            // # WillFix
            int fps;
            if (int.TryParse(tb_Fps.Text.Trim(), out fps))
            {
                App.GV.Speed = fps;
            }
            else
            {
                App.GV.Speed = 24;
                tb_Fps.Text = "24";
                MessageBox.Show("Error Number！");
            }
        }
        private void cb_SkinList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cb_SkinList.SelectedIndex != -1)
            {
                if (cb_SkinList.SelectedItem.ToString() != "")
                {
                    App.GV.SelectSkin = cb_SkinList.SelectedItem.ToString();
                    App.GV.SetSkin = true;
                }

            }
        }


        private void chb_IsLoop_Click(object sender, RoutedEventArgs e)
        {
            App.GV.SetAnime = true;
        }
        private void chb_PreMultiplyAlpha_Click(object sender, RoutedEventArgs e)
        {
            UpdateSpine();
        }



        private void tb_Fps_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regExp = new Regex(@"\d");
            string singleValue = e.Text;
            e.Handled = !regExp.Match(singleValue).Success;
          
        }

        private void loadFileToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = App.LastDir;
            openFileDialog.Filter = "Spine Altas Files (*.atlas)|*.atlas;";

            if (cb_Version.SelectedIndex == -1)
            {
                MessageBox.Show("Please Select Spine Version！");
                return;
            }

            if (openFileDialog.ShowDialog() == true)
            {
                App.GV.SelectFile = openFileDialog.FileName;
                App.GV.PosX = 0;
                App.GV.PosY = 0;
                App.GV.Scale = 1;
                App.GV.SelectAnimeName = "";
                App.GV.SelectSkin = "";
                App.GV.SetSkin = false;
                App.GV.SetAnime = false;
                App.LastDir = Common.GetDirName(openFileDialog.FileName);
                if (App.GV.AnimeList != null)
                    App.GV.AnimeList.Clear();
                if (App.GV.SkinList != null)
                    App.GV.SkinList.Clear();




                MasterMain.cb_AnimeList.Items.Clear();
                MasterMain.cb_SkinList.Items.Clear();
                if (Player.Content != null)
                {
                    App.AppXC.ContentManager.Dispose();
                    App.AppXC.Initialize = null;
                    App.AppXC.Update = null;
                    App.AppXC.LoadContent = null;
                    App.AppXC.Draw = null;
                    Player.Content = null;
                    UC_Player2_1_08 = null;
                    UC_Player2_1_25 = null;
                    UC_Player3_1_07 = null;
                    UC_Player3_4_02 = null;
                    UC_Player3_5_51 = null;
                    UC_Player3_6_32 = null;
                    UC_Player3_6_39 = null;
                }
                App.GV.SpineVersion = cb_Version.SelectionBoxItem.ToString();
                App.AppXC = new XnaControl();
                switch (App.GV.SpineVersion)
                {
                    case "2.1.08":
                        UC_Player2_1_08 = new Player2_1_08();
                        Player.Content = UC_Player2_1_08;
                        break;
                    case "2.1.25":
                        UC_Player2_1_25 = new Player2_1_25();
                        Player.Content = UC_Player2_1_25;
                        break;
                    case "3.1.07":
                        UC_Player3_1_07 = new Player3_1_07();
                        Player.Content = UC_Player3_1_07;
                        break;
                    case "3.4.02":
                        UC_Player3_4_02 = new Player3_4_02();
                        Player.Content = UC_Player3_4_02;
                        break;
                    case "3.5.51":
                        UC_Player3_5_51 = new Player3_5_51();
                        Player.Content = UC_Player3_5_51;
                        break;
                    case "3.6.32":
                        UC_Player3_6_32 = new Player3_6_32();
                        Player.Content = UC_Player3_6_32;
                        break;
                    case "3.6.39":
                        UC_Player3_6_39 = new Player3_6_39();
                        Player.Content = UC_Player3_6_39;
                        break;
                }
            }
        }

        private void cb_gif_q_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cb_gif_q.SelectedIndex != -1)
            {
                if (cb_gif_q.SelectedItem.ToString() != "")
                {
                    App.GV.GifQuality = ((ComboBoxItem)cb_gif_q.SelectedItem).Content.ToString();
                }
            }
        }
    }
}
