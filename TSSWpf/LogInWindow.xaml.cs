﻿using System.Windows;
using System.Windows.Controls;
using Xceed.Wpf.Toolkit;
using System.Linq; //holy shit

namespace TSSWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    
    public partial class MainWindow : Window
    {
        bool debug = true;
        TacoDBEntity db = new TacoDBEntity(); 

        public MainWindow()
        {
            debugCheck(debug);
            InitializeComponent();
        }

        private void login_button_Click(object sender, RoutedEventArgs e)
        {
            string user = loginUsernameBox.Text;
            string password = loginPasswordBox.Password; //fix this in the future, use hash instead of checking password.
            var result = db.logins.SingleOrDefault(i => i.username == user && i.password == password);
            if (result == null)
            {
                System.Windows.MessageBox.Show("Incorrect username or password");
                //add to log?
                if (debug)
                {
                    System.Windows.MessageBox.Show(user + "; " + password);
                }
            } else
            {
                if (result.admin)
                {
                    this.Hide();
                    Window adminWin = new AdminWindow();
                    adminWin.Show();
                } else
                {
                    //get user data, load user data, show window with user data.
                    this.Hide();
                    Window userWin = new UserWindow(result.id);
                    userWin.Show();
                }
            }
        }
        private void debugCheck(bool debug)
        {
            if (debug)
            {
                System.Windows.MessageBox.Show("Program is in debug mode");
            } 
        }
    }
}
