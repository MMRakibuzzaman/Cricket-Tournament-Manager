using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication1.Models;
using WebApplication1.Models.ViewModels;
using WebApplication1.Repositories;

namespace WebApplication1.Controllers
{
    [Authorize(Roles = "Admin")]
    public class MatchesController : Controller
    {
        private readonly IGenericRepository<Match> _matchRepo;
        private readonly IGenericRepository<Tournament> _tournRepo;
        private readonly IGenericRepository<Venue> _venueRepo;
        private readonly IGenericRepository<TeamSquad> _squadRepo;

        public MatchesController(
            IGenericRepository<Match> matchRepo,
            IGenericRepository<Tournament> tournRepo,
            IGenericRepository<Venue> venueRepo,
            IGenericRepository<TeamSquad> squadRepo)
        {
            _matchRepo = matchRepo;
            _tournRepo = tournRepo;
            _venueRepo = venueRepo;
            _squadRepo = squadRepo;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Create()
        {
            var model = new MatchCreateVM();
            await PopulateDropdowns(model);
            return PartialView("_CreatePartial", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MatchCreateVM model)
        {
            if (ModelState.IsValid)
            {
                if (model.HomeSquadId == model.AwaySquadId)
                {
                    ModelState.AddModelError("AwaySquadId", "Home and Away teams must be different.");
                }
                else
                {
                    var match = new Match
                    {
                        TournamentId = model.TournamentId,
                        VenueId = model.VenueId,
                        HomeSquadId = model.HomeSquadId,
                        AwaySquadId = model.AwaySquadId,
                        MatchDate = model.MatchDate,
                        Status = MatchStatus.Scheduled
                    };

                    await _matchRepo.CreateAsync(match);
                    return Json(new { success = true, message = "Match Scheduled Successfully!" });
                }
            }

            await PopulateDropdowns(model);
            return PartialView("_CreatePartial", model);
        }

        private async Task PopulateDropdowns(MatchCreateVM model)
        {
            var tournaments = await _tournRepo.GetAllAsync();
            var venues = await _venueRepo.GetAllAsync();

            var squads = await _squadRepo.GetAllAsync(includeProperties: "Team");

            model.Tournaments = tournaments.Select(t => new SelectListItem
            {
                Value = t.TournamentId.ToString(),
                Text = t.Name
            });

            model.Venues = venues.Select(v => new SelectListItem
            {
                Value = v.VenueId.ToString(),
                Text = v.Name
            });

            model.Squads = squads.Select(s => new SelectListItem
            {
                Value = s.SquadId.ToString(),
                Text = s.Team?.Name ?? $"Squad {s.SquadId}"
            });
        }

        public async Task<IActionResult> Edit(int id)
        {
            var match = await _matchRepo.GetByIdAsync(id);
            if (match == null) return NotFound();

            var vm = new MatchCreateVM
            {
                TournamentId = match.TournamentId,
                VenueId = match.VenueId,
                HomeSquadId = match.HomeSquadId,
                AwaySquadId = match.AwaySquadId,
                MatchDate = match.MatchDate
            };

            ViewData["MatchId"] = id;

            await PopulateDropdowns(vm);
            return PartialView("_EditPartial", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MatchCreateVM model)
        {
            if (ModelState.IsValid)
            {
                if (model.HomeSquadId == model.AwaySquadId)
                {
                    ModelState.AddModelError("AwaySquadId", "Home and Away teams must be different.");
                }
                else
                {
                    var match = await _matchRepo.GetByIdAsync(id);
                    if (match == null) return NotFound();

                    match.TournamentId = model.TournamentId;
                    match.VenueId = model.VenueId;
                    match.HomeSquadId = model.HomeSquadId;
                    match.AwaySquadId = model.AwaySquadId;
                    match.MatchDate = model.MatchDate;

                    await _matchRepo.UpdateAsync(match);
                    return Json(new { success = true, message = "Match Rescheduled!" });
                }
            }

            ViewData["MatchId"] = id;
            await PopulateDropdowns(model);
            return PartialView("_EditPartial", model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _matchRepo.DeleteAsync(id);
            return Json(new { success = true, message = "Match Cancelled!" });
        }
    }
}