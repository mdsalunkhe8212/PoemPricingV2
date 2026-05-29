using POEM.Model.Model;
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

        #endregion

        #region SubCategoryMaster
        List<string> GetAllSubCategoryMasterCodes();

        List<string> GetAllSubCategoryMasterSubCategoryNames();
        void BulkInsertSubCategoryMaster(List<SubCategoryMasterDbDto> subCategories);
        #endregion

        #region CollectionDetails
        List<string> GetAllCollectionDetailsCodes();

        List<string> GetAllCollectionDetailsCollections();
        void BulkInsertCollectionDetailsMaster(List<CollectionDtl> collections);
        #endregion

        #region Company Master
        List<string> GetAllCompanyMasterCodes();

        List<string> GetAllCompanyMasterNames();
        void BulkInsertCompanyMaster(List<CompanyMasterDbDto> master);
        #endregion

        #region FindingDetails
        List<string> GetAllFindingDetailsFindingNumbers();

        void BulkInsertFindingDetails(List<FindingDetail> master);
        #endregion

        #region StoneShapeDetails
        List<string> GetAllStoneShapeDetailsCodes();

        void BulkInsertStoneShapeDetails(List<StoneShapeDetail> master);
        #endregion

        #region SettingLaborDetails
        List<string> GetAllSettingLaborDetailsCodes();

        void BulkInsertSettingLaborDetails(List<SettingLaborDetail> master);

        #endregion

        #region VendorDetails
        List<string> GetAllVendorDetailsCodes();

        void BulkInsertVendorDetails(List<VendorDetails> master);

        #endregion

        #region StoneQualityDetails
        List<string> GetAllStoneQualityDetailsCodes();

        void BulkInsertStoneQualityDetails(List<StoneQualityDetailsDbDto> master);

        #endregion
    }
}
