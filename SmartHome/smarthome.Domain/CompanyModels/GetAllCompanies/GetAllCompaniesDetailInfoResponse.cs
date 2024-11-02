namespace Domain.CompanyModels
{
    public sealed record class GetAllCompaniesDetailInfoResponse
    {
        public string CompanyName { get; set; }
        public string OwnerName { get; set; }
        public string OwnerEmail { get; set; }
        public int CompanyRUT { get; set; }
    }
}