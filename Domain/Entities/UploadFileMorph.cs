

namespace Domain.Entities
{
    public class UploadFileMorph
    {
        public virtual int Id { get; set; }
        public int UploadFileId { get; set; }
        public int RelatedId { get; set; }
        public string RelatedType { get; set; }
        public string Field { get; set; }

        public virtual UploadFile UploadFile { get; set; }
    }
}
