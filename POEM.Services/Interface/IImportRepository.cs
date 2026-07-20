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

        void ReplaceCategories(List<string> codesToDelete, List<CategoryDetails> recordsToInsert);

        int GetCategoryDetailsCount();                          
        List<CategoryDetails> GetAllCategoryRecords();

        #endregion

        #region SubCategoryMaster
        List<string> GetAllSubCategoryMasterCodes();

        List<string> GetAllSubCategoryMasterSubCategoryNames();

        void ReplaceSubCategoryDetails(List<string> codesToDelete, List<SubCategoryMasterDbDto> recordsToInsert);
        int GetSubCategoryDetailsCount();                        
        List<SubCategoryMasterDbDto> GetAllSubCategoryRecords();
        #endregion

        #region CollectionDetails
        List<string> GetAllCollectionDetailsCodes();

        List<string> GetAllCollectionDetailsCollections();
        void ReplaceCollectionDetails(List<string> codesToDelete, List<CollectionDtl> recordsToInsert);
        int GetCollectionDetailsCount();
        List<CollectionDtl> GetAllCollectionDetailsRecords();
        #endregion

        #region CompanyMaster
        List<string> GetAllCompanyMasterCodes();

        List<string> GetAllCompanyMasterNames();

        void ReplaceCompanyMaster(List<string> codesToDelete, List<CompanyMasterDbDto> recordsToInsert);

        int GetCompanyMasterCount();
        List<CompanyMasterDbDto> GetAllCompanyMasterRecords();
        #endregion

        #region FindingDetails
        List<string> GetAllFindingDetailsFindingNumbers();

        void ReplaceFindingDetails(List<string> codesToDelete, List<FindingDetail> recordsToInsert);

        int GetFindingDetailsCount();
        List<FindingDetail> GetAllFindingDetailsRecords();
        #endregion

        #region StoneShapeDetails
        List<string> GetAllStoneShapeDetailsCodes();


        void ReplaceStoneShapeDetails(List<string> codesToDelete, List<StoneShapeDetail> recordsToInsert);

        int GetStoneShapeDetailsCount();
        List<StoneShapeDetail> GetAllStoneShapeDetailsRecords();
        #endregion

        #region SettingLaborDetails
        List<string> GetAllSettingLaborDetailsCodes();

        void ReplaceSettingLaborDetails(List<string> codesToDelete, List<SettingLaborDetail> recordsToInsert);
        int GetSettingLaborDetailsCount();
        List<SettingLaborDetail> GetAllSettingLaborDetailsRecords();
        #endregion

        #region VendorDetails
        List<string> GetAllVendorDetailsCodes();

        void ReplaceVendorDetails(List<string> codesToDelete, List<VendorDetails> recordsToInsert);

        int GetVendorDetailsCount();
        List<VendorDetails> GetAllVendorDetailsRecords();

        #endregion

        #region StoneQualityDetails
        List<string> GetAllStoneQualityDetailsCodes();
        void ReplaceStoneQualityDetails(List<string> codesToDelete, List<StoneQualityDetailsDbDto> recordsToInsert);
        int GetStoneQualityDetailsCount();
        List<StoneQualityDetailsDbDto> GetAllStoneQualityDetailsRecords();

        #endregion

        #region ProcessCostingDetails
        List<string> GetAllProcessCostingDetailsCodes();

        void ReplaceProcessCostingDetails(List<string> codesToDelete, List<ProcessCostingDetails> recordsToInsert);
        int GetProcessCostingDetailsCount();
        List<ProcessCostingDetails> GetAllProcessCostingDetailsRecords();
        #endregion

        #region MarginDetails
        List<string> GetAllMarginDetailsCodes();
        void ReplaceMarginDetails(List<string> codesToDelete, List<MarginDetailsDbDto> recordsToInsert);

        int GetMarginDetailsCount();
        List<MarginDetailsDbDto> GetAllMarginDetailsRecords();
        #endregion

        #region DiamondDetails
        List<string> GetAllDiamondDetailsCodes();
        void ReplaceDiamondDetails(List<string> codesToDelete, List<DiamondDetail> recordsToInsert);

        int GetDiamondDetailsCount();
        List<DiamondDetail> GetAllDiamondDetailssRecords();
        #endregion

        #region DutyChartMaster
        void ReplaceDutyChartMaster(List<DutyChartMasterDbDto> recordsToInsert);
        int GetDutyChartMasterCount();
        List<DutyChartMasterDbDto> GetAllDutyChartMasterRecords();
        #endregion

        #region DutyDetails
        void ReplaceDutyDetails(List<DutyDetailsDbDto> recordsToInsert);
        int GetDutyDetailsCount();
        List<DutyDetailsDbDto> GetAllDutyDetailsRecords();
        #endregion
    }
}
