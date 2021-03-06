﻿using System;
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
            NavigationService.GoBack();
        }

        private void signInHomePage(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordTextBox.Password;
            RestClient client = new RestClient();
            client.Post<LoginMessage>("https://apollo-ws.azurewebsites.net/api/auth/login", new LoginForm() { Username = username, Password = password }, (result) =>
                {
                    if (!result.IsError)
                    {
                        GlobalData.SaveCredential(username, password);

                        if (result.NewAccount)
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