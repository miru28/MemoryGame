using System.ComponentModel;
using System.IO;
using System.Windows.Media.Imaging;

namespace MemoryGAME.Models
{
    public class Card : INotifyPropertyChanged
    {
        private static readonly Random random = new Random();
        private bool _isFlipped;
        private bool _isMatched;
        private string _imagePath;
        private int _id;
        private BitmapImage _cachedImage;

        private static MemoryGAME.Services.ImageService _imageService;

        static Card()
        {

        }


        public static void SetImageService(MemoryGAME.Services.ImageService imageService)
        {
            _imageService = imageService;
        }

        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged(nameof(Id));
            }
        }

        public string ImagePath
        {
            get => _imagePath;
            set
            {
                _imagePath = value;
                _cachedImage = null;
                OnPropertyChanged(nameof(ImagePath));
                OnPropertyChanged(nameof(DisplayImage));
            }
        }

        public bool IsFlipped
        {
            get => _isFlipped;
            set
            {
                _isFlipped = value;
                OnPropertyChanged(nameof(IsFlipped));
                OnPropertyChanged(nameof(DisplayImage));
            }
        }

        public bool IsMatched
        {
            get => _isMatched;
            set
            {
                _isMatched = value;
                OnPropertyChanged(nameof(IsMatched));
                OnPropertyChanged(nameof(Visibility));
            }
        }




        public object DisplayImage
        {
            get
            {
                if (IsFlipped)
                {
                    if (_cachedImage == null && !string.IsNullOrEmpty(ImagePath))
                    {
                        try
                        {

                            if (_imageService != null)
                            {
                                _cachedImage = _imageService.GetImageFromPath(ImagePath);

                            }
                            else
                            {

                                return LoadCardbackImage();
                            }
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"Error loading card image: {ex.Message}");
                            return LoadCardbackImage();
                        }
                    }
                    return _cachedImage ?? LoadCardbackImage();
                }
                else
                {
                    return LoadCardbackImage();
                }
            }
        }




        private static BitmapImage _cardbackImage;

        private static BitmapImage LoadCardbackImage()
        {
            if (_cardbackImage != null)
                return _cardbackImage;

            try
            {

                int width = 100;
                int height = 100;
                int stride = width * 4;
                byte[] pixels = new byte[stride * height];


                byte blue = 128;
                byte green = 0;
                byte red = 0;
                byte alpha = 255;

                for (int i = 0; i < pixels.Length; i += 4)
                {
                    pixels[i] = blue;
                    pixels[i + 1] = green;
                    pixels[i + 2] = red;
                    pixels[i + 3] = alpha;
                }


                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        if ((x + y) % 10 == 0)
                        {
                            int index = y * stride + x * 4;
                            pixels[index] = 200;     // B
                            pixels[index + 1] = 200; // G
                            pixels[index + 2] = 200; // R
                        }
                    }
                }

                var bitmap = System.Windows.Media.Imaging.BitmapSource.Create(
                    width, height, 96, 96, System.Windows.Media.PixelFormats.Bgra32, null, pixels, stride);
                bitmap.Freeze();


                _cardbackImage = new BitmapImage();
                var encoder = new System.Windows.Media.Imaging.PngBitmapEncoder();
                encoder.Frames.Add(System.Windows.Media.Imaging.BitmapFrame.Create(bitmap));

                using (var memoryStream = new MemoryStream())
                {
                    encoder.Save(memoryStream);
                    memoryStream.Position = 0;

                    _cardbackImage.BeginInit();
                    _cardbackImage.CacheOption = BitmapCacheOption.OnLoad;
                    _cardbackImage.StreamSource = memoryStream;
                    _cardbackImage.EndInit();
                    _cardbackImage.Freeze();
                }

                return _cardbackImage;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to create cardback: {ex.Message}");
                return null;
            }
        }

        public string Visibility => IsMatched ? "Hidden" : "Visible";

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
