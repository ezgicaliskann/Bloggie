using Bloggie.Web.Controllers.Data;
using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Controllers
{
    public class AdminTagsController : Controller
    {
        private readonly BloggieDbContext _bloggieDbContext;
        //dependency injection --> daha öncesinde dbcontext'e özgü bir class tanımladık ve program.cs içerisinde bu classı servis özelliklerinden uygulamamıza tanıttık. bunu yapmaktaki amacımız ihtiyaç duyulan herhangi bir nesne içerisinde ihtiyaç duyduğumuz bu nesneyi çağırabilmek. ve nesnenin içerisinde yeniden bu dbcontexti oluşturmadan mevcutta olan dbcontext üzerine erişim sağlamaktı. yani dbcontextimizi istediğimiz her classa enjekte edebiliyoruz. bu işleme dependency injection denmektedir. bu örnekte bunu oluşturduk ve parametre olarak dbcontext nesnemizden bir argüman yolladık bu argüman classın tamamında kullanılamayacağı için bir private field oluşturduk ve bu field ile constructordan gelen argümanı eşitleyerek classımız içerisinde fieldımız üzerinden dbcontext nesnesini kullanabiliyor hale geldik. 

        public AdminTagsController(BloggieDbContext bloggieDbContext)
        {
            this._bloggieDbContext = bloggieDbContext;
        }

        //tag ekleme get metodu

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            return View();
        }

        //tag ekleme post metodu
        [HttpPost]
        [ActionName("Add")]
        public async Task<IActionResult> Add(AddTagRequest addTagRequest)
        {
            //var name = Request.Form["name"];
            //var displayName = Request.Form["displayName"];

            //var name = addTagRequest.Name;
            //var display = addTagRequest.DisplayName;

            var tag = new Tag
            {
                Name = addTagRequest.Name,
                DisplayName = addTagRequest.DisplayName
            };
           await _bloggieDbContext.AddAsync(tag);
           await _bloggieDbContext.SaveChangesAsync();
            return RedirectToAction("List");
        }

        //tagleri listeleme metodu
        [HttpGet]
        public async Task<IActionResult> List()
        {
            var tags = await _bloggieDbContext.Tags.ToListAsync();
            return View(tags);
        }

        //edit sayfasının görüntülenme (GET) actionu
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            //1.metot
            //     var tag = _bloggieDbContext.Tags.Find(id);
            //2.metot
            var tag = await _bloggieDbContext.Tags.FirstOrDefaultAsync(t => t.Id == id);

            if (tag != null)
            {

                var editTagReq = new EditTagRequest
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    DisplayName = tag.DisplayName,
                };
                return View(editTagReq);
            }

            return View(null);
        }

        public async Task<IActionResult> Edit(EditTagRequest editTagRequest)
        {
            var tag = new Tag
            {
                Id = editTagRequest.Id,
                DisplayName = editTagRequest.DisplayName,
                Name = editTagRequest.Name,
            };

            var existingTag = await _bloggieDbContext.Tags.FindAsync(tag.Id);

            if (existingTag != null)
            {
                existingTag.Name = tag.Name;
                existingTag.DisplayName = tag.DisplayName;

               await _bloggieDbContext.SaveChangesAsync();

                //return RedirectToAction("Edit", new { id = editTagRequest.Id });
                return RedirectToAction ("List" ,new { id = existingTag.Id });
            }

            return RedirectToAction("Edit", new { id = editTagRequest.Id });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(EditTagRequest editTagRequest)
        {
            var tag = await _bloggieDbContext.Tags.FindAsync(editTagRequest.Id);

            if(tag != null)
            {
                _bloggieDbContext.Remove(tag);
               await _bloggieDbContext.SaveChangesAsync();

                return RedirectToAction("List");
            }
            return RedirectToAction("Edit", new { id = editTagRequest.Id });
        }

    }
    
}
