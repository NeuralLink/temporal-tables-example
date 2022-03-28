// See https://aka.ms/new-console-template for more information
using TemporalTableTestApp;

Console.WriteLine("How many new events would you like to create? ");

var numberOfNewEvents = int.Parse(Console.ReadLine());
var eventEngine = new EventEngine();
eventEngine.CreateEvents(numberOfNewEvents);

Console.WriteLine($"{numberOfNewEvents} new events were created.");

Console.WriteLine("\n Press any key to update a random event");
Console.WriteLine("Press the Escape (Esc) key to quit: \n");
ConsoleKeyInfo cki;

do
{
    cki = Console.ReadKey();
    if (cki.Key != ConsoleKey.Escape)
    {
        eventEngine.UpdateRandomEvent();
    }
} while (cki.Key != ConsoleKey.Escape);