using ApolloWP.Data.Form;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using Windows.Security.Credentials;
using ApolloWP.Data;
using ApolloWP.Data.Item;

namespace ApolloWP
{
    public class GlobalData
    {
        private static PasswordCredential Credential;
        private static User User;
        private static Avatar Avatar;
        private static IEnumerable<LeaderboardItem> Leaderboard;
        private static Dictionary<string, IEnumerable<RunItem>> History;
        public static PasswordCredential GetCredentials() { return Credential; }
        public static User GetUser() { return User; }
        public static void SetUser(User user) { User = user; }
        public static Avatar GetAvatar() { return Avatar; }
        public static void SetAvatar(Avatar avatar) { Avatar = avatar; }
        public static IEnumerable<LeaderboardItem> GetLeaderboard() { return Leaderboard; }
        public static void SetLeaderboard(IEnumerable<LeaderboardItem> leaderboard) { Leaderboard = leaderboard; }
        public static IEnumerable<RunItem> GetHistory(string key)
        {
            IEnumerable<RunItem> runs;
            History.TryGetValue(key, out runs);
            return runs;
        }
        public static void SetHistory(Dictionary<string, IEnumerable<RunItem>> history) { History = history; }

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
            avatarClient.Get<AvatarProfileItem>("https://apollo-ws.azurewebsites.net/api/avatar/windows/profile", GlobalData.GetCredentials(), (result) =>
            {
                Avatar = new Avatar()
                {
                    Name = result.Name,
                    Level = result.Level,
                    Experience = result.Experience,
                    Distance = result.Distance,
                    Duration = new TimeSpan(result.Duration)
                };
            });

            RestClient historyClient = new RestClient();
            historyClient.Get<AvatarHistoryItem>("https://apollo-ws.azurewebsites.net/api/avatar/windows/history", GlobalData.GetCredentials(), (result) =>
            {
                History = new Dictionary<string, IEnumerable<RunItem>>();
                History.Add("Day", result.Day);
                History.Add("Week", result.Week);
                History.Add("Month", result.Month);
                History.Add("Year", result.Year);
            });

            RestClient leaderboardClient = new RestClient();
            leaderboardClient.Get<IEnumerable<LeaderboardItem>>("https://apollo-ws.azurewebsites.net/api/avatar/leaderboard", GlobalData.GetCredentials(), (result) =>
            {
                Leaderboard = result;
            });
        }
    }
}
