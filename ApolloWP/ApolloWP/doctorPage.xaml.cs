using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using ApolloWP.Data.Item;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;

namespace ApolloWP
{
    public partial class doctorPage : PhoneApplicationPage
    {
        public doctorPage()
        {
            InitializeComponent();

            this.ApplicationBar = new ApplicationBar();
            ApplicationBarIconButton avatarButton = GlobalAppBar.CreateAppBarIconButton("avatar", "/Assets/appBar/running_avatar.png");
            ApplicationBarIconButton profileButton = GlobalAppBar.CreateAppBarIconButton("profile", "/Assets/appBar/profileIcon2.png");
            ApplicationBarMenuItem settingButton = GlobalAppBar.CreateAppBarMenuItem("Settings");
            ApplicationBarMenuItem signoutButton = GlobalAppBar.CreateAppBarMenuItem("Sign Out");
            avatarButton.Click += avatarButton_Click;
            profileButton.Click += profileButton_Click; 
            settingButton.Click += settingButton_Click;
            signoutButton.Click += signoutButton_Click;
            this.ApplicationBar.Buttons.Add(avatarButton);
            this.ApplicationBar.Buttons.Add(profileButton);
            this.ApplicationBar.MenuItems.Add(settingButton);
            this.ApplicationBar.MenuItems.Add(signoutButton);
        }

        void avatarButton_Click(object sender, EventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/avatarRunner.xaml", UriKind.Relative));
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

            if (DoctorListBox.Items.Count == 0)
            {
                RestClient doctorClient = new RestClient();
                doctorClient.Get<IEnumerable<DoctorItem>>("https://apollo-ws.azurewebsites.net/api/doctor/fetch-all", GlobalData.GetCredentials(), (result) =>
                {
                    DoctorListBox.ItemsSource = result;
                });
            }

            if (DiscussionListBox.Items.Count == 0)
            {
                RestClient discussionClient = new RestClient();
                discussionClient.Get<IEnumerable<DiscussionGeneralItem>>("https://apollo-ws.azurewebsites.net/api/doctor/discussion", GlobalData.GetCredentials(), (result) =>
                {
                    ObservableCollection<DiscussionListBoxItem> items = new ObservableCollection<DiscussionListBoxItem>();

                    foreach (DiscussionGeneralItem item in result)
                    {
                        DateTime date = new DateTime(item.LastActive);

                        items.Add(new DiscussionListBoxItem()
                        {
                            Title = item.Title,
                            CreatorName = item.CreatorName,
                            DiscussionId = item.DiscussionId,
                            ReplyCount = item.ReplyCount,
                            ActiveDate = date.Date,
                            ActiveTime = date.TimeOfDay
                        });
                    }

                    DiscussionListBox.ItemsSource = items;
                });
            }
        }

        private void DoctorListBox_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (DoctorListBox.SelectedItem != null)
            {
                DoctorItem doctor = DoctorListBox.SelectedItem as DoctorItem;

                PhoneCallTask task = new PhoneCallTask();
                task.DisplayName = doctor.Name;
                task.PhoneNumber = doctor.Phone;
                task.Show();
            }
        }

        private void DiscussionListBox_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {

        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}