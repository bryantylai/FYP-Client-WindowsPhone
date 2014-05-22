using ApolloWP.Data.Form;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace ApolloWP
{
    public class GlobalData
    {
        public static User User;

        public void displayGlobalData(NavigationEventArgs e)
        {
            RestClient client = new RestClient();
            client.Get<ProfileForm>("https://apollo-ws.azurewebsites.net/api/user/profile", "elsa", "elsaelsa", (result) =>
            {
                //User.FirstName = result.FirstName;
                //User.LastNameTextbox = result.LastName;
                ////dobDatePicker.Value.Value.Ticks = DateOfBirth,;
                ////GenderListPicker.SelectedIndex == 0 ? "Male" : "Female" = Gender;
                //AboutMeTextbox.Text = result.AboutMe == null ? "" : result.AboutMe;
                //PhoneNumberTextBox.Text = result.Phone == null ? "" : result.Phone;
                //WeightTextBox.Text = result.Weight.ToString() == null ? "" : result.Weight.ToString();
                //HeightTextBox.Text = result.Height.ToString() == null ? "" : result.Height.ToString();
            });
        }
    }

    public class User
    {
        public string FirstName { get; set; }
    }
}
