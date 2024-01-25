using AllAuto.DAL.Interfaces;
using AllAuto.Domain.Entity;
using AllAuto.Domain.Enum;
using AllAuto.Domain.Response;
using AllAuto.Service.Interfaces;
using IronXL;
using Microsoft.AspNetCore.Http;
using System.Text;

namespace AllAuto.Service.Implementations
{
    public class ExcelReaderService : IExcelReaderService<SparePart>
    {
        private const string WorkSheetName = "SheetSpareParts";
        private readonly string testFilePath = @"E:\C#\AllAuto\TestData\DataSheet.xlsx";
        private readonly string testFilePathNote = @"C:\Code\С#\AllAuto\TestData\DataSheet.xlsx";
        private readonly IBaseRepository<SparePart> _sparePartRepository;

        public ExcelReaderService(IBaseRepository<SparePart> sparePartRepository)
        {
            _sparePartRepository = sparePartRepository;
        }

        /// <summary>
        /// Добавление файла при старте по пути
        /// </summary>
        /// <returns></returns>
        public async Task ReadExcelFile()
        {
            List<SparePart> spareParts = Read(); //read data from excel<
            await AddPartsToDB(spareParts);
        }

        public async Task<BaseResponse<bool>> ReadExcelFile(IFormFile file)
        {
            try
            {
                List<SparePart> spareParts = new List<SparePart>();
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

                using (var stream = new MemoryStream())
                {
                    file.CopyTo(stream);
                    stream.Position = 0;

                    WorkBook excelWorkBook = WorkBook.Load(stream);
                    WorkSheet excelWorkSheet = excelWorkBook.GetWorkSheet(WorkSheetName);

                    for (int row = 2; row <= excelWorkSheet.RowCount; row++)
                    {
                        SparePart part = ReadPart(excelWorkSheet, row);
                        spareParts.Add(part);
                    }
                }

                return await AddPartsToDB(spareParts);
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>()
                {
                    Description = ex.Message,
                    StatusCode = Domain.Enum.StatusCode.InternalServerError
                };
            }
        }

        private async Task<BaseResponse<bool>> AddPartsToDB(List<SparePart> spareParts)
        {
            try
            {
                if (spareParts.Count == 0)
                {
                    return new BaseResponse<bool>()
                    {
                        StatusCode = StatusCode.CarNotFound,
                        Description = "Пустой список",
                    };
                }

                foreach (var part in spareParts)
                {
                    //TODO: поиск товара в базе, если есть, то едит
                    await _sparePartRepository.Create(part);
                }

                return new BaseResponse<bool>()
                {
                    StatusCode = Domain.Enum.StatusCode.OK,
                    Description = "Записи добавлены",
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>()
                {
                    Description = ex.Message,
                    StatusCode = Domain.Enum.StatusCode.InternalServerError
                };
            }

        }     

        private List<SparePart> Read()
        {
            List<SparePart> spareParts = new List<SparePart>();

            WorkBook excelWorkBook = WorkBook.Load(testFilePathNote);
            WorkSheet excelWorkSheet = excelWorkBook.GetWorkSheet(WorkSheetName);

            for (int row = 2; row <= excelWorkSheet.RowCount; row++)
            {
                SparePart part = ReadPart(excelWorkSheet, row);

                spareParts.Add(part);
            }

            return spareParts;
        }

        private SparePart ReadPart(WorkSheet excelWorkSheet, int row)
        {
            SparePart part = new SparePart(); //create data
            var cells = excelWorkSheet[$"A{row}:G{row}"].ToList();
            part.Name = cells[0].Text;
            part.Model = cells[1].Text;
            part.Description = cells[2].Text;
            part.Price = Convert.ToDecimal(cells[3].Text);
            part.DateCreate = DateTime.ParseExact(cells[4].Text, "dd.MM.yyyy H:mm:ss", null);
            part.TypeSparePart = (TypePart)Convert.ToInt32(cells[5].Text);
            part.Amount = Convert.ToInt32(cells[6].Text);
            part.Avatar = Array.Empty<byte>();
            return part;
        }
    }
}
