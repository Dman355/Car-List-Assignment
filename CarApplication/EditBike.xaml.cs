using System;
using System.Collections.Generic;
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

namespace CarApplication
{
    /// <summary>
    /// Used for adding Bikes or editing existing bikes
    /// </summary>
    public partial class EditBike : Window
    {
        private Bike selectedBike;
        //Called when editing an existing vehicle
        public EditBike(Bike selBike)
        {
            InitializeComponent();

            MainWindow main = Owner as MainWindow;

            //Get the selected bike from the main window
            selectedBike = selBike;

            //Fill Details
            EnterMake.Text = selectedBike.Make;
            EnterModel.Text = selectedBike.Model;
            EnterPrice.Text = selectedBike.Price.ToString();
            EnterYear.Text = selectedBike.Year.ToString();
            EnterColour.Text = selectedBike.Colour;
            EnterMileage.Text = selectedBike.Mileage.ToString();
            EnterType.Text = selectedBike.Type;
            EnterDescription.Text = selectedBike.Description;
            EnterEngine.Text = selectedBike.Engine;
            EnterImage.Source = selectedBike.Image;
        }

        //Called when adding a new vehicle
        public EditBike()
        {
            InitializeComponent();
        }

        //When the window is loaded, information for combo boxes is retrieved from the bike class
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            EnterType.ItemsSource = Bike.Types.Keys;
        }

        //Adds the new vehicle or updates the old one
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = Owner as MainWindow;
            if (InputCheck() == true)
            {
                if (selectedBike == null)
                {
                    selectedBike = new Bike();
                    main.AddToList(selectedBike as Vehicle);
                }
                selectedBike.Make = EnterMake.Text;
                selectedBike.Model = EnterModel.Text;
                selectedBike.Price = decimal.Parse(EnterPrice.Text);
                selectedBike.Year = int.Parse(EnterYear.Text);
                selectedBike.Colour = EnterColour.Text;
                selectedBike.Mileage = int.Parse(EnterMileage.Text);
                selectedBike.Type = EnterType.Text;
                selectedBike.Description = EnterDescription.Text;
                selectedBike.Engine = EnterEngine.Text;
                selectedBike.Image = EnterImage.Source as BitmapImage;

                this.Close();
            }
        }

        // Checks if all the required fields are filled
        private bool InputCheck()
        {
            bool inputsCorrect = true;
            if (EnterMake.Text == null)
            {
                LblMake.Foreground = new SolidColorBrush(Colors.Red);
            }
            if (EnterModel.Text == null)
            {
                LblModel.Foreground = new SolidColorBrush(Colors.Red);
                inputsCorrect = false;
            }
            if (EnterPrice == null)
            {
                LblPrice.Foreground = new SolidColorBrush(Colors.Red);
                inputsCorrect = false;
            }
            if (EnterYear == null)
            {
                LblYear.Foreground = new SolidColorBrush(Colors.Red);
                inputsCorrect = false;
            }
            if (EnterColour == null)
            {
                LblColour.Foreground = new SolidColorBrush(Colors.Red);
                inputsCorrect = false;
            }
            return inputsCorrect;
        }

        //Closes the window when the close button is clicked
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        // LEts the user add an image using a windows dialogue
        private void SelectImage_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "Images (*.JPG;*.JPEG;*.PNG) | *.JPG;*.JPEG;*.PNG";
            Nullable<bool> result = dlg.ShowDialog();
            string filename = "";
            if (result == true)
            {
                filename = dlg.FileName;

                string imageDirectory = GetImageDirectory();

                string shortFileName = filename.Substring(filename.LastIndexOf('\\') + 1);

                string newFile = imageDirectory + shortFileName;

                if (!(File.Exists(newFile)))
                {
                    File.Copy(filename, newFile);
                }

                BitmapImage imageCreated = new BitmapImage(new Uri (newFile, UriKind.RelativeOrAbsolute));
                
                EnterImage.Source = imageCreated;

            }
        }
        //Returns the new path where the image will be stored inside the programs directory
        private string GetImageDirectory()
        {
            string currentDir = Directory.GetCurrentDirectory();
            DirectoryInfo parent = Directory.GetParent(currentDir);
            DirectoryInfo grandParent = Directory.GetParent(parent.FullName);
            string imageDirectory = grandParent + "\\images\\" + "\\vehicles\\";

            return imageDirectory;
        }
    }
}
