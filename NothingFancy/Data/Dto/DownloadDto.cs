using System;

namespace NothingFancy.Data.Dto
{
    public class DownloadDto
    {
        public DateTime ReleaseDate { get; set; }
        public DateTime PreviewDate { get; set; }
        public PackagesDto[] Packages { get; set; }
    }
}