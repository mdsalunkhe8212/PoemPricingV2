using ClosedXML.Excel;
using POEM.Services.Interface;
using POEM.Services.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace POEMPricing.Managers
{
    public class ExportManager
    {
        private readonly IImportRepository _repository;
        public ExportManager()
        {
            _repository = new ImportRepository();
        }
        public byte[] ExportToExcel(string masterType)
        {
            switch (masterType)
            {
                case "CategoryDetails":
                    return ExportCategoryDetails();

                case "SubCategoryMaster":
                    return ExportSubCategoryDetails();

                case "CollectionDetails":
                    return ExportCollectionDetails();

                case "CompanyMaster":
                    return ExportCompanyMaster();
                case "FindingDetails":
                    return ExportFindingDetails();

                case "StoneShapeDetails":
                    return ExportStoneShapeDetails();

                case "SettingLaborDetails":
                    return ExportSettingLaborDetails();

                case "VendorDetails":
                    return ExportVendorDetails();

                case "StoneQualityDetails":
                    return ExportStoneQualityDetails();
                case "ProcessCostingDetails":
                    return ExportProcessCostingDetails();
                default:
                    throw new Exception("Invalid master type");
            }
        }

        public int GetCount(string masterType)
        {
            switch (masterType)
            {
                case "CategoryDetails":
                    return _repository.GetCategoryDetailsCount();

                case "SubCategoryMaster":
                    return _repository.GetSubCategoryDetailsCount();
                case "CollectionDetails":
                    return _repository.GetCollectionDetailsCount();

                case "CompanyMaster":
                    return _repository.GetCompanyMasterCount();

                case "FindingDetails":
                    return _repository.GetFindingDetailsCount();

                case "StoneShapeDetails":
                    return _repository.GetStoneShapeDetailsCount();
                case "SettingLaborDetails":
                    return _repository.GetSettingLaborDetailsCount();
                case "VendorDetails":
                    return _repository.GetVendorDetailsCount();
                case "StoneQualityDetails":
                    return _repository.GetStoneQualityDetailsCount();
                case "ProcessCostingDetails":
                    return _repository.GetProcessCostingDetailsCount();
                default:
                    return 0;
            }
        }

        private byte[] ExportCategoryDetails()
        {
            using (var wb = new XLWorkbook())
            {
                var ws = wb.Worksheets.Add("CategoryDetails");

                //var data = new CategoryDetailsImportManager().GetCurrentRecords();
                var data = _repository.GetAllCategoryRecords();

                ws.Cell(1, 1).Value = "Code";
                ws.Cell(1, 2).Value = "CategoryName";

                for (int i = 0; i < data.Count; i++)
                {
                    ws.Cell(i + 2, 1).Value = data[i].Code;
                    ws.Cell(i + 2, 2).Value = data[i].CategoryName;
                }

                ws.Columns().AdjustToContents();

                using (var stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return stream.ToArray();
                }
            }
        }

        private byte[] ExportSubCategoryDetails()
        {

            using (var wb = new XLWorkbook())
            {
                var ws = wb.Worksheets.Add("SubCategoryMaster");
                var data = _repository.GetAllSubCategoryRecords();
                ws.Cell(1, 1).Value = "Code";
                ws.Cell(1, 2).Value = "SubCategoryName";

                for (int i = 0; i < data.Count; i++)
                {
                    ws.Cell(i + 2, 1).Value = data[i].Code;
                    ws.Cell(i + 2, 2).Value = data[i].SubCategoryName;
                }

                ws.Columns().AdjustToContents();

                using (var stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return stream.ToArray();
                }
            }
        }

        private byte[] ExportCollectionDetails()
        {

            using (var wb = new XLWorkbook())
            {
                var ws = wb.Worksheets.Add("CollectionDetails");
                var data = _repository.GetAllCollectionDetailsRecords();
                ws.Cell(1, 1).Value = "Code";
                ws.Cell(1, 2).Value = "Collection";

                for (int i = 0; i < data.Count; i++)
                {
                    ws.Cell(i + 2, 1).Value = data[i].Code;
                    ws.Cell(i + 2, 2).Value = data[i].Collection;
                }

                ws.Columns().AdjustToContents();

                using (var stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return stream.ToArray();
                }
            }
        }

        private byte[] ExportCompanyMaster()
        {

            using (var wb = new XLWorkbook())
            {
                var ws = wb.Worksheets.Add("CompanyMaster");
                var data = _repository.GetAllCompanyMasterRecords();
                ws.Cell(1, 1).Value = "Code";
                ws.Cell(1, 2).Value = "CompanyName";

                for (int i = 0; i < data.Count; i++)
                {
                    ws.Cell(i + 2, 1).Value = data[i].Code;
                    ws.Cell(i + 2, 2).Value = data[i].CompanyName;
                }

                ws.Columns().AdjustToContents();

                using (var stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return stream.ToArray();
                }
            }
        }

        private byte[] ExportFindingDetails()
        {

            using (var wb = new XLWorkbook())
            {
                var ws = wb.Worksheets.Add("FindingDetails");
                var data = _repository.GetAllFindingDetailsRecords();
                ws.Cell(1, 1).Value = "FindingSupplier";
                ws.Cell(1, 2).Value = "FindingVendorName";
                ws.Cell(1, 3).Value = "FindingVendorCode";
                ws.Cell(1, 4).Value = "Company";
                ws.Cell(1, 5).Value = "FindingNumber";
                ws.Cell(1, 6).Value = "FindingMetalType";
                ws.Cell(1, 7).Value = "FindingMetalKt";
                ws.Cell(1, 8).Value = "FindingMetalColor";
                ws.Cell(1, 9).Value = "FindingType";
                ws.Cell(1, 10).Value = "FindingDescription";
                ws.Cell(1, 11).Value = "FindingShortDescription";
                ws.Cell(1, 12).Value = "PerPcFindingWeightGms";
                ws.Cell(1, 13).Value = "Increment";
                ws.Cell(1, 14).Value = "Decrement";
                ws.Cell(1, 15).Value = "MetalLock";
                ws.Cell(1, 16).Value = "FindingCost";

                for (int i = 0; i < data.Count; i++)
                {
                    ws.Cell(i + 2, 1).Value = data[i].FindingSupplier;
                    ws.Cell(i + 2, 2).Value = data[i].FindingVendorName;
                    ws.Cell(i + 2, 3).Value = data[i].FindingVendorCode;
                    ws.Cell(i + 2, 4).Value = data[i].Company;
                    ws.Cell(i + 2, 5).Value = data[i].FindingNumber;
                    ws.Cell(i + 2, 6).Value = data[i].FindingMetalType;
                    ws.Cell(i + 2, 7).Value = data[i].FindingMetalKt;
                    ws.Cell(i + 2, 8).Value = data[i].FindingMetalColor;
                    ws.Cell(i + 2, 9).Value = data[i].FindingType;
                    ws.Cell(i + 2, 10).Value = data[i].FindingDescription;
                    ws.Cell(i + 2, 11).Value = data[i].FindingShortDescription;
                    ws.Cell(i + 2, 12).Value = data[i].PerPcFindingWeightGms;
                    ws.Cell(i + 2, 13).Value = data[i].Increment;
                    ws.Cell(i + 2, 14).Value = data[i].Decrement;
                    ws.Cell(i + 2, 15).Value = data[i].MetalLock;
                    ws.Cell(i + 2, 16).Value = data[i].FindingCost;
                }

                ws.Columns().AdjustToContents();

                using (var stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return stream.ToArray();
                }
            }


        }

        private byte[] ExportStoneShapeDetails()
        {

            using (var wb = new XLWorkbook())
            {
                var ws = wb.Worksheets.Add("StoneShapeDetails");
                var data = _repository.GetAllStoneShapeDetailsRecords();
                ws.Cell(1, 1).Value = "Code";
                ws.Cell(1, 2).Value = "StoneType";
                ws.Cell(1, 3).Value = "StoneShape";
                ws.Cell(1, 4).Value = "CategoryFancyRound";

                for (int i = 0; i < data.Count; i++)
                {
                    ws.Cell(i + 2, 1).Value = data[i].Code;
                    ws.Cell(i + 2, 2).Value = data[i].StoneType;
                    ws.Cell(i + 2, 3).Value = data[i].StoneShape;
                    ws.Cell(i + 2, 4).Value = data[i].CategoryFancyRound;
                }

                ws.Columns().AdjustToContents();

                using (var stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return stream.ToArray();
                }
            }
        }

        private byte[] ExportSettingLaborDetails()
        {

            using (var wb = new XLWorkbook())
            {
                var ws = wb.Worksheets.Add("SettingLaborDetails");
                var data = _repository.GetAllSettingLaborDetailsRecords();
                ws.Cell(1, 1).Value = "Code";
                ws.Cell(1, 2).Value = "SettingVendor";
                ws.Cell(1, 3).Value = "SettingType";
                ws.Cell(1, 4).Value = "ShapeCode";
                ws.Cell(1, 5).Value = "Shape";
                ws.Cell(1, 6).Value = "DiamondPSWtFrom";
                ws.Cell(1, 7).Value = "DiamondPSWtTo";
                ws.Cell(1, 8).Value = "GoldCostPS";
                ws.Cell(1, 9).Value = "PlatinumCostPS";
                ws.Cell(1, 10).Value = "SilverCostPS";

                for (int i = 0; i < data.Count; i++)
                {
                    ws.Cell(i + 2, 1).Value = data[i].Code;
                    ws.Cell(i + 2, 2).Value = data[i].SettingVendor;
                    ws.Cell(i + 2, 3).Value = data[i].SettingType;
                    ws.Cell(i + 2, 4).Value = data[i].ShapeCode;
                    ws.Cell(i + 2, 5).Value = data[i].Shape;
                    ws.Cell(i + 2, 6).Value = data[i].DiamondPSWtFrom;
                    ws.Cell(i + 2, 7).Value = data[i].DiamondPSWtTo;
                    ws.Cell(i + 2, 8).Value = data[i].GoldCostPS;
                    ws.Cell(i + 2, 9).Value = data[i].PlatinumCostPS;
                    ws.Cell(i + 2, 10).Value = data[i].SilverCostPS;
                }

                ws.Columns().AdjustToContents();

                using (var stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return stream.ToArray();
                }
            }


        }

        private byte[] ExportVendorDetails()
        {

            using (var wb = new XLWorkbook())
            {
                var ws = wb.Worksheets.Add("ExportVendorDetails");
                var data = _repository.GetAllVendorDetailsRecords();
                ws.Cell(1, 1).Value = "VendorLocation";
                ws.Cell(1, 2).Value = "VendorName";
                ws.Cell(1, 3).Value = "VendorCode";
                ws.Cell(1, 4).Value = "DiamondHandlingLab";
                ws.Cell(1, 5).Value = "DiaHndLabLow";
                ws.Cell(1, 6).Value = "DiaHndLabHigh";
                ws.Cell(1, 7).Value = "DiamondHandlingMined";
                ws.Cell(1, 8).Value = "DiaHndMinedLow";
                ws.Cell(1, 9).Value = "DiaHndMinedHigh";
                ws.Cell(1, 10).Value = "FindingHndGold";
                ws.Cell(1, 11).Value = "FindingHndPlatinum";
                ws.Cell(1, 12).Value = "FindingHndSilver";
                ws.Cell(1, 13).Value = "ModelMkgGold";
                ws.Cell(1, 14).Value = "ModelMkgPlatinum";
                ws.Cell(1, 15).Value = "ModelMkgSilver";
                ws.Cell(1, 16).Value = "CAMGold";
                ws.Cell(1, 17).Value = "CAMPlatinum";
                ws.Cell(1, 18).Value = "CAMSilver";
                ws.Cell(1, 19).Value = "ProductVendor";
                ws.Cell(1, 20).Value = "FindingsSupplier";
                ws.Cell(1, 21).Value = "FindingsAssembly";
                ws.Cell(1, 22).Value = "StoneVendor";
                ws.Cell(1, 23).Value = "SettingVendor";
                ws.Cell(1, 24).Value = "LabourLocation";

                for (int i = 0; i < data.Count; i++)
                {
                    ws.Cell(i + 2, 1).Value = data[i].VendorLocation;
                    ws.Cell(i + 2, 2).Value = data[i].VendorName;
                    ws.Cell(i + 2, 3).Value = data[i].VendorCode;
                    ws.Cell(i + 2, 4).Value = data[i].DiamondHandlingLab;
                    ws.Cell(i + 2, 5).Value = data[i].DiaHndLabLow;
                    ws.Cell(i + 2, 6).Value = data[i].DiaHndLabHigh;
                    ws.Cell(i + 2, 7).Value = data[i].DiamondHandlingMined;
                    ws.Cell(i + 2, 8).Value = data[i].DiaHndMinedLow;
                    ws.Cell(i + 2, 9).Value = data[i].DiaHndMinedHigh;
                    ws.Cell(i + 2, 10).Value = data[i].FindingHndGold;
                    ws.Cell(i + 2, 11).Value = data[i].FindingHndPlatinum;
                    ws.Cell(i + 2, 12).Value = data[i].FindingHndSilver;
                    ws.Cell(i + 2, 13).Value = data[i].ModelMkgGold;
                    ws.Cell(i + 2, 14).Value = data[i].ModelMkgPlatinum;
                    ws.Cell(i + 2, 15).Value = data[i].ModelMkgSilver;
                    ws.Cell(i + 2, 16).Value = data[i].CAMGold;
                    ws.Cell(i + 2, 17).Value = data[i].CAMPlatinum;
                    ws.Cell(i + 2, 18).Value = data[i].CAMSilver;
                    ws.Cell(i + 2, 19).Value = data[i].ProductVendor;
                    ws.Cell(i + 2, 20).Value = data[i].FindingsSupplier;
                    ws.Cell(i + 2, 21).Value = data[i].FindingsAssembly;
                    ws.Cell(i + 2, 22).Value = data[i].StoneVendor;
                    ws.Cell(i + 2, 23).Value = data[i].SettingVendor;
                    ws.Cell(i + 2, 24).Value = data[i].LabourLocation;
                }

                ws.Columns().AdjustToContents();

                using (var stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return stream.ToArray();
                }
            }


        }


        private byte[] ExportStoneQualityDetails()
        {

            using (var wb = new XLWorkbook())
            {
                var ws = wb.Worksheets.Add("StoneQualityDetails");
                var data = _repository.GetAllStoneQualityDetailsRecords();
                ws.Cell(1, 1).Value = "CompanyCode";
                ws.Cell(1, 2).Value = "StoneVendorCode";
                ws.Cell(1, 3).Value = "StoneType";
                ws.Cell(1, 4).Value = "StoneShapeCode";
                ws.Cell(1, 5).Value = "StoneShape";
                ws.Cell(1, 6).Value = "StoneQualityCode";
                ws.Cell(1, 7).Value = "StoneQuality";
                ws.Cell(1, 8).Value = "InternationalGrading";


                for (int i = 0; i < data.Count; i++)
                {
                    ws.Cell(i + 2, 1).Value = data[i].Company;
                    ws.Cell(i + 2, 2).Value = data[i].StoneVendorCode;
                    ws.Cell(i + 2, 3).Value = data[i].StoneType;
                    ws.Cell(i + 2, 4).Value = data[i].StoneShapeCode;
                    ws.Cell(i + 2, 5).Value = data[i].StoneShape;
                    ws.Cell(i + 2, 6).Value = data[i].StoneQualityCode;
                    ws.Cell(i + 2, 7).Value = data[i].StoneQuality;
                    ws.Cell(i + 2, 8).Value = data[i].IntertionalGrading;

                }

                ws.Columns().AdjustToContents();

                using (var stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return stream.ToArray();
                }
            }


        }

        private byte[] ExportProcessCostingDetails()
        {

            using (var wb = new XLWorkbook())
            {
                var ws = wb.Worksheets.Add("ProcessCostingDetails");
                var data = _repository.GetAllProcessCostingDetailsRecords();
                ws.Cell(1, 1).Value = "Code";
                ws.Cell(1, 2).Value = "VendorCode";
                ws.Cell(1, 3).Value = "Category";
                ws.Cell(1, 4).Value = "Type";
                ws.Cell(1, 5).Value = "Unit";
                ws.Cell(1, 6).Value = "GoldCharges";
                ws.Cell(1, 7).Value = "PlatinumCharges";
                ws.Cell(1, 8).Value = "SilverCharges";
                ws.Cell(1, 9).Value = "IsOptional";

                for (int i = 0; i < data.Count; i++)
                {
                    ws.Cell(i + 2, 1).Value = data[i].Code;
                    ws.Cell(i + 2, 2).Value = data[i].VendorCode;
                    ws.Cell(i + 2, 3).Value = data[i].Category;
                    ws.Cell(i + 2, 4).Value = data[i].Type;
                    ws.Cell(i + 2, 5).Value = data[i].Unit;
                    ws.Cell(i + 2, 6).Value = data[i].GoldCharges;
                    ws.Cell(i + 2, 7).Value = data[i].PlatinumCharges;
                    ws.Cell(i + 2, 8).Value = data[i].SilverCharges;
                    ws.Cell(i + 2, 9).Value = data[i].IsOptional;

                }

                ws.Columns().AdjustToContents();

                using (var stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return stream.ToArray();
                }
            }


        }

    }
}


