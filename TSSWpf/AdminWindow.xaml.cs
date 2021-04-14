using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
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
    /// Interaction logic for AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        MainWindow prevWindow;
        TacoDBEntity db;
        public AdminWindow(TacoDBEntity database, MainWindow window)
        {
            InitializeComponent();
            db = database;
            prevWindow = window;
            this.Closed += new EventHandler(AdminWindowClose);

            //adminUserGrid.DataContext = db.userDatas.Local;
            //DataTable blah = db.userDatas.Fill
            //adminUssdsderGrid.DataContext = db.userDatas.SingleOrDefault(i => i.id == 3);
            //var blah = db.userDatas.OrderBy(a => a.id);
            //var blah = db.userDatas.Sql;
            var blah = db.userData.SingleOrDefault(i => i.id == 3); //fetches row
            var blah2 = from item in  db.userData select item;
            var query = db.userData.Where(x => true).ToList();

            adminUserGrid.ItemsSource = query.ToList();
            adminIngrGrid.ItemsSource = db.ingredients.Where(x => true).ToList();
            adminRecpGrid.ItemsSource = db.recipes.Where(x => true).ToList();
           
            
        }
        void AdminWindowClose(object sender, EventArgs e)
        {
            //System.Windows.MessageBox.Show("Closing window");
            prevWindow.Show();
        }

        private void adminAddIngrButton_Click(object sender, RoutedEventArgs e)
        {
            string ingr = AddIngredientBox.Text;
            string desc = AddDescrIngrBox.Text;
            decimal cost = Decimal.Parse(AddCostIngrBox.Text);
            var q = new ingredients();
            q.ingredient = ingr; q.description = desc; q.cost = cost;
            db.ingredients.Add(q);
            db.SaveChanges();
        }
    }
}
