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

namespace ApolloWP.Views.Setting
{
    public partial class setting : PhoneApplicationPage
    {
        public setting()
        {
            InitializeComponent();
        }

        private void cancelSetting(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void gotoCamera(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/Views/camera/camera.xaml", UriKind.Relative));
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            RestClient client = new RestClient();
            client.Get<ProfileForm>("https://apollo-ws.azurewebsites.net/api/user/profile", "elsa", "elsaelsa", (result) =>
                {
                    FirstNameTextbox.Text = result.FirstName;
                    LastNameTextbox.Text = result.LastName;
                    //dobDatePicker.Value.Value.Ticks = DateOfBirth,;
                    //GenderListPicker.SelectedIndex == 0 ? "Male" : "Female" = Gender;
                    AboutMeTextbox.Text = result.AboutMe == null ? "" : result.AboutMe;
                    PhoneNumberTextBox.Text = result.Phone == null ? "" : result.Phone;
                    WeightTextBox.Text = result.Weight.ToString() == null ? "" : result.Weight.ToString();
                    HeightTextBox.Text = result.Height.ToString() == null ? "" : result.Height.ToString();
                });
        }

        private void updateProfile(object sender, RoutedEventArgs e)
        {
            RestClient client = new RestClient();
            ProfileForm form = new ProfileForm()
            {
                FirstName = FirstNameTextbox.Text,
                LastName = LastNameTextbox.Text,
                DateOfBirth = dobDatePicker.Value.Value.Ticks,
                Gender = GenderListPicker.SelectedIndex == 0 ? "Male" : "Female",
                AboutMe = AboutMeTextbox.Text,
                Phone = PhoneNumberTextBox.Text,
                Weight = Double.Parse(WeightTextBox.Text),
                Height = Double.Parse(HeightTextBox.Text)
            };

            client.Post<ServerMessage>("https://apollo-ws.azurewebsites.net/api/user/windows/profile", form, "elsa", "elsaelsa", (result) =>
                {
                    if (!result.IsError)
                    {
                        this.NavigationService.Navigate(new Uri("/homepage.xaml", UriKind.Relative));
                    }
                    else
                    {
                        MessageBox.Show(result.Message);
                    }
                });
        }


    }
}