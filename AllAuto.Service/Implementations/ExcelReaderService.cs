using AllAuto.DAL.Interfaces;
using AllAuto.Domain.Entity;
using AllAuto.Domain.Enum;
using AllAuto.Service.Interfaces;
using IronXL;

namespace AllAuto.Service.Implementations
{
    public class ExcelReaderService : IExcelReaderService<SparePart>
    {
        private const string WorkSheetName = "SheetSpareParts";
        private readonly string testFilePath = @"E:\C#\AllAuto\TestData\DataSheet.xlsx";
        private readonly IBaseRepository<SparePart> _sparePartRepository;

        public ExcelReaderService(IBaseRepository<SparePart> sparePartRepository)
        {
            _sparePartRepository = sparePartRepository;
        }

        public void ReadExcelFile()
        {
            List<SparePart> spareParts = Read(); //read data from excel<
            AddStartParts(spareParts);
        }

        public List<SparePart> Read()
        {
            List<SparePart> spareParts = new List<SparePart>();

            WorkBook excelWorkBook = WorkBook.Load(testFilePath);
            WorkSheet excelWorkSheet = excelWorkBook.GetWorkSheet(WorkSheetName);

            IronXL.Range range = excelWorkSheet["A2:G22"];

            for (int row = 2; row <= 22; row++)
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

                spareParts.Add(part);
            }

            return spareParts;
        }

        private async Task AddStartParts(List<SparePart> spareParts)
        {
            foreach (var part in spareParts)
            {
                await _sparePartRepository.Create(part);
            }
        }


    }
}
