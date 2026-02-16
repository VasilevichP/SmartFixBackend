using System.Net;
using SmartFix.Domain.Exceptions;
using SmartFix.Domain.ValueObjects;

namespace SmartFix.Domain.Aggregates;

public class Request
{
    public Guid Id { get; private set; }
    public RequestStatus Status { get; private set; }
    public string Description { get; private set; }
    public decimal? Price { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public DateTime? ClosedAt { get; private set; }
    public string DeviceSerialNumber { get; private set; }

    //---------------------------------------
    public Guid DeviceTypeId { get; private set; }
    public DeviceType DeviceType { get; private set; }

    public Guid? DeviceModelId { get; private set; }
    public DeviceModel? DeviceModel { get; private set; }
    public string DeviceModelName { get; private set; }
    public Guid? SpecialistId { get; private set; }
    public Specialist Specialist { get; private set; }
    public Guid ClientId { get; private set; }
    public User Client { get; private set; }

    public string ContactEmail { get; private set; }
    public string ContactPhoneNumber { get; private set; }
    public string ContactName { get; private set; }
    public Guid? ServiceId { get; private set; }

    public Service? Service { get; private set; }

    //------------------------------------------
    private readonly List<RequestPhoto> _photos = new();
    public IReadOnlyCollection<RequestPhoto> Photos => _photos.AsReadOnly();
    private readonly List<StatusHistory> _statusHistories = new();
    public IReadOnlyCollection<StatusHistory> StatusHistories => _statusHistories.AsReadOnly();

    private Request()
    {
    }

    public static Request Create(Guid clientId, string contactEmail, string contactPhone, string contactName,
        Guid deviceTypeId,
        string description, decimal? price, Guid? serviceId, Guid? deviceModelId, string deviceModelName,
        string deviceSerialNumber)
    {
        var request = new Request
        {
            Status = RequestStatus.New,
            CreatedAt = DateTime.UtcNow,
            ClientId = clientId,

            DeviceTypeId = deviceTypeId,
            DeviceModelName = deviceModelName,
            DeviceModelId = deviceModelId,

            ContactEmail = contactEmail,
            ContactPhoneNumber = contactPhone,
            ContactName = contactName,

            ServiceId = serviceId,

            Price = price,
            Description = description,
            DeviceSerialNumber = deviceSerialNumber
        };
        request.AddStatusHistory(RequestStatus.New);
        return request;
    }

    public void ChangeStatus(RequestStatus newStatus)
    {
        if (Status == newStatus) return;
        if (Status == RequestStatus.Closed || Status == RequestStatus.Cancelled)
        {
            throw new HttpException(HttpStatusCode.BadRequest,
                $"Нельзя изменить статус, так как заявка уже находится в финальном статусе '{Status}'.");
        }

        if (newStatus == RequestStatus.Ready || newStatus == RequestStatus.Closed)
        {
            if (!Price.HasValue || Price <= 0)
            {
                throw new HttpException(HttpStatusCode.BadRequest,
                    "Нельзя завершить заявку, пока не установлена итоговая цена.");
            }

            if (!SpecialistId.HasValue)
            {
                throw new HttpException(HttpStatusCode.BadRequest,
                    "Нельзя завершить заявку без назначенного исполнителя.");
            }
        }

        if (newStatus == RequestStatus.InProgress && !SpecialistId.HasValue)
        {
            throw new HttpException(HttpStatusCode.BadRequest, "Нельзя перевести заявку в работу без исполнителя.");
        }

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

    private void AddStatusHistory(RequestStatus status)
    {
        _statusHistories.Add(StatusHistory.Create(this.Id, status));
    }

    public void AssignSpecialist(Guid specialistId)
    {
        if (Status == RequestStatus.Closed || Status == RequestStatus.Cancelled)
        {
            throw new HttpException(HttpStatusCode.BadRequest, "Нельзя назначать специалиста на закрытую заявку.");
        }

        SpecialistId = specialistId;
        if (Status == RequestStatus.New)
        {
            ChangeStatus(RequestStatus.Diagnostics);
        }
    }

    public void AssignPrice(decimal price)
    {
        if (Status == RequestStatus.Closed || Status == RequestStatus.Cancelled)
        {
            throw new HttpException(HttpStatusCode.BadRequest,
                "Нельзя менять цену для закрытой или отмененной заявки.");
        }

        if (Price.HasValue && Price.Value > 0)
        {
            throw new HttpException(HttpStatusCode.BadRequest, "Цена уже утверждена. Изменение невозможно.");
        }

        if (price <= 0)
            throw new HttpException(HttpStatusCode.BadRequest, "Цена должна быть больше 0");
        Price = price;
    }

    public void AddPhoto(string fileName, string filePath)
    {
        if (_photos.Count >= 5)
        {
            throw new HttpException(HttpStatusCode.BadRequest, "Нельзя добавить более 5 фотографий к одной заявке.");
        }

        _photos.Add(RequestPhoto.Create(this.Id, fileName, filePath));
    }
}