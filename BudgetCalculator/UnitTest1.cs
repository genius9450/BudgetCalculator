using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Linq;

namespace BudgetCalculator
{
    [TestClass]
    public class UnitTest1
    {
        private IBudgetRepo _budgetRepo;
        private BudgetCalculator _budgetCalculator;

        [TestInitialize]
        public void Set_Up()
        {
            _budgetRepo = Substitute.For<IBudgetRepo>();
            _budgetCalculator = new BudgetCalculator(_budgetRepo);
        }

        [TestMethod]
        public void whole_month()
        {
            _budgetRepo.GetAll().Returns(new List<Budget>()
            {
                new Budget(){ YearMonth = "202101", Amount = 31 }
            });

            DateTime start = new DateTime(2021, 1, 1);
            DateTime end = new DateTime(2021, 1, 31);
            var amount = _budgetCalculator.Query(start, end);

            Assert.AreEqual(31, amount);
        }

        [TestMethod]
        public void partial_month()
        {
            _budgetRepo.GetAll().Returns(new List<Budget>()
            {
                new Budget(){ YearMonth = "202101", Amount = 310 }
            });

            DateTime start = new DateTime(2021, 1, 1);
            DateTime end = new DateTime(2021, 1, 10);
            var amount = _budgetCalculator.Query(start, end);

            Assert.AreEqual(100, amount);
        }


        public class BudgetCalculator
        {
            private readonly IBudgetRepo _budgetRepo;

            public BudgetCalculator(IBudgetRepo budgetRepo)
            {
                _budgetRepo = budgetRepo;
            }

            public decimal Query(DateTime start, DateTime end)
            {
                var totalDays = (end - start).TotalDays +1 ;
                var startMonth = start.ToString("yyyyMM");
                var endMonth = end.ToString("yyyyMM");


                var budgets = _budgetRepo.GetAll();

                var monthAmounts = budgets.Where(x => x.YearMonth == startMonth).Sum(x=>x.Amount);
                var daysInMonth = DateTime.DaysInMonth(start.Year, start.Month);

                var sum = monthAmounts * (totalDays/daysInMonth);


                return (decimal) sum;
            }
        }
    }

    public interface IBudgetRepo
    {
        List<Budget> GetAll();
    }

    public class Budget
    {
        public int Amount { get; set; }
        public string YearMonth { get; set; }
    }
}
