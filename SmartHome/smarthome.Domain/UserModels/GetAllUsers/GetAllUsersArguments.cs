namespace Domain.UserModels
{
    public class GetAllUsersArguments
    {
        public string FullName { get; set; }
        public string RoleName { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public GetAllUsersArguments(string fullName, string roleName, int pageNumber, int pageSize)
        {
            FullName = fullName;
            RoleName = roleName;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}
