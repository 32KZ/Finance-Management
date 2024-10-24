using NodaTime;
using FinanceApp;
using System;
using System.IO;

namespace FinanceApp
{
    public class RecurringPayment
    {
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public LocalDate StartDate { get; set; }  // Using NodaTime for dates
        public LocalDate? EndDate { get; set; }   // Nullable for ongoing payments
        public string Type { get; set; }  // Leisure, Mandatory, etc.
    }
}