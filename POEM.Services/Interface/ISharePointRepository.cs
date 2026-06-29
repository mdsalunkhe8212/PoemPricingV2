using POEM.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POEM.Services.Interface
{
    public interface ISharePointRepository
    {
        Task<SharePointImage> GetImageAsync(string filePath);
        //Task<List<string>> GetImageListAsync(string folderPath);
    }
}
