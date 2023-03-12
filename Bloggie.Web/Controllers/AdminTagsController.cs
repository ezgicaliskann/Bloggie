using Bloggie.Web.Controllers.Data;
using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Controllers
{
    public class AdminTagsController : Controller
    {
        private readonly ITagInterface tagRepository;
        //dependency injection --> daha öncesinde dbcontext'e özgü bir class tanımladık ve program.cs içerisinde bu classı servis özelliklerinden uygulamamıza tanıttık. bunu yapmaktaki amacımız ihtiyaç duyulan herhangi bir nesne içerisinde ihtiyaç duyduğumuz bu nesneyi çağırabilmek. ve nesnenin içerisinde yeniden bu dbcontexti oluşturmadan mevcutta olan dbcontext üzerine erişim sağlamaktı. yani dbcontextimizi istediğimiz her classa enjekte edebiliyoruz. bu işleme dependency injection denmektedir. bu örnekte bunu oluşturduk ve parametre olarak dbcontext nesnemizden bir argüman yolladık bu argüman classın tamamında kullanılamayacağı için bir private field oluşturduk ve bu field ile constructordan gelen argümanı eşitleyerek classımız içerisinde fieldımız üzerinden dbcontext nesnesini kullanabiliyor hale geldik. 

        public AdminTagsController(ITagInterface tagRepository)
        {
            this.tagRepository = tagRepository;
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

            await tagRepository.AddAsync(tag);
           
            return RedirectToAction("List");
        }

        //tagleri listeleme metodu
        [HttpGet]
        public async Task<IActionResult> List()
        {
            var tags = await tagRepository.GetAllAsync();
            return View(tags);
        }

        //edit sayfasının görüntülenme (GET) actionu
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            //1.metot
            //     var tag = _bloggieDbContext.Tags.Find(id);
            //2.metot
            var tag = await tagRepository.GetAsync(id);

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

        [HttpPost]

        public async Task<IActionResult> Edit(EditTagRequest editTagRequest)
        {
            var tag = new Tag
            {
                Id = editTagRequest.Id,
                DisplayName = editTagRequest.DisplayName,
                Name = editTagRequest.Name,
            };

            var updatedTag = await tagRepository.UpdateAsync(tag);
            if (updatedTag != null)
            {
                
                return RedirectToAction ("List");
            }
            else
            {
                
            }

            return RedirectToAction("Edit", new { id = editTagRequest.Id });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(EditTagRequest editTagRequest)
        {
            var deletedTag = await tagRepository.DeleteAsync
                (editTagRequest.Id);
            if(deletedTag != null)
            {
                //success notification
                return RedirectToAction("List");
            }

            //error notification
            return RedirectToAction("Edit", new { id = editTagRequest.Id });
        }

    }
    
}
