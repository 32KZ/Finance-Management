using System;
using System.IO;
using NodaTime;
using NodaTime.Extensions;

public class FileStorage
{
    public static void CreateStatementFile(int year, int startMonth, int endMonth)
    {
        // Calculate the last Fridays of the start and end months using NodaTime
        var startFriday = GetLastFriday(year, startMonth);
        var endFriday = GetLastFriday(year, endMonth);
        
        // Format the file name as "YY.StartDay-EndDay.txt"
        string fileName = $"{year % 100}.{startFriday:dd-MM}-{endFriday:dd-MM}.txt";
        
        // Get the Documents/Finance directory path
        string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        string financeDirectory = Path.Combine(documentsPath, "Finance");
        
        // Create the Finance directory if it doesn't exist
        Directory.CreateDirectory(financeDirectory);
        
        // Define the full file path
        string filePath = Path.Combine(financeDirectory, fileName);

        // Check if the file already exists
        if (File.Exists(filePath))
        {
            // Provide feedback to the user
            Console.WriteLine($"[bold red]A statement for this period already exists: {fileName}.[/]");
            Console.Write("Do you want to overwrite it? (y/n): ");
            string overwriteChoice = Console.ReadLine().ToLower();

            if (overwriteChoice != "y")
            {
                Console.WriteLine("[bold yellow]Operation canceled. No changes were made.[/]");
                return;  // Exit the method if the user doesn't want to overwrite the file
            }
        }

        
        // Create the file (empty for now, can be filled with statement info later)
        using (FileStream fs = File.Create(filePath))
        {
            // You can write initial content if needed here
        }
        
        Console.WriteLine($"File created: {filePath}");
    }

    // Helper method to get the last Friday of a month
    private static NodaTime.LocalDate GetLastFriday(int year, int month)
    {
        var dateTimeZone = DateTimeZoneProviders.Tzdb["Europe/London"]; // You can adjust for your preferred time zone
        var date = new LocalDate(year, month, 1);
        var lastDayOfMonth = date.PlusMonths(1).PlusDays(-1); // Go to the last day of the month
        // Loop backwards from the last day of the month until we hit a Friday
        while (lastDayOfMonth.DayOfWeek != IsoDayOfWeek.Friday)
        {
            lastDayOfMonth = lastDayOfMonth.PlusDays(-1);
        }

        return lastDayOfMonth;
    }
}
