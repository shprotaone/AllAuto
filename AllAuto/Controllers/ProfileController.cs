﻿using AllAuto.Domain.Response;
using AllAuto.Domain.ViewModels.Profile;
using AllAuto.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AllAuto.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IProfileService _profileService;

        public ProfileController(IProfileService profileService)
        {
            _profileService = profileService;
        }

        [HttpPost]
        public async Task<IActionResult> Save(ProfileViewModel model)
        {
            ModelState.Remove("Id");
            ModelState.Remove("UserName");
            ModelState.Remove("NewPassword");
            if (ModelState.IsValid)
            {
                var response = await _profileService.Save(model);
                if(response.StatusCode == Domain.Enum.StatusCode.OK)
                {                   
                    return Json(new {description = response.Description});
                }
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        public async Task<IActionResult> Detail() 
        {
            string username = User.Identity.Name;
            BaseResponse<ProfileViewModel> response = await _profileService.GetProfile(username);

            if(response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return View(response.Data);
            }
            
            return View();
        }
    }
}
