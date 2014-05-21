using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApolloWP
{
    public class GlobalAppBar
    {
        public static ApplicationBarIconButton CreateAppBarIconButton(string title, string url)
        {
            return new ApplicationBarIconButton()
            {
                IconUri = new Uri(url, UriKind.Relative),
                Text = title
            };
        }

        public static ApplicationBarMenuItem CreateAppBarMenuItem(string title)
        {
            return new ApplicationBarMenuItem()
            {
                Text = title
            };
        }
    }
}
