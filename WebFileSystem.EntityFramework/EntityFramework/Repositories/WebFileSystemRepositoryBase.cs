using Abp.Domain.Entities;
using Abp.EntityFramework;
using Abp.EntityFramework.Repositories;

namespace WebFileSystem.EntityFramework.Repositories
{
    public abstract class WebFileSystemRepositoryBase<TEntity, TPrimaryKey> : EfRepositoryBase<WebFileSystemDbContext, TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        protected WebFileSystemRepositoryBase(IDbContextProvider<WebFileSystemDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        //add common methods for all repositories
    }

    public abstract class WebFileSystemRepositoryBase<TEntity> : WebFileSystemRepositoryBase<TEntity, int>
        where TEntity : class, IEntity<int>
    {
        protected WebFileSystemRepositoryBase(IDbContextProvider<WebFileSystemDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        //do not add any method here, add to the class above (since this inherits it)
    }
}
