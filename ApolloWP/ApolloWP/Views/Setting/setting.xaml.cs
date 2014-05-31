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
                Height = Double.Parse(HeightTextBox.Text),
                ProfileImage = "https://s3-ap-southeast-1.amazonaws.com/apollo-fyp/589a5bd4-bf46-439d-bff2-857ea6e804d8/profilepic.png?X-Amz-Date=20140531T040241Z&X-Amz-Expires=300&X-Amz-Algorithm=AWS4-HMAC-SHA256&X-Amz-Signature=02be69ff52a0cdf190374d1f57204847337b47276259e7404f8cc23a51f8705e&X-Amz-Credential=ASIAJWVW4AZYZ5TO2UAQ/20140531/ap-southeast-1/s3/aws4_request&X-Amz-SignedHeaders=Host&x-amz-security-token=AQoDYXdzEOX//////////wEawAJtt%2BFKLSak0HnAQew1VlC9Xivf3GOvlavFiHBT%2Bmir6X6vGWMoSV791qcMp63lz1ivbHwOtyE5Vdzmf7ali3DwBAJKBupJd7ILjXA0NJZz5jL3%2Bmhqf2cB2qY7%2BbV1AnaRTvoELHkx8ekdV9qWQT12VQtURVBhNEL6xvmnJXX57K%2BoRSdEF6kWJRkoPsJTC/qiAHFYOwlY2bfDc9rshqgF9qISAr6R4O9PcAOXTSll1u16nCFZs%2BHwgZnlTIVFS8cmIYkYqOJqC5MY4fYkA%2B3r/kb738uOlpg1Cvrf0kyir8LHrutm%2BIw4dv3sO0QhBC0rjxQzyvv4%2Bo0cmD9hJEDfNK06/6jlY8YVmpO1PseRZYbTbQpAgaHHKABxRMS99Cf8P/mNJWayMC03mmjbGT3XVZqeLyeeMtLr3fr47HrdZCDop6WcBQ%3D%3D",
                CoverImage = "https://s3-ap-southeast-1.amazonaws.com/apollo-fyp/589a5bd4-bf46-439d-bff2-857ea6e804d8/coverpic.png?X-Amz-Date=20140531T040143Z&X-Amz-Expires=300&X-Amz-Algorithm=AWS4-HMAC-SHA256&X-Amz-Signature=a8100d14b91ad860942e701ea3b5bcc8f6dd47b9e998711b66f9505e4f5aabbb&X-Amz-Credential=ASIAJWVW4AZYZ5TO2UAQ/20140531/ap-southeast-1/s3/aws4_request&X-Amz-SignedHeaders=Host&x-amz-security-token=AQoDYXdzEOX//////////wEawAJtt%2BFKLSak0HnAQew1VlC9Xivf3GOvlavFiHBT%2Bmir6X6vGWMoSV791qcMp63lz1ivbHwOtyE5Vdzmf7ali3DwBAJKBupJd7ILjXA0NJZz5jL3%2Bmhqf2cB2qY7%2BbV1AnaRTvoELHkx8ekdV9qWQT12VQtURVBhNEL6xvmnJXX57K%2BoRSdEF6kWJRkoPsJTC/qiAHFYOwlY2bfDc9rshqgF9qISAr6R4O9PcAOXTSll1u16nCFZs%2BHwgZnlTIVFS8cmIYkYqOJqC5MY4fYkA%2B3r/kb738uOlpg1Cvrf0kyir8LHrutm%2BIw4dv3sO0QhBC0rjxQzyvv4%2Bo0cmD9hJEDfNK06/6jlY8YVmpO1PseRZYbTbQpAgaHHKABxRMS99Cf8P/mNJWayMC03mmjbGT3XVZqeLyeeMtLr3fr47HrdZCDop6WcBQ%3D%3D"
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