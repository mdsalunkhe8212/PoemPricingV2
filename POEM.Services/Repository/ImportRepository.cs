using POEM.Model.Model;
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
        #endregion

    }
}