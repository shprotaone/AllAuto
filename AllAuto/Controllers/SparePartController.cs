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
                List<SparePartShortView> query = new List<SparePartShortView>();
                ConvertToShortView(response, query);
                return View(query.AsQueryable());
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
                List<SparePartShortView> query = new List<SparePartShortView>();
                ConvertToShortView(response, query);
                return View("GetAllParts",query.AsQueryable());
            }

            return RedirectToAction("Error");
        }

        [HttpGet]
        public async Task<IActionResult> GetSparePart(int id,bool isJson)
        {
            BaseResponse<SparePartOverviewViewModel> response = await _sparePartService.GetPart(id);
            if(isJson)
            {
                return View(response.Data);
            }

            return PartialView("GetSparePart",response.Data);
        }
        [HttpGet]
        public async Task<IActionResult> GetPartsToName(string name)
        {
            var response = await _sparePartService.GetPartByName(name);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                List<SparePartShortView> query = new List<SparePartShortView>();
                ConvertToShortView(response, query);
                return View("GetAllParts", query.AsQueryable());
            }

            return PartialView("GetSparePart", response.Data);
        }

        //[Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _sparePartService.Delete(id);

            if (response.Data)
            {
                return RedirectToAction("GetAllParts");
            }

            return RedirectToAction("GetAllParts");
        }

        [HttpGet]
        public async Task<IActionResult> Save(int id)
        {
            if (id == 0)
                return View();

            var response = await _sparePartService.GetPart(id);
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
                byte[] imageData = ConvertImage(model);

                await _sparePartService.CreatePart(model, imageData);//создание
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
            var response = await _excelReaderService.ReadJsonFile(file);

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

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (id == 0)
                return View();

            var response = await _sparePartService.GetPartForEdit(id);
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return View(response.Data);
            }

            return RedirectToAction("Error");

        }
        [HttpPost]
        public async Task<IActionResult> Edit(SparePartViewModel model)
        {
            if (ModelState.IsValid)
            {
                byte[] imageData = ConvertImage(model);
                await _sparePartService.Edit(model.Id, model,imageData);
            }
            return RedirectToAction("GetAllParts");
        }

        private byte[] ConvertImage(SparePartViewModel model)
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

            return imageData;
        }

        private void ConvertToShortView(BaseResponse<IEnumerable<SparePart>> response, List<SparePartShortView> query)
        {
            foreach (var item in response.Data)
            {
                query.Add(new SparePartShortView
                {
                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description,
                    Model = item.Model,
                    Price = item.Price,
                    TypeSparePart = item.TypeSparePart,
                    Avatar = item.Avatar,

                });
            }
        }
    }
}
