using System.Net;
using SmartFix.Domain.Exceptions;
using SmartFix.Domain.ValueObjects;

namespace SmartFix.Domain.Aggregates;

public class Request
{
    public Guid Id { get; private set; }
    public RequestType Type { get; private set; }
    public RequestStatus Status { get; private set; }

    public string Description { get; private set; }
    public string? DiagnosticResult { get; private set; }
    public string? DeviceAppearance { get; private set; }
    public string? DevicePackage { get; private set; }
    public string? CancellationReason { get; private set; }

    public decimal BasePrice { get; private set; }
    public decimal FinalPrice { get; private set; }

    public DateTime CreatedAt { get; private set; }
    public DateTime? ClosedAt { get; private set; }

    public string DeviceSerialNumber { get; private set; }
    public Guid DeviceTypeId { get; private set; }
    public DeviceType DeviceType { get; private set; }
    public Guid? DeviceModelId { get; private set; }
    public DeviceModel? DeviceModel { get; private set; }
    public string DeviceModelName { get; private set; }

    public Guid ClientId { get; private set; }
    public Client Client { get; private set; }
    public Guid? MasterId { get; private set; }
    public Master? Master { get; private set; }

    public Guid? PromoCodeId { get; set; }
    public PromoCode? PromoCode { get; private set; }
    public string ContactEmail { get; private set; }
    public string ContactName { get; private set; }
    public string ContactPhoneNumber { get; private set; }

    public string? FieldAddress { get; private set; }
    public DateTime? ScheduledTime { get; private set; }

    public Guid? ParentRequestId { get; private set; }

    private readonly List<RequestService> _services = new();
    public IReadOnlyCollection<RequestService> Services => _services.AsReadOnly();

    private readonly List<RequestPhoto> _photos = new();
    public IReadOnlyCollection<RequestPhoto> Photos => _photos.AsReadOnly();

    private readonly List<StatusHistory> _statusHistories = new();
    public IReadOnlyCollection<StatusHistory> StatusHistories => _statusHistories.AsReadOnly();

    private readonly List<RequestDiscount> _appliedDiscounts = new();
    public IReadOnlyCollection<RequestDiscount> AppliedDiscounts => _appliedDiscounts.AsReadOnly();

    private Request()
    {
    }

    public static Request Create(Guid clientId, RequestType type, Guid deviceTypeId, Guid? deviceModelId,
        string deviceModelName,
        string description, string contactName, string contactPhone, string contactEmail, string serialNumber,
        Guid? promoCodeId, string? fieldAddress = null, DateTime? scheduledTime = null, Guid? parentRequestId = null)
    {
        if (type == RequestType.Field && (string.IsNullOrWhiteSpace(fieldAddress) || !scheduledTime.HasValue))
            throw new HttpException(HttpStatusCode.BadRequest,
                "Для выездного ремонта необходимо указать адрес и время визита");

        if (type == RequestType.Warranty && !parentRequestId.HasValue)
            throw new HttpException(HttpStatusCode.BadRequest,
                "Для гарантийного ремонта необходимо указать родительскую заявку");

        var request = new Request
        {
            Id = Guid.NewGuid(),
            Type = type,
            Status = RequestStatus.New,
            CreatedAt = DateTime.UtcNow,

            ClientId = clientId,
            ContactEmail = contactEmail,
            ContactName = contactName,
            ContactPhoneNumber = contactPhone,

            DeviceTypeId = deviceTypeId,
            DeviceModelId = deviceModelId,
            DeviceModelName = deviceModelName,
            DeviceSerialNumber = serialNumber,
            Description = description,

            PromoCodeId = promoCodeId,
            FieldAddress = fieldAddress,
            ScheduledTime = scheduledTime,
            ParentRequestId = parentRequestId
        };

        request.AddStatusHistory(RequestStatus.New);
        return request;
    }

    public void ChangeStatus(RequestStatus newStatus)
    {
        if (Status == newStatus) return;

        if (newStatus == RequestStatus.Accepted && Type == RequestType.InService)
        {
            if (string.IsNullOrWhiteSpace(DeviceAppearance))
                throw new HttpException(HttpStatusCode.BadRequest,
                    "Для приема устройства в сервис необходимо описать его внешний вид");
        }

        if (newStatus == RequestStatus.InProgress && !MasterId.HasValue)
            throw new HttpException(HttpStatusCode.BadRequest, "Нельзя начать ремонт без назначенного мастера");

        Status = newStatus;
        AddStatusHistory(newStatus);

        if (newStatus == RequestStatus.Closed)
        {
            ClosedAt = DateTime.UtcNow;
            foreach (var service in _services)
            {
                service.StartWarranty(ClosedAt.Value);
            }
        }
        else if (newStatus == RequestStatus.Cancelled)
        {
            ClosedAt = DateTime.UtcNow;
        }
        else
        {
            ClosedAt = null;
        }
    }

