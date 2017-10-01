using System.Collections.ObjectModel;
using NothingFancy.Controls;
using NothingFancy.Data;
using NothingFancy.Tracks.ViewModel;

namespace NothingFancy.Tracks.DesignModel
{
    public class TrackListDesignModel : TrackListViewModel
    {

        public static TrackListDesignModel Instance => new TrackListDesignModel();

        public TrackListDesignModel() : base(new TrackRepository())
        {
            Items = new ObservableCollection<TrackViewModel>
            {
                new TrackDesignModel
                {
                    Artist = "Abba",
                    Title = "The winner takes it all",
                    PlayTime = "4:56"
                },
                new TrackDesignModel
                {
                    Artist = "Beatles",
                    Title = "Yesterday",
                    PlayTime = "2:00",
                    IsSelected = true
                },
                new TrackDesignModel
                {
                    Artist = "Ed Sheeran",
                    Title = "Shape of you",
                    PlayTime = "4:23"
                },
                new TrackDesignModel
                {
                    Artist = "Abba",
                    Title = "The winner takes it all",
                    PlayTime = "4:56"
                },
                new TrackDesignModel
                {
                    Artist = "Beatles",
                    Title = "Yesterday",
                    PlayTime = "2:00"
                },
                new TrackDesignModel
                {
                    Artist = "Ed Sheeran",
                    Title = "Shape of you",
                    PlayTime = "4:23"
                },
                new TrackDesignModel
                {
                    Artist = "Abba",
                    Title = "The winner takes it all",
                    PlayTime = "4:56"
                },
                new TrackDesignModel
                {
                    Artist = "Beatles",
                    Title = "Yesterday",
                    PlayTime = "2:00"
                },
                new TrackDesignModel
                {
                    Artist = "Ed Sheeran",
                    Title = "Shape of you",
                    PlayTime = "4:23"
                }
            };
        }
    }
}