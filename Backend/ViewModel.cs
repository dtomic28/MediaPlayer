using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;

namespace MediaPlayer.Backend
{
    public class ViewModel : INotifyPropertyChanged
    {
        private MultimediaFile? _selectedFile = null;

        public ObservableCollection<MultimediaFile> Playlist { get; }

        private ObservableCollection<string> genreList;

        private SettingsWindow? settingsWindow = null;

        public bool CanRemoveOrEdit => _selectedFile != null;
        public MultimediaFile? SelectedFile
        {
            get => _selectedFile;
            set
            {
                _selectedFile = value;

                // Notify RemoveCommand and EditCommand
                (RemoveCommand as RelayCommand)?.RaiseCanExecuteChanged();
                (EditCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }

        public ICommand ImportCommand { get; }
        public ICommand ExportCommand { get; }
        public ICommand AddCommand { get; }
        public ICommand RemoveCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand ExitCommand { get; }
        public ICommand SettingsCommand { get; }

        public ViewModel(ObservableCollection<MultimediaFile> playlist, ObservableCollection<string> genreList)
        {
            Playlist = playlist;
            this.genreList = genreList;
            ImportCommand = new RelayCommand(ImportPlaylist);
            ExportCommand = new RelayCommand(ExportPlaylist);
            AddCommand = new RelayCommand(AddMultimediaFile);
            RemoveCommand = new RelayCommand(RemoveSelectedFile, () => _selectedFile != null);
            EditCommand = new RelayCommand(EditSelectedFile, () => _selectedFile != null);
            ExitCommand = new RelayCommand(() => Application.Current.Shutdown());
            SettingsCommand = new RelayCommand(OpenSettings);
        }

        private void OpenSettings()
        {
            if (settingsWindow == null)
            {
                settingsWindow = new SettingsWindow(genreList);
                settingsWindow.Closed += (s, args) => settingsWindow = null;  // Release reference when closed
                settingsWindow.ShowDialog();
            }
            else
            {
                settingsWindow.Focus();  // Bring the already open window to the front
            }

        }

        private void ImportPlaylist()
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

                    Playlist.Clear();

                    foreach (var element in root.Elements("MultimediaFile"))
                    {
                        Playlist.Add(new MultimediaFile(element));
                    }

                    MessageBox.Show("Playlist imported successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error importing playlist: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ExportPlaylist()
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

                    foreach (var file in Playlist)
                    {
                        var node = new XElement("MultimediaFile");
                        file.UpdateXml(node, isWrite: true); // Write each file to XML
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

        private void AddMultimediaFile()
        {
            string Path = AppDomain.CurrentDomain.BaseDirectory +
                          @"..\..\..\Resources\Music\Frenkie - Izgubljeni Snovi.mp3";
            Debug.Print(Path);
            Playlist.Add(new MultimediaFile(Path, "Genre", "C:/example/image.jpg"));
        }

        private void RemoveSelectedFile()
        {
            if (SelectedFile != null) Playlist.Remove(SelectedFile);
        }

        private void EditSelectedFile()
        {
            SelectedFile?.UpdatePath("C:/example/editedfile.mp4");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => _canExecute?.Invoke() ?? true;

        public void Execute(object parameter) => _execute();

        public event EventHandler CanExecuteChanged;

        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }

}

