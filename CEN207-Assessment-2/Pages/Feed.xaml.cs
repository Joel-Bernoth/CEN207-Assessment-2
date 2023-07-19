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
using CEN207_Assessment_2.Structures;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CEN207_Assessment_2.Pages
{
    /// <summary>
    /// Interaction logic for Feed.xaml
    /// </summary>
    public partial class Feed : Page
    {
        public Feed()
        {
            InitializeComponent();

            ObservableCollection<FeedPost> posts = new ObservableCollection<FeedPost>();
            this.FeedList.ItemsSource = posts;

            ConnectionFactory factory = new ConnectionFactory { HostName = "localhost", Port = 5672 };
            IConnection connection = factory.CreateConnection();
            IModel channel = connection.CreateModel();
            channel.BasicQos(0, 10, false);


            void RecieveFeed(byte[] bytes)
            {
                this.Dispatcher.Invoke(() =>
                {
                    posts.Add(new FeedPost() { UserName = "test", Name = "Tester", Title = "Testing", Body = "Testing system" });
                });

            }



            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                RecieveFeed(body);
            };
            channel.BasicConsume(
                queue: "Feed-Stream",
                autoAck: false,
                consumer: consumer);
        }

    }
}
