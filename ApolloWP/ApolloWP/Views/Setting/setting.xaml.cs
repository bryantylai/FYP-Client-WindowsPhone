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
using ApolloWP.Data;

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

            User user = GlobalData.GetUser();
            if (user != null)
            {
                FirstNameTextbox.Text = user.FirstName;
                LastNameTextbox.Text = user.LastName;
                AboutMeTextbox.Text = user.AboutMe;
                PhoneNumberTextBox.Text = user.Phone;
                WeightTextBox.Text = user.Weight.ToString();
                HeightTextBox.Text = user.Height.ToString();
                GenderListPicker.SelectedIndex = (user.Gender == "Male") ? 0 : 1;
                dobDatePicker.Value = user.DateOfBirth;
            }
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

            client.Post<ServerMessage>("https://apollo-ws.azurewebsites.net/api/user/windows/profile", form, GlobalData.GetCredentials(), (result) =>
                {
                    if (!result.IsError)
                    {
                        User user = GlobalData.GetUser();
                        if (user == null) { user = new User(); }

                        user.FirstName = form.FirstName;
                        user.LastName = form.LastName;
                        user.DateOfBirth = new DateTime(form.DateOfBirth);
                        user.Gender = form.Gender;
                        user.AboutMe = form.AboutMe;
                        user.Phone = form.AboutMe;
                        user.Weight = form.Weight;
                        user.Height = form.Height;

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