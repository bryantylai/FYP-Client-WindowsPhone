using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace ApolloWP.Views.Setting
{
    public partial class setting : PhoneApplicationPage
    {
        public setting()
        {
            InitializeComponent();

            ApplicationBar = App.Current.Resources["GlobalAppBar"] as ApplicationBar;
        }
    }
}