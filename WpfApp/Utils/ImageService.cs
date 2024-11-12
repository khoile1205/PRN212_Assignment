using System;
using System.IO;
using System.Windows.Media.Imaging;
using Microsoft.Win32;

namespace WpfApp.Utils
{
    public interface IImageService
    {
        public (BitmapImage, string) UploadImage();
        public void SaveImage(BitmapImage image, string filePath);
    }

    public class ImageService : IImageService
    {
        private readonly string _imageSaveDirectory;

        public ImageService(string imageSaveDirectory)
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
            return (image, uniqueFileName);
        }

        public void SaveImage(BitmapImage image, string fileName)
        {
            if (image == null || string.IsNullOrEmpty(fileName))
                return;

            var filePath = Path.Combine(_imageSaveDirectory, fileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                var encoder = new PngBitmapEncoder(); // Defaulting to PNG
                encoder.Frames.Add(BitmapFrame.Create(image));
                encoder.Save(fileStream);
            }

            Console.WriteLine($"Image saved to: {filePath}");

        }

        public BitmapImage LoadImage(string fileName)
        {
            string filePath;

            // Check if the provided fileName is an absolute path
            if (Path.IsPathRooted(fileName))
            {
                filePath = fileName;
            }
            else
            {
                // Otherwise, assume it's a file in the _imageSaveDirectory
                filePath = Path.Combine(_imageSaveDirectory, fileName);
            }

            if (!File.Exists(filePath))
            {
                return null; // File does not exist
            }

            // Load the image
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(filePath);
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();

            return bitmap;
        }
    }
}
