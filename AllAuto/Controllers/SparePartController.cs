using AllAuto.Domain.Entity;
using AllAuto.Domain.Enum;
using AllAuto.Domain.Response;
using AllAuto.Domain.ViewModels.SparePart;
using AllAuto.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AllAuto.Controllers
{
    public class SparePartController : Controller
    {
        private readonly ISparePartService _sparePartService;
        private readonly IExcelReaderService<SparePart> _excelReaderService;

        public SparePartController(ISparePartService carService,
            IExcelReaderService<SparePart> excelReaderService)
        {
            _sparePartService = carService;
            _excelReaderService = excelReaderService;
        }

        /// <summary>
        /// Общий поиск
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAllParts()
        {
            var response = await _sparePartService.GetParts();
            if(response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return View(response.Data);
            }

            return RedirectToAction("Error");
        }

        /// <summary>
        /// Поиск по типу
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetPartsToType(int type)
        {
            var response = await _sparePartService.GetPartsToType((TypePart)type);           
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return View("GetAllParts",response.Data);
            }

            return RedirectToAction("Error");
        }

        [HttpGet]
        public async Task<IActionResult> GetSparePart(int id,bool isJson)
        {
            BaseResponse<SparePartOverviewViewModel> response = await _sparePartService.GetCar(id);
            if(isJson)
            {
                return View(response.Data);
            }

            return PartialView("GetSparePart",response.Data);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _sparePartService.DeleteCar(id);

            if (response.Data)
            {
                return RedirectToAction("GetAllParts");
            }

            return RedirectToAction("Error");
        }

        [HttpGet]
        public async Task<IActionResult> Save(int id)
        {
            if (id == 0)
                return View();

            var response = await _sparePartService.GetCar(id);
            if(response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return View(response.Data);
            }

            return RedirectToAction("Error");
        }

        [HttpPost]
        public async Task<IActionResult> Save(SparePartViewModel model)
        {
            ModelState.Remove("Id");
            ModelState.Remove("Avatar");
            ModelState.Remove("Amount");
            //Модальное окно добавить
            if (ModelState.IsValid)
            {
                if (model.Id == 0)
                {
                    byte[] imageData;

                    if (model.Avatar == null)
                        imageData = Array.Empty<byte>();
                    else
                    {
                        using (var binaryReader = new BinaryReader(model.Avatar.OpenReadStream()))
                        {
                            imageData = binaryReader.ReadBytes((int)model.Avatar.Length);
                        }
                    }

                    await _sparePartService.CreatePart(model,imageData);//создание
                }
                else
                {
                    await _sparePartService.Edit(model.Id,model);
                }         
            }
            return RedirectToAction("GetAllParts");
        }

        [HttpGet]
        public IActionResult LoadFromExcel()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoadFromExcel(IFormFile file)
        {
            var response = await _excelReaderService.ReadExcelFile(file);
            if(response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return Json(new { derscription = response.Description });
            }

                return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [HttpPost]
        public JsonResult GetTypes()
        {
            var types = _sparePartService.GetTypes();
            return Json(types);
        }
    }
}
