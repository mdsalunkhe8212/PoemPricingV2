using POEM.Model.Model;
using POEM.Services.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Threading.Tasks;
using System.Web;
using System.Web.Services.Description;

namespace POEM.Services.Repository
{
	public class SkuRepository : ISkuRepository
    {
        private readonly ApplicationDbContext _context;
        public SkuRepository()
        {
            _context = new ApplicationDbContext();
        }
        // ============================================================
        // 1. SKU DETAILS (MASTER)
        // ============================================================
        public long SaveSkuDetails(SKUDetailsDbDto dto)
        {
            dto.IsActive = true;
            _context.SKUDetails.Add(dto);
            _context.SaveChanges();
            return dto.SKUId; // EF automatically populates identity
        }

        public long UpdateSkuDetails(SKUDetailsDbDto dto) {
            dto.IsActive = true;

            var result = _context.SKUDetails.SingleOrDefault(b => b.SKUId == dto.SKUId);
            if (result != null)
            {
                _context.Entry(result).CurrentValues.SetValues(dto);
                _context.Entry(result).State = EntityState.Modified;
                _context.SaveChanges();
            }
           
            return dto.SKUId;

        }

        // ============================================================
        // 2. METALS
        // ============================================================
        public void DeleteMetals(long skuId)
        {
            var items = _context.SKUMetalDetails.Where(x => x.SKUId == skuId);
            _context.SKUMetalDetails.RemoveRange(items);
            _context.SaveChanges();
        }

        public void SaveMetal(long skuId, SKUMetalDbDto dto)
        {
            dto.SKUId = skuId;
            _context.SKUMetalDetails.Add(dto);
            _context.SaveChanges();
        }

        // ============================================================
        // 3. FINDINGS
        // ============================================================
        public void DeleteFindings(long skuId)
        {
            var items = _context.SKUFindingsDetails.Where(x => x.SKUId == skuId);
            _context.SKUFindingsDetails.RemoveRange(items);
            _context.SaveChanges();
        }

        public void SaveFinding(long skuId, SKUFindingsDbDto dto)
        {
            dto.SKUId = skuId;
            _context.SKUFindingsDetails.Add(dto);
            _context.SaveChanges();
        }

        // ============================================================
        // 4. STONES
        // ============================================================
        public void DeleteStones(long skuId)
        {
            var items = _context.SKUStoneDetails.Where(x => x.SKUId == skuId);
            _context.SKUStoneDetails.RemoveRange(items);
            _context.SaveChanges();
        }

        public void SaveStone(long skuId, SKUStoneDbDto dto)
        {
            dto.SKUId = skuId;
            _context.SKUStoneDetails.Add(dto);
            _context.SaveChanges();
        }

        // ============================================================
        // 5. LABOR
        // ============================================================
        public void DeleteLabor(long skuId)
        {
            var items = _context.SKULaborDetails.Where(x => x.SKUId == skuId);
            _context.SKULaborDetails.RemoveRange(items);
            _context.SaveChanges();
        }
        // ============================================================
        // 6. Calculation
        // ============================================================
        public void DeleteCalculation(long skuId)
        {
            var items = _context.SkuCalculations.Where(x => x.SKUId == skuId);
            _context.SkuCalculations.RemoveRange(items);
            _context.SaveChanges();
        }

        public void SaveLabor(long skuId, SKULaborDbDto dto)
        {
            dto.SKUId = skuId;
            _context.SKULaborDetails.Add(dto);
            _context.SaveChanges();
        }
        public void SaveCalculation(long skuId,SkuCalculation dto)
        {
            dto.SKUId = skuId;
            _context.SkuCalculations.Add(dto);
            _context.SaveChanges();
        }

        public long SaveSkuModule(SkuModuleDto model)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    // 1. Save SKUDetails

                    long skuId = model.skuInfo.VendorProduct.skuId;
                    if (model.skuInfo.VendorProduct.skuId > 0)
                    {
                        // ✅ Update existing
                        skuId = UpdateSkuDetails(MapSkuDetails(model.skuInfo.VendorProduct));
                    }
                    else
                    {
                        // ✅ Insert new
                        skuId = SaveSkuDetails(MapSkuDetails(model.skuInfo.VendorProduct));
                    }



                    // 2. Metals
                    DeleteMetals(skuId);
                    foreach (var m in model.skuInfo.Metals)
                        SaveMetal(skuId, MapMetal(m));

