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
            var test = db.userData.SingleOrDefault(i => i.id == id);
            this.db = db;
            user = GetUser(id);
            shop = GetShop(user);
            InitializeComponent();
            userLabel.Content = user.username;

            //show shop name
            //show ingridents
            //show recipes
            populateData(shop);

            this.Closed += new EventHandler(UserWindowClose);
            this.Closing += new CancelEventHandler(UserWindowConfirmClose);
        }

        private User GetUser(int userID) //init User class using userData from table.
        {
            var userQ = db.userData.SingleOrDefault(i => i.id == userID);
            if (userQ == null)
            {
                throw new Exception("userdata not found, init user in userdata.");
            }
            User user = new User();
            user.id = userQ.id;
            user.username = userQ.username;
            user.employees = userQ.employees;
            user.money = userQ.money;
            user.recipes = userQ.learnedRecipes;
            user.shopName = userQ.shopName;
            return user;
        }
        private void populateData(Shop shop)
        {
            //shopQ is bootleg, delete shopStock table and just add it to userData?
            currentStockGrid.ItemsSource = shop.StockPrint();
            knownRecipesGrid.ItemsSource = shop.recipes;
            var x = (from a in db.ingredients select a.ingredient).ToList();
            placeOrderGrid.ItemsSource = x; //WHYYYY
        }
        private Shop GetShop(User user)
        {
            //what if its a new user?  what would the be missing?
            //Shop Name
            //stock would be empty, thats ok.
            //employees = 0 for new user.
            //recipes would also be empty.  ok, no problem.
            var shop = new Shop();
            shop.Owner = user;
            shop.ShopName = user.shopName;
            //var prop = shopQ.GetType().GetProperty(ingr);
            //if (prop == null)
            //{
            //    continue;
            //}
            //var ing = db.ingredients.Single(i => i.ingredient == ingr);
            //var count = prop.GetValue(shopQ);
            // good exercise on reflection.

            //populates shop ingredients and count from dbo.
            if (db.shopStock.Select(i => i.owner_id == user.id) == null)
            {
                //do nothing.
            }
            else //add ingredient stock
            {
                List<string> ingrList = (from a in db.ingredients select a.ingredient).ToList();
                foreach (string ingr in ingrList) //iterates through all ingredients from db.
                {
                    var instock = db.shopStock.SingleOrDefault(i => i.owner_id == user.id && i.ingredient == ingr); //gets shops ing, stock query
                    var ing = db.ingredients.Single(i => i.ingredient == ingr);

                    if (instock == null)
                    {
                        shop.stock.Add(ing, 0);
                    }
                    else
                    {
                        shop.stock.Add(ing, instock.stock);
                    }
                }
            }

            //shop.employees = user.employees; //List<Employee>, employees not implemented yet.  maybe another time.
            shop.employees = user.employees;

            //populates recipes from userData recipe column.
            if (user.recipes.Length == 0)
            {
                //do nothing
            }
            else //add recipes
            {
                List<string> knownRecipes = user.recipes.Split(',').ToList();
                foreach (string recipe in knownRecipes)
                {
                    var rec = db.recipes.Single(i => i.name == recipe);
                    if (rec == null)
                    {
                        throw new Exception("recipe is missing from recipe db. bad name?");
                    }
                    shop.recipes.Add(rec);
                }
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
