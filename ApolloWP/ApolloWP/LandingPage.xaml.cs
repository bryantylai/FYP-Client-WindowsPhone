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
using Windows.Security.Credentials;

namespace ApolloWP
{
    public partial class LandingPage : PhoneApplicationPage
    {
        public LandingPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);


            PasswordCredential credential = GlobalData.GetCredentials();
            if (credential != null)
            {
                RestClient client = new RestClient();
                client.Post<LoginMessage>("https://apollo-ws.azurewebsites.net/api/auth/login", new LoginForm() { Username = credential.UserName, Password = credential.Password }, (result) =>
                {
                    if (!result.IsError)
                    {
                        this.NavigationService.Navigate(new Uri("/homepage.xaml", UriKind.Relative));
                    }
                    else
                    {
                        NavigationService.Navigate(new Uri("/mainPage.xaml", UriKind.Relative));
                    }
                });
            }
            else
            {
                NavigationService.Navigate(new Uri("/mainPage.xaml", UriKind.Relative));
            }
        }
    }
}