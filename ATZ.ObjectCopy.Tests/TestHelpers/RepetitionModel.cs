using ATZ.ObservableObjects;
using System;
using System.Collections.Generic;

namespace CalendarBlocks.Model.Repetitions
{
    public class RepetitionModel : ObservableObject
    {
        // TODO: Why is this ObservableObject if properties are not firing notifications?
        public Guid Id { get; set; }
        public Guid OriginalInstanceId { get; set; }

        public DateTime LastRepeatGenerated { get; set; }

        public int RepeatEvery { get; set; }
        public DateInterval RepeatInterval { get; set; }

        public List<DayOfWeek> RepeatOnDays { get; set; } = new List<DayOfWeek>();
        public int? RepeatOnDayOfMonth { get; set; }
        public int? RepeatInMonthOfYear { get; set; }

        public void CopyTo(RepetitionModel target)
        {
            target.RepeatOnDays.Clear();
            target.RepeatOnDayOfMonth = null;
            target.RepeatInMonthOfYear = null;

            target.RepeatEvery = RepeatEvery;
            target.RepeatInterval = RepeatInterval;

            switch (target.RepeatInterval)
            {
                case DateInterval.Day:
                    break;

                case DateInterval.Week:
                    target.RepeatOnDays.AddRange(RepeatOnDays);
                    break;

                case DateInterval.Month:
                    target.RepeatOnDayOfMonth = RepeatOnDayOfMonth;
                    break;

                case DateInterval.Year:
                    target.RepeatOnDayOfMonth = RepeatOnDayOfMonth;
                    target.RepeatInMonthOfYear = RepeatInMonthOfYear;
                    break;

                default:
                    throw new InvalidOperationException($"{nameof(RepeatInterval)} is outside of the interpreted value set!");
            }
        }
    }
}