    public void Cancel(string reason)
    {
        EnsureActive();
        if (string.IsNullOrWhiteSpace(reason))
            throw new HttpException(HttpStatusCode.BadRequest, "Укажите причину отмены");

        CancellationReason = reason;
        ChangeStatus(RequestStatus.Cancelled);
    }

    public void AssignMaster(Guid masterId)
    {
        EnsureActive();
        MasterId = masterId;

        if (Status == RequestStatus.New || Status == RequestStatus.Accepted)
            ChangeStatus(RequestStatus.Diagnostics);
    }

    public void UpdateAcceptanceInfo(string appearance, string package)
    {
        EnsureActive();
        DeviceAppearance = appearance;
        DevicePackage = package;
    }

    public void SetDiagnosticResult(string result)
    {
        EnsureActive();
        DiagnosticResult = result;
    }

    public void AddService(Guid? serviceId, string name, decimal price, int? warrantyPeriodMonths)
    {
        EnsureActive();
        var requestService = RequestService.Create(Id, serviceId, name, price, warrantyPeriodMonths);
        _services.Add(requestService);

        if (Status == RequestStatus.Diagnostics || Status == RequestStatus.InProgress)
        {
            ChangeStatus(RequestStatus.Pending);
        }

        RecalculatePrice();
    }

    public void UpdateDeviceInfo(Guid deviceTypeId, Guid? deviceModelId, string deviceModelName, string serialNumber)
    {
        EnsureActive();
        DeviceTypeId = deviceTypeId;
        DeviceModelId = deviceModelId;
        DeviceModelName = deviceModelName;
        DeviceSerialNumber = serialNumber;
    }

    public void UpdateContactInfo(string contactName, string contactEmail, string contactPhone)
    {
        EnsureActive();
        ContactEmail = contactEmail;
        ContactName = contactName;
        ContactPhoneNumber = contactPhone;
    }

    public void UpdateFieldRequestInfo(string address, DateTime time)
    {
        EnsureActive();
        if (Type != RequestType.Field)
            return;
        FieldAddress = address;
        ScheduledTime = time;
    }

    public void RemoveService(Guid requestServiceId)
    {
        EnsureActive();
        var item = _services.FirstOrDefault(s => s.Id == requestServiceId);
        if (item != null)
        {
            _services.Remove(item);
            RecalculatePrice();
        }
    }

    public void ReplaceServices(
        IEnumerable<(Guid? serviceId, string name, decimal price, int? warrantyPeriodMonths)> newServices)
    {
        EnsureActive();
        _services.Clear();
        foreach (var s in newServices)
        {
            _services.Add(RequestService.Create(Id, s.serviceId, s.name, s.price, s.warrantyPeriodMonths));
        }

        ChangeStatus(RequestStatus.Pending);
        RecalculatePrice();
    }

    public void ApplyDiscount(Guid? discountRuleId, string ruleName, decimal savedAmount)
    {
        EnsureActive();
        _appliedDiscounts.Add(RequestDiscount.Create(this.Id, discountRuleId, ruleName, savedAmount));
        RecalculatePrice();
    }

    public void ClearDiscounts()
    {
        EnsureActive();
        _appliedDiscounts.Clear();
        RecalculatePrice();
    }

    private void RecalculatePrice()
    {
        BasePrice = _services.Sum(s => s.Price);

        if (Type == RequestType.Warranty)
        {
            FinalPrice = 0;
            return;
        }

        decimal totalDiscounts = _appliedDiscounts.Sum(d => d.SavedAmount);
        FinalPrice = BasePrice - totalDiscounts;

        if (FinalPrice < 0) FinalPrice = 0;
    }

    public void AddPhoto(string fileName, string filePath)
    {
        if (_photos.Count >= 5) throw new HttpException(HttpStatusCode.BadRequest, "Максимум 5 фотографий");
        _photos.Add(RequestPhoto.Create(this.Id, fileName, filePath));
    }

    private void AddStatusHistory(RequestStatus status)
    {
        _statusHistories.Add(StatusHistory.Create(this.Id, status));
    }

    private void EnsureActive()
    {
        if (Status == RequestStatus.Closed || Status == RequestStatus.Cancelled)
            throw new HttpException(HttpStatusCode.BadRequest,
                "Редактирование закрытой или отмененной заявки запрещено");
    }
}