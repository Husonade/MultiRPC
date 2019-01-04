﻿using MultiRPC.Data;
using System;
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

namespace MultiRPC.GUI
{
    /// <summary>
    /// Interaction logic for CustomPage.xaml
    /// </summary>
    public partial class CustomPage : UserControl
    {
        public CustomPage(CustomProfile profile)
        {
            Profile = profile;
            InitializeComponent();
            Name = profile.Name;
            ProfileName.Content = profile.Name;
            TextClientID.Text = profile.ClientID;
            Text1.Text = profile.Text1;
            Text2.Text = profile.Text2;
            TextLargeKey.Text = profile.LargeKey;
            TextLargeText.Text = profile.LargeText;
            TextSmallKey.Text = profile.SmallKey;
            TextSmallText.Text = profile.SmallText;
            if (TextClientID.Text.Length < 15 || !ulong.TryParse(TextClientID.Text, out ulong ID))
            {
                HelpError.ToolTip = "Invalid client ID";
                HelpError.Visibility = Visibility.Visible;
            }
            else
                HelpError.Visibility = Visibility.Hidden;
            if (profile.Name == "Custom")
                (MenuIcons.Items[4] as Image).Visibility = Visibility.Hidden;
            if (RPC.Config.Disabled.HelpIcons)
                DisableHelpIcons();
        }

        public CustomProfile Profile;

        private void ProfileIcon_Click(object sender, MouseButtonEventArgs e)
        {
            Image img = sender as Image;
            switch(img.Name)
            {
                case "ProfileShare":
                    {

                    }
                    break;
                case "ProfileAdd":
                    {
                        if (_Data.Profiles.Keys.Count == 30)
                            return;
                        int Count = 1;
                        string Name = $"Custom1";
                        while (_Data.Profiles.ContainsKey(Name))
                        {
                            Count++;
                            Name = $"Custom{Count}";
                        }
                        CustomProfile p = new CustomProfile { Name = Name };
                        _Data.Profiles.Add(p.Name, p);
                        _Data.SaveProfiles();
                        Button btn = GetButton(p);
                        btn.Click += MainWindow.ProfileBtn_Click;
                        App.WD.MenuProfiles.Items.Add(btn);
                        App.WD.ToggleMenu();
                    }
                    break;
                case "ProfileDelete":
                    {
                        if (_Data.Profiles.Keys.Count == 1)
                            MessageBox.Show("Cannot delete the last custom profile");
                        else
                        {
                            int Index = 0;
                            bool Found = false;
                            foreach(object i in App.WD.MenuProfiles.Items)
                            {
                                Button b = i as Button;
                                if (b.Name == Profile.Name)
                                {
                                    Found = true;
                                    break;
                                }
                                Index++;
                            }
                            if (!Found)
                                return;
                            if (Index == 0)
                            {
                                MessageBox.Show("Cannot delete the first profile");
                                return;
                            }
                            App.WD.MenuProfiles.Items.RemoveAt(Index);
                            MainWindow.CustomPage = new CustomPage(_Data.Profiles.Values.First());
                            (App.WD.MenuProfiles.Items[0] as Button).Background = (Brush)Application.Current.Resources["Brush_Button"];
                            App.WD.ViewCustomPage.Content = MainWindow.CustomPage;
                            _Data.Profiles.Remove(Profile.Name);
                            _Data.SaveProfiles();
                            App.WD.ToggleMenu();
                        }
                    }
                    break;
            }
        }

        private void TextClientID_TextChanged(object sender, TextChangedEventArgs e)
        {
            Profile.ClientID = (sender as TextBox).Text;
            if (TextClientID.Text.Length < 15 || !ulong.TryParse(TextClientID.Text, out ulong ID))
            {
                HelpError.ToolTip = "Invalid client ID";
                HelpError.Visibility = Visibility.Visible;
            }
            else
                HelpError.Visibility = Visibility.Hidden;

        }

        #region Set limit
        private void Textbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox Box = sender as TextBox;
            SetLimitNumber(Box);
            if (Box.Text.Length == 25)
                Box.Opacity = 0.80;
            else
                Box.Opacity = 1;
        }

