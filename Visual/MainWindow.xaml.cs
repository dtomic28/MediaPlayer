using MediaPlayer.Backend;
using Microsoft.Win32;
using System.Collections.ObjectModel;
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
        private ObservableCollection<MultimediaFile> playlist = new ObservableCollection<MultimediaFile>
        {
            //new MultimediaFile("C:\\Users\\danijel\\Videos\\Desktop\\Desktop 2020.05.26 - 19.03.47.01.mp4", "Pop", "C:\\Users\\danijel\\Pictures\\janez.png"),
        };

        private bool isPlaying = false;
        private BitmapImage _playIcon = new BitmapImage(new Uri("pack://application:,,,/Resources/Icons/play_icon.ico"));
        private BitmapImage _pauseIcon = new BitmapImage(new Uri("pack://application:,,,/Resources/Icons/pause_icon.ico"));


        public MainWindow()
        {
            InitializeComponent();
            Init();
            PlayPauseIcon.Source = _playIcon;
        }

        private void PlayMedia(MultimediaFile file)
        {
            MediaTitle.Text = file.Title;
            MediaPlayer.Source = new Uri(file.FilePath);
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
            var openFileDialog = new OpenFileDialog
            {
                Filter = "XML Files (*.xml)|*.xml",
                Title = "Import Playlist"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    XElement root = XElement.Load(openFileDialog.FileName);

                    playlist.Clear(); // Clear the current playlist

                    foreach (var element in root.Elements("MultimediaFile"))
                    {
                        playlist.Add(new MultimediaFile(element));
                    }

                    MessageBox.Show("Playlist imported successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error importing playlist: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "XML Files (*.xml)|*.xml",
                Title = "Export Playlist"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    XElement root = new XElement("Playlist");

                    foreach (var file in playlist)
                    {
                        var node = new XElement("MultimediaFile");
                        file.UpdateXML(node, isWrite: true); // Write each file to XML
                        root.Add(node);
                    }

                    root.Save(saveFileDialog.FileName);

                    MessageBox.Show("Playlist exported successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error exporting playlist: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
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
            isPlaying = true; 
            PlayBtn_OnClick(null!, null!);
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