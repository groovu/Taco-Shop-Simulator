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
            var result = db.login.SingleOrDefault(i => i.username == username);
            if (result != null)
            {
                System.Windows.MessageBox.Show("Username has already been taken.");
            } else
            {
                int newId = db.login.Max(i => i.id) + 1;
                login newUser = new login();

                newLoginData(newUser, username, password);
                addUserData(newUser);
                // I should add error checking here.  make sure previous two succeeded o/w clear additions.
                var blah = db.ChangeTracker.Entries(); //right track, check length == 2; else iterate through entries and remove.
                db.SaveChanges();
                System.Windows.MessageBox.Show("Sign up sucessful, returning to log in screen.");
                loginWin.Show();
                this.Close();
            }


        }
        private void newLoginData(login newUser, string username, string password)
        {
            newUser.id = db.login.Max(i => i.id) + 1;
            newUser.admin = false;
            newUser.username = username;
            newUser.password = password;
            db.login.Add(newUser);
            //db.SaveChanges();
        }

        private void addUserData(login n)
        {
            //User newUser = new User(n.id, n.username); User classs should be loaded in game
            userData newUser = new userData();
            newUser.id = n.id;
            newUser.creation = DateTime.Now;
            newUser.username = n.username;
            newUser.money = 1000;
            newUser.employees = 0;
            db.userData.Add(newUser);
            //db.SaveChanges();
        }
    }
}
