using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Linq;
using System.IO;
using System.Windows.Media.Imaging;
using TagLib;


namespace MediaPlayer.Backend
{
    public class MultimediaFile
    {
        private static readonly BitmapImage DefaultImageUri = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/albumart.jpg"));

        private string _filePath = null!;
        private string _genre = null!;
        private string _imgPath = null!;
        private string _title = null!;
        private TimeSpan _duration;
        private string _format = null!;
        private BitmapImage _image = null!;

        public MultimediaFile(string filePath, string genre, string imgPath)
        {
            if (!System.IO.File.Exists(filePath))
            {
                Debug.WriteLine($"File not found: {filePath}");
                return;
            }

            _filePath = filePath;
            _genre = genre;

            ParseMetadata();
        }

        private void ExtractAlbumArt()
        {
            try
            {
                var file = TagLib.File.Create(_filePath);

                // Check if there is embedded artwork
                if (file.Tag.Pictures != null && file.Tag.Pictures.Length > 0)
                {
                    var pictureData = file.Tag.Pictures[0].Data.Data;

                    // Create a memory stream for the image data
                    using (var ms = new MemoryStream(pictureData))
                    {
                        var bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.StreamSource = ms;
                        bitmap.CacheOption = BitmapCacheOption.OnLoad;
                        bitmap.EndInit();
                        _image = bitmap;
                    }
                }
                else
                {
                    _image = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/albumart.jpg"));
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

            if (!System.IO.File.Exists(_filePath))
            {
                Debug.WriteLine($"File not found: {_filePath}");
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
                _format = Path.GetExtension(FilePath)?.TrimStart('.').ToUpperInvariant();

                _title = Path.GetFileNameWithoutExtension(FilePath);

                _duration = ExtractDuration(FilePath);

                if (!string.IsNullOrEmpty(ImgPath) && !System.IO.File.Exists(ImgPath))
                {
                    Debug.WriteLine($"Image file not found: {ImgPath}");
                    _imgPath = string.Empty;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error parsing metadata for {FilePath}: {ex.Message}");
            }
            ExtractAlbumArt();
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
            node.Add(new XElement("FilePath", _filePath));
            node.Add(new XElement("Genre", _genre));
            node.Add(new XElement("ImagePath", _imgPath));
        }

        private void ReadXml(XElement node)
        {
            _filePath = node.Element("FilePath")?.Value ?? string.Empty;
            _genre = node.Element("Genre")?.Value ?? string.Empty;
            _imgPath = node.Element("ImagePath")?.Value ?? string.Empty;
        }

        public string FilePath => _filePath;

        public string Genre => _genre;

        public string ImgPath => _imgPath;

        public string Title => _title;

        public TimeSpan Duration => _duration;

        public string Format => _format;

        public BitmapImage Image => _image;

        public void UpdatePath(string path)
        {
            if (!System.IO.File.Exists(path))
            {
                Debug.WriteLine($"File not found: {path}");
                return;
            }

            this._filePath = path;
            ParseMetadata();
        }
    }
}
