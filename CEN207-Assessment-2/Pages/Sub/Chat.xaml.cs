using CEN207_Assessment_2.Structures;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace CEN207_Assessment_2.Pages.Sub
{
    /// <summary>
    /// Interaction logic for Chat.xaml
    /// </summary>
    public partial class Chat : Page
    {
        public Chat(string Reciever)
        {
            InitializeComponent();
            username_txt.Content = Reciever;

            if (!Reciever.Equals("x"))
            {
                string queueName = Reciever + "-to";

                ObservableCollection<ChatPost_Struct> chats = new ObservableCollection<ChatPost_Struct>();
                this.chatlist.ItemsSource = chats;

                ConnectionFactory connectionFactory = new ConnectionFactory { HostName = "localhost", Port = 5672 };
                IConnection connection = connectionFactory.CreateConnection();
                IModel channel = connection.CreateModel();
                channel.BasicQos(0, 100, false);

                Dictionary<string, object> offset = new Dictionary<string, object>()
            {
                {"x-stream-offset", "first" }
            };

                void RecieveChat(byte[] bytes)
                {
                    ChatPost_Struct chatPost = new ChatPost_Struct();
                    chatPost = ChatPost.DeSerialize(bytes);

                    this.Dispatcher.Invoke(() =>
                    {
                        chats.Add(new ChatPost_Struct() { Name = chatPost.Name, Body = chatPost.Body });
                    });

                }

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    RecieveChat(body);
                };

                channel.BasicConsume(
                    queue: queueName,
                    autoAck: false,
                    arguments: offset,
                    consumer: consumer
                    ); ;
            }
        }
    }
}
