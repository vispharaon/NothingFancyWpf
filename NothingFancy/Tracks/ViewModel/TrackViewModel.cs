namespace NothingFancy.Tracks.ViewModel
{
    /// <summary>
    /// This is view model for each item from the list of tracks
    /// </summary>
    public class TrackViewModel : BaseViewModel
    {
        public int TrackId { get; set; }
        public string Artist { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string PlayTime { get; set; }
        public string Image { get; set; }
        public bool IsSelected { get; set; }
        public string Popularity { get; set; }
        public string DiscNumber { get; set; }
        public string LicensorName { get; set; }
        public string LabelName { get; set; }
        public string ReleaseDate { get; set; }
        public string PackageType { get; set; }
        public string Price { get; set; }
        public override string ToString()
        {
            return Artist;
        }
    }
}