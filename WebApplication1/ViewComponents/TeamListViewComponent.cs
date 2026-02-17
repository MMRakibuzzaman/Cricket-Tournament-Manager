using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Repositories;

namespace WebApplication1.ViewComponents
{
    public class TeamListViewComponent : ViewComponent
    {
        private readonly IGenericRepository<Team> _repo;

        public TeamListViewComponent(IGenericRepository<Team> repo)
        {
            _repo = repo;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var teams = await _repo.GetAllAsync(includeProperties: "Players");

            return View(teams.OrderBy(t => t.Name).ToList());
        }
    }
}