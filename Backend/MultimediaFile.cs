using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Linq;
using System.IO;
using System.Windows.Media.Imaging;
using TagLib;

namespace MediaPlayer.Backend
{
    public class MultimediaFile : INotifyPropertyChanged
    {
        private string _filePath;
        private string _genre;
        private string _imgPath;
        private string _title;
        private TimeSpan _duration;
        private string _format;
        private BitmapImage _image;

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string FilePath
        {
            get => _filePath;
            set
            {
                if (_filePath != value)
                {
                    _filePath = value;
                    OnPropertyChanged(nameof(FilePath));
                }
            }
        }

        public string Genre
        {
            get => _genre;
            set
            {
                if (_genre != value)
                {
                    _genre = value;
                    OnPropertyChanged(nameof(Genre));
                }
            }
        }

        public string ImgPath
        {
            get => _imgPath;
            set
            {
                if (_imgPath != value)
                {
                    _imgPath = value;
                    OnPropertyChanged(nameof(ImgPath));
                }
            }
        }

        public string Title
        {
            get => _title;
            set
            {
                if (_title != value)
                {
                    _title = value;
                    OnPropertyChanged(nameof(Title));
                }
            }
        }

        public TimeSpan Duration
        {
            get => _duration;
            set
            {
                if (_duration != value)
                {
                    _duration = value;
                    OnPropertyChanged(nameof(Duration));
                }
            }
        }

        public string Format
        {
            get => _format;
            set
            {
                if (_format != value)
                {
                    _format = value;
                    OnPropertyChanged(nameof(Format));
                }
            }
        }

        public BitmapImage Image
        {
            get => _image;
            set
            {
                if (_image != value)
                {
                    _image = value;
                    OnPropertyChanged(nameof(Image));
                }
            }
        }

        public MultimediaFile(string filePath, string genre, string imgPath = "")
        {
            if (!System.IO.File.Exists(filePath))
            {
                Debug.WriteLine($"File not found: {filePath}");
                return;
            }

            FilePath = filePath;
            Genre = genre;

            if (!string.IsNullOrEmpty(imgPath) && System.IO.File.Exists(imgPath))
            {
                ImgPath = imgPath;
                Image = new BitmapImage(new Uri(imgPath));
            }
            else
            {
                ParseMetadata();
            }
        }

        private void ExtractAlbumArt()
        {
            try
            {
                var file = TagLib.File.Create(FilePath);

                if (file.Tag.Pictures != null && file.Tag.Pictures.Length > 0)
                {
                    var pictureData = file.Tag.Pictures[0].Data.Data;

                    using (var ms = new MemoryStream(pictureData))
                    {
                        var bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.StreamSource = ms;
                        bitmap.CacheOption = BitmapCacheOption.OnLoad;
                        bitmap.EndInit();
                        Image = bitmap;
                    }
                }
                else
                {
                    Image = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/albumart.jpg"));
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error trying to extract album art: {e.Message}");
            }
        }

        public MultimediaFile(XElement node)
        {
            ReadXml(node);

            if (!System.IO.File.Exists(FilePath))
            {
                Debug.WriteLine($"File not found: {FilePath}");
                return;
            }

            ParseMetadata();
        }

        public static TimeSpan ExtractDuration(string filePath)
        {
            try
            {
                using var file = TagLib.File.Create(filePath);
                return file.Properties.Duration;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error extracting duration: {ex.Message}");
                return TimeSpan.Zero;
            }
        }

        private void ParseMetadata()
        {
            try
            {
                Format = Path.GetExtension(FilePath)?.TrimStart('.').ToUpperInvariant();
                Title = Path.GetFileNameWithoutExtension(FilePath);
                Duration = ExtractDuration(FilePath);
                ExtractAlbumArt();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error parsing metadata for {FilePath}: {ex.Message}");
            }
        }

        public void UpdateXml(XElement node, bool isWrite)
        {
            if (node == null)
                return;

            if (isWrite)
                WriteXml(node);
            else
                ReadXml(node);
        }

        private void WriteXml(XElement node)
        {
            node.Add(new XElement("FilePath", FilePath));
            node.Add(new XElement("Genre", Genre));
            node.Add(new XElement("ImagePath", ImgPath));
        }

        private void ReadXml(XElement node)
        {
            FilePath = node.Element("FilePath")?.Value ?? string.Empty;
            Genre = node.Element("Genre")?.Value ?? string.Empty;
            ImgPath = node.Element("ImagePath")?.Value ?? string.Empty;
        }

        public void UpdatePath(string path)
        {
            if (!System.IO.File.Exists(path))
            {
                Debug.WriteLine($"File not found: {path}");
                return;
            }

            FilePath = path;
            ParseMetadata();
        }
    }
}
