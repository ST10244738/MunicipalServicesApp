using System;
using System.Collections.Generic;

namespace MunicipalServicesApp.Models
{
    /// <summary>
    /// Represents a municipal service request
    /// submitted by a resident.
    /// </summary>
    public class ServiceRequest
    {
        // Unique reference number
        public string ReferenceNumber { get; set; } = string.Empty;

        // Resident information
        public string ResidentName { get; set; } = "Resident";

        // Request information
        public string Location { get; set; } = string.Empty;

        public string Category { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        // Department responsible for the request
        public ServiceDepartment Department { get; set; }

        // Current request status
        public ServiceRequestStatus Status { get; set; }
            = ServiceRequestStatus.Submitted;

        // Request urgency
        public ServiceRequestPriority Priority { get; set; }
            = ServiceRequestPriority.Normal;

        // Request dates
        public DateTime DateSubmitted { get; set; } = DateTime.Now;

        public DateTime? ExpectedResolutionDate { get; set; }

        public DateTime? DateResolved { get; set; }

        // Complete history of status changes
        public List<ServiceRequestStatusEntry> StatusHistory
        { get; set; } = new List<ServiceRequestStatusEntry>();

        /// <summary>
        /// Updates the request status and records the change.
        /// </summary>
        public void UpdateStatus(
            ServiceRequestStatus newStatus,
            string notes,
            string updatedBy = "Municipal Services")
        {
            Status = newStatus;

            if (newStatus == ServiceRequestStatus.Resolved ||
                newStatus == ServiceRequestStatus.Closed)
            {
                DateResolved = DateTime.Now;
            }

            StatusHistory.Add(
                new ServiceRequestStatusEntry(
                    newStatus,
                    notes,
                    DateTime.Now,
                    updatedBy
                )
            );
        }
    }
}