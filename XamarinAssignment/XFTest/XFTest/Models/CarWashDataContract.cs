using System;
using System.Collections.Generic;
using System.Text;

namespace XFTest.Models
{
    public partial class CarWashDataContract
    {
        public DateTime AppliedTime { get; set; }
        public Guid VisitId { get; set; }
        public Guid HomeBobEmployeeId { get; set; }
        public Guid HouseOwnerId { get; set; }
        public bool IsBlocked { get; set; }
        public string StartTimeUtc { get; set; }
        public string EndTimeUtc { get; set; }
        public string Title { get; set; }
        public bool IsReviewed { get; set; }
        public bool IsFirstVisit { get; set; }
        public bool IsManual { get; set; }
        public long VisitTimeUsed { get; set; }
        public object RememberToday { get; set; }
        public string HouseOwnerFirstName { get; set; }
        public string HouseOwnerLastName { get; set; }
        public string HouseOwnerMobilePhone { get; set; }
        public string HouseOwnerAddress { get; set; }
        public long HouseOwnerZip { get; set; }
        public string HouseOwnerCity { get; set; }
        public double HouseOwnerLatitude { get; set; }
        public double HouseOwnerLongitude { get; set; }
        public bool IsSubscriber { get; set; }
        public object RememberAlways { get; set; }
        public string Professional { get; set; }

        public string VisitState { get; set; }

        public long StateOrder { get; set; }

        public string ExpectedTime { get; set; }

        public List<Task> Tasks { get; set; }
        public string DisplayTask { get; set; }
        public string ToatalTimeForTasks { get; set; }
        public double Location { get; set; }
        public double DistanceFromNextService { get; set; }
        public object[] HouseOwnerAssets { get; set; }

        public object[] VisitAssets { get; set; }

        public object[] VisitMessages { get; set; }
    }

    public  class Task
    {
        public Guid TaskId { get; set; }

        public string Title { get; set; }

        public bool IsTemplate { get; set; }

        public long TimesInMinutes { get; set; }

        public long Price { get; set; }

        public Guid PaymentTypeId { get; set; }

        public DateTimeOffset CreateDateUtc { get; set; }

        public DateTimeOffset LastUpdateDateUtc { get; set; }

        public object PaymentTypes { get; set; }
    }

    public partial class ServiceResut
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<CarWashDataContract> Data { get; set; }
        public long Code { get; set; }
    }
}

