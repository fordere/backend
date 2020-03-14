using System;
using System.Collections.Generic;
using Fordere.RestService.Entities;

namespace Fordere.RestService
{
    public static class TimeSlotFactory
    {
        public static List<DateTime> GetPossibleTimeSlots(DateTime date, TableAvailability availability)
        {
            var possibleTimeSlots = new List<DateTime>();

            int firstTimeSlotDayOfWeek = availability.FirstTimeSlotDayOfWeek;
            int lastTimeSlotDayOfWeek = availability.LastTimeSlotDayOfWeek;
            int dayDifference = 0;

            while (firstTimeSlotDayOfWeek != lastTimeSlotDayOfWeek)
            {
                dayDifference++;
                firstTimeSlotDayOfWeek++;
                if (firstTimeSlotDayOfWeek == 7)
                {
                    firstTimeSlotDayOfWeek = 0;
                }
            }

            var nextTimeSlot = new DateTime(date.Year, date.Month, date.Day, availability.FirstTimeSlot.Hour, availability.FirstTimeSlot.Minute, 0);
            var lastTimeSlot = new DateTime(date.Year, date.Month, date.Day, availability.LastTimeSlot.Hour, availability.LastTimeSlot.Minute, 0).AddDays(dayDifference);

            while (nextTimeSlot <= lastTimeSlot)
            {
                possibleTimeSlots.Add(nextTimeSlot);
                nextTimeSlot = nextTimeSlot.AddMinutes(30);
            }

            return possibleTimeSlots;
        }
    }
}