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
        private static Dictionary<string, IEnumerable<RunDisplayItem>> History;
        public static PasswordCredential GetCredentials() { return Credential; }
        public static User GetUser() { return User; }
        public static void SetUser(User user) { User = user; }
        public static Avatar GetAvatar() { return Avatar; }
        public static void SetAvatar(Avatar avatar) { Avatar = avatar; }
        public static IEnumerable<LeaderboardItem> GetLeaderboard() { return Leaderboard; }
        public static void SetLeaderboard(IEnumerable<LeaderboardItem> leaderboard) { Leaderboard = leaderboard; }
        public static IEnumerable<RunDisplayItem> GetHistory(string key)
        {
            IEnumerable<RunDisplayItem> runs;
            History.TryGetValue(key, out runs);
            return runs;
        }
        public static void SetHistory(Dictionary<string, IEnumerable<RunDisplayItem>> history) { History = history; }

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

        public static void RemoveCredentials()
        {
            PasswordVault vault = new PasswordVault();
            IEnumerable<PasswordCredential> credentials = vault.RetrieveAll();      
            foreach (PasswordCredential credential in credentials) { vault.Remove(credential); }    
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
                    Experience = result.Experience * 100 + "%",
                    Distance = Math.Round(result.Distance / 1000, 2),
                    Duration = TimeSpan.FromTicks(result.Duration).Hours + ":" + TimeSpan.FromTicks(result.Duration).Minutes
                };
            });

            RestClient historyClient = new RestClient();
            historyClient.Get<AvatarHistoryItem>("https://apollo-ws.azurewebsites.net/api/avatar/windows/history", GlobalData.GetCredentials(), (result) =>
            {
                HashSet<RunDisplayItem> dayRunItems = new HashSet<RunDisplayItem>();
                foreach (RunItem runItem in result.Day)
                {
                    TimeSpan time = TimeSpan.FromTicks(runItem.Duration);
                    string hours = "";
                    string minutes = "";
                    if (time.Hours < 10) { hours = "0"; }
                    if (time.Minutes < 10) { minutes = "0"; }

                    hours += time.Hours;
                    minutes += time.Minutes;

                    RunDisplayItem runDisplayItem = new RunDisplayItem();
                    runDisplayItem.RunDate = new DateTime(runItem.RunDate).Date.ToString();
                    runDisplayItem.Distance = Math.Round(runItem.Distance / 1000, 2);
                    runDisplayItem.Duration = hours + ":" + minutes;

                    dayRunItems.Add(runDisplayItem);
                }
                History.Add("Day", dayRunItems);

                HashSet<RunDisplayItem> weekRunItems = new HashSet<RunDisplayItem>();
                foreach (RunItem runItem in result.Week)
                {
                    TimeSpan time = TimeSpan.FromTicks(runItem.Duration);
                    string hours = "";
                    string minutes = "";
                    if (time.Hours < 10) { hours = "0"; }
                    if (time.Minutes < 10) { minutes = "0"; }

                    hours += time.Hours;
                    minutes += time.Minutes;

                    RunDisplayItem runDisplayItem = new RunDisplayItem();
                    runDisplayItem.RunDate = new DateTime(runItem.RunDate).Date.ToString();
                    runDisplayItem.Distance = Math.Round(runItem.Distance / 1000, 2);
                    runDisplayItem.Duration = hours + ":" + minutes;

                    weekRunItems.Add(runDisplayItem);
                }
                History.Add("Week", weekRunItems);

                HashSet<RunDisplayItem> monthRunItems = new HashSet<RunDisplayItem>();
                foreach (RunItem runItem in result.Month)
                {
                    TimeSpan time = TimeSpan.FromTicks(runItem.Duration);
                    string hours = "";
                    string minutes = "";
                    if (time.Hours < 10) { hours = "0"; }
                    if (time.Minutes < 10) { minutes = "0"; }

                    hours += time.Hours;
                    minutes += time.Minutes;

                    RunDisplayItem runDisplayItem = new RunDisplayItem();
                    runDisplayItem.RunDate = new DateTime(runItem.RunDate).Date.ToString();
                    runDisplayItem.Distance = Math.Round(runItem.Distance / 1000, 2);
                    runDisplayItem.Duration = hours + ":" + minutes;

                    monthRunItems.Add(runDisplayItem);
                }
                History.Add("Month", monthRunItems);

                HashSet<RunDisplayItem> yearRunItems = new HashSet<RunDisplayItem>();
                foreach (RunItem runItem in result.Year)
                {
                    TimeSpan time = TimeSpan.FromTicks(runItem.Duration);
                    string hours = "";
                    string minutes = "";
                    if (time.Hours < 10) { hours = "0"; }
                    if (time.Minutes < 10) { minutes = "0"; }

                    hours += time.Hours;
                    minutes += time.Minutes;

                    RunDisplayItem runDisplayItem = new RunDisplayItem();
                    runDisplayItem.RunDate = new DateTime(runItem.RunDate).Date.ToString();
                    runDisplayItem.Distance = Math.Round(runItem.Distance / 1000, 2);
                    runDisplayItem.Duration = hours + ":" + minutes;

                    yearRunItems.Add(runDisplayItem);
                }
                History.Add("Year", yearRunItems);
            });

            RestClient leaderboardClient = new RestClient();
            leaderboardClient.Get<IEnumerable<LeaderboardItem>>("https://apollo-ws.azurewebsites.net/api/avatar/leaderboard", GlobalData.GetCredentials(), (result) =>
            {
                Leaderboard = result;
            });
        }
    }
}
