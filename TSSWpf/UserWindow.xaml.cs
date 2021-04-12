using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        User user;
        TacoDBEntity db;
        MainWindow prevWin;
        public UserWindow(TacoDBEntity db, int id, MainWindow win)
        {
            prevWin = win;
            var test = db.userDatas.SingleOrDefault(i => i.id == id);
            this.db = db;
            user = GetUser(id);
            //somehow load user data from userID.
            InitializeComponent();
            userLabel.Content = user.username;
            this.Closed += new EventHandler(UserWindowClose);
            this.Closing += new CancelEventHandler(UserWindowConfirmClose);
        }

        private User GetUser(int userID) //init User class using userData from table.
        {
            var userQ = db.userDatas.SingleOrDefault(i => i.id == userID);
            if (userQ == null)
            {
                throw new Exception("Could not find user in UserWindow");
            }
            User user = new User();
            user.id = userQ.id;
            user.username = userQ.username;
            user.employees = userQ.employees;
            user.money = userQ.money;
            return user;
        }

        void UserWindowClose(object sender, EventArgs e)
        {
            //backup userdata to db.
            prevWin.Show();
            
        }
        void UserWindowConfirmClose(object sender, CancelEventArgs e) //prompts user to ensure they want to quit.
        {
            if (MessageBox.Show("Do you want to exit?", "Confirm", MessageBoxButton.YesNo) == MessageBoxResult.No)
            {
                e.Cancel = true;
            }
        }
    }
}
