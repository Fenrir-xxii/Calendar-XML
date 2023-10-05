using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShw8_Calendar_XML;

public delegate void PrintAssignment(ICalendarAssignment item);
public class Calendar
{
    public List<ICalendarAssignment> Assignments;
    public PrintAssignment Printer;
    public Calendar()
    {
        Assignments = new List<ICalendarAssignment>();
    }
    public void AddNewAsignment(ICalendarAssignment assignment)
    {
        Assignments.Add(assignment);
        SortByEndTime();
    }
    public void RemoveAssignment(ICalendarAssignment assignment)
    {
        if(Assignments.Contains(assignment))
        {
            Assignments.Remove(assignment);
            //Console.WriteLine("Contains!");
        }
    }
    public void SortByEndTime()
    {
        Assignments.Sort((a1, a2) => DateTime.Compare(a1.EndTime, a2.EndTime));
    }
    public void PrintAssignment(ICalendarAssignment item)
    {
        if (item is Task)
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
        }
        else if (item is Event)
        {
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
        }
        Console.WriteLine(item);
        Console.ResetColor();
    }
    public void ShowAssignments()
    {
        Console.Clear();

        Printer = PrintAssignment;
        Assignments.ForEach(assignment => Printer.Invoke(assignment));
    }
    public void DrawCalendar(int month, int year)
    {
        List<string> monthName= new List<string>() { "J A N U A R Y", "F E B R U A R Y", "M A R C H", "A P R I L", "M A Y", "J U N E",
            "J U L Y", "A U G U S T", "S E P T E M B E R", "O C T O B E R", "N O V E M B E R", "D E C E M B E R" };

        var days = System.DateTime.DaysInMonth(year, month);
        var space = 3;
        DateTime firstDay = DateTime.Parse($"01.{month}.{year}");
        var weekDay = ((int)firstDay.DayOfWeek == 0) ? 7 : (int)firstDay.DayOfWeek;
        var col = (weekDay-1) * space;
        var row = 4;
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine($"{monthName[month - 1]} {year}");
        Console.ResetColor();
        for (int i = 0; i < 7; i++)
        {
            Console.SetCursorPosition(0, 2);
            Console.Write("Mn Tu Wd Th Fr Sa ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Sn");
            Console.ResetColor();
        }

        for (int i = 1; i <= days; i++)
        {
            if (col / space >= 6) // Sunday
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            DateTime temp = DateTime.Parse($"{i}.{month}.{year}");
            // check for tasks and events for that day
            foreach (ICalendarAssignment assignment in Assignments)
            {
                if (assignment.EndTime.Date == temp)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                }
                if(assignment is Event)  // check event repeat days
                {
                    foreach(DateTime date in (assignment as Event).RepeatDate)
                    {
                        if(date.Date == temp)
                        {
                            Console.ForegroundColor = ConsoleColor.Magenta;
                        }
                    }
                }
            }
            if (DateTime.Today == temp) // check current date
            {
                Console.BackgroundColor= ConsoleColor.Yellow;
                Console.ForegroundColor= ConsoleColor.Black;
            }
            Console.SetCursorPosition(col, row);
            Console.Write($"{i}");
            Console.ResetColor();
            if((col/3) > 5)
            {
                row++;
                col = 0;
            }
            else
            {
                col+= space;
            }
        }
        Console.WriteLine();
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.DarkGreen;
        Console.WriteLine("Use '<' / '>' to navigate,\nand ESC to return");
        Console.ResetColor();
    }
    public void Info(ICalendarAssignment item)
    {
        Console.WriteLine($"#{Assignments.IndexOf(item)+1} {item}");
    }
}
