using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Maps.Controls;
using System.Device.Location;
using System.Windows.Threading;
using System.Windows.Media;
using ApolloWP.Data.Form;
using ApolloWP.Data.Item;

namespace ApolloWP.Views.Avatar
{
    public partial class avatarMap : PhoneApplicationPage
    {
        private GeoCoordinateWatcher watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.High);
        private DispatcherTimer timer = new DispatcherTimer();
        private MapPolyline line;
        private long timeStarted;
        private long startTime;
        private double kilometres;
        private long previousPositionChangeTick;
        private double speed;

        public avatarMap()
        {
            InitializeComponent();

            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += timer_Tick;

            timer.Start();
            watcher.Start();
            timeStarted = DateTime.Now.Ticks;
            startTime = Environment.TickCount;

            line = new MapPolyline();
            line.StrokeColor = Colors.Red;
            line.StrokeThickness = 5;
            mapView.MapElements.Add(line);

            watcher.PositionChanged += watcher_PositionChanged;
        }

        void watcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            var coord = new GeoCoordinate(e.Position.Location.Latitude, e.Position.Location.Longitude);

            if (line.Path.Count > 0)
            {
                var prevPoint = line.Path.Last();
                var distance = coord.GetDistanceTo(prevPoint);

                kilometres += distance / 1000;

                TimeSpan runTime = TimeSpan.FromMilliseconds(Environment.TickCount - startTime);
                speed = kilometres / runTime.TotalHours;

                speedLabel.Text = string.Format("{0:f2}", speed) + "km/h";
                distanceLabel.Text = string.Format("{0:f2}", kilometres) + "km";

                mapView.Center = coord;
                mapView.ZoomLevel = mapView.ZoomLevel;
            }

            mapView.Center = coord;
            line.Path.Add(coord);
            previousPositionChangeTick = Environment.TickCount;
        }

        void timer_Tick(object sender, EventArgs e)
        {
            TimeSpan runTime = TimeSpan.FromMilliseconds(Environment.TickCount - startTime);
            timeLabel.Text = runTime.ToString(@"hh\:mm\:ss");
        }

        private void restart()
        {
            timer.Interval = TimeSpan.FromSeconds(1);
            line.Path.Clear();
            kilometres = 0;
            speed = 0;
            distanceLabel.Text = string.Format("{0:f2}", kilometres);
            speedLabel.Text = string.Format("{0:f2}", speed);
            timeLabel.Text = "00:00:00";
        }

        private void EndButton_Click(object sender, RoutedEventArgs e)
        {
            RunForm runForm = new RunForm()
            {
                StartTime = timeStarted,
                EndTime = DateTime.Now.Ticks,
                Distance = kilometres * 1000
            };

            RestClient client = new RestClient();
            client.Post<RunMessage>("https://apollo-ws.azurewebsites.net/api/avatar/run", runForm, GlobalData.GetCredentials(), (result) =>
                {
                    if (result.IsError)
                    {
                        MessageBox.Show(result.Message);
                    }
                    else
                    {
                        this.NavigationService.GoBack();
                    }
                });
            
        }
    }
}