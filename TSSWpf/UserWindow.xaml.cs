﻿using System;
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
            userLabel.Content = user.username; 
            initData(shop); //init UserWindow using shop.

            this.Closed += new EventHandler(UserWindowClose);
            this.Closing += new CancelEventHandler(UserWindowConfirmClose);
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
            placeOrderGrid.ItemsSource = allIngr; //WHY

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

        private void updateCost(object sender, RoutedEventArgs e)
        {
            //method that should run whenever buy menu is updated with user input
            //should display what the cost is.
            //cost is calculated by query cost of ingredient from table
            //then doing count * cost, then sum them all up to return cost.
        }
        private void buyClick(object sender, RoutedEventArgs e)
        {
            //sub cost from money on hand
            //update user invetory with selected items
            //clear buy menu.
        }
        private void getIngrAndRecipes()
        {
            allIngr = (from a in db.ingredients select a.ingredient).ToList(); // makes list of all ingredients
            allRecipes = (from a in db.recipes select a.name).ToList(); // makes list of all recipes
        }
        void UserWindowClose(object sender, EventArgs e) //shows previous window when userWin closes.
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
//var prop = shopQ.GetType().GetProperty(ingr);
//if (prop == null)
//{
//    continue;
//}
//var ing = db.ingredients.Single(i => i.ingredient == ingr);
//var count = prop.GetValue(shopQ);
// good exercise on reflection.