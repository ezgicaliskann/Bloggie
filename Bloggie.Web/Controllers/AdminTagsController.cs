using Bloggie.Web.Controllers.Data;
using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult Add()
        {
            return View();
        }

        //tag ekleme post metodu
        [HttpPost]
        [ActionName("Add")]
        public IActionResult Add(AddTagRequest addTagRequest)
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
            _bloggieDbContext.Add(tag);
            _bloggieDbContext.SaveChanges();
            return RedirectToAction("List");
        }

        //tagleri listeleme metodu
        [HttpGet]
        public IActionResult List()
        {
            var tags = _bloggieDbContext.Tags.ToList();
            return View(tags);
        }

        //edit sayfasının görüntülenme (GET) actionu
        [HttpGet]
        public IActionResult Edit(Guid id)
        {
            //1.metot
            //     var tag = _bloggieDbContext.Tags.Find(id);
            //2.metot
            var tag = _bloggieDbContext.Tags.FirstOrDefault(t => t.Id == id);

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

        public IActionResult Edit(EditTagRequest editTagRequest)
        {
            var tag = new Tag
            {
                Id = editTagRequest.Id,
                DisplayName = editTagRequest.DisplayName,
                Name = editTagRequest.Name,
            };

            var existingTag = _bloggieDbContext.Tags.Find(tag.Id);

            if (existingTag != null)
            {
                existingTag.Name = tag.Name;
                existingTag.DisplayName = tag.DisplayName;

                _bloggieDbContext.SaveChanges();

                //return RedirectToAction("Edit", new { id = editTagRequest.Id });
                return RedirectToAction ("List" ,new { id = existingTag.Id });
            }

            return RedirectToAction("Edit", new { id = editTagRequest.Id });
        }
    }
}
