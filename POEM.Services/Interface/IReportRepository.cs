using POEM.Model.Model.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POEM.Services.Interface
{
    public interface IReportRepository
    {
        List<SKUReportDto> GetSKUReport(
        string company,
        string category,
        string subCategory,
        string collection);
    }
}
