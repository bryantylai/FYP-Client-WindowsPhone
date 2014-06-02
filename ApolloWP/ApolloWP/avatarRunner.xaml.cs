using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using ApolloWP.Data;
using ApolloWP.Data.Item;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace ApolloWP
{
    public partial class avatarRunner : PhoneApplicationPage
    {
        public avatarRunner()
        {
            InitializeComponent();

            this.ApplicationBar = new ApplicationBar();
            ApplicationBarIconButton docButton = GlobalAppBar.CreateAppBarIconButton("doctor", "/Assets/appBar/medical.png");
            ApplicationBarIconButton profileButton = GlobalAppBar.CreateAppBarIconButton("profile", "/Assets/appBar/profileIcon2.png");
            ApplicationBarMenuItem settingButton = GlobalAppBar.CreateAppBarMenuItem("Settings");
            ApplicationBarMenuItem signoutButton = GlobalAppBar.CreateAppBarMenuItem("Sign Out");
            docButton.Click += docButton_Click;
            profileButton.Click += profileButton_Click;
            settingButton.Click += settingButton_Click;
            signoutButton.Click += signoutButton_Click;
            this.ApplicationBar.Buttons.Add(docButton);
            this.ApplicationBar.Buttons.Add(profileButton);
            this.ApplicationBar.MenuItems.Add(settingButton);
            this.ApplicationBar.MenuItems.Add(signoutButton);
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

            RestClient avatarClient = new RestClient();
            avatarClient.Get<AvatarProfileItem>("https://apollo-ws.azurewebsites.net/api/avatar/windows/profile", GlobalData.GetCredentials(), (result) =>
            {
                GlobalData.SetAvatar(new Avatar()
                {
                    Name = result.Name,
                    Level = result.Level,
                    Experience = result.Experience,
                    Distance = result.Distance,
                    Duration = new TimeSpan(result.Duration)
                });

                AvatarProfilePanoramaItem.DataContext = GlobalData.GetAvatar();
            });

            RestClient historyClient = new RestClient();
            historyClient.Get<AvatarHistoryItem>("https://apollo-ws.azurewebsites.net/api/avatar/windows/history", GlobalData.GetCredentials(), (result) =>
            {
                Dictionary<string, IEnumerable<RunItem>> History = new Dictionary<string, IEnumerable<RunItem>>();
                History.Add("Day", result.Day);
                History.Add("Week", result.Week);
                History.Add("Month", result.Month);
                History.Add("Year", result.Year);
                GlobalData.SetHistory(History);
                HistoryLeaderboard.ItemsSource = GlobalData.GetHistory("Day");
            });

            RestClient leaderboardClient = new RestClient();
            leaderboardClient.Get<IEnumerable<LeaderboardItem>>("https://apollo-ws.azurewebsites.net/api/avatar/leaderboard", GlobalData.GetCredentials(), (result) =>
            {
                GlobalData.SetLeaderboard(result);
                LeaderboardListBox.ItemsSource = GlobalData.GetLeaderboard();
            });
        }

        private void TextBlock_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            TextBlock tappedTextBlock = e.OriginalSource as TextBlock;
            HistoryLeaderboard.ItemsSource = GlobalData.GetHistory(tappedTextBlock.Text);
        }

        private void AvatarRunnerMap(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/Views/Avatar/avatarMap.xaml", UriKind.Relative));
        }
    }
}