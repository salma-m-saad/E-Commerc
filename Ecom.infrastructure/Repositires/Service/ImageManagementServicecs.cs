using Ecom.core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.infrastructure.Repositires.Service
{
    class ImageManagementServicecs : IImageManagementServicecs
    {
        private readonly IFileProvider fileProvider;
        public ImageManagementServicecs(IFileProvider fileProvider)
        {
            this.fileProvider= fileProvider;
            
        }
        public async Task<List<string>> AddImageAsync(IFormFileCollection files, string src)
        {
            var SaveImageSrc = new List<string>();
            var ImageDirctory = Path.Combine("wwwroot","Images",src);
            if (Directory.Exists(ImageDirctory) is not true) 
            {
                Directory.CreateDirectory(ImageDirctory);
            }
            foreach (var item in files) 
            {
                if (item.Length >0) 
                {
                    var ImageName = item.FileName;
                    var root = Path.Combine( ImageDirctory,ImageName);
                    var ImageSrc = $"/Images/{src}/{ImageName}";
                    using (FileStream stream = new FileStream(root, FileMode.Create)) 
                    {
                        await item.CopyToAsync(stream);
                    }
                    SaveImageSrc.Add(ImageSrc);
                    
                }
            }
            return SaveImageSrc;


            
        }

        public void DeleteImageAsync(string src)
        {
            var Info= fileProvider.GetFileInfo(src);
            var root = Info.PhysicalPath;
            File.Delete(root);
            var directory = Path.GetDirectoryName(root);
            if (Directory.Exists(directory) & !Directory.EnumerateFileSystemEntries(directory).Any())
            {
                Directory.Delete(directory);
            }

        }
    }
}
