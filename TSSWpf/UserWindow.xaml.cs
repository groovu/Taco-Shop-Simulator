using System;
using System.Collections;
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
        List<string> knownRecipes;
        List<string> allRecipes;
        List<string> allIngr;
        //Utils u = Utils;
        public UserWindow(TacoDBEntity db, int id, MainWindow win)
        {

            prevWin = win; this.db = db; //sets previousWindow and db for use.
            getIngrAndRecipes(); //populates all ingredients and recipes from db.
            user = GetUser(id); //init user class
            shop = GetShop(user); //init shop from user

            InitializeComponent(); //default thingo
            newUserCheck();

            initData(shop); //init UserWindow using shop.

            hideAllGroups();
            setUpLabels();
            homeGroup.Visibility = Visibility.Visible;

            this.Closed += new EventHandler(UserWindowClose);
            this.Closing += new CancelEventHandler(UserWindowConfirmClose);
        }
        private void setUpLabels()
        {
            shopNameLabel.Content = shop.ShopName;
            userLabel.Content = user.username;
            moneyLabel.Content = user.money;
            titleBarLabel.Content = user.shopName;
        }
        private void newUserCheck()
        {
            if (shop.ShopName == null)
            {
                //MessageBox.Show("Welcome to Taco Shop Simulator.  What is your shop's name?");
                nameNewShopBox.Visibility = System.Windows.Visibility.Visible;
            }
        }
        public void nameNewShop(object sender, RoutedEventArgs e)
        {
            nameNewShopBox.Visibility = System.Windows.Visibility.Collapsed;
            shop.ShopName = InputTextBox.Text;
            shopNameLabel.Content = shop.ShopName;
        }

        private User GetUser(int userID) //init User class using userData from table.
        {
            var userQ = db.userData.SingleOrDefault(i => i.id == userID); //checking if user exist.
            if (userQ == null)
            {
                throw new Exception("userdata not found, init user in userdata.");
            }
            User user = new User(); //converting user query into user object.
            user.id = userQ.id;
            user.username = userQ.username;
            user.employees = userQ.employees;
            user.money = userQ.money;
            user.recipes = userQ.learnedRecipes;
            user.shopName = userQ.shopName;
            knownRecipes = user.recipes.Split(',').ToList(); //adds learned recipes to shops recipes.
            return user;
        }

        private Shop GetShop(User user)
        {
            var shop = new Shop();
            shop.Owner = user;
            shop.ShopName = user.shopName;
            knownRecipes = user.recipes.Split(',').ToList();

            //populates shop ingredients and count from dbo.
            if (db.shopStock.Select(i => i.owner_id == user.id) != null)
            {
                foreach (string ingr in allIngr) //iterates through all ingredients from db.
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
            } //else don't fetch stock.
            
            //populates recipes from userData recipe column.
            if (user.recipes.Length != 0)
            {
                foreach (string recipe in knownRecipes)
                {
                    var rec = db.recipes.Single(i => i.name == recipe);
                    if (rec == null)
                    {
                        throw new Exception("recipe is missing from recipe db. bad name?");
                    }
                    shop.recipes.Add(rec);
                }
            } //else don't fetch recipes.
            shop.employees = user.employees; //employee class not impleted yet, just using num for now.
            return shop;
        }
        private void initData(Shop shop)
        {
            currentStockGrid.ItemsSource = shop.StockPrint(); //show current stock.
            placeOrderGrid.ItemsSource = placeOrderBuilder();
            //var x = ObservableCollection();
            //placeOrderGrid.Columns[1].IsReadOnly = false;
            //placeOrderGrid.header
            //placeOrderGrid.Columns[0].Header = "Ingredient";
            //placeOrderGrid.Columns[1].Header = "Qty";
            //placeOrderGrid.Columns[1].IsReadOnly = false;

            knownRecipesGrid.ItemsSource = shop.recipes; //shows know recipes
            //missing research grid
            
        }

        private void researchClick(object sender, RoutedEventArgs e)
        {
            //checkListBox.ItemsSource = ingrList;
            //IList test = checkListBox.SelectedItems;
            //have to make ingredients in recipe in alphabetical order
            //if selected items == ingredients for recipe, success
                //add recipe to user and update userdata to contain recipe.
                //display recipe and details in resarch
                //update resarch page.
            //else display not successful.
        }

        private void updateCost(object sender, EventArgs e)
        {
            //method that should run whenever buy menu is updated with user input
            //should display what the cost is.
            //cost is calculated by query cost of ingredient from table
            //then doing count * cost, then sum them all up to return cost.
            //placeOrderGrid.ItemsSource not yet updated?
            List<buyItem> list = (List<buyItem>)placeOrderGrid.ItemsSource;
            decimal cost = 0;
            foreach(buyItem i in list)
            {
                string ing = i.Ingredient;
                int qty = i.Qty;
                var q = db.ingredients.SingleOrDefault(x => x.ingredient == ing);
                if (q == null)
                {
                    throw new Exception("Error in update cost");
                }
                cost += (decimal)q.cost * qty;
            }
            costLabel.Content = cost;
        }
        private void buyClick(object sender, RoutedEventArgs e)
        {
            if(user.money < (decimal)costLabel.Content)
            {
                MessageBox.Show("You do not have enough money");
                return;
            } else
            {
                shop.updateStock((List<buyItem>)placeOrderGrid.ItemsSource, db);
                user.money -= (decimal)costLabel.Content;
                moneyLabel.Content = user.money;
                currentStockGrid.ItemsSource = shop.StockPrint();
                placeOrderGrid.ItemsSource = placeOrderBuilder();
            }

        }
        private List<buyItem> placeOrderBuilder()
        {

            List<buyItem> menu = new List<buyItem>();
                foreach(string i in allIngr)
            {
                menu.Add(new buyItem { Ingredient = i, Qty = 0 });
            }
            
            return menu;
        }
        public class buyItem
        {
            public string Ingredient { get; set; }
            public int Qty { get; set; }
            public buyItem()
            {

            }
        }

        
        private void getIngrAndRecipes()
        {
            allIngr = (from a in db.ingredients select a.ingredient).ToList(); // makes list of all ingredients
            allRecipes = (from a in db.recipes select a.name).ToList(); // makes list of all recipes
        }
        void UserWindowClose(object sender, EventArgs e) //shows previous window when userWin closes.
        {
            //backup userdata to db.
            //update shopstock
            //update userData
            var q = (from u in db.userData where u.id == user.id select u).SingleOrDefault();
            if (q == null)
            {
                throw new Exception("user is missing from userData.");
            }
            q.money = user.money;
            q.shopName = shop.ShopName;
            foreach(KeyValuePair<string, int> ingr in shop.StockPrint())
            {
                var stockQ = (from u in db.shopStock where u.owner_id == user.id && u.ingredient == ingr.Key select u).SingleOrDefault();
                if (stockQ == null)
                {
                    shopStock newItem = new shopStock();
                    newItem.owner_id = user.id;
                    newItem.ingredient = ingr.Key;
                    newItem.stock = ingr.Value;
                    //newItem.ingredients = (from u in db.ingredients where u.ingredient == ingr.Key select u).FirstOrDefault();
                    //newItem.userData = (from u in db.userData where u.id == user.id select u).FirstOrDefault();
                    shopStock itemTest = (from u in db.shopStock where u.ingredient == "cheese" select u).FirstOrDefault();

                    db.shopStock.Add(newItem);
                    //throw new Exception("error fetching shopStock query.");
                }
                else
                {
                    stockQ.stock = ingr.Value;
                }
            }
            db.SaveChanges();
            prevWin.Show();

        }
        void UserWindowConfirmClose(object sender, CancelEventArgs e) //prompts user to ensure they want to quit.
        {
            if (MessageBox.Show("Do you want to exit?", "Confirm", MessageBoxButton.YesNo) == MessageBoxResult.No)
            {
                e.Cancel = true;
            }
        }

        private void placeOrderGrid_CurrentCellChanged(object sender, EventArgs e)
        {
            MessageBox.Show("changed");
        }

        private void placeOrderGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            //update placeOrder source?
            placeOrderGrid.ItemsSource = placeOrderGrid.ItemsSource;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            user.money += 500;
            moneyLabel.Content = user.money;
        }

        private void userWindowClose(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void communityClick(object sender, RoutedEventArgs e)
        {
            if (messageButton.Visibility == Visibility.Visible)
            {
                messageButton.Visibility = Visibility.Hidden;
                leaderboardButton.Visibility = Visibility.Hidden;
                return;
            }
            messageButton.Visibility = Visibility.Visible;
            leaderboardButton.Visibility = Visibility.Visible;
        }
        private void messageClick(object sender, RoutedEventArgs e)
        {
            messageButton.Visibility = Visibility.Collapsed;
            leaderboardButton.Visibility = Visibility.Collapsed;
            Messenger MsgWin = new Messenger(db, user.id);
            MsgWin.Show();
            //show message window or w/e.
        }

        private void showHome(object sender, RoutedEventArgs e)
        {
            //return home;
            hideAllGroups();
            homeGroup.Visibility = Visibility.Visible;
        }

        private void showBuy(object sender, RoutedEventArgs e)
        {
            if (buyEquipmentButton.Visibility == Visibility.Visible)
            {
                buyIngredientsButton.Visibility = Visibility.Hidden;
                buyEquipmentButton.Visibility = Visibility.Hidden;
                return;
            }
            buyIngredientsButton.Visibility = Visibility.Visible;
            buyEquipmentButton.Visibility = Visibility.Visible;
        }

        private void showResearch(object sender, RoutedEventArgs e)
        {
            if (researchRecipesButton.Visibility == Visibility.Visible)
            {
                researchRecipesButton.Visibility = Visibility.Hidden;
                researchTechniquesButton.Visibility = Visibility.Hidden;
                return;
            }
            researchRecipesButton.Visibility = Visibility.Visible;
            researchTechniquesButton.Visibility = Visibility.Visible;
        }

        private void showEmployee(object sender, RoutedEventArgs e)
        {
            if (hireButton.Visibility == Visibility.Visible)
            {
                hireButton.Visibility = Visibility.Hidden;
                viewEmpButton.Visibility = Visibility.Hidden;
                return;
            }
            hireButton.Visibility = Visibility.Visible;
            viewEmpButton.Visibility = Visibility.Visible;
        }

        private void showUpgrades(object sender, RoutedEventArgs e)
        {
            if (adsButton.Visibility == Visibility.Visible)
            {
                adsButton.Visibility = Visibility.Hidden;
                locationButton.Visibility = Visibility.Hidden;
                return;
            }
            adsButton.Visibility = Visibility.Visible;
            locationButton.Visibility = Visibility.Visible;
        }
        private void hideAllGroups()
        {
            var v = Visibility.Hidden;
            buyIngredientsGroup.Visibility = v;
            buyEquipmentGroup.Visibility = v;
            homeGroup.Visibility = v;
            researchRecipeGroup.Visibility = v;
            researchTecniquesGroup.Visibility = v;
        }

        private void buyIngredientsButton_Click(object sender, RoutedEventArgs e)
        {
            hideAllGroups();
            buyIngredientsGroup.Visibility = Visibility.Visible;
            showBuy(sender, e);
        }

        private void buyEquipmentButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void researchRecipesButton_Click(object sender, RoutedEventArgs e)
        {
            hideAllGroups();
            researchRecipeGroup.Visibility = Visibility.Visible;
            showResearch(sender, e);
        }

        private void researchTechniquesButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void hireButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void viewEmpButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void locationButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void adsButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void leaderboardButton_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}
//var prop = shopQ.GetType().GetProperty(ingr);
//if (prop == null)
//{
//    continue;
//}
//var ing = db.ingredients.Single(i => i.ingredient == ingr);
//var count = prop.GetValue(shopQ);
// good exercise on reflection.