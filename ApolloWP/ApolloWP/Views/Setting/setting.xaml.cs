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
using Microsoft.Phone.Tasks;
using System.IO;
using Amazon.S3;
using Amazon.S3.Model;
using System.Windows.Media.Imaging;
using Amazon.Runtime;

namespace ApolloWP.Views.Setting
{
    public partial class setting : PhoneApplicationPage
    {
        private string CoverImagePath;
        private string ProfileImagePath;

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
            //this.NavigationService.Navigate(new Uri("/Views/camera/camera.xaml", UriKind.Relative));
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            //GlobalData.GetAppData();
            if (!this.NavigationContext.QueryString.ContainsKey("new"))
            {
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
                ProfileImage = ProfileImagePath,
                CoverImage = CoverImagePath
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
                        user.CoverImage = form.CoverImage;
                        user.ProfileImage = form.ProfileImage;

                        this.NavigationService.Navigate(new Uri("/homepage.xaml", UriKind.Relative));
                    }
                    else
                    {
                        MessageBox.Show(result.Message);
                    }
                });
        }

        private void ProfileImageButton_Click(object sender, RoutedEventArgs e)
        {
            PhotoChooserTask task = new PhotoChooserTask();
            task.ShowCamera = true;
            task.Show();
            task.Completed += ProfileImage_Completed;
        }

        async void ProfileImage_Completed(object sender, PhotoResult e)
        {
            try
            {
                if (e.TaskResult == TaskResult.OK)
                {
                    if (e.TaskResult == TaskResult.OK)
                    {
                        // Create a client
                        AmazonS3Client client = new AmazonS3Client(AmazonKeys.AccessKey, AmazonKeys.SecretKey, Amazon.RegionEndpoint.APSoutheast1);

                        // Create a PutObject request
                        PutObjectRequest request = new PutObjectRequest
                        {
                            BucketName = "apollo-fyp",
                            Key = GlobalData.GetUser().Id + "/" + "ProfileImage.png",
                            ContentType = "image/png",
                            CannedACL = S3CannedACL.PublicRead
                        };
                        ProfileImagePath = "https://s3-ap-southeast-1.amazonaws.com/apollo-fyp/" + GlobalData.GetUser().Id + "/" + "ProfileImage.png";
                        BitmapImage img = new BitmapImage();
                        img.SetSource(e.ChosenPhoto);
                        ProfileImage.Source = img;

                        request.InputStream = e.ChosenPhoto;

                        // Put object
                        PutObjectResponse response = await client.PutObjectAsync(request);
                    }
                }
            }
            catch (AmazonServiceException ex)
            {
            }
        }

        private void CoverImageButton_Click(object sender, RoutedEventArgs e)
        {
            PhotoChooserTask task = new PhotoChooserTask();
            task.ShowCamera = true;
            task.Show();
            task.Completed += CoverImage_Completed;
        }

        async void CoverImage_Completed(object sender, PhotoResult e)
        {
            try
            {
                if (e.TaskResult == TaskResult.OK)
                {
                    // Create a client
                    AmazonS3Client client = new AmazonS3Client(AmazonKeys.AccessKey, AmazonKeys.SecretKey, Amazon.RegionEndpoint.APSoutheast1);

                    // Create a PutObject request
                    PutObjectRequest request = new PutObjectRequest
                    {
                        BucketName = "apollo-fyp",
                        Key = GlobalData.GetUser().Id + "/" + "CoverImage.png",
                        ContentType = "image/png",
                        CannedACL = S3CannedACL.PublicRead
                    };
                    CoverImagePath = "https://s3-ap-southeast-1.amazonaws.com/apollo-fyp/" + GlobalData.GetUser().Id + "/" + "CoverImage.png";
                    BitmapImage img = new BitmapImage();
                    img.SetSource(e.ChosenPhoto);
                    CoverImage.Source = img;

                    request.InputStream = e.ChosenPhoto;

                    // Put object
                    PutObjectResponse response = await client.PutObjectAsync(request);
                }
            }
            catch (AmazonServiceException ex)
            {
            }
        }


    }
}