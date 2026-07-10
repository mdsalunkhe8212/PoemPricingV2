using POEM.Model.Model;
using POEM.Model.Model.Report;
using POEM.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POEM.Services.Repository
{
    public class ReportRepository : IReportRepository
    {
        private readonly ApplicationDbContext _context;

        public ReportRepository()
        {
            _context = new ApplicationDbContext();
        }

        #region ThumbanialPropsal
        public List<SKUItemDto> GetSkus(
            string term,
            string company,
            List<string> category,
            List<string> subCategory,
            List<string> collection)
        {
            var query = _context.SKUDetails.AsQueryable();

            if (!string.IsNullOrEmpty(company))
                query = query.Where(x => x.Company == company);

            if (category != null && category.Any())
                query = query.Where(x =>
                    category.Contains(x.CategoryCode));

            if (subCategory != null && subCategory.Any())
                query = query.Where(x =>
                    subCategory.Contains(x.SubCategoryCode));

            if (collection != null && collection.Any())
                query = query.Where(x =>
                    collection.Contains(x.CollectionCode));

            if (!string.IsNullOrEmpty(term))
                query = query.Where(x =>
                    x.SKUNumber.Contains(term));

            return query
                .OrderBy(x => x.SKUNumber)
                .Take(30)
                .Select(x => new SKUItemDto
                {
                    Value = x.SKUNumber,
                    Text = x.SKUNumber
                })
                .ToList();
        }

        public List<SKUReportDto> GetSKUReport(
     string company,
     List<string> category,
     List<string> subCategory,
     List<string> collection,
     List<string> skus)
        {
            var query =
                from sd in _context.SKUDetails

                join sc1 in _context.SkuCalculations
                    on sd.SKUId equals sc1.SKUId into scJoin
                from sc in scJoin.DefaultIfEmpty()

                join cm1 in _context.CompanyMaster
                    on sd.Company equals cm1.Code into cmJoin
                from cm in cmJoin.DefaultIfEmpty()

                where
                    string.IsNullOrEmpty(company)
                    || sd.Company == company

                select new
                {
                    sd,
                    sc,
                    cm
                };


            var skuData = query.ToList();


            if (category != null && category.Any())
            {
                skuData = skuData
                    .Where(x => category.Contains(x.sd.CategoryCode))
                    .ToList();
            }

            if (subCategory != null && subCategory.Any())
            {
                skuData = skuData
                    .Where(x => subCategory.Contains(x.sd.SubCategoryCode))
                    .ToList();
            }

            if (collection != null && collection.Any())
            {
                skuData = skuData
                    .Where(x => collection.Contains(x.sd.CollectionCode))
                    .ToList();
            }

            if (skus != null && skus.Any())
            {
                skuData = skuData
                    .Where(x => skus.Contains(x.sd.SKUNumber))
                    .ToList();
            }


            var result = skuData.Select(x => new SKUReportDto
            {
                SKUId = x.sd.SKUId,

                Company = x.sd.Company,

                CompanyName = x.cm != null
                    ? x.cm.CompanyName
                    : null,

                SKUNumber = x.sd.SKUNumber,

                Metal =
                    _context.SKUMetalDetails
                    .Where(m => m.SKUId == x.sd.SKUId)
                    .Select(m =>
                        m.KaratText + " " +
                        m.ColorText + " " +
                        m.MetalText)
                    .FirstOrDefault(),

                SemiMount = x.sd.semiMinWt,

                SemiPrice1 = x.sc != null
                    ? x.sc.SemiPrice1
                    : (decimal?)null,

                SemiPrice2 = x.sc != null
                    ? x.sc.SemiPrice2
                    : (decimal?)null,

                SemiPrice3 = x.sc != null
                    ? x.sc.SemiPrice3
                    : (decimal?)null,

                SemiPrice4 = x.sc != null
                    ? x.sc.SemiPrice4
                    : (decimal?)null,

                CenterShapeSize =
                    _context.SKUStoneDetails
                    .Where(s => s.SKUId == x.sd.SKUId)
                    .OrderByDescending(s => s.StoneMMSize)
                    .Select(s =>
                        s.ShapeText + " " +
                        s.StoneMMSize)
                    .FirstOrDefault(),

                CenterPrice1 = x.sc != null
                    ? x.sc.CenterPrice1
                    : (decimal?)null,

                CenterPrice2 = x.sc != null
                    ? x.sc.CenterPrice2
                    : (decimal?)null,

                CenterPrice3 = x.sc != null
                    ? x.sc.CenterPrice3
                    : (decimal?)null,

                CenterPrice4 = x.sc != null
                    ? x.sc.CenterPrice4
                    : (decimal?)null,

                TotalWeight =
                    _context.SKUStoneDetails
                    .Where(s => s.SKUId == x.sd.SKUId)
                    .Select(s => (decimal?)s.TotalStoneWt)
                    .Sum(),

                StoneTotalCost =
                    _context.SKUStoneDetails
                    .Where(s => s.SKUId == x.sd.SKUId)
                    .Select(s => (decimal?)s.StoneTotalCost)
                    .Sum(),

                Findings =
                    string.Join("#",
                        _context.SKUFindingsDetails
                        .Where(f => f.SKUId == x.sd.SKUId)
                        .Select(f => f.FindingDescription)
                        .ToList()
                    ),

                Collection = x.sd.Collection

            }).ToList();


            return result;
        }


        #endregion
        public List<CollectionProposalDto> GetCollectionProposalReport(
            string company,
            string collection)
        {
            var query =
                from sd in _context.SKUDetails

                join sc1 in _context.SkuCalculations
                    on sd.SKUId equals sc1.SKUId into scJoin
                from sc in scJoin.DefaultIfEmpty()

                join cm1 in _context.CompanyMaster
                    on sd.Company equals cm1.Code into cmJoin
                from cm in cmJoin.DefaultIfEmpty()

                where

                (
                    !string.IsNullOrEmpty(company)
                        ? sd.Company == company
                        : sd.Company != null
                )

                &&

                (
                    !string.IsNullOrEmpty(collection)
                        ? sd.CollectionCode == collection
                        : sd.CollectionCode != null
                )

                orderby sd.CategoryCode

                select new
                {
                    sd,
                    sc,
                    cm
                };


            // Close DataReader
            var skuData = query.ToList();



            var result = skuData.Select(x => new CollectionProposalDto
            {
                SKUId = x.sd.SKUId,

                Company = x.sd.Company,

                CompanyName = x.cm != null
                    ? x.cm.CompanyName
                    : null,

                SKUNumber = x.sd.SKUNumber,

                Metal =
                    _context.SKUMetalDetails
                    .Where(m => m.SKUId == x.sd.SKUId)
                    .Select(m =>
                        m.KaratText + " " +
                        m.ColorText + " " +
                        m.MetalText)
                    .FirstOrDefault(),

                SemiMount = x.sd.semiMinWt,

                SemiPrice1 = x.sc != null
                    ? x.sc.SemiPrice1
                    : (decimal?)null,

                SemiPrice2 = x.sc != null
                    ? x.sc.SemiPrice2
                    : (decimal?)null,

                SemiPrice3 = x.sc != null
                    ? x.sc.SemiPrice3
                    : (decimal?)null,

                SemiPrice4 = x.sc != null
                    ? x.sc.SemiPrice4
                    : (decimal?)null,

                CenterShapeSize =
                    _context.SKUStoneDetails
                    .Where(s => s.SKUId == x.sd.SKUId)
                    .OrderByDescending(s => s.StoneMMSize)
                    .Select(s =>
                        s.ShapeText + " " +
                        s.StoneMMSize)
                    .FirstOrDefault(),

                CenterPrice1 = x.sc != null
                    ? x.sc.CenterPrice1
                    : (decimal?)null,

                CenterPrice2 = x.sc != null
                    ? x.sc.CenterPrice2
                    : (decimal?)null,

                CenterPrice3 = x.sc != null
                    ? x.sc.CenterPrice3
                    : (decimal?)null,

                CenterPrice4 = x.sc != null
                    ? x.sc.CenterPrice4
                    : (decimal?)null,

                TotalWeight =
                    _context.SKUStoneDetails
                    .Where(s => s.SKUId == x.sd.SKUId)
                    .Select(s => (decimal?)s.TotalStoneWt)
                    .Sum(),

                StoneTotalCost =
                    _context.SKUStoneDetails
                    .Where(s => s.SKUId == x.sd.SKUId)
                    .Select(s => (decimal?)s.StoneTotalCost)
                    .Sum(),

                Findings =
                    string.Join("#",
                        _context.SKUFindingsDetails
                        .Where(f => f.SKUId == x.sd.SKUId)
                        .Select(f => f.FindingDescription)
                        .ToList()
                    ),

                CategoryCode = x.sd.CategoryCode,

                Category = x.sd.Category,

                Collection = x.sd.Collection,

                CollectionCode = x.sd.CollectionCode

            }).ToList();

            return result;
        }

    }
}