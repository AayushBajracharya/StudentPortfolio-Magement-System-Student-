
using Application.Interfaces.Repositories.UploadFileMorphRepository;
using Infrastructure.Persistence.Contexts;


namespace Infrastructure.Persistence.Repositories.UploadFileMorphRepository
{
    public class UploadFileMorphRepository : Repository<UploadFileMorph>, IUploadFileMorphRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public UploadFileMorphRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            this._applicationDbContext = dbContext;
        }
    }
}
