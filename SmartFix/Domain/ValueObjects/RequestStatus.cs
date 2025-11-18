namespace SmartFix.Domain.ValueObjects;

public enum RequestStatus
{
    New = 0,          
    Diagnostics = 1, 
    InProgress = 2,  
    Ready = 3,        
    Closed = 4,     
    Cancelled = 5 
}