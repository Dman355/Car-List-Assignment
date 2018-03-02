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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using Microsoft.Win32;

namespace CarApplication
{
    /// <summary>
    /// By Daire Finn, S00173047
    /// A Vehicle listing app for cars, bikes and vans.
    /// The user can add, edit and delete vehicles.
    /// They can also save the list or load an existing one
    /// </summary>
    public partial class MainWindow : Window
    {
        static private ObservableCollection<Vehicle> VehiclesCollection = new ObservableCollection<Vehicle>();
        static private ObservableCollection<Vehicle> FilteredVehicles = new ObservableCollection<Vehicle>();
        public MainWindow()
        {
            InitializeComponent();
        }
        //When the window loads, add sample cars to the list and update the source of the listbox
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AddSampleVehicles();

            ListboxVehicles.ItemsSource = VehiclesCollection;

            List<string> SortByCategories = new List<string>()
            {
                "None","Price", "Model", "Make"
            };
            CboSortBy.ItemsSource = SortByCategories;
            CboSortBy.SelectedIndex = 0;
        }

        //Create a set of sample vehicles of each type for testing
        public void AddSampleVehicles()
        {
            //Cars
            VehiclesCollection.Add(new Car()
            {
                Make = "Ford",
                Model = "Focus",
                Price = 10000,
                Year = 2010,
                Mileage = 50000,
                Description = "Lovely car. 1 owner from new.",
                BodyType = "Hatchback",
                Engine = "2 Litre",
                Image = GetImage("/images/vehicles/ford-focus-2010-red.png"),
                Colour = "Red"
            });
            //Bikes
            VehiclesCollection.Add(new Bike()
            {
                Make = "BMW",
                Model = "F800 R",
                Price = 11000,
                Year = 2017,
                Mileage = 25000,
                Description = "It's got an engine and stuff",
                Engine = "798cc",
                Image = GetImage("./images/vehicles/bmw-f800r-2017-white.png"),
                Colour = "White",
                Type = "Sports"
            });
            //Vans
            VehiclesCollection.Add(new Van()
            {
                Make = "Nissan",
                Model = "NV300",
                Price = 26000,
                Year = 2018,
                Mileage = 5000,
                Description = "IT'S A VAN MAN",
                Engine = "1.6 Diesel",
                Image = GetImage("./images/vehicles/nissan-nv300-2018-blue.png"),
                Colour = "Blue",
                Type = "Panel Van",
                Wheelbase = "Medium"
            });
        }

        //Returns an image based on the inputted path
        public BitmapImage GetImage(string path)
        {
            return (new BitmapImage(new Uri(path, UriKind.Relative)));
        }

        //When the vehicle selection is changed, updates the information on the side
        private void ListboxVehicles_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Get selected object

