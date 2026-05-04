namespace SmartFix.Domain.ValueObjects;

public enum RequestStatus
{
    New = 0, 
    Accepted = 1,
    Diagnostics = 2, 
    InProgress = 3,  
    Pending = 4,
    Ready = 5,        
    Closed = 6,     
    Cancelled = 7 
}