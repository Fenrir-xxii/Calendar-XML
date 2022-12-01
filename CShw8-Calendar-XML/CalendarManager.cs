using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShw8_Calendar_XML;

public enum MainMenu
{
    ShowAssignment = 0, AddNewAssignment, EditAssignment, DeleteAssignment, ShowCalendar
}
public enum AssignmentType
{
    Task = 0, Event
}
public enum TaskProperties
{
    TaskTitle = 0, TaskEndTime
}
public enum EventProperties
{
    EventTitle = 0, EventStartTime, EventEndTime, EventRepeatOption, EventLifeTimeDays
}

public class CalendarManager
{
    public Calendar Calendar { get; set; }
    public CalendarManager(Calendar calendar)
    {
        this.Calendar = calendar;
        Load();
    }
    public void Load()
    {
        if(File.Exists("tasks.xml"))
        {
            Calendar.Assignments.AddRange(XmlHelper<List<Task>>.Deserialize("tasks.xml"));
        }
        if(File.Exists("events.xml"))
        {
            Calendar.Assignments.AddRange(XmlHelper<List<Event>>.Deserialize("events.xml"));
        }
    }
    public void Save()
    {
        XmlHelper<List<Task>>.Serialize(Calendar.Assignments.Where(x => x is Task).Select(x => x as Task).ToList(), "tasks.xml");
        XmlHelper<List<Event>>.Serialize(Calendar.Assignments.Where(x => x is Event).Select(x => x as Event).ToList(), "events.xml");
    }
    public void Run()
    {
        List<string> menuOptions = new List<string>() { "Show assignments", "Add new assignment", "Edit assignment", "Delete assignment", "Show calendar" };
        Menu menu = new Menu(menuOptions);

        ConsoleKeyInfo input;
        int selection = -1;
        bool run = true;

        while (run)
        {
            Console.Clear();
            menu.DrawOptions();
            menu.DrawFrame();
            input = Console.ReadKey();

            switch (input.Key)
            {
                case ConsoleKey.UpArrow:
                    menu.Up();
                    break;
                case ConsoleKey.DownArrow:
                    menu.Down();
                    break;
                case ConsoleKey.Enter:
                    selection = menu.GetSelectedOption();
                    MainMenuWork(selection);
                    break;
                case ConsoleKey.Escape:
                    Console.Clear();
                    run = false;
                    break;
                default:
                    break;
            }
        }
    }
    public void MainMenuWork(int option)
    {
        int AssignmentIdx = -1;
        switch (option)
        {
            case (int)MainMenu.ShowAssignment:
                if(Calendar.Assignments.Count ==0)
                {
                    Console.Clear();
                    Console.WriteLine("No assignments yet\n");
                }
                else
                {
                    Calendar.ShowAssignments();
                }
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey(true);
                break;
            case (int)MainMenu.AddNewAssignment:
                var assignmentOption = PickAssignment();
                if(assignmentOption !=-1)
                {
                    Calendar.AddNewAsignment(CreateNewAssignment(assignmentOption));
                    Save();
                }
                break;
            case (int)MainMenu.EditAssignment:
                AssignmentIdx = MenuAssignments();
                if(AssignmentIdx!=-1)  // -1 didn't pick an assignment to edit 
                {
                    EditAssignment(AssignmentIdx);
                    Save();
                }
                break;
            case (int)MainMenu.DeleteAssignment:
                AssignmentIdx = MenuAssignments();
                if (AssignmentIdx != -1)
                {
                    Calendar.RemoveAssignment(Calendar.Assignments[AssignmentIdx]);
                    Save();
                }
                break;
            case (int)MainMenu.ShowCalendar:
                bool run = true;
                var month = DateTime.Now.Month;
                var year = DateTime.Now.Year;
                Console.Clear();
                Calendar.DrawCalendar(month, year);
                while (run)
                {
                    var input = Console.ReadKey();

                    switch (input.Key)
                    {
                        case ConsoleKey.RightArrow:
                            month++;
                            if (month == 13)
                            {
                                month = 1;
                                year++;
                            }
                            break;
                        case ConsoleKey.LeftArrow:
                            month--;
                            if (month == 0)
                            {
                                month = 12;
                                year--;
                            }
                            break;
                        case ConsoleKey.Escape:
                            run = false;
                            return;
                    }
                    Console.Clear();
                    Calendar.DrawCalendar(month, year);
                }
                break;
            default:
                break;
        }
    }
    public int PickAssignment() // 0 - Task, 1 - Event
    {
        List<string> menuOptions = new List<string>() { "Add new task", "Add new event" };
        Menu menu = new Menu(menuOptions);

        ConsoleKeyInfo input;
        int selection = -1;
        bool run = true;

        while (run)
        {
            Console.Clear();
            menu.DrawOptions();
            menu.DrawFrame();
            input = Console.ReadKey();

            switch (input.Key)
            {
                case ConsoleKey.UpArrow:
                    menu.Up();
                    break;
                case ConsoleKey.DownArrow:
                    menu.Down();
                    break;
                case ConsoleKey.Enter:
                    return menu.GetSelectedOption();
                case ConsoleKey.Escape:
                    return selection;
                default:
                    break;
            }
        }
        return selection;
    }
    public ICalendarAssignment CreateNewAssignment(int option) //0 - Task, 1 - Event 
    {
        if (option == (int)AssignmentType.Task)
        {
            Task newTask = new Task();
            Console.BackgroundColor = ConsoleColor.DarkYellow;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("Enter task title");
            Console.ResetColor();
            newTask.Title = Console.ReadLine();
            newTask.EndTime = DataInput("Enter end date");
            return newTask;
        }
        else
        {
            Event newEvent = new Event();
            Console.BackgroundColor = ConsoleColor.DarkYellow;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("Enter event title");
            Console.ResetColor();
            newEvent.Title = Console.ReadLine();
            newEvent.StartTime = DataInput("Enter start date");
            newEvent.EndTime = DataInput("Enter end date");
            Console.WriteLine("Pick repeat option");
            Console.ResetColor();
            System.Threading.Thread.Sleep(1500);
            newEvent.RepeatOption = (RepeatTime)PickRepeatOption();
            if ((int)newEvent.RepeatOption > 0)
            {
                Console.WriteLine();
                Console.BackgroundColor = ConsoleColor.DarkYellow;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine("Enter perid of time (in days) for term of repeating");
                Console.ResetColor();
                newEvent.LifeTimeDays = int.Parse(Console.ReadLine());
            }
            return newEvent;
        }
    }
    public void EditAssignment(int idxOfAssignment)
    {
        if (Calendar.Assignments[idxOfAssignment] is Task)
        {
            List<string> menuOptions = new List<string>() { "Title", "End Time"};
            Menu menu = new Menu(menuOptions);
            ConsoleKeyInfo input;
            int selection = -1;
            bool run = true;

            while (run)
            {
                Console.Clear();
                menu.DrawOptions();
                menu.DrawFrame(ConsoleColor.DarkYellow);
                input = Console.ReadKey();

                switch (input.Key)
                {
                    case ConsoleKey.UpArrow:
                        menu.Up();
                        break;
                    case ConsoleKey.DownArrow:
                        menu.Down();
                        break;
                    case ConsoleKey.Enter:
                        selection =  menu.GetSelectedOption();
                        switch(selection)
                        {
                            case (int)TaskProperties.TaskTitle:
                                Console.BackgroundColor = ConsoleColor.DarkYellow;
                                Console.ForegroundColor = ConsoleColor.Black;
                                Console.WriteLine("\nEnter new title for this task");
                                Console.ResetColor();
                                Calendar.Assignments[idxOfAssignment].Title = Console.ReadLine();
                                return;
                            case (int)TaskProperties.TaskEndTime:
                                Calendar.Assignments[idxOfAssignment].EndTime = DataInput("\nEnter new end date for this task");
                                Calendar.SortByEndTime();
                                return;
                        }
                        break;
                    default:
                        break;
                }
            }
        }
        else
        {
            List<string> menuOptions = new List<string>() { "Title", "Start Time", "End Time", "Repeat Option", "Period of time for repeating" };
            Menu menu = new Menu(menuOptions);
            ConsoleKeyInfo input;
            int selection = -1;
            bool run = true;

            while (run)
            {
                Console.Clear();
                menu.DrawOptions();
                menu.DrawFrame(ConsoleColor.DarkYellow);
                input = Console.ReadKey();

                switch (input.Key)
                {
                    case ConsoleKey.UpArrow:
                        menu.Up();
                        break;
                    case ConsoleKey.DownArrow:
                        menu.Down();
                        break;
                    case ConsoleKey.Enter:
                        selection = menu.GetSelectedOption();
                        switch (selection)
                        {
                            case (int)EventProperties.EventTitle:
                                Console.BackgroundColor = ConsoleColor.DarkYellow;
                                Console.ForegroundColor = ConsoleColor.Black;
                                Console.WriteLine("\nEnter new title for this event");
                                Console.ResetColor();
                                Calendar.Assignments[idxOfAssignment].Title = Console.ReadLine();
                                return;
                            case (int)EventProperties.EventStartTime:
                                (Calendar.Assignments[idxOfAssignment] as Event).StartTime = DataInput("\nEnter new start date for this event");
                                return;
                            case (int)EventProperties.EventEndTime:
                                Calendar.Assignments[idxOfAssignment].EndTime = DataInput("\nEnter new end date for this event");
                                Calendar.SortByEndTime();
                                return;
                            case (int)EventProperties.EventRepeatOption:
                                (Calendar.Assignments[idxOfAssignment]as Event).RepeatOption = (RepeatTime)PickRepeatOption();
                                return;
                            case (int)EventProperties.EventLifeTimeDays:
                                if((Calendar.Assignments[idxOfAssignment] as Event).RepeatOption ==0)
                                {
                                    Console.Clear();
                                    Console.WriteLine("You haven't pick repeat option. Pick repeat option first");
                                    System.Threading.Thread.Sleep(2000);
                                    return;
                                }
                                Console.WriteLine();
                                Console.BackgroundColor = ConsoleColor.DarkYellow;
                                Console.ForegroundColor = ConsoleColor.Black;
                                Console.WriteLine("\nEnter perid of time (in days) for term of repeating");
                                Console.ResetColor();
                                (Calendar.Assignments[idxOfAssignment] as Event).LifeTimeDays = int.Parse(Console.ReadLine());
                                return;
                        }
                        break;
                    default:
                        break;
                }
            }
        }
    }
    public int PickRepeatOption()
    {
        List<string> menuOptions = new List<string>() { "Do not repeat", "Daily", "Weekly", "Monthly", "Annualy"};
        Menu menu = new Menu(menuOptions);

        ConsoleKeyInfo input;
        bool run = true;

        while (run)
        {
            Console.Clear();
            menu.DrawOptions();
            menu.DrawFrame(ConsoleColor.DarkYellow);
            input = Console.ReadKey();

            switch (input.Key)
            {
                case ConsoleKey.UpArrow:
                    menu.Up();
                    break;
                case ConsoleKey.DownArrow:
                    menu.Down();
                    break;
                case ConsoleKey.Enter:
                    return  menu.GetSelectedOption();
                default:
                    break;
            }
        }
        return -1;
    }
    public int MenuAssignments()
    {
        List<string> assignmentOptions = new List<string>();
        foreach (ICalendarAssignment assignment in Calendar.Assignments)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(assignment is Task? "Task: ": "Event: ");
            sb.Append(assignment.Title);
            sb.Append(" (");
            sb.Append(assignment.EndTime);
            sb.Append(")");
            var fullTitle = sb.ToString();
            assignmentOptions.Add(fullTitle);
        }
        Menu menu = new Menu(assignmentOptions);

        ConsoleKeyInfo input;
        int selection = -1;
        bool run = true;
        while (run)
        {
            Console.Clear();
            menu.DrawOptions();
            menu.DrawFrame();
            input = Console.ReadKey();

            switch (input.Key)
            {
                case ConsoleKey.UpArrow:
                    menu.Up();
                    break;
                case ConsoleKey.DownArrow:
                    menu.Down();
                    break;
                case ConsoleKey.Enter:
                    return menu.GetSelectedOption();
                case ConsoleKey.Escape:
                    return selection;
                default:
                    break;
            }
        }
        return selection;
    }
    public DateTime DataInput(string message) //User input date checker
    {
        string input;
        DateTime result;
        do
        {
            Console.BackgroundColor = ConsoleColor.DarkYellow;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine(message);
            Console.ResetColor();
            input = Console.ReadLine();
        } while (!(DateTime.TryParse(input, out result)));
        return result;
    }
}
