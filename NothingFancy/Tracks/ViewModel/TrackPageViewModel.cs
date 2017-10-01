using System.Globalization;

namespace NothingFancy.Tracks.ViewModel
{
    public class TrackPageViewModel : BaseViewModel
    {
        public string Artist { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public string AudioTrack { get; set; }
        public string Playtime { get; set; }
        public string Popularity { get; set; }
        public string DiscNumber { get; set; }
        public string LicensorName { get; set; }
        public string LabelName { get; set; }
        public string ReleaseDate { get; set; }
        public string PackageType { get; set; }
        public string Price { get; set; }

        public bool IsPaused { get; set; }
        public bool IsPlaying { get; set; }

        public void Pause()
        {
            IsPaused = true;
            IsPlaying = false;
        }

        public void Play()
        {
            IsPaused = false;
            IsPlaying = true;
        }
    }
}