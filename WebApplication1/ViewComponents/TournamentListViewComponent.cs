using Microsoft.AspNetCore.Mvc;
using WebApplication1.Repositories;
using WebApplication1.Models;
using WebApplication1.Models.ViewModels;

namespace WebApplication1.ViewComponents
{
    public class TournamentListViewComponent : ViewComponent
    {
        private readonly IGenericRepository<Tournament> _repo;

        public TournamentListViewComponent(IGenericRepository<Tournament> repo)
        {
            _repo = repo;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var tournaments = await _repo.GetAllAsync();
            
            var model = tournaments.Select(t => new TournamentVM
            {
                TournamentId = t.TournamentId,
                Name = t.Name,
                StartDate = t.StartDate,
                EndDate = t.EndDate
            }).OrderByDescending(t => t.StartDate).ToList();

            return View(model);
        }
    }
}