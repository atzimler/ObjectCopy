using System;
using System.Collections.Generic;
using ATZ.TimeCalculations;
using CalendarBlocks.Model.TimeBlocks;
using Colors;

namespace CalendarBlocks.Model
{
    public interface ILifeArea
    {
        Guid Id { get; set; }
        string CalendarIdentifier { get; }
        Color Color { get; set; }
        string Name { get; }
        
        void AddTimeBlock(TimeBlockModel timeBlock);
        void SaveTimeBlock(TimeBlockModel timeBlock);
        IEnumerable<TimeBlockModel> TimeBlocks(ClosedTimeInterval interval);
    }
}