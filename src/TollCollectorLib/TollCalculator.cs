using System;
using CommercialRegistration;
using ConsumerVehicleRegistration;
using LiveryRegistration;

namespace TollCollectorLib

{
    public static class TollCalculator
    {
        private const decimal carBase = 2.00m;
        private const decimal taxiBase = 3.50m;
        private const decimal busBase = 5.00m;
        private const decimal deliveryTruckBase = 10.00m;

        // TODO: Conversion of this
        public static decimal CalculateToll(object vehicle)
            => vehicle switch
            {
                Car { Passengers: 0 } => carBase + 0.5m,
                Car { Passengers: 1 } => carBase,
                Car { Passengers: 2 } => carBase - 0.5m,
                Car => carBase - 1.00m,
                Taxi { Fares: 0 } => taxiBase + 1.0m,
                Taxi { Fares: 1 } => taxiBase,
                Taxi { Fares: 2 } => taxiBase - 0.5m,
                Taxi => taxiBase - 1.0m,
                Bus b => ((double)b.Riders / b.Capacity) switch
                {
                    < 0.50 => busBase + 2.00m,
                    > 0.90 => busBase - 1.00m,
                    _ => busBase
                },
                DeliveryTruck { GrossWeightClass: > 5000 } => deliveryTruckBase + 5.00m,
                DeliveryTruck { GrossWeightClass: < 3000 } => deliveryTruckBase - 2.00m,
                DeliveryTruck => deliveryTruckBase,
                _ => int.MinValue
            };


        public static decimal PeakTimePremium(DateTime timeOfToll, bool inbound)
            => (timeband: GetTimeBand(timeOfToll), weekDay: IsWeekDay(timeOfToll), inbound: inbound) switch
            {
                (_, false, _) => 1.00m,
                (TimeBand.MorningRush, weekDay: true, inbound: true) => 2.00m,
                (TimeBand.MorningRush, true, false) => 2.00m,
                (TimeBand.Daytime , _, _) => 1.5m,
                (TimeBand.EveningRush, _, inbound: true) => 1.00m,
                (TimeBand.EveningRush, _, inbound: false) => 2.00m,
                (TimeBand.Overnight, _, _) => 0.75m,
                _ => throw new NotImplementedException(),
            };

        private static bool IsWeekDay(DateTime timeOfToll) => timeOfToll.DayOfWeek switch
        {
            DayOfWeek.Saturday or 
                DayOfWeek.Sunday => false,
            _ => true,
        };

        private enum TimeBand
        {
            MorningRush,
            Daytime,
            EveningRush,
            Overnight
        }

        private static TimeBand GetTimeBand(DateTime timeOfToll) => timeOfToll.Hour switch
        {
            < 6 => TimeBand.Overnight,
            < 10 => TimeBand.MorningRush,
            < 16 => TimeBand.Daytime,
            < 20 => TimeBand.EveningRush,
            _ => TimeBand.Overnight
        };

    }
}
