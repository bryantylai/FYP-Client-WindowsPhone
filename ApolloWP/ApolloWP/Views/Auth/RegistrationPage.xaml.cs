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
using Windows.Security.Credentials;
using ApolloWP.Data.Form;

namespace ApolloWP.Views.Auth
{
    public partial class RegistrationPage : PhoneApplicationPage
    {
        public RegistrationPage()
        {
            InitializeComponent();
        }

        private void signUpHomePage(object sender, RoutedEventArgs e)
        {
            RestClient client = new RestClient();
            client.Post<ServerMessage>("https://apollo-ws.azurewebsites.net/api/auth/register", new RegistrationForm() { Email = EmailTextBox.Text, Username = UsernameTextBox.Text, Password = PasswordTextBox.Password, Phone = PhoneTextBox.Text }, (result) =>
            {
                if (!result.IsError)
                {
                    PasswordVault passwordVault = new PasswordVault();
                    passwordVault.Add(new PasswordCredential("Credentials", UsernameTextBox.Text, PasswordTextBox.Password));

                    this.NavigationService.Navigate(new Uri("/Views/Auth/SignInPage.xaml", UriKind.Relative));
                }
                else
                {
                    MessageBox.Show(result.Message);
                }
            });
        }
    }
}