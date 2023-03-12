
using Bloggie.Web.Controllers.Data;
using Bloggie.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;

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

        public async Task<Tag?> DeleteAsync(Guid id)
        {
           var existingTag = await _bloggieDbContext.Tags.FindAsync(id);

       

            if (existingTag != null)
            {
                _bloggieDbContext.Remove(existingTag);
                await _bloggieDbContext.SaveChangesAsync();

                return existingTag;
            }
            return null;
        }

        public async Task<IEnumerable<Tag>> GetAllAsync()
        {
            return await _bloggieDbContext.Tags.ToListAsync();
        }

        public async Task<Tag?> GetAsync(Guid id)
        {
            return await _bloggieDbContext.Tags.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Tag?> UpdateAsync(Tag tag)
        {
            var existingTag = await _bloggieDbContext.Tags.FindAsync(tag.Id);

            if (existingTag != null)
            {
                existingTag.Name = tag.Name;
                existingTag.DisplayName = tag.DisplayName;

                await _bloggieDbContext.SaveChangesAsync();

                return existingTag;
            }
            return null;
        }
    }
}
