using System;
using System.Collections.Generic;

using Fordere.RestService;
using Fordere.RestService.Entities;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fordere.UnitTest
{
    [TestClass]
    public class TimeSlotFactoryTests
    {
        [TestMethod]
        public void SaturdayToSundayTimeSlots()
        {
            // Arrange
            var availability = new TableAvailability() { FirstTimeSlotDayOfWeek = 6, LastTimeSlotDayOfWeek = 0, FirstTimeSlot = new DateTime(2014, 12, 12, 19, 00, 00), LastTimeSlot = new DateTime(2014, 12, 12, 02, 00, 00) };
            var sourceDate = new DateTime(2014, 2, 20);

            // Act
            List<DateTime> possibleTimeSlots = TimeSlotFactory.GetPossibleTimeSlots(sourceDate, availability);

            // Assert
            // 19:00 / 19:30 / 20:00 / 20:30 / 21:00 / 21:30 / 22:00 / 22:30 / 23:00 / 23:30 / 00:00 / 00:30 / 01:00 / 01:30 / 02:00
            Assert.AreEqual(15, possibleTimeSlots.Count);
        }

        [TestMethod]
        public void GetPossibleTimeSlotsEvening()
        {
            // Arrange
            var availability = new TableAvailability() { FirstTimeSlotDayOfWeek = 1, LastTimeSlotDayOfWeek = 1, FirstTimeSlot = new DateTime(2014, 12, 12, 19, 00, 00), LastTimeSlot = new DateTime(2014, 12, 12, 23, 00, 00) };
            var sourceDate = new DateTime(2014, 2, 20);

            // Act
            List<DateTime> possibleTimeSlots = TimeSlotFactory.GetPossibleTimeSlots(sourceDate, availability);

            // Assert
            // 19:00 / 19:30 / 20:00 / 20:30 / 21:00 / 21:30 / 22:00 / 22:30 / 23:00
            Assert.AreEqual(9, possibleTimeSlots.Count);
        }

        [TestMethod]
        public void GetPossibleTimeSlotsOverMidnight()
        {
            // Arrange
            var availability = new TableAvailability() { FirstTimeSlotDayOfWeek = 1, LastTimeSlotDayOfWeek = 2, FirstTimeSlot = new DateTime(2014, 12, 12, 19, 00, 00), LastTimeSlot = new DateTime(2014, 12, 12, 1, 00, 00) };
            var sourceDate = new DateTime(2014, 2, 20);

            // Act
            List<DateTime> possibleTimeSlots = TimeSlotFactory.GetPossibleTimeSlots(sourceDate, availability);

            // Assert
            // 19:00 / 19:30 / 20:00 / 20:30 / 21:00 / 21:30 / 22:00 / 22:30 / 23:00 / 23:30 / 00:00 / 00:30 / 01:00
            Assert.AreEqual(13, possibleTimeSlots.Count);
            for (int i = 0; i < possibleTimeSlots.Count; i++)
            {
                if (i < 10)
                {
                    Assert.AreEqual(20, possibleTimeSlots[i].Day);
                }
                else
                {
                    Assert.AreEqual(21, possibleTimeSlots[i].Day);
                }
            }
        }

        [TestMethod]
        public void GetPossibleTimeSlotsOverMonthChange()
        {
            // Arrange
            var availability = new TableAvailability() { FirstTimeSlotDayOfWeek = 1, LastTimeSlotDayOfWeek = 2, FirstTimeSlot = new DateTime(2014, 1, 31, 19, 00, 00), LastTimeSlot = new DateTime(2014, 12, 12, 1, 00, 00) };
            var sourceDate = new DateTime(2014, 1, 31);

            // Act
            List<DateTime> possibleTimeSlots = TimeSlotFactory.GetPossibleTimeSlots(sourceDate, availability);

            // Assert
            // 19:00 / 19:30 / 20:00 / 20:30 / 21:00 / 21:30 / 22:00 / 22:30 / 23:00 / 23:30 / 00:00 / 00:30 / 01:00
            Assert.AreEqual(13, possibleTimeSlots.Count);
            for (int i = 0; i < possibleTimeSlots.Count; i++)
            {
                if (i < 10)
                {
                    Assert.AreEqual(31, possibleTimeSlots[i].Day);
                    Assert.AreEqual(1, possibleTimeSlots[i].Month);
                }
                else
                {
                    Assert.AreEqual(1, possibleTimeSlots[i].Day);
                    Assert.AreEqual(2, possibleTimeSlots[i].Month);
                }
            }
        }

    }
}
