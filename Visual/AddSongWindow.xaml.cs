using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MediaPlayer.Backend;
using Microsoft.Win32;

namespace MediaPlayer.Visual
{
    /// <summary>
    /// Interaction logic for AddSongWindow.xaml
    /// </summary>
    public partial class AddSongWindow : Window
    {
        private ObservableCollection<MultimediaFile> playlist;
        private MultimediaFile? _selectedFile;
        private int _selectedFileIndex = -1;
        private string _tempImgPath = string.Empty;

        public void SetSelectedFile(int index)
        {
            if (index >= 0 && index < playlist.Count)  // Ensure index is valid
            {
                _selectedFileIndex = index;
                _selectedFile = playlist[index];  // Fetch the file from the playlist
                SongTitleTextBox.Text = _selectedFile.Title;
                FilePathTextBox.Text = _selectedFile.FilePath;
                GenreComboBox.Text = _selectedFile.Genre;
                UpdateUI();  // Call update method if necessary
            }
        }

        private void UpdateUI()
        {
            if (_selectedFile != null)
            {
                Title = "Edit file";
                AddSongButton.Content = "Edit file";
            }
            else
            {
                Title = "Add file";
                AddSongButton.Content = "Add file";
            }
        }

        public AddSongWindow(ObservableCollection<MultimediaFile> playlist, ObservableCollection<string> genreList)
        {
            this.playlist = playlist;
            InitializeComponent();
            GenreComboBox.ItemsSource = genreList;
        }

        private void SelectFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Media Files (*.mp4;*.mp3;*.avi;*.wav)|*.mp4;*.mp3;*.avi;*.wav|All Files|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                FilePathTextBox.Text = openFileDialog.FileName;
            }
        }

        private void AddSong_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SongTitleTextBox.Text) ||
                string.IsNullOrWhiteSpace(FilePathTextBox.Text) ||
                GenreComboBox.SelectedItem == null)
            {
                MessageBox.Show("Please fill out all fields.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (_selectedFile == null)
            {
                playlist.Add(new MultimediaFile(FilePathTextBox.Text, (string)GenreComboBox.SelectedItem,_tempImgPath));
            }
            else
            {
                playlist[_selectedFileIndex].Title = SongTitleTextBox.Text;
                playlist[_selectedFileIndex].FilePath = FilePathTextBox.Text;
                playlist[_selectedFileIndex].Genre = ((string)GenreComboBox.SelectedItem);
                playlist[_selectedFileIndex].ImgPath = _tempImgPath;
            }
            this.DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            Close();
        }

        private void SongImage_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files (*.png;*.jpg;*.jpeg;*.bmp)|*.png;*.jpg;*.jpeg;*.bmp|All Files|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    _tempImgPath = openFileDialog.FileName;
                    SongImage.Source = new BitmapImage(new Uri(openFileDialog.FileName));
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading image: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

        }
    }
}
