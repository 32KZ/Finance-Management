
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
                        running = false;
                        AnsiConsole.Markup("[bold red]Exiting the app. Goodbye![/]");
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
            Console.WriteLine("4. Exit");
            Console.Write("Enter your choice: ");
        }

        static void AddRecurringPayment()
        {
            // Code for adding recurring payments (to be implemented)
            AnsiConsole.Markup("[bold green]Adding Recurring Payment...[/]\n");
        }

        static void AddOneOffPayment()
        {
            // Code for adding one-off payments (to be implemented)
            AnsiConsole.Markup("[bold green]Adding One-Off Payment...[/]\n");
        }

        static void ViewSpending()
        {
            // Code for viewing spending (to be implemented)
            AnsiConsole.Markup("[bold green]Viewing Spending...[/]\n");
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