                    // 3. Findings
                    DeleteFindings(skuId);
                    foreach (var f in model.skuInfo.Findings)
                        SaveFinding(skuId, MapFinding(f));

                    // 4. Stones
                    DeleteStones(skuId);
                    foreach (var s in model.stoneInfo)
                        SaveStone(skuId, MapStone(s));

                    // 5. Labor
                    DeleteLabor(skuId);
                    SaveLabor(skuId, MapLabor(model.laborInfo));

                    // 6. Calculation
                    DeleteCalculation(skuId);
                    SaveCalculation(skuId, MapCalculation(model.calculations));

                    transaction.Commit();
                    return skuId;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        // ----------------- MAPPERS -----------------

        private SKUDetailsDbDto MapSkuDetails(VendorProductDto dto)
        {
            return new SKUDetailsDbDto
            {
                SKUId=dto.skuId,
                Company = dto.company,
                SKUNumber = dto.skuNumber,
                Vendor = dto.vendor,
                VendorNumber = dto.vendorNumber,
                OrderType = dto.orderType,
                CategoryCode = dto.categoryCode,
                Category = dto.category,
                SubCategoryCode = dto.subCategoryCode,
                SubCategory = dto.subCategory,
                SKUStatus = true,
                CollectionCode = string.IsNullOrWhiteSpace(dto.collectionCode) ? null : dto.collectionCode,
                Collection = string.IsNullOrWhiteSpace(dto.collection) ? null : dto.collection,
                SizeLength = ParseDecimal(dto.sizeLength),
                MMWidth = ParseDecimal(dto.mmWidth),
                MMHeight = ParseDecimal(dto.mmHeight),
                semiMinWt = ParseDecimal(dto.semiMinWt),
                centerMinWt = ParseDecimal(dto.centerMinWt),
                SemiAdjWt = ParseDecimal(dto.SemiAdjWt),
                CenterAdjWt = ParseDecimal(dto.CenterAdjWt),
                CreatedBy = 1,
                CreatedOn = DateTime.Now
            };
        }

        private SKUMetalDbDto MapMetal(MetalDto dto)
        {
            return new SKUMetalDbDto
            {
                MetalText = dto.metalText,
                MetalIdText = dto.metalId,
                KaratText = dto.karatText,
                KaratId = dto.karatId,
                ColorText = dto.colorText,
                ColorId = dto.colorId,

                GmWt = dto.gmWt,
                RatePOz = dto.ratePOz,
                RatePerGm = dto.ratePerGm,
                MetalCost = dto.metalCost,
                MetalInc = ParseDecimal(dto.inc),
                MetalDec = ParseDecimal(dto.dec),

                CreatedBy = 1,
                CreatedOn = DateTime.Now
            };
        }

        private SKUFindingsDbDto MapFinding(FindingDto dto)
        {
            return new SKUFindingsDbDto
            {
                Supplier = dto.supplier,
                SupplierId = dto.supplierId,
                FindingType = dto.type,
                FindingTypeId = dto.typeId,
                FindingDescription = dto.description,
                FindingSku = dto.sku,

                FindingMetal = dto.metal,
                FindingKarat = dto.karat,
                FindingColor = dto.color,

                FindingAssembly = dto.assembly,
                AssemblyId = dto.assemblyId,

                FindingQty = dto.qty,
                FindingGmWtPerPc = dto.gmWtPerPc,
                FindingTotalGm = dto.totalGm,
                FindingCostPerPc = dto.costPerPc,
                FindingTotalCost = dto.totalCost,
                FindingInc = ParseDecimal(dto.inc),
                FindingDec = ParseDecimal(dto.dec),

                CreatedBy = 1,
                CreatedOn = DateTime.Now
            };
        }