            //Null check
            if (ListboxVehicles.SelectedItem is Vehicle selectedVehicle)
            {
                //Fill Details
                DisplayImage.Source = selectedVehicle.Image;
                DisplayMake.Text = selectedVehicle.Make;
                DisplayModel.Text = selectedVehicle.Model;
                DisplayPrice.Text = selectedVehicle.Price.ToString();
                DisplayYear.Text = selectedVehicle.Year.ToString();
                DisplayMileage.Text = selectedVehicle.Mileage.ToString();
                DisplayDescription.Text = selectedVehicle.Description;
                DisplayEngine.Text = selectedVehicle.Engine;
            }
            else
            {
                //Empty Details
                DisplayImage.Source = null;
                DisplayMake.Text = "";
                DisplayModel.Text = "";
                DisplayPrice.Text = "";
                DisplayYear.Text = "";
                DisplayMileage.Text = "";
                DisplayDescription.Text = "";
                DisplayEngine.Text = "";
            }
        }

        //When the All radio button is checked, show all types of vehicles in the listbox
        private void RadioAll_Checked(object sender, RoutedEventArgs e)
        {
            //If vehicles collection isn't empty
            if (VehiclesCollection.Count != 0)
            {
                //Updates the source of vehicles collection
                ListboxVehicles.ItemsSource = VehiclesCollection;
            }
        }

        //When the Cars radio button is checked, show only Cars in the listbox
        private void RadioCars_Checked(object sender, RoutedEventArgs e)
        {
            FilteredVehicles.Clear();

            foreach (Vehicle vehicle in VehiclesCollection)
            {
                if (vehicle.GetType() == typeof(Car))
                {
                    FilteredVehicles.Add(vehicle);
                }
            }
            ListboxVehicles.ItemsSource = FilteredVehicles;

            BtnAdd.Content = "Add Car";
        }

        //When the Bikes radio button is checked, show only Bikes in the listbox
        private void RadioBikes_Checked(object sender, RoutedEventArgs e)
        {
            FilteredVehicles.Clear();

            foreach (Vehicle vehicle in VehiclesCollection)
            {
                if (vehicle.GetType() == typeof(Bike))
                {
                    FilteredVehicles.Add(vehicle);
                }
            }
            ListboxVehicles.ItemsSource = FilteredVehicles;

            BtnAdd.Content = "Add Bike";
        }

        //When the Vans radio button is checked, show only Vans in the listbox
        private void RadioVans_Checked(object sender, RoutedEventArgs e)
        {
            FilteredVehicles.Clear();

            foreach (Vehicle vehicle in VehiclesCollection)
            {
                if (vehicle.GetType() == typeof(Van))
                {
                    FilteredVehicles.Add(vehicle);
                }
            }
            ListboxVehicles.ItemsSource = FilteredVehicles;

            BtnAdd.Content = "Add Van";
        }

        //When the load button is clicked, let the user load a list of vehicles
        private void BtnLoad_Click(object sender, RoutedEventArgs e)
        {
            //Open a windows dialogue to allow the user to select a file
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "txt files(*.txt) | *.txt | All files(*.*) | *.*";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                //Read through the file
                using (StreamReader sr = new StreamReader(dlg.FileName))
                {
                    string currentLine = sr.ReadLine();
                    string[] typeSplit = new string[2];
                    string[] propertiesSplit = new string[11];


                    //Step through the file line by line
                    while (currentLine != null)
                    {
                        //Read in a line and split it where there's a ':'
                        typeSplit = currentLine.Split(':');
                        propertiesSplit = typeSplit[1].Split(',');

                        //Determine vehicle type
                        if (typeSplit[0] == "car")
                        {
                            VehiclesCollection.Add(new Car()
                            {
                                Make = propertiesSplit[0],
                                Model = propertiesSplit[1],
                                Price = int.Parse(propertiesSplit[2]),
                                Year = int.Parse(propertiesSplit[3]),
                                Mileage = int.Parse(propertiesSplit[4]),
                                Description = propertiesSplit[5],
                                Engine = propertiesSplit[6],
                                Image = GetImage(propertiesSplit[7]),
                                Colour = propertiesSplit[8],
                                BodyType = propertiesSplit[9]
                            });
                        }
                        else if (typeSplit[0] == "bike")
                        {
                            VehiclesCollection.Add(new Bike()
                            {
                                Make = propertiesSplit[0],
                                Model = propertiesSplit[1],
                                Price = int.Parse(propertiesSplit[2]),
                                Year = int.Parse(propertiesSplit[3]),
                                Mileage = int.Parse(propertiesSplit[4]),
                                Description = propertiesSplit[5],
                                Engine = propertiesSplit[6],
                                Image = GetImage(propertiesSplit[7]),
                                Colour = propertiesSplit[8],
                                Type = propertiesSplit[9]
                            });
                        }
                        else if (typeSplit[0] == "van")
                        {
                            VehiclesCollection.Add(new Van()
                            {
                                Make = propertiesSplit[0],
                                Model = propertiesSplit[1],
                                Price = int.Parse(propertiesSplit[2]),
                                Year = int.Parse(propertiesSplit[3]),
                                Mileage = int.Parse(propertiesSplit[4]),
                                Description = propertiesSplit[5],
                                Engine = propertiesSplit[6],
                                Image = GetImage(propertiesSplit[7]),
                                Colour = propertiesSplit[8],
                                Type = propertiesSplit[9],
                                Wheelbase = propertiesSplit[10]
                            });
                        }

                        currentLine = sr.ReadLine();
                    }
                }
            }
        }

        //When the save button is clicked, let the user save the list of vehicles
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            using (StreamWriter sw = new StreamWriter("./SavedVehicles.txt"))
            {
                //Loop through each vehicle in the vehicle collection
                foreach (Vehicle currentVehicle in VehiclesCollection)
                {
                    string currentLine = (
                        currentVehicle.Make + ',' +
                        currentVehicle.Model + ',' +
                        currentVehicle.Price + ',' +
                        currentVehicle.Year + ',' +
                        currentVehicle.Mileage + "," +
                        currentVehicle.Description + ',' +
                        currentVehicle.Engine + "," +
                        currentVehicle.Image.UriSource + "," +
                        currentVehicle.Colour + ",");

                    //Determine its type
                    if (currentVehicle.GetType() == typeof(Car))
                    {
                        Car c = currentVehicle as Car;

                        //label its type and add any type specific information
                        currentLine = "car:" + currentLine + c.BodyType;
                    }
                    else if (currentVehicle.GetType() == typeof(Bike))
                    {
                        Bike b = currentVehicle as Bike;

                        //label its type and add any type specific information
                        currentLine = "bike:" + currentLine + b.Type;
                    }
                    else if (currentVehicle.GetType() == typeof(Van))
                    {
                        Van v = currentVehicle as Van;

                        //label its type and add any type specific information
                        currentLine = "van:" + currentLine + v.Type + ',' + v.Wheelbase;
                    }
                    //Write to file
                    sw.WriteLine(currentLine);
                }
            }
        }

        //When the add button is clicked, a new vehicle dialogue is opened.
        //The type of vehicle created is based on selectedadio buttons at the top
        //But defaults to Car
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            //Get the cotent of the add button and store it as a string
            string btnContent = BtnAdd.Content.ToString();

            //Check if the button says "Add Car", "Add Bike" or "Add Van"
            //And open the appropriate window
            if (btnContent.Contains("Car"))
            {
                EditCar AddCar = new EditCar();
                AddCar.Owner = this;
                AddCar.ShowDialog();
            }
            else if (btnContent.Contains("Bike"))
            {
                EditBike AddBike = new EditBike();
                AddBike.Owner = this;
                AddBike.ShowDialog();
            }
            else if (btnContent.Contains("Van"))
            {
                EditVan AddVan = new EditVan();
                AddVan.Owner = this;
                AddVan.ShowDialog();
            }
        }

        //When the edit button is clicked, open the edit vehicle dialogue
        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            //Null check
            if (ListboxVehicles.SelectedItem != null)
            {
                //Get selected vehicle
                Vehicle selectedVehicle = ListboxVehicles.SelectedItem as Vehicle;

                //If the selected vehicle is a Car
                if (selectedVehicle.GetType() == typeof(Car))
                {
                    Car selectedCar = selectedVehicle as Car;
                    //Make a new edit car window
                    EditCar editCar = new EditCar(selectedCar);
                    editCar.Owner = this;

                    editCar.ShowDialog();
                }
                //If the selected vehicle is a Bike
                else if (selectedVehicle.GetType() == typeof(Bike))
                {
                    Bike selectedBike = selectedVehicle as Bike;
                    //Make a new edit bike window
                    EditBike editBike = new EditBike(selectedBike);
                    editBike.Owner = this;

                    editBike.ShowDialog();
                }
                //If the selected vehicle is a Van
                else if (selectedVehicle.GetType() == typeof(Van))
                {
                    Van selectedVan = selectedVehicle as Van;
                    //Make a new edit van window
                    EditVan editVan = new EditVan(selectedVan);
                    editVan.Owner = this;

                    editVan.ShowDialog();
                }
            }
        }

        //When the delete button is clicked, delete the selected vehicle from the list
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            //Get the selected vehicle
            Vehicle selectedVehicle = ListboxVehicles.SelectedItem as Vehicle;

            //Null check
            if (selectedVehicle != null)
            {
                //Remove the vehicle from the list
                VehiclesCollection.Remove(selectedVehicle);
            }
        }

        //Adds the vehicle passed to it to the vehicles list and refreshes based on filtering
        public void AddToList(Vehicle newVehicle)
        {
            VehiclesCollection.Add(newVehicle);
            if (RadioAll.IsChecked == true)
            {
                ListboxVehicles.ItemsSource = VehiclesCollection;
            }
            else if (RadioCars.IsChecked == true)
            {
                FilteredVehicles.Clear();

                foreach (Vehicle vehicle in VehiclesCollection)
                {
                    if (vehicle.GetType() == typeof(Car))
                    {
                        FilteredVehicles.Add(vehicle);
                    }
                }
                ListboxVehicles.ItemsSource = FilteredVehicles;
            }
            else if (RadioBikes.IsChecked == true)
            {
                FilteredVehicles.Clear();

                foreach (Vehicle vehicle in VehiclesCollection)
                {
                    if (vehicle.GetType() == typeof(Bike))
                    {
                        FilteredVehicles.Add(vehicle);
                    }
                }
                ListboxVehicles.ItemsSource = FilteredVehicles;
            }
            else if (RadioVans.IsChecked == true)
            {
                FilteredVehicles.Clear();

                foreach (Vehicle vehicle in VehiclesCollection)
                {
                    if (vehicle.GetType() == typeof(Van))
                    {
                        FilteredVehicles.Add(vehicle);
                    }
                }
                ListboxVehicles.ItemsSource = FilteredVehicles;
            }
        }
        //When the sort by combo box is changed
        private void CboSortBy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Get selected value
            string selectedValue = CboSortBy.SelectedValue.ToString();
            //Store sort
            IEnumerable<Vehicle> sortedVehicles = VehiclesCollection;
            
            switch (selectedValue)
            {
                case "Price":
                    sortedVehicles = VehiclesCollection.OrderBy(a => a.Price);
                    ListboxVehicles.ItemsSource = sortedVehicles;
                    break;
                case "Model":
                    sortedVehicles = VehiclesCollection.OrderBy(a => a.Model);
                    ListboxVehicles.ItemsSource = sortedVehicles;
                    break;
                case "Make":
                    sortedVehicles = VehiclesCollection.OrderBy(a => a.Make);
                    ListboxVehicles.ItemsSource = sortedVehicles;
                    break;
                case "None":
                    ListboxVehicles.ItemsSource = VehiclesCollection;
                    break;
            }
        }
    }
}
