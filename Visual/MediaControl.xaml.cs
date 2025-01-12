using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace MediaPlayer.Visual
{
    public partial class MediaControl : UserControl
    {
        private BitmapImage _playIcon = new BitmapImage(new Uri("pack://application:,,,/Resources/Icons/play_icon.ico"));
        private BitmapImage _pauseIcon = new BitmapImage(new Uri("pack://application:,,,/Resources/Icons/pause_icon.ico"));

        public event EventHandler PlayPauseClicked;
        public event EventHandler StopClicked;
        public event EventHandler SkipNextClicked;
        public event EventHandler SkipPrevClicked;
        public event EventHandler RepeatClicked;
        public event EventHandler ShuffleClicked;

        public MediaControl()
        {
            InitializeComponent();
            PlayPauseIcon.Source = _playIcon;
        }

        public Slider GetProgressSlider()
        {
            return ProgressSlider;
        }

        public Label GetCurrentTimeLabel()
        {
            return CurrentTimeLabel;
        }

        public void SetPlayPauseIcon(bool isPlaying)
        {
            PlayPauseIcon.Source = isPlaying ? _pauseIcon : _playIcon;
        }

        private void PlayPause_Click(object sender, RoutedEventArgs e)
        {
            PlayPauseClicked?.Invoke(this, EventArgs.Empty);
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            StopClicked?.Invoke(this, EventArgs.Empty);
        }

        private void SkipNext_Click(object sender, RoutedEventArgs e)
        {
            SkipNextClicked?.Invoke(this, EventArgs.Empty);
        }

        private void SkipPrev_Click(object sender, RoutedEventArgs e)
        {
            SkipPrevClicked?.Invoke(this, EventArgs.Empty);
        }

        private void Repeat_Click(object sender, RoutedEventArgs e)
        {
            RepeatClicked?.Invoke(this, EventArgs.Empty);
        }

        private void Shuffle_Click(object sender, RoutedEventArgs e)
        {
            ShuffleClicked?.Invoke(this, EventArgs.Empty);
        }

        private void ProgressSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
        }
    }
}
