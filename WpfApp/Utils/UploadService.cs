using System;
using System.IO;
using System.Windows.Media.Imaging;
using Microsoft.Win32;

namespace WpfApp.Utils
{
    public interface IUploadService
    {
        public (BitmapImage, string) UploadImage();
        public void SaveImage(BitmapImage image, string filePath);
    }

    public class UploadService : IUploadService
    {
        private readonly string _imageSaveDirectory;

        public UploadService(string imageSaveDirectory)
        {
            _imageSaveDirectory = imageSaveDirectory;

            // Ensure the directory exists
            if (!Directory.Exists(_imageSaveDirectory))
                Directory.CreateDirectory(_imageSaveDirectory);
        }

        public (BitmapImage, string) UploadImage()
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png",
                Title = "Select an Image"
            };

            if (openFileDialog.ShowDialog() != true)
                return (null, null);

            var image = new BitmapImage(new Uri(openFileDialog.FileName));

            // Generate a unique file name with UUID
            var fileExtension = Path.GetExtension(openFileDialog.FileName);
            var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
            var filePath = Path.Combine(_imageSaveDirectory, uniqueFileName);

            return (image, filePath);
        }

        public void SaveImage(BitmapImage image, string filePath)
        {
            if (image == null || string.IsNullOrEmpty(filePath))
                return;

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                var encoder = new PngBitmapEncoder(); // Defaulting to PNG
                encoder.Frames.Add(BitmapFrame.Create(image));
                encoder.Save(fileStream);
            }
        }
    }
}
