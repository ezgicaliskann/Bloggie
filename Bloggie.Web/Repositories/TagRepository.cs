
using Bloggie.Web.Controllers.Data;
using Bloggie.Web.Models.Domain;

namespace Bloggie.Web.Repositories
{
    //Tag repository isimli classımız daha öncesinde sadece metotların imzalarını tanıtmış olduğumuz interface üzerinden kalıtım alacak ve bu metotların body kısmını dolduracaktır. Fakat en nihayetinde amacımız Dbcontextimizin işlevini yerine getirmek olduğu için bu alanda bir yandan dbcontexi enjekte etmemiz gerekiyor. 
    public class TagRepository : ITagInterface
    {
        private readonly BloggieDbContext _bloggieDbContext;

        public TagRepository(BloggieDbContext _bloggieDbContext)
        {
            this._bloggieDbContext = _bloggieDbContext;
        }

        public async Task<Tag?> AddAsync(Tag tag)
        {
            await _bloggieDbContext.AddAsync(tag);
            await _bloggieDbContext.SaveChangesAsync();
            return tag;
        }

        public Task<Tag?> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Tag>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Tag?> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Tag?> UpdateAsync(Tag tag)
        {
            throw new NotImplementedException();
        }
    }
}
