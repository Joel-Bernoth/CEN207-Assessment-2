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
using System.Collections.Immutable;
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
        private string body = "";
        private string title = "";

        public Feed()
        {
            InitializeComponent();

            ObservableCollection<FeedPost_Struct> posts = new ObservableCollection<FeedPost_Struct>();
            this.FeedList.ItemsSource = posts;

            ConnectionFactory factory = new ConnectionFactory { HostName = "localhost", Port = 5672 };
            IConnection connection = factory.CreateConnection();
            IModel channel = connection.CreateModel();
            channel.BasicQos(0, 100, false);


            void RecieveFeed(byte[] bytes)
            {
                FeedPost_Struct feedPost = new FeedPost_Struct();
                feedPost = FeedPost.DeSerialize(bytes);

                this.Dispatcher.Invoke(() =>
                {
                    posts.Add(new FeedPost_Struct() { UserName = feedPost.UserName, Name = feedPost.Name, Title = feedPost.Title, Body = feedPost.Body });
                });

            }

            Dictionary<string, object> offset = new Dictionary<string, object>()
            {
                {"x-stream-offset", "first" }
            };


            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                RecieveFeed(body);
            };

            channel.BasicConsume(
                queue: "Feed-Stream",
                autoAck: false,
                arguments: offset,
                consumer: consumer
                );;
        }

        private void BodyTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            body = this.BodyTB.Text;
        }


        private void TitleTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            title = this.TitleTB.Text;
        }


        private void PostBtn_Click(object sender, RoutedEventArgs e)
        {
            ConnectionFactory factory = new ConnectionFactory { HostName = "localhost", Port = 5672 };
            IConnection connection = factory.CreateConnection();
            IModel channel = connection.CreateModel();
            Dictionary<string, object> args = new Dictionary<string, object>()
            {
                {"x-queue-type", "stream" }
            };

            FeedPost_Struct post = new FeedPost_Struct();
            post.Name = App.getDisplayName();
            post.UserName = "JBernoth";
            post.Title = this.title;
            post.Body = this.body;

            byte[] body = FeedPost.Serialize(post);

            channel.QueueDeclare(
                 queue: "Feed-Stream",
                 durable: true,
                 autoDelete: false,
                 exclusive: false,
                 arguments: args
             );

            channel.BasicPublish(
                exchange: string.Empty,
                routingKey: "Feed-Stream",
                basicProperties: null,
                body: body);

        }
    }
}
