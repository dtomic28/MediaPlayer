using MediaPlayer.Backend;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace MediaPlayer.Visual
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //private List<string> playlist = new List<string> { "Song 1", "Song 2", "Song 3" };

        private ObservableCollection<MultimediaFile> playlist = new ObservableCollection<MultimediaFile>
        {
            new MultimediaFile("C:\\Users\\danijel\\Videos\\Desktop\\Desktop 2020.05.26 - 19.03.47.01.mp4", "Pop", "C:\\Users\\danijel\\Pictures\\janez.png", "Song 1", new TimeSpan(0, 3, 45), "MP3"),
            new MultimediaFile("C:/Music/song2.mp4", "Rock", "C:\\Users\\danijel\\Pictures\\janez.png", "Song 2", new TimeSpan(0, 4, 10), "MP4"),
            new MultimediaFile("C:/Music/song3.wav", "Jazz", "C:\\Users\\danijel\\Pictures\\janez.png", "Song 3", new TimeSpan(0, 5, 0), "WAV")
        };

        private bool isPlaying = false;
        private BitmapImage _playIcon = new BitmapImage(new Uri("pack://application:,,,/Resources/Icons/play_icon.ico"));
        private BitmapImage _pauseIcon = new BitmapImage(new Uri("pack://application:,,,/Resources/Icons/pause_icon.ico"));


        public MainWindow()
        {
            InitializeComponent();
            Init();
            PlayPauseIcon.Source = _playIcon;
            MediaPlayer.Source = new Uri(playlist[0].FilePath);
        }

        private void PlayMedia(MultimediaFile file)
        {
            MediaTitle.Text = file.Title;
            Debug.WriteLine(file.Title);
        }

        private void Reset()
        {
            MediaTitle.Clear();
        }

        public void Init()
        {
            CurrentTimeLabel.Content = "--:--";
            PlaylistView.ItemsSource = playlist;
        }

        private void Import_Click(object sender, RoutedEventArgs e)
        {
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
        }

        private void PlaylistView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void PlaylistView_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            if ((PlaylistView.SelectedItem is MultimediaFile file))
            {
                PlayMedia(file);
            }
        }

        private void ProgressSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
        }


        private void StopBtn_OnClick(object sender, RoutedEventArgs e)
        {
            MediaTitle.Clear();
            MediaPlayer.Stop();
        }

        private void PlayBtn_OnClick(object sender, RoutedEventArgs e)
        {
            isPlaying = !isPlaying;
            PlayPauseIcon.Source = isPlaying ? _pauseIcon : _playIcon;

            if (isPlaying)
            {
                MediaPlayer.Play();
            }
            else
            {
                MediaPlayer.Pause();
            }
        }
        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            Reset();
        }
    }

}