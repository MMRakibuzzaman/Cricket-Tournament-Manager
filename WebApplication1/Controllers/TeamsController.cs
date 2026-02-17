using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Models.ViewModels;
using WebApplication1.Repositories;

namespace WebApplication1.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TeamsController : Controller
    {
        private readonly IGenericRepository<Team> _teamRepo;
        private readonly IGenericRepository<Player> _playerRepo;
        private readonly IWebHostEnvironment _webHost;

        public TeamsController(IGenericRepository<Team> teamRepo,
                               IGenericRepository<Player> playerRepo,
                               IWebHostEnvironment webHost)
        {
            _teamRepo = teamRepo;
            _playerRepo = playerRepo;
            _webHost = webHost;
        }

        public IActionResult CreateWithRoster()
        {
            var model = new TeamMasterDetailVM();
            model.Players.Add(new PlayerEntryVM());
            return PartialView("_CreateWithRoster", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateWithRoster(TeamMasterDetailVM model)
        {
            if (ModelState.IsValid)
            {
                string? logoPath = await UploadLogo(model.LogoFile);

                var team = new Team
                {
                    Name = model.Name,
                    LogoUrl = logoPath,
                    EstablishedDate = model.EstablishedDate,
                    Revenue = model.Revenue,
                    IsActive = model.IsActive
                };

                await _teamRepo.CreateAsync(team); 

                if (model.Players != null && model.Players.Count > 0)
                {
                    foreach (var p in model.Players)
                    {
                        if (string.IsNullOrWhiteSpace(p.FullName)) continue;

                        var player = new Player
                        {
                            FullName = p.FullName,
                            Role = p.Role,
                            DateOfBirth = p.DateOfBirth,
                            TeamId = team.TeamId 
                        };
                        await _playerRepo.CreateAsync(player);
                    }
                }

                return Json(new { success = true, message = "Team & Roster Created Successfully!" });
            }
            return PartialView("_CreateWithRoster", model);
        }

        [HttpPost]
        public async Task<IActionResult> DeletePlayer(int id)
        {
            await _playerRepo.DeleteAsync(id);
            return Json(new { success = true, message = "Player removed." });
        }

        public async Task<IActionResult> Edit(int id)
        {
            var teams = await _teamRepo.FindAsync(t => t.TeamId == id, includeProperties: "Players");
            var team = teams.FirstOrDefault();

            if (team == null) return NotFound();

            var vm = new TeamMasterDetailVM
            {
                TeamId = team.TeamId,
                Name = team.Name,
                CurrentLogoUrl = team.LogoUrl,
                EstablishedDate = team.EstablishedDate,
                Revenue = team.Revenue,
                IsActive = team.IsActive,
                Players = team.Players.Select(p => new PlayerEntryVM
                {
                    PlayerId = p.PlayerId,
                    FullName = p.FullName,
                    Role = p.Role,
                    DateOfBirth = p.DateOfBirth
                }).ToList()
            };

            return PartialView("_EditPartial", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TeamMasterDetailVM model)
        {
            if (ModelState.IsValid)
            {
                var team = await _teamRepo.GetByIdAsync(model.TeamId);
                if (team == null) return NotFound();

                team.Name = model.Name;
                team.EstablishedDate = model.EstablishedDate;
                team.Revenue = model.Revenue;
                team.IsActive = model.IsActive;

                if (model.LogoFile != null)
                {
                    team.LogoUrl = await UploadLogo(model.LogoFile);
                }

                await _teamRepo.UpdateAsync(team);

                if (model.Players != null)
                {
                    foreach (var p in model.Players)
                    {
                        if (string.IsNullOrWhiteSpace(p.FullName)) continue;

                        if (p.PlayerId > 0)
                        {
                            var existingPlayer = await _playerRepo.GetByIdAsync(p.PlayerId);
                            if (existingPlayer != null)
                            {
                                existingPlayer.FullName = p.FullName;
                                existingPlayer.Role = p.Role;
                                existingPlayer.DateOfBirth = p.DateOfBirth;
                                await _playerRepo.UpdateAsync(existingPlayer);
                            }
                        }
                        else
                        {
                            var newPlayer = new Player
                            {
                                FullName = p.FullName,
                                Role = p.Role,
                                DateOfBirth = p.DateOfBirth,
                                TeamId = team.TeamId
                            };
                            await _playerRepo.CreateAsync(newPlayer);
                        }
                    }
                }

                return Json(new { success = true, message = "Team & Roster Updated!" });
            }
            return PartialView("_EditPartial", model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var players = await _playerRepo.FindAsync(p => p.TeamId == id);

            foreach (var player in players)
            {
                await _playerRepo.DeleteAsync(player.PlayerId);
            }

            await _teamRepo.DeleteAsync(id);

            return Json(new { success = true, message = "Team and Roster Deleted!" });
        }

        private async Task<string> UploadLogo(IFormFile? file)
        {
            if (file == null) return null;

            string uploadsFolder = Path.Combine(_webHost.WebRootPath, "images/teams");
            if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

            string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
            return "/images/teams/" + uniqueFileName;
        }
    }
}