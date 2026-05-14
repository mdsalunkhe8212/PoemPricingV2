using POEM.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POEM.Services.Interface
{
    interface IMasterDataRepository
    {
        List<KeyValuePair<string,string>> GetDropdown(string category);
        List<KeyValuePair<string, string>> GetDropdownFromDb(string category,string param="",string param1="", string param2 = "", string param3 = "", string param4 = "");
        FindingDetail GetFindingDetails(string findingnumber);
        Task<decimal?> GetPerStoneWeight(string stoneType, string growingType, string stoneShape, string lengthDiameter);
        Task<decimal?> GetStoneCostPerCarat(string vendor,string stoneType, string growingType, string stoneShape, string lengthDiameter, string stoneQuality);
        Task<decimal?> GetSettingCostPerStone(string vendor, string settingType, decimal perStoneWt, string shape, string category, string subCategory);

    }
}
