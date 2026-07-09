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
        List<SKUItemDto> GetSkus(
       string term,
       string company,
       List<string> category,
       List<string> subCategory,
       List<string> collection);
        List<SKUReportDto> GetSKUReport(
            string company,
            List<string> category,
            List<string> subCategory,
            List<string> collection,
            List<string> skus);

        List<CollectionProposalDto> GetCollectionProposalReport(
           string company,
           string collection);
    }

}
