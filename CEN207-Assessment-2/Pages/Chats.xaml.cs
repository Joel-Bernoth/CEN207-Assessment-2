using CEN207_Assessment_2.Pages.Sub;
using CEN207_Assessment_2.Structures;
using RabbitMQ.Client;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CEN207_Assessment_2.Pages
{



    public partial class Chats : Page
    {
        private string body = "";
        private string queueName = "";

        private Chat chat = new Chat("x");

        public Chats()
        {
            InitializeComponent();
        }


        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBoxItem? lbi = ChatList.SelectedItem as ListBoxItem;

            if (lbi != null)
            {
                chat = new Sub.Chat(lbi.Content.ToString());
                queueName = lbi.Content.ToString() + "-to";
                chatFrame.Content = chat;
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.body = this.messageTXT.Text;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ConnectionFactory factory = new ConnectionFactory { HostName = "localhost", Port = 5672 };
            IConnection connection = factory.CreateConnection();
            IModel channel = connection.CreateModel();
            Dictionary<string, object> args = new Dictionary<string, object>()
            {
                {"x-queue-type", "stream" }
            };

            ChatPost_Struct post = new ChatPost_Struct();
            post.Name = App.getDisplayName();
            post.Body = this.body;

            byte[] body = ChatPost.Serialize(post);
            

            channel.QueueDeclare(
                 queue: queueName,
                 durable: true,
                 autoDelete: false,
                 exclusive: false,
                 arguments: args
             );

            channel.BasicPublish(
                exchange: string.Empty,
                routingKey: queueName,
                basicProperties: null,
                body: body);
        }
    }
}
