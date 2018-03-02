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
    /// Used for adding Cars or editing existing cars
    /// </summary>
    public partial class EditCar : Window
    {
        private Car selectedCar;

        // Method called if a car that already exists is to be edited
        public EditCar(Car selCar)
        {
            InitializeComponent();
            MainWindow main = Owner as MainWindow;

            //Get the selected car from the main window
            selectedCar = selCar;

            //Fill Details of car
            EnterMake.Text = selectedCar.Make;
            EnterModel.Text = selectedCar.Model;
            EnterPrice.Text = selectedCar.Price.ToString();
            EnterYear.Text = selectedCar.Year.ToString();
            EnterColour.Text = selectedCar.Colour;
            EnterMileage.Text = selectedCar.Mileage.ToString();
            EnterBodyType.SelectedValue = selectedCar.BodyType;
            EnterDescription.Text = selectedCar.Description;
            EnterEngine.Text = selectedCar.Engine;
            EnterImage.Source = selectedCar.Image;
        }

        //Method called if a new car is being created
        public EditCar()
        {
            InitializeComponent();
        }

        //When the window loads, the body types are retrieved from the car class
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            EnterBodyType.ItemsSource = Car.BodyTypes.Keys;
        }

        //When the add button is clicked, the car is added to the collection of the main list
        //Or its information is updated after input checks
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = Owner as MainWindow;
            if (InputCheck() == true)
            {
                if (selectedCar == null)
                {
                    selectedCar = new Car();
                    main.AddToList(selectedCar as Vehicle);
                }
                selectedCar.Make = EnterMake.Text;
                selectedCar.Model = EnterModel.Text;
                selectedCar.Price = decimal.Parse(EnterPrice.Text);
                selectedCar.Year = int.Parse(EnterYear.Text);
                selectedCar.Colour = EnterColour.Text;
                selectedCar.Mileage = int.Parse(EnterMileage.Text);
                selectedCar.BodyType = EnterBodyType.SelectedValue.ToString();
                selectedCar.Description = EnterDescription.Text;
                selectedCar.Engine = EnterEngine.Text;
                selectedCar.Image = EnterImage.Source as BitmapImage;

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
        //If cancel is clicked, the window is closed
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        //If select image is clicked, a new windows dialogue is opened so the user can pick a picture for the car
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
        //Returns the new directory inside the programs path where the image will be stored
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
