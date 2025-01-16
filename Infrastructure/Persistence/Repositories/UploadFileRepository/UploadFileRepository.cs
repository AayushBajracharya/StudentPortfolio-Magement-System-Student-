using Application.Interfaces.Repositories.UploadFileRepository;
using Infrastructure.Persistence.Contexts;

namespace Infrastructure.Persistence.Repositories.UploadFileRepository
{
    public class UploadFileRepository : Repository<UploadFile>, IUploadFileRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public UploadFileRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            this._applicationDbContext = dbContext;
        }
    }
}
