namespace Domain.CompanyModels
{
    public class GetAllCompaniesArguments
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string CompanyName { get; set; }
        public string OwnerName { get; set; }

        public GetAllCompaniesArguments(int pageNumber, int pageSize, 
            string companyName, string ownerName)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            CompanyName = companyName;
            OwnerName = ownerName;
        }
    }
}