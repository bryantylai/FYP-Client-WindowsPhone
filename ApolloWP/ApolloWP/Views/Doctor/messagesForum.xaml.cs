using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using ApolloWP.Data.Form;
using ApolloWP.Data.Item;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace ApolloWP.Views.Doctor
{
    public partial class messagesForum : PhoneApplicationPage
    {
        private Guid discussionId = new Guid();

        public messagesForum()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (NavigationContext.QueryString.ContainsKey("discussionId"))
            {
                string id = "";
                if (NavigationContext.QueryString.TryGetValue("discussionId", out id))
                {
                    discussionId = Guid.Parse(id);
                    RestClient client = new RestClient();
                    client.Get<DiscussionDetailedItem>("https://apollo-ws.azurewebsites.net/api/doctor/discussion/" + id, GlobalData.GetCredentials(), (result) => 
                    {
                        TitleTextBlock.Text = result.Title;
                        TitleTextBlock.IsEnabled = false;

                        if (result.Replies.Count() != 0)
                        {
                            foreach (ReplyItem replyItem in result.Replies)
                            {
                                ContentStackPanel.Children.Add(new TextBlock() { Text = replyItem.Content });
                            }
                        }
                    });
                }
            }
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            string title = TitleTextBlock.Text;
            string content = ContentTextBox.Text;
            RestClient client = new RestClient();

            // Reply
            if (discussionId != new Guid() && !String.IsNullOrWhiteSpace(content))
            {
                client.Post<ServerMessage>("https://apollo-ws.azurewebsites.net/api/doctor/discussion", new ReplyForm() { DiscussionId = discussionId, Content = content }, GlobalData.GetCredentials(), (result) =>
                    {
                        if (result.IsError)
                        {
                            MessageBox.Show(result.Message);
                        }
                        //else
                        //{
                        //    ContentStackPanel.Children.Add(new TextBlock() { Text = content });
                        //}
                    });
            }
            // New
            else if (!String.IsNullOrWhiteSpace(title) && !String.IsNullOrWhiteSpace(content))
            {
                client.Post<ServerMessage>("https://apollo-ws.azurewebsites.net/api/doctor/discussion", new DiscussionForm() { Title = title, Content = content }, GlobalData.GetCredentials(), (result) =>
                {
                    if (result.IsError)
                    {
                        MessageBox.Show(result.Message);
                    }
                    //else
                    //{
                    //    ContentStackPanel.Children.Add(new TextBlock() { Text = content });
                    //}
                });
            }

            NavigationService.GoBack();
        }

        private void TitleTextBlock_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            TitleTextBlock.Text = "";
        }
    }
}