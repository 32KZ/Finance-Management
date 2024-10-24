    using System;
    using FinanceApp;
    using NodaTime;
    using System.IO; 

    namespace FinanceApp
{
    public class MonthlyStatement
    {
        public List<Payment> RecurringPayments { get; set; }
        public List<Payment> OneOffPayments { get; set; }
        public decimal TotalSpending { get; set; }
        public decimal RemainingBalance { get; set; }
    }
    
}