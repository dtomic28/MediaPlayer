using MediaPlayer.Backend;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Xml.Linq;

namespace MediaPlayer.Visual
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<MultimediaFile> playlist = new ObservableCollection<MultimediaFile>();
        private ObservableCollection<string> genreList = new ObservableCollection<string>();

        private bool _isPlaying = false;
        private BitmapImage _playIcon = new BitmapImage(new Uri("pack://application:,,,/Resources/Icons/play_icon.ico"));
        private BitmapImage _pauseIcon = new BitmapImage(new Uri("pack://application:,,,/Resources/Icons/pause_icon.ico"));
        private ViewModel _vm;
        private bool IsSelected;


        public MainWindow()
        {
            _vm = new ViewModel(playlist, genreList);
            InitializeComponent();
            LoadGenres();
            Init();
            PlayPauseIcon.Source = _playIcon;
            DataContext = _vm;
        }

        private void PlayMedia(MultimediaFile file)
        {
            try
            {
                MediaTitle.Text = file.Title;
                MediaPlayer.Source = new Uri(file.FilePath);
                Debug.WriteLine(file.Title);
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error playing file: {file.FilePath}");
            }
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
            _isPlaying = true; 
            PlayBtn_OnClick(null!, null!);
        }

        private void PlayBtn_OnClick(object sender, RoutedEventArgs e)
        {
            _isPlaying = !_isPlaying;
            PlayPauseIcon.Source = _isPlaying ? _pauseIcon : _playIcon;

            if (_isPlaying)
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

        private void PlaylistView_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var clickedItem = (e.OriginalSource as FrameworkElement)?.DataContext as MultimediaFile;

            // If clicked outside of an item, deselect
            if (clickedItem == null)
            {
                PlaylistView.SelectedItem = null;
                _vm.SelectedFile = clickedItem;
            }
        }

        private void LoadGenres()
        {
            if (Properties.Settings.Default.GenresList == null)
            {
                Properties.Settings.Default.GenresList = new StringCollection();
            }
            genreList.Clear();
            foreach (string genre in Properties.Settings.Default.GenresList)
            {
                genreList.Add(genre);
            }
        }

        // Save genres to application settings
        private void SaveGenres()
        {
            Properties.Settings.Default.GenresList.Clear();
            foreach (var genre in genreList)
            {
                Properties.Settings.Default.GenresList.Add(genre);
            }
            Properties.Settings.Default.Save();
        }

        protected override void OnClosed(EventArgs e)
        {
            SaveGenres();
            base.OnClosed(e);
        }
    }

}