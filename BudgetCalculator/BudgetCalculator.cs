using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BudgetCalculator
{
    public class BudgetCalculator
    {
        private readonly IBudgetRepo _budgetRepo;
        public BudgetCalculator(IBudgetRepo budgetRepo)
        {
            _budgetRepo = budgetRepo;
        }

        public double Query(DateTime start, DateTime end)
        {
            var budgets = _budgetRepo.GetAll();
            var startMonth = start.ToString("yyyyMM");
            var endMonth = end.ToString("yyyyMM");

            var monthAmountList = budgets.Where(x => int.Parse(x.YearMonth) >= int.Parse(startMonth) && int.Parse(x.YearMonth) <= int.Parse(endMonth)).OrderBy(x => x.YearMonth).ToList();
            double sum = monthAmountList.Sum(x => x.Amount);

            var startNotEnoughAmount = GetStartNotEnoughAmount(start, monthAmountList);
            var endNotEnoughAmount = GetEndNotEnoughAmount(end, monthAmountList);

            sum = sum - startNotEnoughAmount - endNotEnoughAmount;

            return sum;
        }

        private static int GetEndNotEnoughAmount(DateTime end, List<Budget> monthAmountList)
        {
            var monthAmount = monthAmountList.LastOrDefault()?.Amount ?? 0;
            return monthAmount * (DateTime.DaysInMonth(end.Year, end.Month) - end.Day) / DateTime.DaysInMonth(end.Year, end.Month);
        }

        private static int GetStartNotEnoughAmount(DateTime start, List<Budget> monthAmountList)
        {
            var monthAmount = monthAmountList.FirstOrDefault()?.Amount ?? 0;
            return monthAmount * (start.Day - 1) / DateTime.DaysInMonth(start.Year, start.Month);
        }
    }
}
