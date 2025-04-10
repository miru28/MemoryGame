using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace MemoryGAME.Services
{
    public class ImageService
    {
        private readonly Dictionary<string, List<string>> _categoryImages;
        private readonly Random _random = new Random();

        public ImageService()
        {
            _categoryImages = new Dictionary<string, List<string>>();
            LoadCategories();
        }

        private void LoadCategories()
        {
            try
            {
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;

                string[] categories = { "Animals", "Nature", "Sports" };

                var testCategory = new List<string>();

                for (int i = 1; i <= 8; i++)
                {
                    string colorPath = $"Color{i}";
                    testCategory.Add(colorPath);
                }

                _categoryImages["Test"] = testCategory;


                foreach (var category in categories)
                {
                    string categoryPath = Path.Combine(baseDir, "Images", category);
                    bool dirExists = Directory.Exists(categoryPath);



                    if (dirExists)
                    {
                        var images = Directory.GetFiles(categoryPath, "*.jpg")
                            .Union(Directory.GetFiles(categoryPath, "*.png"))
                            .Union(Directory.GetFiles(categoryPath, "*.jpeg"))
                            .Union(Directory.GetFiles(categoryPath, "*.gif"))
                            .ToList();



                        if (images.Any())
                        {
                            _categoryImages[category] = images;
                        }
                        else
                        {
                            _categoryImages[category] = new List<string>();
                        }
                    }
                    else
                    {
                        _categoryImages[category] = new List<string>();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading categories: {ex.Message}\n\n{ex.StackTrace}", "Error");
            }
        }

        public List<string> GetCategories()
        {
            return _categoryImages.Where(kv => kv.Value.Count >= 8).Select(kv => kv.Key).ToList();
        }

        public List<string> GetRandomImagesForCategory(string category, int count)
        {
            if (!_categoryImages.ContainsKey(category) || _categoryImages[category].Count < count / 2)
            {
                throw new ArgumentException($"Category {category} doesn't exist or doesn't have enough images. Need at least {count / 2} images.");
            }

            var selectedImages = _categoryImages[category]
                .OrderBy(x => _random.Next())
                .Take(count / 2)
                .ToList();

            var result = new List<string>();
            foreach (var image in selectedImages)
            {
                result.Add(image);
                result.Add(image);
            }


            return result.OrderBy(x => _random.Next()).ToList();
        }


        public BitmapImage GetImageFromPath(string imagePath)
        {

            if (imagePath.StartsWith("Color"))
            {
                return CreateColorImage(imagePath);
            }


            try
            {
                if (File.Exists(imagePath))
                {
                    var image = new BitmapImage();
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.UriSource = new Uri(imagePath, UriKind.Absolute);
                    image.EndInit();
                    image.Freeze();
                    return image;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"Image file does not exist: {imagePath}");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading image from {imagePath}: {ex.Message}");
            }


            return CreateColorImage("Color1");
        }




        private BitmapImage CreateColorImage(string colorName)
        {
            var colors = new Dictionary<string, System.Windows.Media.Color>
            {
                { "Color1", System.Windows.Media.Colors.Red },
                { "Color2", System.Windows.Media.Colors.Blue },
                { "Color3", System.Windows.Media.Colors.Green },
                { "Color4", System.Windows.Media.Colors.Yellow },
                { "Color5", System.Windows.Media.Colors.Purple },
                { "Color6", System.Windows.Media.Colors.Cyan },
                { "Color7", System.Windows.Media.Colors.Orange },
                { "Color8", System.Windows.Media.Colors.Pink }
            };

            System.Windows.Media.Color color = colors.ContainsKey(colorName)
                ? colors[colorName]
                : System.Windows.Media.Colors.Gray;


            int width = 100;
            int height = 100;
            int stride = width * 4;
            byte[] pixels = new byte[stride * height];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int index = y * stride + x * 4;
                    pixels[index] = color.B;     // Blue
                    pixels[index + 1] = color.G; // Green
                    pixels[index + 2] = color.R; // Red
                    pixels[index + 3] = color.A; // Alpha
                }
            }

            var bitmap = System.Windows.Media.Imaging.BitmapSource.Create(
                width, height, 96, 96, System.Windows.Media.PixelFormats.Bgra32, null, pixels, stride);
            bitmap.Freeze();


            var bitmapImage = new BitmapImage();
            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(System.Windows.Media.Imaging.BitmapFrame.Create(bitmap));

            using (var memoryStream = new MemoryStream())
            {
                encoder.Save(memoryStream);
                memoryStream.Position = 0;

                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = memoryStream;
                bitmapImage.EndInit();
                bitmapImage.Freeze();
            }

            return bitmapImage;
        }

        public void DebugCategory(string category)
        {
            if (!_categoryImages.ContainsKey(category))
            {
                MessageBox.Show($"Category {category} doesn't exist", "Debug");
                return;
            }

            var images = _categoryImages[category];
            var message = $"Category: {category}\nImages: {images.Count}\n\nFirst 5 images:";

            foreach (var image in images.Take(5))
            {
                bool exists = File.Exists(image);
                message += $"\n- {image} (Exists: {exists})";
            }

        }
    }
}
