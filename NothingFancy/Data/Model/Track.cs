namespace NothingFancy.Data.Model
{
    /// <summary>
    /// Holds data from after api call
    /// </summary>
    public class Track
    {
        public int TrackId { get; set; }
        public string Artist { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string PlayTime { get; set; }
        public string Image { get; set; }
        public string Popularity { get; set; }
        public string DiscNumber { get; set; }
        public string LicensorName { get; set; }
        public string LabelName { get; set; }
        public string ReleaseDate { get; set; }
        public string PackageType { get; set; }
        public string Price { get; set; }
    }
}
