using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetCalculator
{
    public interface IBudgetRepo
    {
        List<Budget> GetAll();
    }

}
