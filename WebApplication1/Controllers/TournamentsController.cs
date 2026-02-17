using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Models.ViewModels;
using WebApplication1.Repositories;

namespace WebApplication1.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TournamentsController : Controller
    {
        private readonly IGenericRepository<Tournament> _repo;

        public TournamentsController(IGenericRepository<Tournament> repo)
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
        public async Task<IActionResult> Create(TournamentVM model)
        {
            if (ModelState.IsValid)
            {
                var tournament = new Tournament
                {
                    Name = model.Name,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate
                };
                await _repo.CreateAsync(tournament);

                return Json(new { success = true, message = "Tournament Created!" });
            }

            return PartialView("_CreatePartial", model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var t = await _repo.GetByIdAsync(id);
            if (t == null) return NotFound();

            var vm = new TournamentVM
            {
                TournamentId = t.TournamentId,
                Name = t.Name,
                StartDate = t.StartDate,
                EndDate = t.EndDate
            };
            return PartialView("_EditPartial", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TournamentVM model)
        {
            if (ModelState.IsValid)
            {
                var t = await _repo.GetByIdAsync(model.TournamentId);
                if (t == null) return NotFound();

                t.Name = model.Name;
                t.StartDate = model.StartDate;
                t.EndDate = model.EndDate;

                await _repo.UpdateAsync(t);
                return Json(new { success = true, message = "Tournament Updated!" });
            }

            return PartialView("_EditPartial", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _repo.DeleteAsync(id);
            return Json(new { success = true, message = "Tournament Deleted!" });
        }
    }
}