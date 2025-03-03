//using Data.Entities;
//using Data.Interface;

//namespace Data.Repositories
//{
//    public class UOW : IUOW
//    {
//        private bool disposed = false;
//        private readonly NewsManagementDbContext _dbContext;
//        public UOW(NewsManagementDbContext dbContext)
//        {
//            _dbContext = dbContext;
//        }

//        public IGenericRepository<T> GetRepository<T>() where T : class
//        {
//            return new GenericRepository<T>(_dbContext);
//        }
//        public async Task SaveAsync()
//        {
//            await _dbContext.SaveChangesAsync();
//        }
//        public void Save()
//        {
//            _dbContext.SaveChanges();
//        }
//        protected virtual void Dispose(bool disposing)
//        {
//            if (!disposed)
//            {
//                if (disposing)
//                {
//                    _dbContext.Dispose();
//                }
//            }
//            disposed = true;
//        }
//        public void Dispose()
//        {
//            Dispose(true);
//            GC.SuppressFinalize(this);
//        }
//        public void BeginTransaction()
//        {
//            _dbContext.Database.BeginTransaction();
//        }

//        public void CommitTransaction()
//        {
//            _dbContext.Database.CommitTransaction();
//        }

//        public void RollBack()
//        {
//            _dbContext.Database.RollbackTransaction();
//        }
//    }
//}
