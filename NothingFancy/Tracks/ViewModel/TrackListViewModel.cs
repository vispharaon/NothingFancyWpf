using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using NothingFancy.Controls;
using NothingFancy.Data;

namespace NothingFancy.Tracks.ViewModel
{
    public class TrackListViewModel : BaseViewModel
    {
        private readonly ITrackRepository _trackRepository;

        #region Public properties

        /// <summary>
        /// Items that will be presented in the list of tracks
        /// </summary>
        public ObservableCollection<TrackViewModel> Items { get; set; }
        public TrackViewModel SelectedAudio { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor of track list VM
        /// </summary>
        /// <param name="trackRepository"></param>
        /// <param name="smc"></param>
        public TrackListViewModel(ITrackRepository trackRepository)
        {
            _trackRepository = trackRepository;

            Items = GetSearchResult("Happy", Constants.NumberOfTracksShown);

            //SelectedAudio = Items[0].PreviewSource;
            //_smc.SelectedAudio = Items[0];
        }

        #endregion

        public int Refresh(string searchTerm)
        {
            var searchResult = GetSearchResult(searchTerm, Constants.NumberOfTracksShown);

            var i = 0;
            foreach (var res in searchResult)
            {
                Items[i] = res;
                i++;
            }

            return searchResult.Count;
        }

        public void RefreshCurrent()
        {
            var i = 0;
            foreach (var res in Items.ToArray())
            {
                Items[i++] = new TrackViewModel
                {
                    Artist = res.Artist,
                    Title = res.Title,
                    IsSelected = res.IsSelected,
                    PlayTime = res.PlayTime,
                    Image = res.Image,
                    Name = res.Name,
                    TrackId = res.TrackId,
                    Popularity = res.Popularity,
                    DiscNumber = res.DiscNumber,
                    LicensorName = res.LicensorName,
                    LabelName = res.LabelName,
                    PackageType = res.PackageType,
                    Price = res.Price,
                    ReleaseDate = res.ReleaseDate
                };

                if (res.IsSelected) SelectedAudio = res;
            }
        }

        #region Private methods

        private ObservableCollection<TrackViewModel> GetSearchResult(string queryString, int pageSize)
        {
            var tracks = _trackRepository.GetTracks(queryString, pageSize);

            return new ObservableCollection<TrackViewModel>(tracks.Select(x => new TrackViewModel
            {
                PlayTime = x.PlayTime,
                Name = x.Name,
                Title = x.Title,
                Artist = x.Artist,
                TrackId = x.TrackId,
                Image = x.Image,
                Popularity = x.Popularity,
                DiscNumber = x.DiscNumber,
                LicensorName = x.LicensorName,
                LabelName = x.LabelName,
                PackageType = x.PackageType,
                ReleaseDate = x.ReleaseDate,
                Price = x.Price
            }));
        }

        #endregion
        
    }
}