using MediaPlayer.Backend;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Xml.Linq;

namespace MediaPlayer.Visual
{
    public partial class MainWindow : Window
    {
        private ObservableCollection<MultimediaFile> playlist = new ObservableCollection<MultimediaFile>();
        private ObservableCollection<string> genreList = new ObservableCollection<string>();

        private ViewModel _vm;
        private bool _isPlaying = false;
        private bool _isRepeating = false;

        private DispatcherTimer _progressTimer;
        private Slider slider;
        private Label currentTimeLabel;

        public MainWindow()
        {
            _vm = new ViewModel(playlist, genreList);
            InitializeComponent();
            LoadGenres();
            Init();
            DataContext = _vm;
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

        private void PlayMedia(MultimediaFile file)
        {
            try
            {
                MediaTitle.Text = file.Title;
                MediaPlayer.Source = new Uri(file.FilePath);
                MediaPlayer.Play();
                _isPlaying = true;
                MediaControl.SetPlayPauseIcon(true);
                _progressTimer.Start();
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error playing file: {file.FilePath} - {e.Message}");
            }
        }

        private void Reset(object sender, RoutedEventArgs routedEventArgs)
        {
            MediaTitle.Clear();
            MediaPlayer.Stop();
            _isPlaying = false;
            MediaControl.SetPlayPauseIcon(false);
        }

        public void Init()
        {
            PlaylistView.ItemsSource = playlist;
            _progressTimer = new DispatcherTimer();
            _progressTimer.Interval = TimeSpan.FromSeconds(1); // Update every second
            _progressTimer.Tick += ProgressTimer_Tick;
            slider = MediaControl.GetProgressSlider(); 
            currentTimeLabel = MediaControl.GetCurrentTimeLabel();
        }

        private void ProgressTimer_Tick(object? sender, EventArgs e)
        {
            if (MediaPlayer.NaturalDuration.HasTimeSpan)
            {
                slider.Maximum = MediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;
                slider.Value = MediaPlayer.Position.TotalSeconds;
                currentTimeLabel.Content = $"{MediaPlayer.Position.Minutes:D2}:{MediaPlayer.Position.Seconds:D2}";
            }   
        }


        private void PlaylistView_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (PlaylistView.SelectedItem is MultimediaFile file)
            {
                PlayMedia(file);
            }
        }

        private void MediaControl_PlayPauseClicked(object sender, EventArgs e)
        {
            if (MediaPlayer.Source == null && PlaylistView.SelectedItem is MultimediaFile file)
            {
                PlayMedia(file);
            }
            else
            {
                if (_isPlaying)
                {
                    MediaPlayer.Pause();
                    _isPlaying = false;
                    MediaControl.SetPlayPauseIcon(false);
                }
                else
                {
                    MediaPlayer.Play();
                    _isPlaying = true;
                    MediaControl.SetPlayPauseIcon(true);
                }
            }
        }

        private void MediaControl_StopClicked(object sender, EventArgs e)
        {
            MediaPlayer.Stop();
            _isPlaying = false;
            MediaControl.SetPlayPauseIcon(false);
            _progressTimer.Stop();
            slider.Value = 0;
        }

        private void MediaControl_SkipNextClicked(object sender, EventArgs e)
        {
            int nextIndex = PlaylistView.SelectedIndex + 1;
            if (nextIndex >= playlist.Count) nextIndex = 0;
            PlaylistView.SelectedIndex = nextIndex;
            PlayMedia(playlist[nextIndex]);
        }

        private void MediaControl_SkipPrevClicked(object sender, EventArgs e)
        {
            int prevIndex = PlaylistView.SelectedIndex - 1;
            if (prevIndex < 0) prevIndex = playlist.Count - 1;
            PlaylistView.SelectedIndex = prevIndex;
            PlayMedia(playlist[prevIndex]);
        }

        private void MediaControl_ShuffleClicked(object sender, EventArgs e)
        {
            var random = new Random();
            int index = random.Next(playlist.Count);
            PlaylistView.SelectedIndex = index;
            PlayMedia(playlist[index]);
        }

        private void MediaControl_RepeatClicked(object sender, EventArgs e)
        {
            _isRepeating = !_isRepeating;
            MessageBox.Show(_isRepeating ? "Repeat mode enabled." : "Repeat mode disabled.");
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

        private void MediaTitle_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;

            // Create the fade-in animation
            DoubleAnimation fadeInAnimation = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(1)
            };

            textBox.BeginAnimation(TextBox.OpacityProperty, fadeInAnimation);
        }

    }
}
