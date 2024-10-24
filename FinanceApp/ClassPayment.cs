    using System;
    using FinanceApp;
    using NodaTime;
    using System.IO;   

    namespace FinanceApp
{
    public class Payment
    {
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public LocalDate Date { get; set; }  // Using NodaTime for dates
        public string Note { get; set; }
    }
    
}