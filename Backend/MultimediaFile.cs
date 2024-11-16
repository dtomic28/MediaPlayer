using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.IO;
using TagLib;


namespace MediaPlayer.Backend
{
    class MultimediaFile
    {

        private string filePath = null!;
        private string genre = null!;
        private string imgPath = null!;
        private string title = null!;
        private TimeSpan duration;
        private string format = null!;


        public MultimediaFile(string filePath, string genre, string imgPath)
        {
            if (!System.IO.File.Exists(filePath))
            {
                Console.WriteLine($"File not found: {filePath}");
                return;
            }

            this.filePath = filePath;
            this.genre = genre;
            this.imgPath = imgPath;

            ParseMetadata();
        }

        public MultimediaFile(XElement node)
        {
            ReadXml(node);

            if (!System.IO.File.Exists(filePath))
            {
                Console.WriteLine($"File not found: {filePath}");
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
                Console.WriteLine($"Error extracting duration: {ex.Message}");
                return TimeSpan.Zero;
            }
        }

        private void ParseMetadata()
        {
            try
            {
                format = Path.GetExtension(FilePath)?.TrimStart('.').ToUpperInvariant();

                title = Path.GetFileNameWithoutExtension(FilePath);

                duration = ExtractDuration(FilePath);

                if (!string.IsNullOrEmpty(ImgPath) && !System.IO.File.Exists(ImgPath))
                {
                    Console.WriteLine($"Image file not found: {ImgPath}");
                    imgPath = string.Empty;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing metadata for {FilePath}: {ex.Message}");
            }
        }

        public void UpdateXML(XElement node, bool isWrite)
        {
            if (node == null)
                return;

            if(isWrite)
                WriteXml(node);
            else
                ReadXml(node);
        }

        private void WriteXml(XElement node)
        {
            node.Add(new XElement("FilePath", filePath));
            node.Add(new XElement("Genre", genre));
            node.Add(new XElement("ImagePath", imgPath));
        }

        private void ReadXml(XElement node)
        {
            filePath = node.Element("FilePath")?.Value ?? string.Empty;
            genre = node.Element("Genre")?.Value ?? string.Empty;
            imgPath = node.Element("ImagePath")?.Value ?? string.Empty;
        }

        public string FilePath => filePath;

        public string Genre => genre;

        public string ImgPath => imgPath;

        public string Title => title;

        public TimeSpan Duration => duration;

        public string Format => format;
    }
}
