using SmartFix.Domain.Aggregates;
using SmartFix.Domain.ValueObjects;

namespace SmartFix.Application.Helpers;

public static class DiscountCalculator
{
    public static void CalculateAndApplyDiscounts(Request request, List<Discount> allActiveRules, int clientTotalOrders,
        decimal personalDiscountPercent)
    {
        request.ClearDiscounts();

        if (request.Type == RequestType.Warranty || request.BasePrice <= 0)
            return;
        var applicableRules = allActiveRules
            .Where(r => r.IsApplicable(request, clientTotalOrders))
            .ToList();

        var topRulesPerCategory = applicableRules
            .GroupBy(r => r.GetType())
            .Select(group => group.OrderByDescending(r => r.Priority).First())
            .ToList();

        var sortedRulesToApply = topRulesPerCategory
            .OrderByDescending(r => r.Priority)
            .ThenBy(GetDiscountTypeSortOrder)
            .ToList();

        decimal currentBalance = request.BasePrice;

        foreach (var rule in sortedRulesToApply)
        {
            if (currentBalance <= 0) break;

            decimal savedAmount = 0;
            if (rule.Type == DiscountType.Percent)
            {
                savedAmount = currentBalance * (rule.Value / 100m);
            }
            else if (rule.Type == DiscountType.FixedAmount)
            {
                savedAmount = rule.Value;
            }

            if (savedAmount > currentBalance) savedAmount = currentBalance;

            currentBalance -= savedAmount;

            request.ApplyDiscount(rule.Id, rule.Name, savedAmount);
        }
        if (personalDiscountPercent > 0 && currentBalance > 0)
        {
            decimal personalSaved = currentBalance * (personalDiscountPercent / 100m);
            if (personalSaved > currentBalance) personalSaved = currentBalance;
            
            request.ApplyDiscount(null, $"Персональная скидка клиента ({personalDiscountPercent}%)", personalSaved);
        }
    }

    private static int GetDiscountTypeSortOrder(Discount rule)
    {
        return rule switch
        {
            RequestSumDiscount => 1,
            RequestsCountDiscount => 2,
            DayOfWeekDiscount => 3,
            _ => 4
        };
    }
}