
//Imports
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Spectre.Console;
using NodaTime;
using NodaTime.Text;

using FinanceApp;
//Main Namespace.
namespace FinanceApp
{
   //Main Program
    class Program
    {
        // Entry point of your application
        static void Main(string[] args)
        {
            // Setting up your directory in the Documents folder
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string appDirectory = Path.Combine(documentsPath, "FinanceAppData");
            Directory.CreateDirectory(appDirectory);

            // Application start
            AnsiConsole.Markup("[bold green]Welcome to the Finance Management App![/]");
            
            // Clear the screen before showing the menu
            Console.Clear();
            // Temporary placeholder for input loop (add more functionality later)
            
            bool running = true;
            while (running)
            {

                
                string choice = ShowMenu();
                
                switch (choice)
                {
                    case "Add Recurring Payment":
                        AddNewRecurringPayment();
                        break;
                    case "Add One-Off Payment":
                        AddOneOffPayment();
                        break;
                    case "View Spending":
                        ViewSpending();
                        break;
                    case "Create New Month Statement":
                        NewStatement();
                        break;
                    case "Exit":
                        running = false;
                        AnsiConsole.Markup("[bold red]------[/]");
                        AnsiConsole.Markup("[bold red]----[/]");
                        AnsiConsole.Markup("[bold red]--[/]");
                        break;
                    default:
                        AnsiConsole.Markup("[bold yellow]Invalid choice, please try again.[/]");
                        break;
                }
            }
        }

        public static string ShowMenu()
        {
            string Choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("What would you like to do?")
                    .AddChoices("Add Recurring Payment", "Add One-Off Payment", "View Spending", "Create New Month Statement", "Exit"));
        
        AnsiConsole.Clear();

        return Choice;
        }




        public static void AddNewRecurringPayment()
        {
            // Input for payment name using Spectre.Console
            string name = AnsiConsole.Ask<string>("Enter the [green]payment name[/]:");

            // Input for payment amount using Spectre.Console
            decimal amount = AnsiConsole.Ask<decimal>("Enter the [green]payment amount[/]:");

            // Input for start date using Spectre.Console (yyyy-MM-dd)
            string startDateInput = AnsiConsole.Ask<string>("Enter the [green]start date[/] (format: yyyy-MM-dd):");
            var startDatePattern = LocalDatePattern.Iso;
            LocalDate startDate = startDatePattern.Parse(startDateInput).Value;

            // Input for end date or allow ongoing payments using Spectre.Console
            string endDateInput = AnsiConsole.Prompt(new TextPrompt<string>("Enter the [green]end date[/] (or press Enter for ongoing):").AllowEmpty());

            LocalDate? endDate = null;
            if (!string.IsNullOrEmpty(endDateInput))
            {
                endDate = startDatePattern.Parse(endDateInput).Value;  // Parse the end date if provided
            }

            // Input for payment type (Leisure or Mandatory) using Spectre.Console
            string type = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select the [green]payment type[/]:")
                    .AddChoices("Leisure", "Mandatory"));

            // Create a new RecurringPayment instance
            RecurringPayment payment = new RecurringPayment
            {
                Name = name,
                Amount = amount,
                StartDate = startDate,
                EndDate = endDate,
                Type = type
            };

            // Call AddRecurringPayment method to store the payment
            FileStorage.StoreRecurringPayment(payment);

            // Display success message using Spectre.Console
            AnsiConsole.Markup("[bold green]Recurring payment added successfully![/]");
        }



        static void AddOneOffPayment()
        {
            // Code for adding one-off payments (to be implemented)
            Console.Clear();
            AnsiConsole.Markup("[bold green]Adding One-Off Payment...[/]\n");
        }

        static void ViewSpending()
        {
            // Code for viewing spending (to be implemented)
            Console.Clear();
            AnsiConsole.Markup("[bold green]Viewing Spending...[/]\n");
        }

       static void NewStatement()
        {
            AnsiConsole.Markup("[bold cyan]Please Provide the Relevant year. YY [/]");
            string selectedYearInput = Console.ReadLine();

            int selectedYear;
            // Validate the year (expecting two digits, e.g., "24")
            while (!int.TryParse(selectedYearInput, out selectedYear) || selectedYear < 0 || selectedYear > 99)
            {
                AnsiConsole.Markup("[bold red]Invalid year. Please provide a valid two-digit year (e.g., 24 for 2024). [/] ");
                selectedYearInput = Console.ReadLine();
            }

            AnsiConsole.Markup("[bold cyan]Please Provide the Relevant Month. Name or Number. [/]");

            string selectedMonthInput = Console.ReadLine();

            int selectedMonth;
            // Try to parse the month if it's a number or convert it if it's a name
            while (!TryGetMonthNumber(selectedMonthInput, out selectedMonth))
            {
                AnsiConsole.Markup("[bold red]Invalid month. Please provide a valid month name (e.g., October) or number (e.g., 10). [/] ");
                selectedMonthInput = Console.ReadLine();
            }

            // Determine the start and end months and handle the year overflow case
            int endMonth = selectedMonth;  // End month is the selected month (no incrementing)
            int endYear = selectedYear;  // Default to the same year
            int startYear = selectedYear; // This will change if we are going back to December of the previous year
            int startMonth = selectedMonth - 1; // Default: start from the previous month

            if (selectedMonth == 1)
            {
                // Special case for January (month 1): start from December of the previous year
                startMonth = 12;  // Start month becomes December
                startYear = selectedYear - 1;  // Start in December of the previous year
            }
            else if (selectedMonth == 12)
            {
                // Special case for December: start from November, but end in December
                startMonth = 11;  // Start in November
            }

            // Call the method to create the statement file, passing in the selected year and months
            FileStorage.CreateStatementFile(startYear + 2000, startMonth, endYear + 2000, endMonth);  // Pass both the start and end years now

            Console.Clear();
            AnsiConsole.Markup("[bold green]Making A New Statement...[/]\n");
        }



        private static bool TryGetMonthNumber(string input, out int monthNumber)
        {
            // Try to parse the input directly if it's a number
            if (int.TryParse(input, out monthNumber) && monthNumber >= 1 && monthNumber <= 12)
            {
                return true;
            }

            // Otherwise, try to match it with a month name
            try
            {
                // Parse the month name
                DateTime month = DateTime.ParseExact(input, "MMMM", System.Globalization.CultureInfo.InvariantCulture);
                monthNumber = month.Month;
                return true;
            }
            catch (FormatException)
            {
                // If the input doesn't match a month name, return false
                monthNumber = 0;
                return false;
            }
        }


        public static List<RecurringPayment> GetActiveRecurringPayments(LocalDate statementMonth)
        {
        string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Finance", "recurring_payments.json");
        
        // Load the recurring payments from the file
        if (!File.Exists(filePath))
        {
            return new List<RecurringPayment>();
        }

        string json = File.ReadAllText(filePath);
        List<RecurringPayment> payments = JsonConvert.DeserializeObject<List<RecurringPayment>>(json);

        // Filter the payments to only those that are active during the statement month
        var activePayments = payments.Where(p => p.StartDate <= statementMonth && (p.EndDate == null || p.EndDate >= statementMonth)).ToList();
        return activePayments;
        }


    }

    

}
