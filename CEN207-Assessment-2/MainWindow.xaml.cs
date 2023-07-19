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

            CheckRabbitMQStatus();
            // Checks the
            this.MainFrame.Content = new Pages.Feed();
            

            ConnectionFactory factory = new ConnectionFactory { HostName = "localhost", Port = 5672 };
            IConnection conn = factory.CreateConnection();
            IModel channel = conn.CreateModel();


            Dictionary<string, object> args = new Dictionary<string, object>()
            {
                {"x-queue-type", "stream" }
            };

        }

        private void CheckRabbitMQStatus()
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
                    Console.WriteLine("RabbitMQ Down");
                    Debug.WriteLine("RabbitMQ is Down");
                    this.MainFrame.Content = new Pages.Error("RabbitMQ Is Down");
                }
            }
        }
    }
}