        private SKUStoneDbDto MapStone(StoneDto dto)
        {
            return new SKUStoneDbDto
            {
                StoneVendor = dto.StoneVendor,
                StoneType = dto.StoneType,
                Growing = dto.Growing,
                SettingLocation = dto.SettingLocation,
                Lab = dto.Lab,

                StoneShape = dto.Shape,
                ShapeText = dto.ShapeText,
                StoneMMSize = dto.MMSize,
                StoneWidth1 = dto.Width1,
                StoneWidth2 = dto.Width2,

                PerStoneWt = ParseDecimal(dto.PerStoneWt),
                StoneQty = ParseInt(dto.Qty),
                TotalStoneWt = ParseDecimal(dto.TotalStoneWt),
                TotalAdjStoneWt = ParseDecimal(dto.TotalAdjStoneWt),

                StoneQuality = dto.StoneQuality,
                StoneCostPerCarat = ParseDecimal(dto.StoneCostPerCarat),
                StoneTotalCost = ParseDecimal(dto.StoneTotalCost),

                StoneSettingVendor = dto.SettingVendor,
                StoneSettingType = dto.SettingType,
                CostPerStone = ParseDecimal(dto.CostPerStone),
                SettingTotalCost = ParseDecimal(dto.TotalCost),

                SemiMinWt = ParseDecimal(dto.SemiMinWt),
                SemiAdjWt = ParseDecimal(dto.SemiAdjWt),
                CenterMinWt = ParseDecimal(dto.CenterMinWt),
                CenterAdjWt = ParseDecimal(dto.CenterAdjWt),
                TotalMinWt = ParseDecimal(dto.TotalMinWt),
                TotalAdjWt = ParseDecimal(dto.TotalAdjWt),

                CreatedBy = 1,
                CreatedOn = DateTime.Now
            };
        }

        private SKULaborDbDto MapLabor(LaborDto dto)
        {
            return new SKULaborDbDto
            {
                LaborLocation = dto.LaborLocation,
                VendorCode=dto.VendorCode,
                VendorName=dto.VendorName,
                ProcessType =dto.ProcessType,
                CastingLabor = dto.CastingLabor,
                CastPcs = dto.CastPcs,
                CFP = dto.CFP,
                Rhodium = dto.Rhodium,
                LaborAssembly = dto.LaborAssembly,
                Solder = dto.Solder,
                Tag = dto.Tag,
                DiaHandling = dto.DiaHandling,
                FinHandling = dto.FinHandling,
                Stamping = dto.Stamping,
                Model = dto.Model,
                CAM = dto.CAM,
                GiftBox = dto.GiftBox,
                TotalLabor = dto.TotalLabor,

                OtherHead1 = dto.OtherHead1,
                OtherCost1 = dto.OtherCost1,
                OtherHead2 = dto.OtherHead2,
                OtherCost2 = dto.OtherCost2,
                OtherHead3 = dto.OtherHead3,
                OtherCost3 = dto.OtherCost3,

                SemiFOB = dto.SemiFOB,
                SemiDuty = dto.SemiDuty,
                LandedCost = dto.LandedCost,

                Price1 = dto.Price1,
                Price2 = dto.Price2,
                Price3 = dto.Price3,

                Margin1 = dto.Margin1,
                Margin2 = dto.Margin2,
                Margin3 = dto.Margin3,

                CompleteFOB = dto.CompleteFOB,
                CompleteDuty = dto.CompleteDuty,
                LandedCostComplete = dto.LandedCostComplete,

                CompletePrice1 = dto.CompletePrice1,
                CompletePrice2 = dto.CompletePrice2,
                CompletePrice3 = dto.CompletePrice3,

                CompleteMargin1 = dto.CompleteMargin1,
                CompleteMargin2 = dto.CompleteMargin2,
                CompleteMargin3 = dto.CompleteMargin3,

                Remark = dto.Remark,

                CreatedBy = 1,
                CreatedOn = DateTime.Now
            };
        }

        private SkuCalculation MapCalculation(SkuCalculationDto dto)
        {
            return new SkuCalculation
            {
                TotalMetalCost = dto.totalMetalCost,
                TotalFindingCost = dto.totalFindingCost,
                TotalSemiStoneCost = dto.totalSemiStoneCost,
                TotalCenterStoneCost = dto.totalCenterStoneCost,
                TotalSemiSettingCost = dto.totalSemiSettingCost,
                TotalCenterSettingCost = dto.totalCenterSettingCost,
                TotalLaborCost = dto.totalLaborCost,
                SemiDuty = dto.semiDuty,
                SemiFOB = dto.semiFOB,
                CompleteFOB = dto.completeFOB,
                SemiPrice1 = dto.semiPrice1,
                SemiPrice2 = dto.semiPrice2,
                SemiPrice3 = dto.semiPrice3,
                CenterPrice1 = dto.centerPrice1,
                CenterPrice2 = dto.centerPrice2,
                CenterPrice3 = dto.centerPrice3,
                SemiMargin1 = dto.semiMargin1,
                SemiMargin2 = dto.semiMargin2,
                SemiMargin3 = dto.semiMargin3,
                CenterMargin1 = dto.centerMargin1,
                CenterMargin2 = dto.centerMargin2,
                CenterMargin3 = dto.centerMargin3,
                LandedCost = dto.landedCost,
                LandedCostCenter = dto.landedCostCenter,
                CreatedBy = 1,
                CreatedOn = DateTime.Now
            };
        } 
        

