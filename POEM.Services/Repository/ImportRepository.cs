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

        public void ReplaceCategories(List<string> codesToDelete, List<CategoryDetails> recordsToInsert)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (codesToDelete.Any())
                    {
                        var existing = _context.CategoryDetails
                            .Where(x => codesToDelete.Contains(x.Code))
                            .ToList();
                        _context.CategoryDetails.RemoveRange(existing);
                    }

                    _context.CategoryDetails.AddRange(recordsToInsert);

                    _context.SaveChanges(); // ← both delete + insert sent together
                    transaction.Commit();   // ← only commits if everything succeeded
                }
                catch
                {
                    transaction.Rollback(); // ← undoes delete too if insert fails
                    throw;
                }
            }
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

        public void ReplaceSubCategoryDetails(List<string> codesToDelete, List<SubCategoryMasterDbDto> recordsToInsert)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (codesToDelete.Any())
                    {
                        var existing = _context.SubCategoryMasters
                            .Where(x => codesToDelete.Contains(x.Code))
                            .ToList();
                        _context.SubCategoryMasters.RemoveRange(existing);
                    }

                    _context.SubCategoryMasters.AddRange(recordsToInsert);

                    _context.SaveChanges(); // ← both delete + insert sent together
                    transaction.Commit();   // ← only commits if everything succeeded
                }
                catch
                {
                    transaction.Rollback(); // ← undoes delete too if insert fails
                    throw;
                }
            }
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


        public void ReplaceCollectionDetails(List<string> codesToDelete, List<CollectionDtl> recordsToInsert)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (codesToDelete.Any())
                    {
                        var existing = _context.CollectionDtls
                            .Where(x => codesToDelete.Contains(x.Code))
                            .ToList();
                        _context.CollectionDtls.RemoveRange(existing);
                    }

                    _context.CollectionDtls.AddRange(recordsToInsert);

                    _context.SaveChanges(); // ← both delete + insert sent together
                    transaction.Commit();   // ← only commits if everything succeeded
                }
                catch
                {
                    transaction.Rollback(); // ← undoes delete too if insert fails
                    throw;
                }
            }
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

        public void ReplaceCompanyMaster(List<string> codesToDelete, List<CompanyMasterDbDto> recordsToInsert)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (codesToDelete.Any())
                    {
                        var existing = _context.CompanyMaster
                            .Where(x => codesToDelete.Contains(x.Code))
                            .ToList();
                        _context.CompanyMaster.RemoveRange(existing);
                    }

                    _context.CompanyMaster.AddRange(recordsToInsert);

                    _context.SaveChanges(); // ← both delete + insert sent together
                    transaction.Commit();   // ← only commits if everything succeeded
                }
                catch
                {
                    transaction.Rollback(); // ← undoes delete too if insert fails
                    throw;
                }
            }
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



        public void ReplaceFindingDetails(List<string> codesToDelete, List<FindingDetail> recordsToInsert)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (codesToDelete.Any())
                    {
                        var existing = _context.findingDetails
                            .Where(x => codesToDelete.Contains(x.FindingNumber))
                            .ToList();
                        _context.findingDetails.RemoveRange(existing);
                    }

                    _context.findingDetails.AddRange(recordsToInsert);

                    _context.SaveChanges(); // ← both delete + insert sent together
                    transaction.Commit();   // ← only commits if everything succeeded
                }
                catch
                {
                    transaction.Rollback(); // ← undoes delete too if insert fails
                    throw;
                }
            }
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



        public void ReplaceStoneShapeDetails(List<string> codesToDelete, List<StoneShapeDetail> recordsToInsert)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (codesToDelete.Any())
                    {
                        var existing = _context.StoneShapeDetails
                            .Where(x => codesToDelete.Contains(x.Code))
                            .ToList();
                        _context.StoneShapeDetails.RemoveRange(existing);
                    }

                    _context.StoneShapeDetails.AddRange(recordsToInsert);

                    _context.SaveChanges(); // ← both delete + insert sent together
                    transaction.Commit();   // ← only commits if everything succeeded
                }
                catch
                {
                    transaction.Rollback(); // ← undoes delete too if insert fails
                    throw;
                }
            }
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


        

        public void ReplaceSettingLaborDetails(List<string> codesToDelete, List<SettingLaborDetail> recordsToInsert)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (codesToDelete.Any())
                    {
                        var existing = _context.SettingLaborDetails
                            .Where(x => codesToDelete.Contains(x.Code))
                            .ToList();
                        _context.SettingLaborDetails.RemoveRange(existing);
                    }

                    _context.SettingLaborDetails.AddRange(recordsToInsert);

                    _context.SaveChanges(); // ← both delete + insert sent together
                    transaction.Commit();   // ← only commits if everything succeeded
                }
                catch
                {
                    transaction.Rollback(); // ← undoes delete too if insert fails
                    throw;
                }
            }
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


        public void ReplaceVendorDetails(List<string> codesToDelete, List<VendorDetails> recordsToInsert)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (codesToDelete.Any())
                    {
                        var existing = _context.VendorDetails
                            .Where(x => codesToDelete.Contains(x.VendorCode))
                            .ToList();
                        _context.VendorDetails.RemoveRange(existing);
                    }

                    _context.VendorDetails.AddRange(recordsToInsert);

                    _context.SaveChanges(); // ← both delete + insert sent together
                    transaction.Commit();   // ← only commits if everything succeeded
                }
                catch
                {
                    transaction.Rollback(); // ← undoes delete too if insert fails
                    throw;
                }
            }
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


        public void ReplaceStoneQualityDetails(List<string> codesToDelete, List<StoneQualityDetailsDbDto> recordsToInsert)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (codesToDelete.Any())
                    {
                        var existing = _context.StoneQualityDetails
                            .Where(x => codesToDelete.Contains(x.StoneQualityCode))
                            .ToList();
                        _context.StoneQualityDetails.RemoveRange(existing);
                    }

                    _context.StoneQualityDetails.AddRange(recordsToInsert);

                    _context.SaveChanges(); // ← both delete + insert sent together
                    transaction.Commit();   // ← only commits if everything succeeded
                }
                catch
                {
                    transaction.Rollback(); // ← undoes delete too if insert fails
                    throw;
                }
            }
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


        public void ReplaceProcessCostingDetails(List<string> codesToDelete, List<ProcessCostingDetails> recordsToInsert)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (codesToDelete.Any())
                    {
                        var existing = _context.ProcessCostingDetails
                            .Where(x => codesToDelete.Contains(x.Code))
                            .ToList();
                        _context.ProcessCostingDetails.RemoveRange(existing);
                    }

                    _context.ProcessCostingDetails.AddRange(recordsToInsert);

                    _context.SaveChanges(); // ← both delete + insert sent together
                    transaction.Commit();   // ← only commits if everything succeeded
                }
                catch
                {
                    transaction.Rollback(); // ← undoes delete too if insert fails
                    throw;
                }
            }
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

      

        public void ReplaceMarginDetails(List<string> codesToDelete, List<MarginDetailsDbDto> recordsToInsert)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (codesToDelete.Any())
                    {
                        var existing = _context.MarginDetails
                            .Where(x => codesToDelete.Contains(x.Code))
                            .ToList();
                        _context.MarginDetails.RemoveRange(existing);
                    }

                    _context.MarginDetails.AddRange(recordsToInsert);

                    _context.SaveChanges(); // ← both delete + insert sent together
                    transaction.Commit();   // ← only commits if everything succeeded
                }
                catch
                {
                    transaction.Rollback(); // ← undoes delete too if insert fails
                    throw;
                }
            }
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