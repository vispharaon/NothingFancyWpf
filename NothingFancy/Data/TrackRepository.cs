using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NothingFancy.Data.Model;

namespace NothingFancy.Data
{
    public interface ITrackRepository
    {
        List<Track> GetTracks(string queryString, int pageSize);
        string GetPreviewSource(int trackId);
    }

    public class TrackRepository : ITrackRepository
    {
        public List<Track> GetTracks(string queryString, int pageSize)
        {
            //var tempTrack = HandleApi.GetPreviewTrackSource(63807806);
            var tracks = HandleApi.GetListOfTracks(queryString, pageSize);

            if(tracks == null || tracks.SearchResults == null) return new List<Track>();

            return tracks.SearchResults.SearchResult.Select(x => new Track
            {
                Artist = x.Track.Artist.Name,
                Title = x.Track.Title,
                PlayTime = FormatPlaytime(x.Track.Duration),
                Name = "unused",
                Image = x.Track.Artist.Image,
                TrackId = x.Track.Id,
                Popularity = x.Track.Popularity.ToString(CultureInfo.InvariantCulture),
                DiscNumber = x.Track.DiscNumber.ToString(),
                LicensorName = x.Track.Release.Licensor.Name,
                LabelName = x.Track.Release.Label.Name,
                ReleaseDate = x.Track.Download != null ? x.Track.Download.ReleaseDate.ToShortDateString() : "no data",
                PackageType = x.Track.Download != null ? x.Track.Download.Packages.First().Description : "no data",
                Price = x.Track.Download != null ? x.Track.Download.Packages.First().Price.CurrencyCode + " " +
                        (x.Track.Download.Packages.First().Price.RecommendedRetailPrice.HasValue
                            ? x.Track.Download.Packages.First().Price.RecommendedRetailPrice.Value.ToString("0.###")
                            : "-") : "no data"
            }).ToList();
        }

        public string GetPreviewSource(int trackId)
        {
            return HandleApi.GetPreviewTrackSource(trackId);
        }

        private string FormatPlaytime(int playtime)
        {
            TimeSpan time = TimeSpan.FromSeconds(playtime);

            //here backslash is must to tell that colon is
            //not the part of format, it just a character that we want in output
            return time.ToString(@"mm\:ss");
        }
    }
}