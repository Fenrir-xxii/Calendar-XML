using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShw8_Calendar_XML;
public class Menu
{
    public int ActiveOption { get; set; }
    public List<string> Options;

    public Menu(List<string> options)
    {
        this.Options = options;
    }
    private int GetMaxItemSize()
    {

        var max = 0;
        foreach (string item in Options)
        {
            if (item.Length > max)
            {
                max = item.Length;
            }
        }
        return max;
    }
    public int GetFrameWidth()
    {
        return GetMaxItemSize() + 6;
    }
    public int GetFrameHeight()
    {
        return Options.Count + 4;
    }
    public void DrawOptions()
    {
        int startX = 3;
        int startY = 2;
        int max = GetMaxItemSize();
        for (int i = 0; i < Options.Count; i++)
        {
            Console.SetCursorPosition(startX, startY + i);
            if (ActiveOption == i)
            {
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Black;
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
            }
            Console.WriteLine(Options[i]);
        }
        Console.ResetColor();
    }
    public void DrawFrame()
    {
        var widht = GetFrameWidth();
        var height = GetFrameHeight();
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < widht; x++)
            {
                if (x == 0 || x == widht - 1 || y == 0 || y == height - 1)
                {
                    Console.SetCursorPosition(x, y);
                    Console.BackgroundColor = ConsoleColor.Magenta;
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.Write(' ');
                }
            }
        }
        Console.ResetColor();
        Console.WriteLine();
    }
    public void DrawFrame(ConsoleColor color)
    {
        var widht = GetFrameWidth();
        var height = GetFrameHeight();
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < widht; x++)
            {
                if (x == 0 || x == widht - 1 || y == 0 || y == height - 1)
                {
                    Console.SetCursorPosition(x, y);
                    Console.BackgroundColor = color;
                    Console.ForegroundColor = color;
                    Console.Write(' ');
                }
            }
        }
        Console.ResetColor();
        Console.WriteLine();
    }
    public int GetSelectedOption()
    {
        return ActiveOption;
    }
    public void Down()
    {
        ActiveOption++;
        if (ActiveOption >= Options.Count)
        {
            ActiveOption = 0;
        }
    }
    public void Up()
    {
        ActiveOption--;
        if (ActiveOption < 0)
        {
            ActiveOption = Options.Count - 1;
        }
    }
}

