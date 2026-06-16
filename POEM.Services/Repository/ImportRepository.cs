using POEM.Model.Model;
using POEM.Model.Model.Import;
using POEM.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POEM.Services.Repository
{
    public class ImportRepository : IImportRepository
    {
        private readonly ApplicationDbContext _context;

        public ImportRepository()
        {
            _context = new ApplicationDbContext();
        }

        #region CategoryDetails
        public List<string> GetAllCategoryCodes()
        {
            return _context.CategoryDetails
                .Select(x => x.Code)
                .ToList();
        }

        public List<string> GetAllCategoryNames()
        {
            return _context.CategoryDetails
                .Select(x => x.CategoryName)
                .ToList();
        }



        public void BulkInsertCategories(List<CategoryDetails> categories)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.CategoryDetails.AddRange(categories);

                    _context.SaveChanges();

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();

                    throw;
                }
            }
        }

        public void DeleteCategoryCodesDetailsByCodes(List<string> codes)
        {
            //var lowerCodes = codes.Select(x => x.ToLower()).ToList();

            //var existing = _context.CategoryDetails
            //    .Where(x => lowerCodes.Contains(x.Code.ToLower()))
            //    .ToList();
            var existing = _context.CategoryDetails
                .Where(x => codes.Contains(x.Code))  // ← no ToLower inside EF query
                .ToList();

            _context.CategoryDetails.RemoveRange(existing);
            _context.SaveChanges();
        }

        public int GetCategoryDetailsCount()
        {
            return _context.CategoryDetails.Count();
        }
        public List<CategoryDetails> GetAllCategoryRecords()
        {
            return _context.CategoryDetails.ToList();
        }
        #endregion

        #region SubCategoryMaster
        public List<string> GetAllSubCategoryMasterCodes()
        {
            return _context.SubCategoryMasters
                .Select(x => x.Code)
                .ToList();
        }

        public List<string> GetAllSubCategoryMasterSubCategoryNames()
        {
            return _context.SubCategoryMasters
                .Select(x => x.SubCategoryName)
                .ToList();
        }

        public void BulkInsertSubCategoryMaster(List<SubCategoryMasterDbDto> subCategories)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.SubCategoryMasters.AddRange(subCategories);

                    _context.SaveChanges();

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();

                    throw;
                }
            }
        }

        public void DeleteSubCategoryCodesDetailsByCodes(List<string> codes)
        {
            var lowerCodes = codes.Select(x => x.ToLower()).ToList();

            var existing = _context.SubCategoryMasters
                .Where(x => lowerCodes.Contains(x.Code.ToLower()))
                .ToList();

            _context.SubCategoryMasters.RemoveRange(existing);
            _context.SaveChanges();
        }

        public int GetSubCategoryDetailsCount()
        {
            return _context.SubCategoryMasters.Count();
        }
        public List<SubCategoryMasterDbDto> GetAllSubCategoryRecords()
        {
            return _context.SubCategoryMasters.ToList();
        }
        #endregion


        #region CollectionDetails
        public List<string> GetAllCollectionDetailsCodes()
        {
            return _context.CollectionDtls
                .Select(x => x.Code)
                .ToList();
        }

        public List<string> GetAllCollectionDetailsCollections()
        {
            return _context.CollectionDtls
                .Select(x => x.Collection)
                .ToList();
        }

        public void BulkInsertCollectionDetailsMaster(List<CollectionDtl> collections)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.CollectionDtls.AddRange(collections);

                    _context.SaveChanges();

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();

                    throw;
                }
            }
        }

        public void DeleteCollectionDetailsByCodes(List<string> codes)
        {
            var lowerCodes = codes.Select(x => x.ToLower()).ToList();

            var existing = _context.CollectionDtls
                .Where(x => lowerCodes.Contains(x.Code.ToLower()))
                .ToList();

            _context.CollectionDtls.RemoveRange(existing);
            _context.SaveChanges();
        }
        public int  GetCollectionDetailsCount()
        {
            return _context.CollectionDtls.Count();
        }
        public List<CollectionDtl> GetAllCollectionDetailsRecords()
        {
            return _context.CollectionDtls.ToList();
        }
        #endregion


        #region CompanyMaster
        public List<string> GetAllCompanyMasterCodes()
        {
            return _context.CompanyMaster
                .Select(x => x.Code)
                .ToList();
        }

        public List<string> GetAllCompanyMasterNames()
        {
            return _context.CompanyMaster
                .Select(x => x.CompanyName)
                .ToList();
        }

        public void BulkInsertCompanyMaster(List<CompanyMasterDbDto> master)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.CompanyMaster.AddRange(master);

                    _context.SaveChanges();

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();

                    throw;
                }
            }
        }

        public void DeleteCompanyMasterByCodes(List<string> codes)
        {
            var lowerCodes = codes.Select(x => x.ToLower()).ToList();

            var existing = _context.CompanyMaster
                .Where(x => lowerCodes.Contains(x.Code.ToLower()))
                .ToList();

            _context.CompanyMaster.RemoveRange(existing);
            _context.SaveChanges();
        }
        public int GetCompanyMasterCount()
        {
            return _context.CompanyMaster.Count();
        }
        public List<CompanyMasterDbDto> GetAllCompanyMasterRecords()
        {
            return _context.CompanyMaster.ToList();
        }
        #endregion

        #region FindingDetails
        public List<string> GetAllFindingDetailsFindingNumbers()
        {
            return _context.findingDetails
                .Select(x => x.FindingNumber)
                .ToList();
        }


        public void BulkInsertFindingDetails(List<FindingDetail> master)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.findingDetails.AddRange(master);

                    _context.SaveChanges();

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();

                    throw;
                }
            }
        }

        public void DeleteFindingDetailsByFindingNumbers(List<string> codes)
        {
            var lowerCodes = codes.Select(x => x.ToLower()).ToList();

            var existing = _context.findingDetails
                .Where(x => lowerCodes.Contains(x.FindingNumber.ToLower()))
                .ToList();

            _context.findingDetails.RemoveRange(existing);
            _context.SaveChanges();
        }
        public int GetFindingDetailsCount()
        {
            return _context.findingDetails.Count();
        }
        public List<FindingDetail> GetAllFindingDetailsRecords()
        {
            return _context.findingDetails.ToList();
        }
        #endregion

        #region StoneShapeDetails
        public List<string> GetAllStoneShapeDetailsCodes()
        {
            return _context.StoneShapeDetails
                .Select(x => x.Code)
                .ToList();
        }


        public void BulkInsertStoneShapeDetails(List<StoneShapeDetail> master)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.StoneShapeDetails.AddRange(master);

                    _context.SaveChanges();

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();

                    throw;
                }
            }
        }

        public void DeleteStoneShapeDetailsByCodes(List<string> codes)
        {
            var lowerCodes = codes.Select(x => x.ToLower()).ToList();

            var existing = _context.StoneShapeDetails
                .Where(x => lowerCodes.Contains(x.Code.ToLower()))
                .ToList();

            _context.StoneShapeDetails.RemoveRange(existing);
            _context.SaveChanges();
        }

        public int GetStoneShapeDetailsCount()
        {
            return _context.StoneShapeDetails.Count();
        }
        public List<StoneShapeDetail> GetAllStoneShapeDetailsRecords()
        {
            return _context.StoneShapeDetails.ToList();
        }
        #endregion


        #region SettingLaborDetails
        public List<string> GetAllSettingLaborDetailsCodes()
        {
            return _context.SettingLaborDetails
                .Select(x => x.Code)
                .ToList();
        }


        public void BulkInsertSettingLaborDetails(List<SettingLaborDetail> master)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.SettingLaborDetails.AddRange(master);

                    _context.SaveChanges();

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();

                    throw;
                }
            }
        }

        public void DeleteSettingLaborDetailsByCodes(List<string> codes)
        {
            var lowerCodes = codes.Select(x => x.ToLower()).ToList();

            var existing = _context.SettingLaborDetails
                .Where(x => lowerCodes.Contains(x.Code.ToLower()))
                .ToList();

            _context.SettingLaborDetails.RemoveRange(existing);
            _context.SaveChanges();
        }

        public int GetSettingLaborDetailsCount()
        {
            return _context.SettingLaborDetails.Count();
        }
        public List<SettingLaborDetail> GetAllSettingLaborDetailsRecords()
        {
            return _context.SettingLaborDetails.ToList();
        }
        #endregion

        #region VendorDetails
        public List<string> GetAllVendorDetailsCodes()
        {
            return _context.VendorDetails
                .Select(x => x.VendorCode)
                .ToList();
        }


        public void BulkInsertVendorDetails(List<VendorDetails> master)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.VendorDetails.AddRange(master);

                    _context.SaveChanges();

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();

                    throw;
                }
            }
        }

        public void DeleteVendorDetailsByCodes(List<string> codes)
        {
            var lowerCodes = codes.Select(x => x.ToLower()).ToList();

            var existing = _context.VendorDetails
                .Where(x => lowerCodes.Contains(x.VendorCode.ToLower()))
                .ToList();

            _context.VendorDetails.RemoveRange(existing);
            _context.SaveChanges();
        }

        public int GetVendorDetailsCount()
        {
            return _context.VendorDetails.Count();
        }
        public List<VendorDetails> GetAllVendorDetailsRecords()
        {
            return _context.VendorDetails.ToList();
        }
        #endregion

        #region StoneQualityDetails
        public List<string> GetAllStoneQualityDetailsCodes()
        {
            return _context.StoneQualityDetails
                .Select(x => x.StoneQualityCode)
                .ToList();
        }


        public void BulkInsertStoneQualityDetails(List<StoneQualityDetailsDbDto> master)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.StoneQualityDetails.AddRange(master);

                    _context.SaveChanges();

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();

                    throw;
                }
            }
        }
        public void DeleteStoneQualityDetailsByCodes(List<string> codes)
        {
            var lowerCodes = codes.Select(x => x.ToLower()).ToList();

            var existing = _context.StoneQualityDetails
                .Where(x => lowerCodes.Contains(x.StoneQualityCode.ToLower()))
                .ToList();

            _context.StoneQualityDetails.RemoveRange(existing);
            _context.SaveChanges();
        }

        public int GetStoneQualityDetailsCount()
        {
            return _context.StoneQualityDetails.Count();
        }
        public List<StoneQualityDetailsDbDto> GetAllStoneQualityDetailsRecords()
        {
            return _context.StoneQualityDetails.ToList();
        }
        #endregion

        #region ProcessCostingDetails
        public List<string> GetAllProcessCostingDetailsCodes()
        {
            return _context.ProcessCostingDetails
               .Select(x => x.Code)
               .ToList();
        }

        public void BulkInsertProcessCostingDetails(List<ProcessCostingDetails> master)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.ProcessCostingDetails.AddRange(master);

                    _context.SaveChanges();

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();

                    throw;
                }
            }
        }

        public void DeleteProcessCostingDetailsByCodes(List<string> codes)
        {
            var lowerCodes = codes.Select(x => x.ToLower()).ToList();

            var existing = _context.ProcessCostingDetails
                .Where(x => lowerCodes.Contains(x.Code.ToLower()))
                .ToList();

            _context.ProcessCostingDetails.RemoveRange(existing);
            _context.SaveChanges();
        }
        public int GetProcessCostingDetailsCount()
        {
            return _context.ProcessCostingDetails.Count();
        }
        public List<ProcessCostingDetails> GetAllProcessCostingDetailsRecords()
        {
            return _context.ProcessCostingDetails.ToList();
        }
        #endregion

        #region MarginDetails
        public List<string> GetAllMarginDetailsCodes()
        {
            return _context.MarginDetails
               .Select(x => x.Code)
               .ToList();
        }

        public void BulkInsertMarginDetails(List<MarginDetailsDbDto> master)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.MarginDetails.AddRange(master);

                    _context.SaveChanges();

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();

                    throw;
                }
            }
        }

        public void DeleteMarginDetailsByCodes(List<string> codes)
        {
            var lowerCodes = codes.Select(x => x.ToLower()).ToList();

            var existing = _context.MarginDetails
                .Where(x => lowerCodes.Contains(x.Code.ToLower()))
                .ToList();

            _context.MarginDetails.RemoveRange(existing);
            _context.SaveChanges();
        }
        public int GetMarginDetailsCount()
        {
            return _context.MarginDetails.Count();
        }
        public List<MarginDetailsDbDto> GetAllMarginDetailsRecords()
        {
            return _context.MarginDetails.ToList();
        }
        #endregion

    }
}