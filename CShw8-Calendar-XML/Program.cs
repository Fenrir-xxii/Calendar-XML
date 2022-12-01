// See https://aka.ms/new-console-template for more information
using CShw8_Calendar_XML;
using Task = CShw8_Calendar_XML.Task;
using Event = CShw8_Calendar_XML.Event;

//Calendar calendar = new Calendar();
//Task t = new Task
//{
//    Title = "Test task",
//    //EndTime= DateTime.Now.AddDays(1),
//    EndTime = DateTime.Parse("12.12.2022")
//};
//Event e = new Event
//{
//    Title = "Test event",
//    StartTime = DateTime.Now,
//    EndTime = DateTime.Now,
//    RepeatOption = RepeatTime.Weekly,
//    LifeTimeDays = 66
//};
//Event e2 = new Event
//{
//    Title = "Birthday",
//    StartTime = DateTime.Parse("01.12.2000"),
//    EndTime = DateTime.Parse($"01.12.{DateTime.Now.Year}"),
//    //EndTime = DateTime.Now.AddDays(30),
//    RepeatOption = RepeatTime.Annually,
//    LifeTimeDays = 365 * 50
//};

//Task tCopy = new Task
//{
//    Title = "Test task",
//    EndTime = DateTime.Parse("12.12.2022")
//};


//calendar.AddNewAsignment(t);
//calendar.AddNewAsignment(e);
//calendar.AddNewAsignment(e2);
//calendar.SortByEndTime();
////calendar.RemoveAssignment(tCopy);

CalendarManager manager = new CalendarManager(new Calendar());
manager.Run();


//List<ICalendarAssignment> list = new List<ICalendarAssignment> { t, e, e2 };

//XmlHelper<List<Task>>.Serialize(list.Where(x => x is Task).Select(x => x as Task).ToList(), "tasks.xml");
//XmlHelper<List<Event>>.Serialize(list.Where(x => x is Event).Select(x => x as Event).ToList(), "events.xml");

//var list2 = new List<ICalendarAssignment>();
//list2.AddRange(XmlHelper<List<Task>>.Deserialize("tasks.xml"));
//list2.AddRange(XmlHelper<List<Event>>.Deserialize("events.xml"));
//list2.ForEach(Console.WriteLine);