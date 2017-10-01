namespace NothingFancy.Data.Dto
{
    public class ReleaseDto
    {
        public int Id { get; set; }
        public LicensorDto Licensor { get; set; }
        public LabelDto Label { get; set; }
    }
}