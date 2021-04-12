using System.Windows;
using System.Windows.Controls;
using Xceed.Wpf.Toolkit;

namespace TSSWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    
    public partial class MainWindow : Window
    {
        bool debug = true;
        public MainWindow()
        {
            debugCheck(debug);
            InitializeComponent();
        }

        private void login_button_Click(object sender, RoutedEventArgs e)
        {
            string user = loginUsernameBox.Text;
            string password = loginPasswordBox.Password;

            if (user == "user" && password == "password")
            {
                System.Windows.MessageBox.Show("Correct");
                this.Hide();
                Window w = new AdminWindow();
                w.Show();

            } else
            {
                System.Windows.MessageBox.Show("Incorrect username or password");
                //add to log?
                if (debug)
                {
                    System.Windows.MessageBox.Show(user + "; " + password);
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
