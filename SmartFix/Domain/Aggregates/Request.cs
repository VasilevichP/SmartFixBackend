using System.Net;
using SmartFix.Domain.Exceptions;
using SmartFix.Domain.ValueObjects;

namespace SmartFix.Domain.Aggregates;

public class Request
{
    public Guid Id { get; private set; }
    public RequestStatus Status { get; private set; }
    public string Description { get; private set; }
    public string? DiagnosticResult { get; private set; }
    public string? DeviceAppearance { get; private set; }
    public string? DevicePackage { get; private set; }
    public decimal Price { get; private set; }

    public string? CancellationReason { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? ClosedAt { get; private set; }

    public string DeviceSerialNumber { get; private set; }

    // --- ---
    public Guid DeviceTypeId { get; private set; }
    public DeviceType DeviceType { get; private set; }
    public Guid? DeviceModelId { get; private set; }
    public DeviceModel? DeviceModel { get; private set; }
    public string DeviceModelName { get; private set; }
    public Guid? SpecialistId { get; private set; }
    public Specialist? Specialist { get; private set; }
    public Guid ClientId { get; private set; }
    public User Client { get; private set; }
    public string ContactEmail { get; private set; }
    public string ContactPhoneNumber { get; private set; }
    public string ContactName { get; private set; }
    public bool IsCourierDelivery { get; private set; }
    public string? DeliveryAddress { get; private set; }

    public decimal DeliveryCost { get; private set; }

    // --- ---
    private readonly List<RequestService> _services = new();
    public IReadOnlyCollection<RequestService> Services => _services.AsReadOnly();

    private readonly List<RequestPhoto> _photos = new();
    public IReadOnlyCollection<RequestPhoto> Photos => _photos.AsReadOnly();

    private readonly List<StatusHistory> _statusHistories = new();
    public IReadOnlyCollection<StatusHistory> StatusHistories => _statusHistories.AsReadOnly();

    private Request()
    {
    }

    public static Request Create(Guid clientId, Guid deviceTypeId, string deviceModelName,
        string description, string contactName, string contactPhone, string contactEmail,
        bool isCourier, string? address, string? serialNumber, Guid? deviceModelId = null,
        Service? initialService = null)
    {
        if (isCourier && string.IsNullOrWhiteSpace(address))
            throw new HttpException(HttpStatusCode.BadRequest, "Для курьерской доставки необходимо указать адрес.");

        var request = new Request
        {
            Status = RequestStatus.New,
            CreatedAt = DateTime.UtcNow,

            ClientId = clientId,
            ContactName = contactName,
            ContactPhoneNumber = contactPhone,
            ContactEmail = contactEmail,

            DeviceTypeId = deviceTypeId,
            DeviceModelId = deviceModelId,
            DeviceModelName = deviceModelName,
            DeviceSerialNumber = serialNumber ?? string.Empty,
            Description = description,

            IsCourierDelivery = isCourier,
            DeliveryAddress = address,
            DeliveryCost = 0
        };
        if (initialService != null)
        {
            request.AddService(initialService);
        }

        request.AddStatusHistory(RequestStatus.New);
        return request;
    }

    public void ChangeStatus(RequestStatus newStatus)
    {
        if (Status == newStatus) return;
        
        if (newStatus == RequestStatus.Accepted)
        {
            if (string.IsNullOrWhiteSpace(DeviceAppearance) || string.IsNullOrWhiteSpace(DevicePackage))
                throw new HttpException(HttpStatusCode.BadRequest,"Для приема заявки необходимо заполнить Акт приемки (внешний вид и комплектацию).");
        }

        if (newStatus == RequestStatus.InProgress)
        {
            if (!SpecialistId.HasValue)
                throw new HttpException(HttpStatusCode.BadRequest,"Нельзя начать работу без назначенного специалиста.");
        }

        // if (newStatus == RequestStatus.Ready || newStatus == RequestStatus.Closed)
        // {
        //     if (_services.Count == 0 && Price <= 0 && Status != RequestStatus.Cancelled)
        //         throw new DomainException("Нельзя завершить заявку с нулевой стоимостью и без услуг (кроме отмены).");
        // }

        Status = newStatus;
        AddStatusHistory(newStatus);

        if (newStatus == RequestStatus.Closed || newStatus == RequestStatus.Cancelled)
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
            throw new HttpException(HttpStatusCode.BadRequest,"Укажите причину отмены.");

        CancellationReason = reason;
        ChangeStatus(RequestStatus.Cancelled);
    }

    public void AddService(Service service)
    {
        EnsureActive();

        var requestService = RequestService.Create(Id, service);
        _services.Add(requestService);

        RecalculatePrice();
    }
    
    public void AddService(string name, decimal price)
    {
        EnsureActive();

        var requestService = RequestService.Create(Id, name, price);
        _services.Add(requestService);

        RecalculatePrice();
    }

    public void RemoveService(Guid serviceId)
    {
        EnsureActive();

        var item = _services.FirstOrDefault(s => s.ServiceId == serviceId); // Или по Id записи
        if (item != null)
        {
            _services.Remove(item);
            RecalculatePrice();
        }
    }


    private void AddStatusHistory(RequestStatus status)
    {
        _statusHistories.Add(StatusHistory.Create(this.Id, status));
    }

    public void UpdateAcceptanceInfo(string appearance, string package)
    {
        EnsureActive();
        DeviceAppearance = appearance;
        DevicePackage = package;
    }

    public void UpdateDeviceInfo(Guid deviceTypeId, Guid? deviceModelId, string deviceModelName, string serialNumber)
    {
        EnsureActive();
        DeviceTypeId = deviceTypeId;
        DeviceModelId = deviceModelId;
        DeviceModelName = deviceModelName;
        DeviceSerialNumber = serialNumber;
    }

    public void AssignSpecialist(Guid specialistId)
    {
        EnsureActive();

        SpecialistId = specialistId;
        if (Status == RequestStatus.New)
        {
            ChangeStatus(RequestStatus.Diagnostics);
        }
    }

    public void SetDeliveryCost(decimal cost)
    {
        EnsureActive();
        if (!IsCourierDelivery && cost > 0)
            throw new HttpException(HttpStatusCode.BadRequest,"Нельзя выставить счет за доставку для самовывоза.");
        
        DeliveryCost = cost;
        RecalculatePrice();
    }
    
    public void SetDiagnosticResult(string result)
    {
        EnsureActive();
        DiagnosticResult = result;
    }

    public void AddPhoto(string fileName, string filePath)
    {
        if (_photos.Count >= 5)
        {
            throw new HttpException(HttpStatusCode.BadRequest, "Нельзя добавить более 5 фотографий к одной заявке.");
        }

        _photos.Add(RequestPhoto.Create(this.Id, fileName, filePath));
    }
    private void EnsureActive()
    {
        if (Status == RequestStatus.Closed || Status == RequestStatus.Cancelled)
            throw new HttpException(HttpStatusCode.BadRequest,
                "Редактирование закрытой или отмененной заявки запрещено.");
    }

    private void RecalculatePrice()
    {
        Price = _services.Sum(s => s.Price) + DeliveryCost;
    }
}