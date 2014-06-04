using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using ApolloWP.Data;

namespace ApolloWP.Views.userProfile
{
    public partial class userProfile : PhoneApplicationPage
    {
        public userProfile()
        {
            InitializeComponent();

            this.ApplicationBar = new ApplicationBar();
            ApplicationBarIconButton docButton = GlobalAppBar.CreateAppBarIconButton("doctor", "/Assets/appBar/medical.png");
            ApplicationBarIconButton avatarButton = GlobalAppBar.CreateAppBarIconButton("avatar", "/Assets/appBar/running_avatar.png");
            ApplicationBarMenuItem settingButton = GlobalAppBar.CreateAppBarMenuItem("Settings");
            ApplicationBarMenuItem signoutButton = GlobalAppBar.CreateAppBarMenuItem("Sign Out");
            avatarButton.Click += avatarButton_Click;
            docButton.Click += docButton_Click;
            settingButton.Click += settingButton_Click;
            signoutButton.Click += signoutButton_Click;
            this.ApplicationBar.Buttons.Add(avatarButton);
            this.ApplicationBar.Buttons.Add(docButton);
            this.ApplicationBar.MenuItems.Add(settingButton);
            this.ApplicationBar.MenuItems.Add(signoutButton);

            //User user = GlobalData.GetUser();
            //FirstName.Text = user.FirstName;
            //LastName.Text = user.LastName;
            //AboutMe.Text = user.AboutMe;
        }

        void avatarButton_Click(object sender, EventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/avatarRunner.xaml", UriKind.Relative));
        }

        void docButton_Click(object sender, EventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/doctorPage.xaml", UriKind.Relative));
        }

        void settingButton_Click(object sender, EventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/Views/Setting/setting.xaml", UriKind.Relative));
        }

        void signoutButton_Click(object sender, EventArgs e)
        {
            GlobalData.RemoveCredentials();
            this.NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }


        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            base.OnBackKeyPress(e);

            while (this.NavigationService.CanGoBack)
            {
                this.NavigationService.RemoveBackEntry();
            }

            this.NavigationService.Navigate(new Uri("/homepage.xaml", UriKind.Relative));
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            User user = GlobalData.GetUser();
            LayoutRoot.DataContext = user;
        }
    }
}