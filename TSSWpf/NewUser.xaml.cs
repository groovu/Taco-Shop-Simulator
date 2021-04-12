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
using System.Windows.Shapes;

namespace TSSWpf
{
    /// <summary>
    /// Interaction logic for NewUser.xaml
    /// </summary>
    public partial class NewUser : Window
    {
        TacoDBEntity db;
        MainWindow loginWin;
        public NewUser(TacoDBEntity database, MainWindow login)
        {
            db = database;
            loginWin = login;
            InitializeComponent();
        }

        private void cancelSignup(object sender, RoutedEventArgs e)
        {
            loginWin.Show();
            this.Close();
        }

        private void signUpClick(object sender, RoutedEventArgs e)
        {
            string username = signupUsernameBox.Text;
            string password = signupPasswordBox.Password;
            string confirm = signupPasswordConfirmBox.Password;
            if (password != confirm)
            {
                System.Windows.MessageBox.Show("Passwords do not match.");
            }
            var result = db.logins.SingleOrDefault(i => i.username == username);
            if (result != null)
            {
                System.Windows.MessageBox.Show("Username has already been selected.");
            } else
            {
                login newUser = new login();
                newUser.id = db.logins.Max(i => i.id) + 1;
                newUser.username = username;
                newUser.password = password;
                newUser.admin = false;
                db.logins.Add(newUser);
                db.SaveChanges();
                System.Windows.MessageBox.Show("Sign up sucessful, returning to log in screen.");
                loginWin.Show();
                this.Close();

            }


        }
    }
}
