using Microsoft.AspNetCore.Mvc;
using WebApplication1.Repositories;
using WebApplication1.Models;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.ViewComponents
{
    public class TeamSquadListViewComponent : ViewComponent
    {
        private readonly IGenericRepository<TeamSquad> _repo;

        public TeamSquadListViewComponent(IGenericRepository<TeamSquad> repo)
        {
            _repo = repo;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var squads = await _repo.GetAllAsync(includeProperties: "Team,Tournament");
            return View(squads.OrderBy(s => s.TournamentId).ToList());
        }
    }
}