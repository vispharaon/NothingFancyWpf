using NothingFancy.Tracks.ViewModel;

namespace NothingFancy.Tracks.DesignModel
{
    public class TrackDesignModel : TrackViewModel
    {

        public static TrackDesignModel Instance => new TrackDesignModel();
        
        public TrackDesignModel()
        {
            Title = "This is title of track";
            Artist = "Artist name goes here";
            PlayTime = "1:01";
        }
    }
}