using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using ApolloWP.Data.Item;
using ApolloWP.Data.Form;

namespace ApolloWP.Views.Auth
{
    public partial class SignInPage : PhoneApplicationPage
    {
        public SignInPage()
        {
            InitializeComponent();
        }

        private void backMainPage(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/mainPage.xaml", UriKind.Relative));
        }

        private void signInHomePage(object sender, RoutedEventArgs e)
        {
            RestClient client = new RestClient();
            client.Post<LoginMessage>("https://apollo-ws.azurewebsites.net/api/auth/login", new LoginForm() { Username = UsernameTextBox.Text, Password = PasswordTextBox.Password }, (result) =>
                {
                    if (!result.IsError)
                    {
                        if (result.IsNewAccount)
                        {
                            this.NavigationService.Navigate(new Uri("/Views/Setting/setting.xaml", UriKind.Relative));
                        }
                        else
                        {
                            this.NavigationService.Navigate(new Uri("/homepage.xaml", UriKind.Relative));
                        }
                    }
                    else
                    {
                        MessageBox.Show(result.Message);
                    }
                });
        }
    }
}