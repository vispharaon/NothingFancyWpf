using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using NothingFancy.Controls;
using NothingFancy.Data;
using NothingFancy.Tracks.ViewModel;

namespace NothingFancy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //public string SelectedVideo { get; set; }
        //private TrackListViewModel trackListvm;
        public MainWindow()
        {
            InitializeComponent();

            // trackListvm = new TrackListViewModel(new TrackRepository());

            //this.DataContext = trackListvm;
            //FillMusicList();

            //TrackPreview.Source = new Uri(trackListvm.SelectedAudio);
            SetMainFrameTrack(null);
        }

        private void SetMainFrameTrack(TrackViewModel tvm)
        {
            MainFrame.NavigationService.Content = new TrackPage(tvm);
        }


        private void SideMenu_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var sideMenu = sender as SideMenuControl;
            if (sideMenu != null)
            {
                SetMainFrameTrack(sideMenu.GetSelectedAudio());
            }
        }
    }
}