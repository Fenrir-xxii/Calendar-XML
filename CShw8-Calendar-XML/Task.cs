using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShw8_Calendar_XML;

public class Task : ICalendarAssignment
{
    public Task() { IsCompleted = false; }
    public string Title { get; set; }
    public DateTime EndTime { get; set; }
    public bool IsCompleted { get; set; }

    public void Complete()
    {
        IsCompleted = true;
    }
    public override bool Equals(object obj)
    {
        if (obj == null)
        {
            return false;
        }
        if (!(obj is Task))
        {
            return false;
        }
        return Title == (obj as Task).Title && EndTime == (obj as Task).EndTime;
    }
    public override string ToString()
    {
        return $"{EndTime:dd.MM.yyyy} - Task: \"{Title}\": do until {EndTime:dd.MM.yyyy HH:mm}";
    }
}
