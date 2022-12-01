using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShw8_Calendar_XML;

public enum RepeatTime
{
    None = 0, Daily, Weekly, Monthly, Annually
}

public class Event : ICalendarAssignment
{
    public Event() 
    {
        _repeatDays = new List<DateTime> { };
        LifeTimeDays = 365;
    }
    private List<DateTime> _repeatDays;
    public string Title { get; set; }
    public DateTime EndTime { get; set; }
    public DateTime StartTime { get; set; }
    public RepeatTime RepeatOption { get; set; }
    public int LifeTimeDays { get; set; } // how long Events will repeat
    public List<DateTime> RepeatDate
    {
        get 
        {
            FillRepeatDate();
            return _repeatDays; 
        }
    }
    public void FillRepeatDate()
    {
        switch (RepeatOption)
        {
            case RepeatTime.Daily:
                for (int i = 0; i <= LifeTimeDays; i++)
                {
                    DateTime temp = StartTime.AddDays(i);
                    _repeatDays.Add(temp);
                }
                break;
            case RepeatTime.Weekly:
                for (int i = 0; i <= LifeTimeDays; i+=7)
                {
                    DateTime temp = StartTime.AddDays(i);
                    _repeatDays.Add(temp);
                }
                break;
            case RepeatTime.Monthly:
                // REDO
                var qtyOfMonths = LifeTimeDays / 30;
                for (int i = 0; i <= qtyOfMonths; i++)
                {
                    DateTime temp = StartTime.AddMonths(i);
                    _repeatDays.Add(temp);
                }
                break;
            case RepeatTime.Annually:
                var qtyOfYears = LifeTimeDays / 365;
                for (int i = 0; i <= qtyOfYears; i++)
                {
                    DateTime temp = StartTime.AddYears(i);
                    _repeatDays.Add(temp);
                }
                break;
            default:
                break;
        }
    }
    public override bool Equals(object obj)
    {
        if (obj == null)
        {
            return false;
        }
        if (!(obj is Event))
        {
            return false;
        }
        return Title == (obj as Event).Title && EndTime == (obj as Event).EndTime && StartTime == (obj as Event).StartTime;
    }
    public override string ToString()
    {
        // REDO
        if(Title.Contains("birthday") || Title.Contains("Birthday") || Title.Contains("Birth day") || Title.Contains("birth day"))
        {
            DateTime CurrentYearBirthday = DateTime.Parse($"{StartTime.Day}.{StartTime.Month}.{DateTime.Now.Year}");
            return $"{CurrentYearBirthday:dd.MM.yyyy} - Event (Anniversary): \"{Title}\" {CurrentYearBirthday.Year - StartTime.Year} year(s): starts at {StartTime:dd.MM.yyyy}";
        }
        return $"{EndTime:dd.MM.yyyy} - Event: \"{Title}\": starts at {StartTime:dd.MM.yyyy HH:mm}, ends at {EndTime:dd.MM.yyyy HH:mm}. Repeat: {RepeatOption.ToString()}";
    }
}
