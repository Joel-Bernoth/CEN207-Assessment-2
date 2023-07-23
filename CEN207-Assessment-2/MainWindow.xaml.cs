using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using CEN207_Assessment_2.Structures;
using RabbitMQ.Client;

namespace CEN207_Assessment_2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool isLoggedIn = false;

        public bool IsLoggedIn
        {
            get { return isLoggedIn; }
            set { isLoggedIn = value; }
        }

        public MainWindow()
        {

            InitializeComponent(); // Starts the Main Window

            if (CheckRabbitMQStatus())
            {
                // Checks the
                this.MainFrame.Content = new Pages.Feed();


                ConnectionFactory factory = new ConnectionFactory { HostName = "localhost", Port = 5672 };
                IConnection conn = factory.CreateConnection();
                IModel channel = conn.CreateModel();

                Dictionary<string, object> args = new Dictionary<string, object>()
                {
                    {"x-queue-type", "stream" }
                };

                this.feed_btn.Click += new RoutedEventHandler(feedbtn_Click);
                this.login_btn.Click += new RoutedEventHandler(loginbtn_Click);
                this.chat_btn.Click += new RoutedEventHandler(chatbtn_Click);
                this.admin_btn.Click += new RoutedEventHandler(adminbtn_Click);
            }

        }


        private void feedbtn_Click(object sender, RoutedEventArgs e)
        {
            this.MainFrame.Content = new Pages.Feed();
        }

        private void loginbtn_Click(object sender, RoutedEventArgs e)
        {
            this.MainFrame.Content = new Pages.LoginPage();
        }

        private void chatbtn_Click(object sender, RoutedEventArgs e)
        {
            this.MainFrame.Content = new Pages.Chats();
        }

        private void adminbtn_Click(object sender, RoutedEventArgs e)
        {
            this.MainFrame.Content = new Pages.Admin();
        }

        // Check To see if the Server is up, If it is return 
        private bool CheckRabbitMQStatus()
        {
            ConnectionFactory factory = new ConnectionFactory { HostName = "localhost", Port = 5672 };
            IConnection conn = null;

            try
            {
                conn = factory.CreateConnection();
                conn.Close();
                conn.Dispose();
            }
            catch (Exception ex)
            {
                if (ex.Message == "None of the specified endpoints were reachable")
                {
                    this.MainFrame.Content = new Pages.Error("RabbitMQ Is Down");
                    return false;
                }
            }
            return true;
        }
    }
}
