using System;
using System.Collections.Generic;
using System.Linq;

namespace MunicipalServicesApp.Models
{
    /// <summary>
    /// Provides an in-memory repository for service requests.
    /// The sample records are demonstration data.
    /// </summary>
    public static class ServiceRequestRepository
    {
        private static readonly List<ServiceRequest> requests =
            CreateSampleRequests();

        /// <summary>
        /// Returns all service requests.
        /// </summary>
        public static IReadOnlyList<ServiceRequest> GetAll()
        {
            return requests.AsReadOnly();
        }

        /// <summary>
        /// Finds a service request using its reference number.
        /// </summary>
        public static ServiceRequest? FindByReference(
            string referenceNumber)
        {
            if (string.IsNullOrWhiteSpace(referenceNumber))
            {
                return null;
            }

            return requests.FirstOrDefault(request =>
                request.ReferenceNumber.Equals(
                    referenceNumber.Trim(),
                    StringComparison.OrdinalIgnoreCase
                ));
        }

        /// <summary>
        /// Adds a new service request.
        /// </summary>
        public static void Add(ServiceRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (string.IsNullOrWhiteSpace(request.ReferenceNumber))
            {
                throw new ArgumentException(
                    "A service request reference number is required.",
                    nameof(request)
                );
            }

            if (FindByReference(request.ReferenceNumber) != null)
            {
                throw new InvalidOperationException(
                    "A request with this reference number already exists."
                );
            }

            requests.Add(request);
        }

        /// <summary>
        /// Creates demonstration service requests.
        /// </summary>
        private static List<ServiceRequest> CreateSampleRequests()
        {
            DateTime today = DateTime.Today;

            var data = new List<ServiceRequest>
            {
                // REQUEST 1
                new ServiceRequest
                {
                    ReferenceNumber = "SR-2026-0001",
                    ResidentName = "Sample Resident",
                    Location = "Ward 4, Main Road",
                    Category = "Roads",
                    Description = "A pothole has formed near the pedestrian crossing.",
                    Department = ServiceDepartment.Roads,
                    Status = ServiceRequestStatus.InProgress,
                    Priority = ServiceRequestPriority.High,
                    DateSubmitted = today.AddDays(-6),
                    ExpectedResolutionDate = today.AddDays(3),

                    StatusHistory = new List<ServiceRequestStatusEntry>
                    {
                        new(
                            ServiceRequestStatus.Submitted,
                            "Request received by the municipality.",
                            today.AddDays(-6)
                        ),

                        new(
                            ServiceRequestStatus.UnderReview,
                            "The request was checked by the roads department.",
                            today.AddDays(-5)
                        ),

                        new(
                            ServiceRequestStatus.Assigned,
                            "A maintenance team was assigned.",
                            today.AddDays(-4)
                        ),

                        new(
                            ServiceRequestStatus.InProgress,
                            "Repair work is currently in progress.",
                            today.AddDays(-2)
                        )
                    }
                },

                // REQUEST 2
                new ServiceRequest
                {
                    ReferenceNumber = "SR-2026-0002",
                    ResidentName = "Sample Resident",
                    Location = "Ward 2, Oak Street",
                    Category = "Water Supply",
                    Description = "A leaking water pipe is causing water loss.",
                    Department = ServiceDepartment.WaterAndSanitation,
                    Status = ServiceRequestStatus.Assigned,
                    Priority = ServiceRequestPriority.Critical,
                    DateSubmitted = today.AddDays(-3),
                    ExpectedResolutionDate = today.AddDays(1),

                    StatusHistory = new List<ServiceRequestStatusEntry>
                    {
                        new(
                            ServiceRequestStatus.Submitted,
                            "Water leak reported.",
                            today.AddDays(-3)
                        ),

                        new(
                            ServiceRequestStatus.UnderReview,
                            "The report was reviewed as urgent.",
                            today.AddDays(-2)
                        ),

                        new(
                            ServiceRequestStatus.Assigned,
                            "An emergency response team was assigned.",
                            today.AddDays(-1)
                        )
                    }
                },

                // REQUEST 3
                new ServiceRequest
                {
                    ReferenceNumber = "SR-2026-0003",
                    ResidentName = "Sample Resident",
                    Location = "Ward 7, Community Park",
                    Category = "Waste Management",
                    Description = "A public waste bin has not been collected.",
                    Department = ServiceDepartment.WasteManagement,
                    Status = ServiceRequestStatus.Resolved,
                    Priority = ServiceRequestPriority.Normal,
                    DateSubmitted = today.AddDays(-10),
                    ExpectedResolutionDate = today.AddDays(-5),
                    DateResolved = today.AddDays(-6),

                    StatusHistory = new List<ServiceRequestStatusEntry>
                    {
                        new(
                            ServiceRequestStatus.Submitted,
                            "Missed collection reported.",
                            today.AddDays(-10)
                        ),

                        new(
                            ServiceRequestStatus.Assigned,
                            "The collection team was notified.",
                            today.AddDays(-8)
                        ),

                        new(
                            ServiceRequestStatus.InProgress,
                            "Collection was scheduled.",
                            today.AddDays(-7)
                        ),

                        new(
                            ServiceRequestStatus.Resolved,
                            "The waste bin was emptied.",
                            today.AddDays(-6)
                        )
                    }
                },

                // REQUEST 4
                new ServiceRequest
                {
                    ReferenceNumber = "SR-2026-0004",
                    ResidentName = "Sample Resident",
                    Location = "Ward 1, Cedar Avenue",
                    Category = "Street Lighting",
                    Description = "The streetlight outside the community hall is not working.",
                    Department = ServiceDepartment.Electricity,
                    Status = ServiceRequestStatus.Submitted,
                    Priority = ServiceRequestPriority.Normal,
                    DateSubmitted = today.AddDays(-1),
                    ExpectedResolutionDate = today.AddDays(7),

                    StatusHistory = new List<ServiceRequestStatusEntry>
                    {
                        new(
                            ServiceRequestStatus.Submitted,
                            "Streetlight fault submitted.",
                            today.AddDays(-1)
                        )
                    }
                },

                // REQUEST 5
                new ServiceRequest
                {
                    ReferenceNumber = "SR-2026-0005",
                    ResidentName = "Sample Resident",
                    Location = "Ward 5, Riverside",
                    Category = "Sanitation",
                    Description = "A blocked stormwater drain needs inspection.",
                    Department = ServiceDepartment.WaterAndSanitation,
                    Status = ServiceRequestStatus.UnderReview,
                    Priority = ServiceRequestPriority.High,
                    DateSubmitted = today.AddDays(-2),
                    ExpectedResolutionDate = today.AddDays(2),

                    StatusHistory = new List<ServiceRequestStatusEntry>
                    {
                        new(
                            ServiceRequestStatus.Submitted,
                            "Blocked drain reported.",
                            today.AddDays(-2)
                        ),

                        new(
                            ServiceRequestStatus.UnderReview,
                            "The drainage team is assessing the report.",
                            today.AddDays(-1)
                        )
                    }
                }
            };

            return data;
        }
    }
}