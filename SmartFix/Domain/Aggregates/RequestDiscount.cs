namespace SmartFix.Domain.Aggregates;

public class RequestDiscount
{
    public Guid Id { get; private set; }
    public Guid RequestId { get; private set; }
    public Request Request { get; private set; }
    public Guid? DiscountId { get; private set; }
    public Discount? Discount { get; private set; }
    public string RuleName { get; private set; }
    public decimal SavedAmount { get; private set; }

    private RequestDiscount()
    {
    }

    public static RequestDiscount Create(Guid requestId, Guid? discountId, string ruleName, decimal savedAmount)
    {
        return new RequestDiscount 
        { 
            Id = Guid.NewGuid(), 
            RequestId = requestId, 
            DiscountId = discountId,
            RuleName = ruleName, 
            SavedAmount = savedAmount 
        };
    }
}