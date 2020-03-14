using System;
using System.Collections.Generic;
using forderebackend.ServiceInterface.Entities;

namespace forderebackend.ServiceInterface
{
    public static class TimeSlotFactory
    {
        public static List<DateTime> GetPossibleTimeSlots(DateTime date, TableAvailability availability)
        {
            var possibleTimeSlots = new List<DateTime>();

            var firstTimeSlotDayOfWeek = availability.FirstTimeSlotDayOfWeek;
            var lastTimeSlotDayOfWeek = availability.LastTimeSlotDayOfWeek;
            var dayDifference = 0;

            while (firstTimeSlotDayOfWeek != lastTimeSlotDayOfWeek)
            {
                dayDifference++;
                firstTimeSlotDayOfWeek++;
                if (firstTimeSlotDayOfWeek == 7) firstTimeSlotDayOfWeek = 0;
            }

            var nextTimeSlot = new DateTime(date.Year, date.Month, date.Day, availability.FirstTimeSlot.Hour,
                availability.FirstTimeSlot.Minute, 0);
            var lastTimeSlot = new DateTime(date.Year, date.Month, date.Day, availability.LastTimeSlot.Hour,
                availability.LastTimeSlot.Minute, 0).AddDays(dayDifference);

            while (nextTimeSlot <= lastTimeSlot)
            {
                possibleTimeSlots.Add(nextTimeSlot);
                nextTimeSlot = nextTimeSlot.AddMinutes(30);
            }

            return possibleTimeSlots;
        }
    }
}