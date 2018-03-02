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
using System.Windows.Shapes;
using System.IO;

namespace CarApplication
{
    /// <summary>
    /// Used for adding Vans or editing existing vans
    /// </summary>
    public partial class EditVan : Window
    {
        private Van selectedVan;
        //Called when an existing van is being edited
        public EditVan(Van selVan)
        {
            InitializeComponent();

            MainWindow main = Owner as MainWindow;

            //Get the selected car from the main window
            selectedVan = selVan;

            //Fill Details
            EnterMake.Text = selectedVan.Make;
            EnterModel.Text = selectedVan.Model;
            EnterPrice.Text = selectedVan.Price.ToString();
            EnterYear.Text = selectedVan.Year.ToString();
            EnterColour.Text = selectedVan.Colour;
            EnterMileage.Text = selectedVan.Mileage.ToString();
            EnterType.Text = selectedVan.Type;
            EnterWheelbase.Text = selectedVan.Wheelbase.ToString();
            EnterDescription.Text = selectedVan.Description;
            EnterEngine.Text = selectedVan.Engine;
            EnterImage.Source = selectedVan.Image;
        }
        //Called when a new van is being added
        public EditVan()
        {
            InitializeComponent();
        }
        //When the window is loaded, combo box values are retrieved from the van class
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            EnterWheelbase.ItemsSource = Van.Wheelbases.Keys;
            EnterType.ItemsSource = Van.Types.Keys;
        }
        //Adds the new van or updates the old ones information
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = Owner as MainWindow;
            if (InputCheck() == true)
            {
                if (selectedVan == null)
                {
                    selectedVan = new Van();
                    main.AddToList(selectedVan as Vehicle);
                }
                selectedVan.Make = EnterMake.Text;
                selectedVan.Model = EnterModel.Text;
                selectedVan.Price = decimal.Parse(EnterPrice.Text);
                selectedVan.Year = int.Parse(EnterYear.Text);
                selectedVan.Colour = EnterColour.Text;
                selectedVan.Mileage = int.Parse(EnterMileage.Text);
                selectedVan.Type = EnterType.Text;
                selectedVan.Wheelbase = EnterType.Text;
                selectedVan.Description = EnterDescription.Text;
                selectedVan.Engine = EnterEngine.Text;
                selectedVan.Image = EnterImage.Source as BitmapImage;
                
                this.Close();
            }
        }
        // Checks if all the required fields are filled
        private bool InputCheck()
        {
            //Reset all label colours
            LblMake.Foreground = new SolidColorBrush(Colors.LightGray);
            LblModel.Foreground = new SolidColorBrush(Colors.LightGray);
            LblPrice.Foreground = new SolidColorBrush(Colors.LightGray);
            LblYear.Foreground = new SolidColorBrush(Colors.LightGray);
            LblWheelbase.Foreground = new SolidColorBrush(Colors.LightGray);
            LblColour.Foreground = new SolidColorBrush(Colors.LightGray);

            #region input checks
            bool inputsCorrect = true;
            if (EnterMake.Text == null)
            {
                LblMake.Foreground = new SolidColorBrush(Colors.Red);
                inputsCorrect = false;
            }
            if (EnterModel.Text == null)
            {
                LblModel.Foreground = new SolidColorBrush(Colors.Red);
                inputsCorrect = false;
            }
            if (EnterPrice.Text == null)
            {
                LblPrice.Foreground = new SolidColorBrush(Colors.Red);
                inputsCorrect = false;
            }
            if (EnterYear.Text == null)
            {
                LblYear.Foreground = new SolidColorBrush(Colors.Red);
                inputsCorrect = false;
            }
            if (int.TryParse(EnterWheelbase.Text, out int x))
            {
                LblWheelbase.Foreground = new SolidColorBrush(Colors.Red);
                inputsCorrect = false;
            }
            if (EnterColour.Text == null)
            {
                LblColour.Foreground = new SolidColorBrush(Colors.Red);
                inputsCorrect = false;
            }
            #endregion

            return inputsCorrect;
        }
        //Closes the program when the cancel button is clicked
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        //Lets the user add an image to the van using a windows dialogue
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

                BitmapImage imageCreated = new BitmapImage(new Uri(newFile, UriKind.RelativeOrAbsolute));

                EnterImage.Source = imageCreated;

            }
        }
        //Returns the directory the image will be saved to inside the programs directory
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
