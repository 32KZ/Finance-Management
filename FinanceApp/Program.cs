
//Imports
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Spectre.Console;
using NodaTime;

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

                ShowMenu();
                string choice = Console.ReadLine();
                
                switch (choice)
                {
                    case "1":
                        AddRecurringPayment();
                        break;
                    case "2":
                        AddOneOffPayment();
                        break;
                    case "3":
                        ViewSpending();
                        break;
                    case "4":
                        NewStatement();
                        break;
                    case "5":
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

        static void ShowMenu()
        {
            AnsiConsole.Markup("\n[bold cyan]What would you like to do?[/]\n");
            Console.WriteLine("1. Add Recurring Payment");
            Console.WriteLine("2. Add One-Off Payment");
            Console.WriteLine("3. View Spending");
            Console.WriteLine("4. Create New Month Statement");
            Console.WriteLine("5. Exit");
            Console.Write("Enter your choice: ");
        }

        static void AddRecurringPayment()
        {
            // Code for adding recurring payments (to be implemented)
            Console.Clear();
            AnsiConsole.Markup("[bold green]Adding Recurring Payment...[/]\n");
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

            // Call the method to create the statement file, passing in the selected year and month
            FileStorage.CreateStatementFile(selectedYear + 2000, selectedMonth, selectedMonth+1);  // Assuming the 2000s for the 2-digit year
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

    }

    


    // Define your data models here
    public class MonthlyStatement
    {
        public List<Payment> RecurringPayments { get; set; }
        public List<Payment> OneOffPayments { get; set; }
        public decimal TotalSpending { get; set; }
        public decimal RemainingBalance { get; set; }
    }

    public class Payment
    {
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public LocalDate Date { get; set; }  // Using NodaTime for dates
        public string Note { get; set; }
    }
}
