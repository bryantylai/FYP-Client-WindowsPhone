using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

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
            NavigationService.Navigate(new Uri("/homepage.xaml", UriKind.Relative));
        }
    }
}