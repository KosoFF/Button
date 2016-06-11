using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Chat;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Button.Core.Helpers;
using Button.SqlLinkService;

namespace Button.Services
{
    public interface IImageService
    {
        Task<byte[]> GetPrivateImageBytes(string imageUploadId);
        Task<ImageSource> GetPrivateImage(string imageUploadId, int? decodePixelWidth = null, int? decodePixelHeight = null);
        Task<ImageSource> GetPublicImage(string imageUrl);
        Task<ImageSource> SetImage(string userid, StorageFile file);

        Task<StorageFile> PickPhoto();
        void ClearImagesCache();
    }

    public class ImagesService : IImageService
    {
        #region Fields

        private const string imagesFolder = "images";
        private const string fileNameFormat = "{0}.jpg";
        private readonly ISqlLinkService linkService;
        private readonly IFileStorage fileStorage;
        private static readonly List<string> thisSessionImages = new List<string>(); // image links that were loaded during this session of application
        private readonly object thisLock = new object();



        #endregion

        #region Ctor

        public ImagesService(ISqlLinkService linkService, IFileStorage fileStorage)
        {
            this.fileStorage = fileStorage;
            this.linkService = linkService;
        }

        #endregion

        #region IImageService Members
        public async Task<StorageFile> PickPhoto()
        {

            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".png");
            StorageFile file = await openPicker.PickSingleFileAsync();
            return file;
        }
        public async Task<ImageSource> SetImage(string userid, StorageFile file)
        {
           
            if(userid == null || file == null)
                return null;
            var stream = (FileRandomAccessStream)await file.OpenAsync(FileAccessMode.Read);
            var streamForImage = stream.CloneStream();

            var userImageBytes = new byte[stream.Size];
            using (var dataReader = new DataReader(stream))
            {
                await dataReader.LoadAsync((uint)stream.Size);
                dataReader.ReadBytes(userImageBytes);
            }
            await fileStorage.WriteToFile(imagesFolder, string.Format(fileNameFormat, userid), userImageBytes);
            lock (thisLock)
            {
                thisSessionImages.Add(userid);
            }
            await linkService.SetPhoto(userid, userImageBytes);
            return ImageHelper.CreateBitmapImage(streamForImage);
        }
        public async Task<byte[]> GetPrivateImageBytes(string imageUploadId)
        {
            bool isInThisSession;
            lock (this)
            {
                isInThisSession = thisSessionImages.Contains(imageUploadId);
            }

            return isInThisSession ? await GetImageBytesFromStorage(imageUploadId) : await GetImageBytesFromServer(imageUploadId);
        }

        public async Task<ImageSource> GetPrivateImage(string imageUploadId, int? decodePixelWidth = null, int? decodePixelHeight = null)
        {
            var imageBytes = await GetPrivateImageBytes(imageUploadId);
            if (imageBytes == null) return null;

            return await ImageHelper.CreateBitmapImage(imageBytes, decodePixelWidth, decodePixelHeight);
        }

        public async Task<ImageSource> GetPublicImage(string imageUrl)
        {
            if (string.IsNullOrWhiteSpace(imageUrl)) return null;

            try
            {
                return new BitmapImage(new System.Uri(imageUrl));
            }
            catch (Exception)
            {
                return null;
            }

        }

        public void ClearImagesCache()
        {
            lock (thisLock)
            {
                thisSessionImages.Clear();
            }
        }

        #endregion

        #region Private methods

        private async Task<byte[]> GetImageBytesFromServer(string imageUploadId)
        {
            var img = await linkService.GetPhoto(imageUploadId);
            if (img == null) return null;
            await fileStorage.WriteToFile(imagesFolder, string.Format(fileNameFormat, imageUploadId), img);
            lock (thisLock)
            {
                thisSessionImages.Add(imageUploadId);
            }
            return img;
        }


        private async Task<byte[]> GetImageBytesFromStorage(string imageUploadId)
        {
            return await fileStorage.ReadFile(imagesFolder, string.Format(fileNameFormat, imageUploadId));
        }

        #endregion

    }
}
