using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Button.Services
{
    public interface IFileStorage
    {
        Task WriteToFile(string folderName, string fileName, byte[] bytes);
        Task<byte[]> ReadFile(string folderName, string fileName);
    }


    public class FileStorage : IFileStorage
    {
       

        public async Task WriteToFile(string folderName, string fileName, byte[] bytes)
        {
           
                var folder = await ApplicationData.Current.LocalFolder.CreateFolderAsync(folderName, CreationCollisionOption.OpenIfExists);
                StorageFile file = await folder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
                using (var stream = await file.OpenStreamForWriteAsync())
                {
                    await stream.WriteAsync(bytes, 0, bytes.Length);
                }
        }

        public async Task<byte[]> ReadFile(string folderName, string fileName)
        {
            var folder = await ApplicationData.Current.LocalFolder.CreateFolderAsync(folderName, CreationCollisionOption.OpenIfExists);

            try
            {
                using (Stream stream = await folder.OpenStreamForReadAsync(fileName))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        stream.CopyTo(ms);
                        return ms.ToArray();
                    }
                }
            }
            catch (FileNotFoundException)
            {
                return null;
            }

        }
    }
}