        private void SetLimitNumber(TextBox box)
        {
            double db = 0.50;
            if (box.Text.Length == 25)
                db = 1;
            else if (box.Text.Length > 20)
                db = 0.90;
            else if (box.Text.Length > 15)
                db = 0.80;
            else if (box.Text.Length > 10)
                db = 0.70;
            else if (box.Text.Length > 5)
                db = 0.60;
            switch (box.Name)
            {
                case "Text1":
                    Profile.Text1 = Text1.Text;
                    LimitCustomText1.Content = 25 - box.Text.Length;
                    LimitCustomText1.Opacity = db;
                    break;
                case "Text2":
                    Profile.Text2 = Text2.Text;
                    LimitCustomText2.Content = 25 - box.Text.Length;
                    LimitCustomText2.Opacity = db;
                    break;
                case "TextLargeKey":
                    Profile.LargeKey = TextLargeKey.Text;
                    LimitCustomLargeKey.Content = 25 - box.Text.Length;
                    LimitCustomLargeKey.Opacity = db;
                    break;
                case "TextLargeText":
                    Profile.LargeText = TextLargeText.Text;
                    LimitCustomLargeText.Content = 25 - box.Text.Length;
                    LimitCustomLargeText.Opacity = db;
                    break;
                case "TextSmallKey":
                    Profile.SmallKey = TextSmallKey.Text;
                    LimitCustomSmallKey.Content = 25 - box.Text.Length;
                    LimitCustomSmallKey.Opacity = db;
                    break;
                case "TextSmallText":
                    Profile.SmallText = TextSmallText.Text;
                    LimitCustomSmallText.Content = 25 - box.Text.Length;
                    LimitCustomSmallText.Opacity = db;
                    break;
            }
        }
        #endregion

        #region Toggle limit number
        private void Textbox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox box = sender as TextBox;
            SetLimitVisibility(box, Visibility.Visible);
        }

        private void Textbox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox box = sender as TextBox;
            SetLimitVisibility(box, Visibility.Hidden);
        }

        private void SetLimitVisibility(TextBox box, Visibility vis)
        {
            switch (box.Name)
            {
                case "Text1":
                    LimitCustomText1.Visibility = vis;
                    break;
                case "Text2":
                    LimitCustomText2.Visibility = vis;
                    break;
                case "TextLargeKey":
                    LimitCustomLargeKey.Visibility = vis;
                    break;
                case "TextLargeText":
                    LimitCustomLargeText.Visibility = vis;
                    break;
                case "TextSmallKey":
                    LimitCustomSmallKey.Visibility = vis;
                    break;
                case "TextSmallText":
                    LimitCustomSmallText.Visibility = vis;
                    break;
            }
        }
        #endregion

        public Button GetButton(CustomProfile p)
        {
            return new Button
            {
                Name = p.Name,
                Content = p.Name,
                Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255)),
                BorderBrush = new SolidColorBrush(Color.FromRgb(96, 96, 96)),
                Background = new SolidColorBrush(Color.FromRgb(96, 96, 96)),
                Padding = new Thickness(10, 1, 10, 1),
                Margin = new Thickness(5, 0, 5, 0),
                Height = 20
            };
        }

        private void HelpButton_Click(object sender, MouseButtonEventArgs e)
        {
            Image img = sender as Image;
            if (img.Opacity == 1)
            {
                img.Opacity = 0.7;
                ImageHelp.Source = null;
                return;
            }
            HelpClientID.Opacity = 0.7;
            HelpText1.Opacity = 0.7;
            HelpText2.Opacity = 0.7;
            HelpLargeKey.Opacity = 0.7;
            HelpLargeText.Opacity = 0.7;
            HelpSmallKey.Opacity = 0.7;
            HelpSmallText.Opacity = 0.7;
            img.Opacity = 1;
            Uri Url = null;
            switch (img.Name)
            {
                case "HelpClientID":
                    Url = new Uri("https://i.imgur.com/QFO9nnY.png");
                    break;
                case "HelpText1":
                    Url = new Uri("https://i.imgur.com/WF0sOBx.png");
                    break;
                case "HelpText2":
                    Url = new Uri("https://i.imgur.com/loGpAh7.png");
                    break;
                case "HelpLargeKey":
                    Url = new Uri("https://i.imgur.com/UzHaAgw.png");
                    break;
                case "HelpLargeText":
                    Url = new Uri("https://i.imgur.com/CH9JmHG.png");
                    break;
                case "HelpSmallKey":
                    Url = new Uri("https://i.imgur.com/EoyRYhC.png");
                    break;
                case "HelpSmallText":
                    Url = new Uri("https://i.imgur.com/9CkGNiB.png");
                    break;
            }
            if (Url != null)
            {
                ImageSource imgSource = new BitmapImage(Url);
                ImageHelp.Source = imgSource;
            }
        }

        public void EnableHelpIcons()
        {
            HelpClientID.Visibility = Visibility.Visible;
            HelpText1.Visibility = Visibility.Visible;
            HelpText2.Visibility = Visibility.Visible;
            HelpLargeKey.Visibility = Visibility.Visible;
            HelpLargeText.Visibility = Visibility.Visible;
            HelpSmallKey.Visibility = Visibility.Visible;
            HelpSmallText.Visibility = Visibility.Visible;
        }

        public void DisableHelpIcons()
        {
            HelpClientID.Visibility = Visibility.Hidden;
            HelpText1.Visibility = Visibility.Hidden;
            HelpText2.Visibility = Visibility.Hidden;
            HelpLargeKey.Visibility = Visibility.Hidden;
            HelpLargeText.Visibility = Visibility.Hidden;
            HelpSmallKey.Visibility = Visibility.Hidden;
            HelpSmallText.Visibility = Visibility.Hidden;
        }
    }
}