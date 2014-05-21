using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace ApolloWP
{
    public partial class mainPage : PhoneApplicationPage
    {
        public mainPage()
        {
            InitializeComponent();
        }

        private void goToSignInPage(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Auth/SignInPage.xaml", UriKind.Relative));
        }

        private void goToSignUpPage(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Auth/RegistrationPage.xaml", UriKind.Relative));
            
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);


            while (this.NavigationService.CanGoBack)
            {
                this.NavigationService.RemoveBackEntry();
            }
        }
    }
}