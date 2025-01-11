using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using TagLib;

namespace MediaPlayer
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        private ObservableCollection<string> genreList;

        public SettingsWindow(ObservableCollection<string> genreList)
        {
            this.genreList = genreList;
            InitializeComponent();
            genreListBox.ItemsSource = genreList;
        }

        private void AddGenre_Click(object sender, RoutedEventArgs e)
        {
            string newGenre = genreTextBox.Text;
            if (!string.IsNullOrWhiteSpace(newGenre) && !genreList.Contains(newGenre))
            {
                genreList.Add(newGenre);
            }
            else
            {
                MessageBox.Show("Invalid or duplicate genre.");
            }
        }

        private void EditGenre_Click(object sender, RoutedEventArgs e)
        {
            if (genreList.Contains(genreTextBox.Text))
            {
                MessageBox.Show("Invalid or duplicate genre.");
                return;
            } 

            genreList[genreListBox.SelectedIndex] = genreTextBox.Text;
        }

        private void DeleteGenre_Click(object sender, RoutedEventArgs e)
        {
            genreList.RemoveAt(genreListBox.SelectedIndex);
        }

        private void GenreListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (genreListBox.SelectedItem != null)
            {
                editGenreButton.IsEnabled = true;
                deleteGenreButton.IsEnabled = true;
            }
            else
            {
                editGenreButton.IsEnabled = false;
                deleteGenreButton.IsEnabled = false;
            }
        }

    }
}
