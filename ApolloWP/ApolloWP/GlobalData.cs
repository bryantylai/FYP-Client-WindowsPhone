using ApolloWP.Data.Form;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using Windows.Security.Credentials;
using ApolloWP.Data;

namespace ApolloWP
{
    public class GlobalData
    {
        private static PasswordCredential Credential;
        private static User User;
        private static Avatar Avatar;
        public static PasswordCredential GetCredentials() { return Credential; }
        public static User GetUser() { return User; }
        public static void SetUser(User user) { User = user; }
        public static Avatar GetAvatar() { return Avatar; }

        public static void InitCredential()
        {
            PasswordVault vault = new PasswordVault();
            IEnumerable<PasswordCredential> credentials = vault.RetrieveAll();
            if (credentials.Count() != 0)
            {
                Credential = vault.RetrieveAll().First();
                Credential.RetrievePassword();
            }
            else
            {
                Credential = null;
            }            
        }

        public static void SaveCredential(string username, string password)
        {
            PasswordVault vault = new PasswordVault();
            IEnumerable<PasswordCredential> credentials = vault.RetrieveAll();
            foreach (PasswordCredential credential in credentials) { vault.Remove(credential); }            
            Credential = new PasswordCredential("Credentials", username, password);
            vault.Add(Credential);
        }

        public static void GetAppData()
        {
            RestClient profileClient = new RestClient();
            profileClient.Get<ProfileForm>("https://apollo-ws.azurewebsites.net/api/user/profile", Credential, (result) =>
                {
                    User = new User()
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
                });

            RestClient avatarClient = new RestClient();
            avatarClient.Get<object>("https://apollo-ws.azurewebsites.net/api/avatar/profile", Credential, (result) =>
                {
                    Avatar = new Avatar()
                    {

                    };
                });
        }
    }
}
