using System.Windows;
using System.Windows.Controls;
using Xceed.Wpf.Toolkit;
using System.Linq; //holy shit
using System.Windows.Media;

namespace TSSWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    
    public partial class MainWindow : Window
    {
        bool debug = true;
        TacoDBEntity db = new TacoDBEntity();
        //Global test = new Global(); //global db access how?  vs. passing the ref through each window

        public MainWindow()
        {
            debugCheck(debug);
            //debug1();
            InitializeComponent();
        }

        private void login_button_Click(object sender, RoutedEventArgs e)
        {
            string user = loginUsernameBox.Text;
            string password = loginPasswordBox.Password; //fix this in the future, use hash instead of checking password.
            var result = db.login.SingleOrDefault(i => i.username == user && i.password == password);
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
                    Window adminWin = new AdminWindow(db, this);
                    adminWin.Show();
                } else
                {
                    //get user data, load user data, show window with user data.
                    this.Hide();
                    Window userWin = new UserWindow(db, result.id, this);
                    userWin.Show();
                }
                clearFields();
            }
        }
        private void debugCheck(bool debug)
        {
            if (debug)
            {
                System.Windows.MessageBox.Show("Program is in debug mode");
            } 
        }

        private void login_signupButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            Window signUpWin = new NewUser(db, this);
            signUpWin.Show();
            
        }
        private void clearFields()
        {
            loginUsernameBox.Clear();
            loginPasswordBox.Clear();
        }
        private void debug1()
        {
            shopStock example = (from u in db.shopStock where u.owner_id == 2 select u).FirstOrDefault();
            shopStock test = new shopStock();
            test.owner_id = 2;
            test.ingredient = "hard_tortilla";
            test.stock = 333;
            //test.ingredients = (from u in db.ingredients where u.ingredient == "hard_tortilla" select u).FirstOrDefault();
            //test.userData = (from u in db.userData where u.id == 2 select u).FirstOrDefault();
            //test.ingredients = new ingredients();
            //test.userData = new userData();
            db.shopStock.Add(test);
            db.SaveChanges();
        }

        private void exitTSS(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        private void registerClick(object sender, RoutedEventArgs e)
        {
            //slide up text boxes
            //unhide verify password box
            //change login in button to sign up button.
            //change register to cancel.

            //registerButton.Margin = new Thickness(0, 10, 0, 0);
            //registerButton.RenderTransform = new TranslateTransform(0, 10);
            //var x = registerButton.RenderTransform;
            //var y = registerButton.Margin;
            //if (registerButton.Margin == new Thickness(0,0,0,0))
            //{
            //    registerButton.Margin = new Thickness(0, 10, 0, 0);
            //} else
            //{
            //    registerButton.Margin = new Thickness(0, 0, 0, 0);
            //}

            //if(buttonStackPanel.Margin.Top == 15)
            //{
            //    buttonStackPanel.Margin = new Thickness(Top = 50);
            //} else
            //{
            //    buttonStackPanel.Margin = new Thickness(Top = 15);
            //}
            //if(registerButton.Margin.Top == 10)
            //{
            //    registerButton.Margin = new Thickness(Top = 30);
            //} else
            //{
            //    registerButton.Margin = new Thickness(Top = 10);
            //}
            Window signUpWin = new NewUser(db, this);
            signUpWin.Show();

        }
    }
}
