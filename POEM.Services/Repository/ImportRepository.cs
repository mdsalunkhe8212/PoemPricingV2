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
        #endregion

        #region StoneQualityDetails
        public List<string> GetAllStoneQualityDetailsCodes()
        {
            return _context.StoneQualityDetails
                .Select(x => x.Code)
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
                .Where(x => lowerCodes.Contains(x.Code.ToLower()))
                .ToList();

            _context.StoneQualityDetails.RemoveRange(existing);
            _context.SaveChanges();
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

        #endregion
    }
}