using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Models.ViewModels;
using WebApplication1.Repositories;

namespace WebApplication1.Controllers
{
    [Authorize(Roles = "Admin")]
    public class VenuesController : Controller
    {
        private readonly IGenericRepository<Venue> _repo;

        public VenuesController(IGenericRepository<Venue> repo)
        {
            _repo = repo;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return PartialView("_CreatePartial");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VenueVM model)
        {
            if (ModelState.IsValid)
            {
                var venue = new Venue
                {
                    Name = model.Name
                };

                await _repo.CreateAsync(venue);

                return Json(new { success = true, message = "Venue Added Successfully!" });
            }

            return PartialView("_CreatePartial", model);
        }
        
        public async Task<IActionResult> Edit(int id)
        {
            var venue = await _repo.GetByIdAsync(id);
            if (venue == null) return NotFound();

            var vm = new VenueVM
            {
                VenueId = venue.VenueId,
                Name = venue.Name
            };
            return PartialView("_EditPartial", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(VenueVM model)
        {
            if (ModelState.IsValid)
            {
                var venue = await _repo.GetByIdAsync(model.VenueId);
                if (venue == null) return NotFound();

                venue.Name = model.Name;

                await _repo.UpdateAsync(venue);
                return Json(new { success = true, message = "Venue Updated Successfully!" });
            }
            return PartialView("_EditPartial", model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _repo.DeleteAsync(id);
            return Json(new { success = true, message = "Venue Deleted!" });
        }
    }
}