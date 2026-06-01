using POEM.Model.Model;
using POEM.Model.Model.Import;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POEM.Services.Interface
{
    public interface IImportRepository
    {
        #region CategoryDetails
        List<string> GetAllCategoryCodes();

        List<string> GetAllCategoryNames();

        void BulkInsertCategories(List<CategoryDetails> categories);

        void DeleteCategoryCodesDetailsByCodes(List<string> codes);

        #endregion

        #region SubCategoryMaster
        List<string> GetAllSubCategoryMasterCodes();

        List<string> GetAllSubCategoryMasterSubCategoryNames();
        void BulkInsertSubCategoryMaster(List<SubCategoryMasterDbDto> subCategories);

        void DeleteSubCategoryCodesDetailsByCodes(List<string> codes);
        #endregion

        #region CollectionDetails
        List<string> GetAllCollectionDetailsCodes();

        List<string> GetAllCollectionDetailsCollections();
        void BulkInsertCollectionDetailsMaster(List<CollectionDtl> collections);

        void DeleteCollectionDetailsByCodes(List<string> codes);
        #endregion

        #region Company Master
        List<string> GetAllCompanyMasterCodes();

        List<string> GetAllCompanyMasterNames();
        void BulkInsertCompanyMaster(List<CompanyMasterDbDto> master);
        void DeleteCompanyMasterByCodes(List<string> codes);
        #endregion

        #region FindingDetails
        List<string> GetAllFindingDetailsFindingNumbers();

        void BulkInsertFindingDetails(List<FindingDetail> master);
        void DeleteFindingDetailsByFindingNumbers(List<string> codes);
        #endregion

        #region StoneShapeDetails
        List<string> GetAllStoneShapeDetailsCodes();

        void BulkInsertStoneShapeDetails(List<StoneShapeDetail> master);

        void DeleteStoneShapeDetailsByCodes(List<string> codes);
        #endregion

        #region SettingLaborDetails
        List<string> GetAllSettingLaborDetailsCodes();

        void BulkInsertSettingLaborDetails(List<SettingLaborDetail> master);

        void DeleteSettingLaborDetailsByCodes(List<string> codes);

        #endregion

        #region VendorDetails
        List<string> GetAllVendorDetailsCodes();

        void BulkInsertVendorDetails(List<VendorDetails> master);

        void DeleteVendorDetailsByCodes(List<string> codes);

        #endregion

        #region StoneQualityDetails
        List<string> GetAllStoneQualityDetailsCodes();

        void BulkInsertStoneQualityDetails(List<StoneQualityDetailsDbDto> master);

        void DeleteStoneQualityDetailsByCodes(List<string> codes);

        #endregion

        #region ProcessCostingDetails
        List<string> GetAllProcessCostingDetailsCodes();

        void BulkInsertProcessCostingDetails(List<ProcessCostingDetails> master);

        void DeleteProcessCostingDetailsByCodes(List<string> codes);

        #endregion
    }
}
