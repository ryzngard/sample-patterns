﻿using System;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CommercialRegistration;
using Common;
using ConsumerVehicleRegistration;
using LiveryRegistration;
using TollCollectorLib;
using TollCollectorLib.BillingSystem;

namespace TollCollectorApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, ILogger
    {
        private readonly Random _random;

        public MainWindow()
        {
            InitializeComponent();

            _random = new Random();
            TollSystem.Initialize(this);
        }

        private DateTime GenerateTimeStamp()
        {
            var result = DateTime.Now;

            result = result.AddDays(_random.Next(0, 7));
            result = result.AddHours(_random.Next(0, 24));
            result = result.AddMinutes(_random.Next(0, 60));

            return result;
        }

        private void btnCar_Click(object sender, RoutedEventArgs e)
        {
            var vehicle = new Car
            {
                Passengers = _random.Next(1, 8)
            };

            var inbound = _random.NextBool();

            TollSystem.AddEntry(vehicle, GenerateTimeStamp(), inbound, AccountList.GenerateTestLicense());
        }

        private void btnDeliveryTruck_Click(object sender, RoutedEventArgs e)
        {
            var vehicle = new DeliveryTruck
            {
                GrossWeightClass = _random.Next(1000, 10000)
            };

            var inbound = _random.NextBool();

            TollSystem.AddEntry(vehicle, GenerateTimeStamp(), inbound, AccountList.GenerateTestLicense());
        }

        private void btnTaxi_Click(object sender, RoutedEventArgs e)
        {
            var vehicle = new Taxi
            {
                Fares = _random.Next(1, 8)
            };

            var inbound = _random.NextBool();

            TollSystem.AddEntry(vehicle, GenerateTimeStamp(), inbound, AccountList.GenerateTestLicense());
        }

        private void btnBus_Click(object sender, RoutedEventArgs e)
        {
            var capacity = _random.Next(1, 6);
            var riders = _random.Next(1, capacity);

            var vehicle = new Bus
            {
                Capacity = capacity,
                Riders = riders
            };

            var inbound = _random.NextBool();

            TollSystem.AddEntry(vehicle, GenerateTimeStamp(), inbound, AccountList.GenerateTestLicense());
        }

        private void btnNull_Click(object sender, RoutedEventArgs e) 
            => TollSystem.AddEntry(null, DateTime.Now, false, null);

        void ILogger.SendMessage(string message, LogLevel level)
        {
            var item = new LogItem(level, message);
            lstMessages.Items.Add(item);
            lstMessages.ScrollIntoView(item);
        }
    }
}
