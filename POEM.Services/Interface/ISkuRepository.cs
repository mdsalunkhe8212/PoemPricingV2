using POEM.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POEM.Services.Interface
{
    interface ISkuRepository
    {
        // ============================
        // SKU DETAILS (MASTER)
        // ============================
        long SaveSkuDetails(SKUDetailsDbDto dto);



        // ============================
        // METALS
        // ============================
        void DeleteMetals(long skuId);
        void SaveMetal(long skuId, SKUMetalDbDto dto);



        // ============================
        // FINDINGS
        // ============================
        void DeleteFindings(long skuId);
        void SaveFinding(long skuId, SKUFindingsDbDto dto);



        // ============================
        // STONES
        // ============================
        void DeleteStones(long skuId);
        void SaveStone(long skuId, SKUStoneDbDto dto);



        // ============================
        // LABOR
        // ============================
        void DeleteLabor(long skuId);
        void SaveLabor(long skuId, SKULaborDbDto dto);

        long SaveSkuModule(SkuModuleDto model);

        // ============================
        // SKU List
        // ============================

        Task<PagedResult<SkuListItemDto>> GetPagedAsync(string company,string skuPrefix,string category,string subcategory,string collection,int page,int pageSize);
        Task<SkuListItemDto> GetByIdAsync(int id);
        Task<bool> ExistsAsync(string skuNumber);
        SkuModuleDto GetSkuByNumber(string skuNumber);

    }
}
