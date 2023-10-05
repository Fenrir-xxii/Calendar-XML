// See https://aka.ms/new-console-template for more information
using CShw8_Calendar_XML;
using Task = CShw8_Calendar_XML.Task;
using Event = CShw8_Calendar_XML.Event;

CalendarManager manager = new CalendarManager(new Calendar());
manager.Run();
