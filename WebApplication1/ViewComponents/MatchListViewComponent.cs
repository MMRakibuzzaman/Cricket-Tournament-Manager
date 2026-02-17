using Microsoft.AspNetCore.Mvc;
using WebApplication1.Repositories;
using WebApplication1.Models;
using WebApplication1.Models.ViewModels;

namespace WebApplication1.ViewComponents
{
    public class MatchListViewComponent : ViewComponent
    {
        private readonly IGenericRepository<Match> _matchRepo;

        public MatchListViewComponent(IGenericRepository<Match> matchRepo)
        {
            _matchRepo = matchRepo;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var matches = await _matchRepo.GetAllAsync(includeProperties: "HomeSquad.Team,AwaySquad.Team,Venue,Tournament");
            return View(matches.OrderBy(m => m.MatchDate).ToList());
        }
    }
}