namespace MunicipalServicesApp.Models
{
    /// <summary>
    /// Represents the current stage of a municipal service request.
    /// </summary>
    public enum ServiceRequestStatus
    {
        Submitted,
        UnderReview,
        Assigned,
        InProgress,
        AwaitingMaterials,
        Resolved,
        Closed,
        Rejected
    }
}