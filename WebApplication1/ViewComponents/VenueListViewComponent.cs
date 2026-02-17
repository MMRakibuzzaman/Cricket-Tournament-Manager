using Microsoft.AspNetCore.Mvc;
using WebApplication1.Repositories;
using WebApplication1.Models;
using WebApplication1.Models.ViewModels;

namespace WebApplication1.ViewComponents
{
    public class VenueListViewComponent : ViewComponent
    {
        private readonly IGenericRepository<Venue> _repo;

        public VenueListViewComponent(IGenericRepository<Venue> repo)
        {
            _repo = repo;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var venues = await _repo.GetAllAsync();
            
            var model = venues.Select(v => new VenueVM
            {
                VenueId = v.VenueId,
                Name = v.Name
            }).ToList();

            return View(model);
        }
    }
}