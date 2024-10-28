using System;
using System.IO;
using NodaTime;
using NodaTime.Extensions;
using FinanceApp;
using Spectre.Console;
using Newtonsoft.Json;

public class FileStorage
{
    public static void CreateStatementFile(int startYear, int startMonth, int endYear, int endMonth)
    {
        bool overflowCheck = false;

        // Calculate the last Fridays of the start and end months using NodaTime
        var startFriday = GetLastFriday(startYear, startMonth, overflowCheck);
        var endFriday = GetLastFriday(endYear, endMonth, overflowCheck);

        // Format the file name as "YY.StartDay-EndDay.txt"
        string fileName = $"{startYear % 100}-{startFriday:dd-MM}--{endYear % 100}-{endFriday:dd-MM}.txt";

        // Get the Documents/Finance directory path
        string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        string financeDirectory = Path.Combine(documentsPath, "Finance");

        // Create the Finance directory if it doesn't exist
        Directory.CreateDirectory(financeDirectory);

        // Define the full file path
        string filePath = Path.Combine(financeDirectory, fileName);

        if (File.Exists(filePath))
        {
            // Provide feedback to the user
            AnsiConsole.Markup($"[bold red]A statement for this period already exists: {fileName}.\n[/]");

            // Use a SelectionPrompt for Yes/No choices with arrow keys
            string overwriteChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold yellow]Do you want to overwrite it?[/]")
                    .AddChoices("Yes", "No"));

            if (overwriteChoice == "No")
            {
                AnsiConsole.Markup("[bold yellow]Operation canceled. No changes were made.[/]");
                return;  // Exit the method if the user doesn't want to overwrite the file
            }
        }



        // Create the file (empty for now, can be filled with statement info later)
        using (FileStream fs = File.Create(filePath))
        {
            // You can write initial content if needed here
        }

        AnsiConsole.Markup($"[bold white]File created:[/] [bold green]{fileName}[/]");
    }


    
    public static void StoreRecurringPayment(RecurringPayment payment)
        {
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string financeDirectory = Path.Combine(documentsPath, "Finance");
            Directory.CreateDirectory(financeDirectory);
            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Finance", "recurring_payments.json");

            // Load existing payments
            List<RecurringPayment> payments = new List<RecurringPayment>();
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                payments = JsonConvert.DeserializeObject<List<RecurringPayment>>(json) ?? new List<RecurringPayment>();
            }

            // Add the new recurring payment
            payments.Add(payment);

            // Write updated list back to the file
            string updatedJson = JsonConvert.SerializeObject(payments, Formatting.Indented);
            File.WriteAllText(filePath, updatedJson);
            
            // Code for adding recurring payments (to be implemented)
            Console.Clear();
            AnsiConsole.Markup("[bold green]Adding Recurring Payment...[/]\n");
        }

    
    
    
    
    
    
    //Helper Methods:
    // Helper method to get the last Friday of a month
    private static NodaTime.LocalDate GetLastFriday(int year, int month , bool overflowCheck)
    {
        if (overflowCheck == true) {year--;}
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
