using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
        Shop shop;
        public UserWindow(TacoDBEntity db, int id, MainWindow win)
        {
            prevWin = win;
            var test = db.userDatas.SingleOrDefault(i => i.id == id);
            this.db = db;
            user = GetUser(id);
            //somehow load user data from userID.
            shop = GetShop(user);
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
                throw new Exception("userdata not found, init user in userdata.");
            }
            User user = new User();
            user.id = userQ.id;
            user.username = userQ.username;
            user.employees = userQ.employees;
            user.money = userQ.money;
            return user;
        }
        private Shop GetShop(User user)
        {
            var shopQ = db.shopStocks.SingleOrDefault(i => i.owner_id == user.id);
            var columns = shopQ.GetType().GetFields();
            if (shopQ == null)
            {
                throw new NotImplementedException("user does not have shop, create shop.");
            }
            var shop = new Shop();
            shop.Owner = user;

            var items = db.shopStocks.Select(i => i.owner_id == user.id).ToList();
            
            foreach(var item in columns)
            {
                //db.shopStocks.Select
                //shop.stock.Add(item, db.shopStocks.Select())
            }

            //foreach(var property in shopQ.GetType().GetProperties())
            //{
            //    //var test = shopQ.GetType().GetProperty("seasoned beef");
            //    var test2 = test.GetValue(shopQ);
            //    var blah = property.GetValue(shopQ);
            //    //shop.stock.Add(property, shopQ.property.get);
            //}
            String[] items2 = new string[] { "cheese", "lettuce", "seasoned beef" };
            List<string> list = (from a in db.ingredients select a.ingredient1).ToList();

            foreach (string s in list)
            {
                var prop = shopQ.GetType().GetProperty(s);
                if (prop == null)
                {
                    continue;
                }
                var ing = db.ingredients.Single(i => i.ingredient1 == s);
                var count = prop.GetValue(shopQ);
                shop.stock.Add(ing, (int)count);


            }
            
            return shop;
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
