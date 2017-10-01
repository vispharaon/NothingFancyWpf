namespace NothingFancy.Data.Dto
{
    public class TrackDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public TrackArtistDto  Artist { get; set; }
        public int Duration { get; set; }
        public double Popularity { get; set; }
        public int DiscNumber { get; set; }
        public ReleaseDto Release { get; set; }
        public DownloadDto Download { get; set; }
    }
}