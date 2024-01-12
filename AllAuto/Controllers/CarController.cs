using AllAuto.DAL.Interfaces;
using AllAuto.Domain.Entity;
using AllAuto.Domain.Response;
using AllAuto.Domain.ViewModels.Car;
using AllAuto.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AllAuto.Controllers
{
    public class CarController : Controller
    {
        private readonly ICarService _carService;

        public CarController(ICarService carService)
        {
            _carService = carService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCars()
        {
            var response = await _carService.GetCars();
            if(response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return View(response.Data);
            }

            return RedirectToAction("Error");
        }

        [HttpGet]
        public async Task<IActionResult> GetCar(int id,bool isJson)
        {
            BaseResponse<Car> response = await _carService.GetCar(id);
            if(isJson)
            {
                return View(response.Data);
            }

            return PartialView("GetCar",response.Data);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _carService.DeleteCar(id);

            if (response.Data)
            {
                return RedirectToAction("GetCars");
            }

            return RedirectToAction("Error");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Save(int id)
        {
            if (id == 0)
                return View();

            var response = await _carService.GetCar(id);
            if(response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return View(response.Data);
            }

            return RedirectToAction("Error");
        }

        [HttpPost]
        public async Task<IActionResult> Save(CarViewModel model)
        {
            if(ModelState.IsValid)
            {
                if(model.Id == 0)
                {
                    byte[] imageData;
                    using(var binaryReader = new BinaryReader(model.Avatar.OpenReadStream()))
                    {
                        imageData = binaryReader.ReadBytes((int)model.Avatar.Length);
                    }

                    await _carService.CreateCar(model,imageData);//создание
                }
                else
                {
                    await _carService.Edit(model.Id,model);
                }
                return RedirectToAction("GetCars");
            }
            return View();
            
        }

        [HttpPost]
        public JsonResult GetTypes()
        {
            var types = _carService.GetTypes;
            return Json(types);
        }
    }
}
