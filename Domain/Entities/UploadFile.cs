

namespace Domain.Entities
{
    public class UploadFile
    {
        public virtual int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string AlternativeText { get; set; } = string.Empty;

        public string Url { get; set; } = string.Empty;
        public string Hash { get; set; } = string.Empty;

        public string Ext { get; set; } = string.Empty;

        public string Mime { get; set; } = string.Empty;

        public double Size { get; set; }

        public virtual ICollection<UploadFileMorph> UploadFileMorph { get; set; }
    }
}
