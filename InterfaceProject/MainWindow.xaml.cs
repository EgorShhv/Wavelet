using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.IO;
using System.Security;
using Prism.Services.Dialogs;

namespace InterfaceProject
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;

            if (openFileDialog.ShowDialog() == true)
            {
                int count_images = 0;
                // Read the files
                foreach (String file in openFileDialog.FileNames)
                {
                    // Create a PictureBox.
                    try
                    {
                        ImageListBox.Items.Add(file);
                        count_images++;
                    }
                    catch (SecurityException ex)
                    {
                        ProgressConsole.Text += ("\n> Security error. Please contact your administrator for details.\n\n" +
                            "Error message: " + ex.Message + "\n\n" +
                            "Details (send to Support):\n\n" + ex.StackTrace + "\n"
                        );
                    }
                    catch (Exception ex)
                    {
                        // Could not load the image - probably related to Windows file system permissions.
                        ProgressConsole.Text += ("\n> Cannot display the image: " + file.Substring(file.LastIndexOf('\\'))
                            + ". You may not have permission to read the file, or " +
                            "it may be corrupt.\n\nReported error: " + ex.Message + "\n");
                    }
                }
                ProgressConsole.Text += "\n> Загруженно " + count_images + " изображение(й)\n";
            }
        }

        private void ImageListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ImageListBox.SelectedIndex >= 0 && ImageListBox.SelectedIndex < ImageListBox.Items.Count)
            {
                BitmapImage bi3 = new BitmapImage();
                bi3.BeginInit();
                bi3.UriSource = new Uri((string)ImageListBox.SelectedItem);
                bi3.EndInit();
                CurrentImage.Source = bi3;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (ImageListBox.Items.Count > 0)
            {
                ImageListBox.Items.RemoveAt(ImageListBox.SelectedIndex);
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            ImageListBox.Items.Clear();
        }
    }
}
