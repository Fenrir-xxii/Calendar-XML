using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShw8_Calendar_XML;

public delegate void Message(string name);
public delegate void Print(ICalendarAssignment item);
public  class MyDelegate
{
    public Message message { get; set; }
    public Print print { get; set; }
    public void Use(string name)
    {
        message.Invoke(name);
    }
    public void Printer(ICalendarAssignment item)
    {
        if(item is Task)
        {
            Console.ForegroundColor = ConsoleColor.Red;
        }
        else if(item is Event)
        {
            Console.ForegroundColor = ConsoleColor.Green;
        }
        print.Invoke(item);
        //Console.WriteLine(item);
        Console.ResetColor();
    }
}

