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
using ApolloWP.Data.Form;

namespace ApolloWP
{
    public partial class homepage : PhoneApplicationPage
    {
        public homepage()
        {
            InitializeComponent();

            this.ApplicationBar = new ApplicationBar();
            ApplicationBarIconButton docButton = GlobalAppBar.CreateAppBarIconButton("doctor", "/Assets/appBar/medical.png");
            ApplicationBarIconButton avatarButton = GlobalAppBar.CreateAppBarIconButton("avatar", "/Assets/appBar/running_avatar.png");
            ApplicationBarIconButton profileButton = GlobalAppBar.CreateAppBarIconButton("profile", "/Assets/appBar/profileIcon2.png");
            ApplicationBarMenuItem settingButton = GlobalAppBar.CreateAppBarMenuItem("Settings");
            ApplicationBarMenuItem signoutButton = GlobalAppBar.CreateAppBarMenuItem("Sign Out");
            avatarButton.Click += avatarButton_Click;
            docButton.Click += docButton_Click;
            profileButton.Click += profileButton_Click;
            settingButton.Click += settingButton_Click;
            signoutButton.Click += signoutButton_Click;
            this.ApplicationBar.Buttons.Add(avatarButton);
            this.ApplicationBar.Buttons.Add(docButton);
            this.ApplicationBar.Buttons.Add(profileButton);
            this.ApplicationBar.MenuItems.Add(settingButton);
            this.ApplicationBar.MenuItems.Add(signoutButton);
        }

        void avatarButton_Click(object sender, EventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/avatarRunner.xaml", UriKind.Relative));
        }

        void docButton_Click(object sender, EventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/doctorPage.xaml", UriKind.Relative));
        }

        void profileButton_Click(object sender, EventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/Views/userProfile/userProfile.xaml", UriKind.Relative));
        }

        void settingButton_Click(object sender, EventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/Views/Setting/setting.xaml", UriKind.Relative));
        }

        void signoutButton_Click(object sender, EventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            while (this.NavigationService.CanGoBack)
            {
                this.NavigationService.RemoveBackEntry();
            }

            RestClient profileClient = new RestClient();
            profileClient.Get<ProfileForm>("https://apollo-ws.azurewebsites.net/api/user/profile", GlobalData.GetCredentials(), (result) =>
            {
                User user = new User()
                {
                    Id = result.Id,
                    FirstName = result.FirstName,
                    LastName = result.LastName,
                    Phone = result.Phone,
                    AboutMe = result.AboutMe,
                    Gender = result.Gender,
                    Height = result.Height,
                    Weight = result.Weight,
                    DateOfBirth = new DateTime(result.DateOfBirth),
                    ProfileImage = result.ProfileImage,
                    CoverImage = result.CoverImage
                };

                GlobalData.SetUser(user);
                if (user != null)
                {
                    Weight.Text = user.Weight.ToString();
                    Height.Text = user.Height.ToString();

                    double HeightInCm = user.Height / 100;
                    double BmiTotalCalc = (user.Weight / (HeightInCm * HeightInCm));
                    BmiTotal.Text = Math.Round(BmiTotalCalc, 2).ToString();
                }
            });
        }
    }
}