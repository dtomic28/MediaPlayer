using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaPlayer.Backend
{
    class MultimediaFile
    {

        private string filePath;
        private string genre;
        private string imgPath;
        private string title;
        private TimeSpan duration;
        private string format;


        public MultimediaFile(string filePath, string genre, string imgPath, string title, TimeSpan duration, string format)
        {
            this.filePath = filePath;
            this.genre = genre;
            this.imgPath = imgPath;
            this.title = title;
            this.duration = duration;
            this.format = format;
        }

        public string FilePath => filePath;

        public string Genre => genre;

        public string ImgPath => imgPath;

        public string Title => title;

        public TimeSpan Duration => duration;

        public string Format => format;
    }
}
