using SmartFix.Domain.Aggregates;
using SmartFix.Domain.ValueObjects;

namespace SmartFix.Application.Helpers;

public static class DiscountCalculator
{
    public static void CalculateAndApplyDiscounts(Request request, List<Discount> allActiveRules, int clientTotalOrders,
        decimal personalDiscountPercent, PromoCode? appliedPromoCode = null)
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
        if (appliedPromoCode != null && currentBalance > 0)
        {
            decimal promoSaved = 0;
            if (appliedPromoCode.Type == DiscountType.Percent)
            {
                promoSaved = currentBalance * (appliedPromoCode.Value / 100m);
            }
            else if (appliedPromoCode.Type == DiscountType.FixedAmount)
            {
                promoSaved = appliedPromoCode.Value;
            }

            if (promoSaved > currentBalance) promoSaved = currentBalance;
            currentBalance -= promoSaved;

            request.ApplyDiscount(null, $"Промокод '{appliedPromoCode.Code}'", promoSaved);
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

    public static void ApplyPromoCode(Request request, PromoCode promoCode)
    {
        decimal savedAmount = 0;
        if (promoCode.Type == DiscountType.Percent)
        {
            savedAmount = request.FinalPrice * (promoCode.Value / 100m);
        }
        else if (promoCode.Type == DiscountType.FixedAmount)
        {
            savedAmount = promoCode.Value;
        }

        if (savedAmount > request.FinalPrice) savedAmount = request.FinalPrice;
        request.ApplyDiscount(null, $"Промокод '{promoCode.Code}'", savedAmount);
    }
}