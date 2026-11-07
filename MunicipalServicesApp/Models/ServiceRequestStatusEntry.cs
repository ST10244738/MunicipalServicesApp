using System;

namespace MunicipalServicesApp.Models
{
    /// <summary>
    /// Stores a record of a service request status change.
    /// </summary>
    public class ServiceRequestStatusEntry
    {
        public DateTime ChangedAt { get; set; } = DateTime.Now;

        public ServiceRequestStatus Status { get; set; }

        public string Notes { get; set; } = string.Empty;

        public string UpdatedBy { get; set; } = "Municipal Services";

        public ServiceRequestStatusEntry()
        {
        }

        public ServiceRequestStatusEntry(
            ServiceRequestStatus status,
            string notes,
            DateTime? changedAt = null,
            string updatedBy = "Municipal Services")
        {
            Status = status;
            Notes = notes;
            ChangedAt = changedAt ?? DateTime.Now;
            UpdatedBy = updatedBy;
        }
    }
}