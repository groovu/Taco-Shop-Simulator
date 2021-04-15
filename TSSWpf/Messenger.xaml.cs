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
    /// Interaction logic for Messenger.xaml
    /// </summary>
    public partial class Messenger : Window
    {
        int id;
        TacoDBEntity db;
        public Messenger(TacoDBEntity database, int userID)
        {
            id = userID;
            db = database;
            InitializeComponent();
            List<int> a = new List<int>();
            for (int i = 0; i < 10; i += 1)
            {
                a.Add(i);
            }
            //msgBox.ItemsSource = a;
            var q = (from u in db.login where u.id == id select u).FirstOrDefault();
            this.Title = "Messenger " + q.username;
            populateMessageList();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button selectUser = new Button
            {
                Content = "User Name"
            };
            msgUserList.Children.Add(selectUser);
        }

        private void msgFindUser(object sender, RoutedEventArgs e)
        {
            string username = findUsernameBox.Text;
            var q = (from u in db.login where u.username == username select u).FirstOrDefault();
            if (q == null)
            {
                MessageBox.Show("User does not exist.");
                return;
            }
            userNameLabel.Content = q.username;
            refreshMessagePanel();
            populateMessageList();
            //query messages db to see if msgs already exist.
            //if true, then populate.

        }

        private void sendMessage(object sender, RoutedEventArgs e)
        {
            string recipient = (string)userNameLabel.Content;
            if (recipient == "")
            {
                MessageBox.Show("Please select a user before sending a message.");
                return;
            }
            var q = (from u in db.login where u.username == recipient select u).FirstOrDefault();
            if (q == null)
            {
                throw new Exception("User not found in db.login?");
            }
            int recID = q.id;

            messages msg = new messages();
            int msgid = db.messages.Max(i => i.msg_id) + 1;
            msg.from_id = id; msg.to_id = recID; msg.msg_id = msgid;
            msg.message = sendMessageBox.Text;
            msg.time = DateTime.Now;
            db.messages.Add(msg);
            db.SaveChanges();
            sendMessageBox.Clear();
            refreshMessagePanel();
            populateMessageList();
        }
        private void refreshMessagePanel()
        {
            string recipient = (string)userNameLabel.Content;
            if (recipient == "")
            {
                MessageBox.Show("Cannot refresh message panel for no user.");
                return;
            }
            var q = (from u in db.login where u.username == recipient select u).FirstOrDefault();
            if (q == null)
            {
                throw new Exception("User not found in db.login during refreshMessagePanel.");
            }
            messagePanel.Children.Clear();
            int recID = q.id;
            //go to db.messages, grab all rows that contain id and recID.
            //deal with multiple rows.
            var received = (from u in db.messages where u.from_id == recID && u.to_id == id select u);
            var sent = (from u in db.messages where u.from_id == id && u.to_id == recID select u);

            var coll = received.Concat(sent);
            List<messages> x = coll.OrderBy(u => u.time).ToList();

            //foreach do stuff to each message to display.
            foreach(messages m in x)
            {
                TextBlock block = new TextBlock();
                TextBlock timeStamp = new TextBlock();
                block.Text = m.message;
                block.FontSize = 16;
                block.Margin = new Thickness(5, 0, 0, 0);
                timeStamp.Text = m.time.ToShortTimeString();
                timeStamp.FontSize = 10;
                timeStamp.Margin = block.Margin;
                if (m.to_id != id)
                {
                    block.HorizontalAlignment = HorizontalAlignment.Right;
                    timeStamp.HorizontalAlignment = HorizontalAlignment.Right;
                    block.Margin = new Thickness(0, 0, 5, 0);
                    timeStamp.Margin = block.Margin;
                }

                messagePanel.Children.Add(block);
                messagePanel.Children.Add(timeStamp);
            }
        }
        private void populateMessageList()
        {
            msgUserList.Children.Clear(); //what's the best way to sort this by order of latest message?
            var q = (from u in db.messages where u.from_id == id || u.to_id == id select u); //select all messages related to user.
            List<int> ids = new List<int>();
            foreach(messages m in q)
            {
                int fromID = m.from_id;
                int toID = m.to_id;
                if (fromID != id || toID != id) //redundant w/e
                {
                    if (ids.Contains(fromID) || ids.Contains(toID)) //id already in list, skip
                    {
                        continue;
                    }
                    if (fromID == id) //user was sender.
                    {
                        var id1 = (from u in db.login where u.id == toID select u.id).FirstOrDefault() ;
                        ids.Add(id1);
                    } else //user was receive
                    {
                        var id2 = (from u in db.login where u.id == fromID select u.id).FirstOrDefault();
                        ids.Add(id2);
                    }
                }
            }
            //at this point, you should have a list of ids where user has had messages with.
            //now populate left message list.
            foreach(int i in ids)
            {
                var username = (from u in db.login where u.id == i select u.username).FirstOrDefault();
                Button msgUserButton = new Button();
                msgUserButton.Content = username;
                msgUserButton.Click += clickUserShowMessages;
                msgUserList.Children.Add(msgUserButton);
            }

        }
        // what was the point of this again?
        private void getMessages(object sender, RoutedEventArgs e, string username)
        {
            var q = (from u in db.login where u.username == username select u).FirstOrDefault();
            if (q == null)
            {
                MessageBox.Show("User does not exist.");
                return;
            }
            userNameLabel.Content = q.username;
            refreshMessagePanel();
            populateMessageList();
            //query messages db to see if msgs already exist.
            //if true, then populate.

        }

        private void clickUserShowMessages(object sender, RoutedEventArgs e)
        {
            string test = (sender as Button).Content.ToString();
            userNameLabel.Content = test;
            refreshMessagePanel();
        }
    }
}