        private decimal ParseDecimal(string v)
        {
            if (string.IsNullOrWhiteSpace(v)) return 0m;
            return decimal.TryParse(v, NumberStyles.Any, CultureInfo.InvariantCulture, out var d)
                ? d
                : 0m;
        }

        private int ParseInt(string v)
        {
            if (string.IsNullOrWhiteSpace(v)) return 0;
            return int.TryParse(v, NumberStyles.Any, CultureInfo.InvariantCulture, out var i)
                ? i
                : 0;
        }

        public async Task<PagedResult<SkuListItemDto>> GetPagedAsync(string company,string skuPrefix,string category,string subcategory,string collection,int page,int pageSize)
        {
            // Base query from SKUDetails
            var query = from sku in _context.SKUDetails
                        where sku.Company == company
                        select new
                        {
                            sku.SKUId,
                            sku.SKUNumber,
                            sku.ModifiedOn,
                            sku.IsActive,
                            sku.Category,
                            sku.SubCategory,
                            sku.Collection,
                        };

            // SKU prefix filter (min length 5)
            if (!string.IsNullOrEmpty(skuPrefix) && skuPrefix.Length >= 5)
            {
                query = query.Where(s => s.SKUNumber.StartsWith(skuPrefix));
            }
            else
            {
                // category/subcategory/collection filters can be applied here
                // assuming SKUDetails has these columns
                if (!string.IsNullOrEmpty(category))
                    query = query.Where(s => s.Category == category);

                if (!string.IsNullOrEmpty(subcategory))
                    query = query.Where(s => s.SubCategory == subcategory);

                if (!string.IsNullOrEmpty(collection))
                    query = query.Where(s => s.Collection == collection);
            }

            var total = await query.CountAsync();

            // Join with Metal and Labor tables
            var items = await (from sku in query
                               join labor in _context.SKULaborDetails
                                   on sku.SKUId equals labor.SKUId into laborJoin
                               from labor in laborJoin.DefaultIfEmpty()
                               join metal in _context.SKUMetalDetails
                                   on sku.SKUId equals metal.SKUId into metalJoin
                               from metal in metalJoin
                                   .OrderBy(m => m.MetalId) // pick top 1
                                   .Take(1)
                                   .DefaultIfEmpty()
                               orderby sku.SKUId descending
                               select new SkuListItemDto
                               {
                                   Sku = sku.SKUNumber,
                                   Top1Metal = metal.MetalText,
                                   Price1 = labor.Price1 ?? 0.00m,
                                   Price2 = labor.Price2 ?? 0.00m,
                                   Price3 = labor.Price3 ?? 0.00m,
                                   ModifiedDate = sku.ModifiedOn,
                                   IsActive = sku.IsActive 
                               })
                               .Skip((page - 1) * pageSize)
                               .Take(pageSize)
                               .ToListAsync();

            return new PagedResult<SkuListItemDto>
            {
                TotalCount = total,
                Items = items
            };

        }

        public async Task<SkuListItemDto> GetByIdAsync(int id)
        {
            var result = await (from sku in _context.SKUDetails
                                where sku.SKUId == id
                                join labor in _context.SKULaborDetails
                                    on sku.SKUId equals labor.SKUId into laborJoin
                                from labor in laborJoin.DefaultIfEmpty()
                                join metal in _context.SKUMetalDetails
                                    on sku.SKUId equals metal.SKUId into metalJoin
                                from metal in metalJoin
                                    .OrderBy(m => m.MetalId)
                                    .Take(1)
                                    .DefaultIfEmpty()
                                select new SkuListItemDto
                                {
                                    Sku = sku.SKUNumber,
                                    Top1Metal = metal.MetalText,
                                    Price1 = labor.Price1 ?? 0.00m,
                                    Price2 = labor.Price2 ?? 0.00m,
                                    Price3 = labor.Price3 ?? 0.00m,
                                    ModifiedDate = sku.ModifiedOn,   // may be null
                                    IsActive = sku.IsActive
                                })
                                .FirstOrDefaultAsync();

            return result;
        }
        public async Task<bool> ExistsAsync(string skuNumber)
        {
            return await _context.SKUDetails.AnyAsync(s => s.SKUNumber == skuNumber);
        }

