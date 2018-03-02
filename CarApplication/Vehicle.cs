using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Collections.ObjectModel;
using System.Collections;

namespace CarApplication
{
    abstract public class Vehicle
    {
        #region props
        public string Make { get; set; }
        public string Model { get; set; }
        public decimal Price { get; set; }
        public int Year { get; set; }
        public int Mileage { get; set; }
        public string Description { get; set; }
        public string Engine { get; set; }
        public BitmapImage Image { get; set; }
        public BitmapImage TypeImage { get; set; }
        public string Colour { get; set; }
        #endregion
    }
    public class Car : Vehicle
    {
        //Props
        static public Dictionary<string, int> BodyTypes = new Dictionary<string, int>()
        {
            {"Convertible",1 },
            {"Hatchback",2},
            {"Coupe",3},
            {"Estate",4},
            {"MPV",5},
            {"SUV",6},
            {"Saloon",7},
            {"Unlisted",8}
        };
        public string BodyType { get; set; }
        //Ctors
        public Car()
        {
            TypeImage = new BitmapImage(new Uri("/images/categories/car.png", UriKind.Relative));
        }
    }
    public class Bike : Vehicle
    {
        static public Dictionary<string, int> Types = new Dictionary<string, int>()
        {
            {"Scooter",1 },
            {"TrailBike",2},
            {"Sports",3},
            {"Commuter",4},
            {"Tourer",5}
        };
        public string Type { get; set; }
        public Bike()
        {
            TypeImage = new BitmapImage(new Uri("/images/categories/bike.png", UriKind.Relative));
        }
    }
    public class Van : Vehicle
    {
        static public Dictionary<string, int> Wheelbases = new Dictionary<string, int>()
        {
            {"Short",1},
            {"Medium",2},
            {"Long",3},
            {"Unlisted",4}
        };
        static public Dictionary<string, int> Types = new Dictionary<string, int>()
        {
            {"Combi Van",1},
            {"Dropside",2},
            {"Panel Van",3},
            {"Pickup",4},
            {"Tipper",5},
            {"Unlisted",6}
        };
        public string Wheelbase { get; set; }
        public string Type { get; set; }
        public Van()
        {
            TypeImage = new BitmapImage(new Uri("/images/categories/van.png", UriKind.Relative));
        }
    }
}
