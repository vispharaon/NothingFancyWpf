using System.Windows;
using System.Windows.Controls;
using NothingFancy.Data;
using NothingFancy.Tracks.ViewModel;

namespace NothingFancy
{
    /// <summary>
    /// Interaction logic for TrackPage.xaml
    /// </summary>
    public partial class TrackPage : Page
    {
        private readonly TrackPageViewModel _vm;
        public TrackPage(TrackViewModel tvm)
        {
            InitializeComponent();

            if (tvm == null)
            {
                SetControlsVisible(Visibility.Hidden);
                return;
            }

            var trackRepo = new TrackRepository();
            var previewSource = trackRepo.GetPreviewSource(tvm.TrackId);
            
            this.DataContext = _vm = new TrackPageViewModel
            {
                Artist = tvm.Artist,
                Title = tvm.Title,
                Image = tvm.Image,
                AudioTrack = previewSource,
                Playtime = tvm.PlayTime,
                Popularity = tvm.Popularity,
                DiscNumber = tvm.DiscNumber,
                LicensorName = tvm.LicensorName,
                LabelName = tvm.LabelName,
                ReleaseDate = tvm.ReleaseDate,
                PackageType = tvm.PackageType,
                Price = tvm.Price
            };

            StopTrack();
        }

        private void SetControlsVisible(Visibility visibility)
        {
            MainGrid.Visibility = visibility;
        }

        private void PauseTrack()
        {
            Play.IsEnabled = true;
            Play.Content = "Play";

            Pause.IsEnabled = false;
            Pause.Content = "Paused...";
            CurrentTrack.Pause();
        }

        private void PlayTrack()
        {
            Play.IsEnabled = false;
            Play.Content = "Playing...";

            Pause.IsEnabled = true;
            Pause.Content = "Pause";
            CurrentTrack.Play();
        }

        private void StopTrack()
        {
            Play.IsEnabled = true;
            Pause.IsEnabled = false;
            Play.Content = "Play";
            Pause.Content = "Stopped";
            
            CurrentTrack.Stop();
        }

        private void Play_OnClick(object sender, RoutedEventArgs e)
        {
            PlayTrack();
        }

        private void Pause_OnClick(object sender, RoutedEventArgs e)
        {
            PauseTrack();
        }

        private void CurrentTrack_OnMediaEnded(object sender, RoutedEventArgs e)
        {
            StopTrack();
        }
    }
}