        public SkuModuleDto GetSkuByNumber(string skuNumber)
        {
            // 1. VendorProduct (SkuMaster)
            var skuEntity = _context.SKUDetails
                .FirstOrDefault(s => s.SKUNumber == skuNumber);

            if (skuEntity == null) return null;

            // 2. Metals
            var metals = _context.SKUMetalDetails
                .Where(m => m.SKUId == skuEntity.SKUId)
                .Select(m => new MetalDto
                {
                    metalText = m.MetalText,
                    metalId = m.MetalId.ToString(),
                    karatText = m.KaratText,
                    karatId = m.KaratId,
                    colorText = m.ColorText,
                    colorId = m.ColorId,

                    gmWt = m.GmWt,
                    ratePOz = m.RatePOz,
                    ratePerGm = m.RatePerGm,
                    metalCost = m.MetalCost,

                    inc = m.MetalInc.ToString(),
                    dec = m.MetalDec.ToString()

                }).ToList();

            // 3. Findings
            var findings = _context.SKUFindingsDetails
                .Where(f => f.SKUId == skuEntity.SKUId)
                .Select(f => new FindingDto
                {
                    supplier = f.Supplier,
                    supplierId = f.SupplierId,
                    type = f.FindingType,
                    typeId = f.FindingTypeId,
                    description = f.FindingDescription,
                    sku = f.FindingSku,

                    metal = f.FindingMetal,
                    karat = f.FindingKarat,
                    color = f.FindingColor,

                    assembly = f.FindingAssembly,
                    assemblyId = f.AssemblyId,

                    qty = f.FindingQty,
                    gmWtPerPc = f.FindingGmWtPerPc,
                    totalGm = f.FindingTotalGm,
                    costPerPc = f.FindingCostPerPc,
                    totalCost = f.FindingTotalCost,

                    inc = f.FindingInc.ToString(),
                    dec = f.FindingDec.ToString()

                }).ToList();

            // 4. Stones
            var stones = _context.SKUStoneDetails
                .Where(st => st.SKUId == skuEntity.SKUId)
                .Select(st => new StoneDto
                {
                    StoneVendor = st.StoneVendor,
                    StoneVendorCode = st.StoneVendorCode,
                    StoneType = st.StoneType,
                    ShapeText = st.ShapeText,
                    Qty = st.StoneQty.ToString(),
                    SemiMinWt = st.SemiMinWt.ToString(),
                    CenterMinWt = st.CenterMinWt.ToString(),
                    TotalMinWt = st.TotalMinWt.ToString(),
                    StoneQuality = st.StoneQuality,
                    StoneTotalCost = st.StoneTotalCost.ToString(),
                    SettingLocation = st.SettingLocation,
                    PerStoneWt = st.PerStoneWt.ToString(),
                    TotalStoneWt = st.TotalStoneWt.ToString(),
                    TotalAdjWt = st.TotalAdjWt.ToString(),
                    TotalAdjStoneWt = st.TotalAdjStoneWt.ToString(),
                    Growing = st.Growing,
                    Shape = st.StoneShape,
                    StoneCostPerCarat = st.StoneCostPerCarat.ToString(),
                    TotalCost = st.SettingTotalCost.ToString(),
                    SettingType = st.StoneSettingType,
                    SettingTypeCode = st.StoneSettingTypeCode,
                    SettingVendor = st.StoneSettingVendor,
                    SettingVendorCode=st.StoneSettingVendorCode,
                    CostPerStone = st.CostPerStone.ToString(),
                    SemiAdjWt = st.SemiAdjWt.ToString(),
                    MMSize = st.StoneMMSize,
                    Width1 = st.StoneWidth1,
                    Width2 = st.StoneWidth2,
                    CenterAdjWt = st.CenterAdjWt.ToString(),
                    Lab = st.Lab                    
                }).ToList();

            // 5. LaborInfo
            var labor = _context.SKULaborDetails
                .FirstOrDefault(l => l.SKUId == skuEntity.SKUId);

            // 6. Calculations
            var calculation = _context.SkuCalculations
                .FirstOrDefault(l => l.SKUId == skuEntity.SKUId);

            // 7. Map into SkuModule
            var skuModule = new SkuModuleDto
            {
                skuInfo = new SkuInfoDto
                {                    
                    VendorProduct = new VendorProductDto
                    {
                        skuId = skuEntity.SKUId,
                        company = skuEntity.Company,
                        vendor = skuEntity.Vendor,
                        orderType = skuEntity.OrderType,
                        vendorNumber = skuEntity.VendorNumber,
                        skuNumber = skuEntity.SKUNumber,
                        collection = skuEntity.Collection,
                        category = skuEntity.Category,
                        subCategory = skuEntity.SubCategory,
                        sizeLength = skuEntity.SizeLength.ToString(),
                        semiMinWt = skuEntity.semiMinWt.ToString(),
                        centerMinWt = skuEntity.centerMinWt.ToString(),
                        SemiAdjWt = skuEntity.SemiAdjWt.ToString(),
                        CenterAdjWt = skuEntity.CenterAdjWt.ToString(),
                        mmWidth = skuEntity.MMWidth.ToString(),
                        mmHeight= skuEntity.MMHeight.ToString(),
                        categoryCode=skuEntity.CategoryCode,
                        subCategoryCode=skuEntity.SubCategoryCode,
                        collectionCode=skuEntity.CollectionCode,
                    },
                    Metals = metals,
                    Findings = findings
                },
                stoneInfo = stones,
                laborInfo = labor != null ? new LaborDto
                {
                    CastPcs = labor.CastPcs,
                    Model = labor.Model,
                    GiftBox = labor.GiftBox,
                    TotalLabor = labor.TotalLabor,
                    SemiFOB = labor.SemiFOB,
                    Price1 = labor.Price1 ?? 0.00m,
                    Price2 = labor.Price2 ?? 0.00m,
                    Price3 = labor.Price3 ?? 0.00m,
                    Price4=labor.Price4 ?? 0.00m,
                    CompleteFOB = labor.CompleteFOB,
                    Remark = labor.Remark,
                    /* Added By Mahesh Start*/
                    DiaHandling= labor.DiaHandling,
                    CFP=labor.CFP,
                    Rhodium=labor.Rhodium,
                    Tag=labor.Tag,
                    LaborAssembly=labor.LaborAssembly,
                    LaborLocation=labor.LaborLocation,
                    VendorCode=labor.VendorCode,
                    VendorName=labor.VendorName,
                    ProcessType=labor.ProcessType,
                    CastingLabor=labor.CastingLabor
                    /* Added By Mahesh Start End*/
                } : new LaborDto(),
                calculations = new SkuCalculationDto
                {
                    totalMetalCost = calculation.TotalMetalCost,
                    totalFindingCost = calculation.TotalFindingCost,
                    totalSemiStoneCost = calculation.TotalSemiStoneCost,
                    totalCenterStoneCost = calculation.TotalCenterStoneCost,
                    totalSemiSettingCost = calculation.TotalSemiSettingCost,
                    totalCenterSettingCost = calculation.TotalCenterSettingCost,
                    totalLaborCost = calculation.TotalLaborCost,
                    semiDuty = calculation.SemiDuty,
                    semiFOB = calculation.SemiFOB,
                    completeFOB = calculation.CompleteFOB,

                    semiPrice1 = calculation.SemiPrice1,
                    semiPrice2 = calculation.SemiPrice2,
                    semiPrice3 = calculation.SemiPrice3,
                    semiPrice4=calculation.SemiPrice4,

                    centerPrice1 = calculation.CenterPrice1,
                    centerPrice2 = calculation.CenterPrice2,
                    centerPrice3 = calculation.CenterPrice3,
                    centerPrice4 = calculation.CenterPrice4,

                    semiMargin1 = calculation.SemiMargin1,
                    semiMargin2 = calculation.SemiMargin2,
                    semiMargin3 = calculation.SemiMargin3,
                    semiMargin4 = calculation.SemiMargin4,

                    centerMargin1 = calculation.CenterMargin1,
                    centerMargin2 = calculation.CenterMargin2,
                    centerMargin3 = calculation.CenterMargin3,
                    centerMargin4 = calculation.CenterMargin4,

                    landedCost = calculation.LandedCost,
                    landedCostCenter = calculation.LandedCostCenter
                }
            };


            return skuModule;
        }


    }
}