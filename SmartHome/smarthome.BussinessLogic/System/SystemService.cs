using smarthome.Services.System.BusinessLogic;
using CQ.Utility;
using Domain;
using Domain.Exceptions;
using Domain.UserModels;
using smarthome.BussinessLogic.Services.HomeServices;

namespace smarthome.BussinessLogic.Services.System
{
    public class SystemService : ISystemService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Role> _roleRepository;

        public SystemService(IRepository<User> userRepository, 
            IRepository<Role> roleRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        public User AddAdministrator(CreateUserArguments arguments)
        {
            Guard.ThrowIsInvalidEmailFormat(arguments.Email);
            Guard.ThrowIsInvalidPasswordFormat(arguments.Password);

            if (_userRepository.Exists(x => x.Email == arguments.Email))
            {
                throw new ServiceException("There is already a user with this email");
            }

            Role administratorRole = _roleRepository.Get(x => x.RoleName == "Administrator");

            var user = new User
            {
                FirstName = arguments.FirstName,
                LastName = arguments.LastName,
                FullName = arguments.FirstName + " " + arguments.LastName,
                Email = arguments.Email,
                Password = arguments.Password,
                Roles = new List<Role>()
                {
                    administratorRole
                },
                CreationDate = DateTime.Now,
                IsComplete = false
            };

            _userRepository.Add(user);

            return user;
        }
        
        public User AddCompanyOwner(CreateUserArguments arguments)
        {
            Guard.ThrowIsInvalidEmailFormat(arguments.Email);
            Guard.ThrowIsInvalidPasswordFormat(arguments.Password);
            
            if (_userRepository.Exists(x => x.Email == arguments.Email))
            {
                throw new ServiceException("There is already a user with this email");
            }

            Role companyOwnerRole = _roleRepository.Get(x => x.RoleName == "CompanyOwner");

            var user = new User
            {
                FirstName = arguments.FirstName,
                LastName = arguments.LastName,
                FullName = arguments.FirstName + " " + arguments.LastName,
                Email = arguments.Email,
                Password = arguments.Password,
                Roles = new List<Role>()
                {
                    companyOwnerRole
                },
                CreationDate = DateTime.Now,
                IsComplete = false
            };
            
            _userRepository.Add(user);
            
            return user;
        }
        
        public User AddHomeOwner(CreateHomeOwnerArguments arguments)
        {
            Guard.ThrowIsInvalidEmailFormat(arguments.Email);
            Guard.ThrowIsInvalidPasswordFormat(arguments.Password);
            
            if (_userRepository.Exists(x => x.Email == arguments.Email))
            {
                throw new ServiceException("There is already a user with this email");
            }
            
            if (!Uri.TryCreate(arguments.ProfilePicture, UriKind.Absolute, out var uriResult)
                || (uriResult.Scheme != Uri.UriSchemeHttp && uriResult.Scheme != Uri.UriSchemeHttps))
            {
                throw new ArgumentException("Profile picture must be a valid URL", nameof(arguments.ProfilePicture));
            }

            Role HomeOwnerRole = _roleRepository.Get(x => x.RoleName == "HomeOwner");

            var user = new User
            {
                FirstName = arguments.FirstName,
                LastName = arguments.LastName,
                FullName = arguments.FirstName + " " + arguments.LastName,
                Email = arguments.Email,
                Password = arguments.Password,
                ProfilePicture = arguments.ProfilePicture,
                Roles = new List<Role>()
                {
                    HomeOwnerRole
                },
                CreationDate = DateTime.Now,
                IsComplete = false
            };
            
            _userRepository.Add(user);
            
            return user;
        }

        public List<User> GetUsers(GetAllUsersArguments arguments)
        {
            if (string.IsNullOrEmpty(arguments.FullName) && string.IsNullOrEmpty(arguments.RoleName))
            {
                return _userRepository.GetAllWithPagination(arguments.PageNumber, arguments.PageSize, null, 
                    u => u.Roles);
            }
            if (!string.IsNullOrEmpty(arguments.FullName) && string.IsNullOrEmpty(arguments.RoleName))
            {
                return _userRepository.GetAllWithPagination(arguments.PageNumber, arguments.PageSize,
                    x => x.FullName == arguments.FullName, u => u.Roles);
            }
            if (string.IsNullOrEmpty(arguments.FullName) && !string.IsNullOrEmpty(arguments.RoleName))
            {
                return _userRepository.GetAllWithPagination(arguments.PageNumber, arguments.PageSize, 
                    x => x.Roles.Any(r => r.RoleName == arguments.RoleName), u => u.Roles);
            }
            return _userRepository.GetAllWithPagination(arguments.PageNumber, arguments.PageSize,
                x => x.FullName == arguments.FullName && x.Roles.Any(r => r.RoleName == arguments.RoleName),
                u => u.Roles);
        }

        public User GetUserByEmail(string email)
        {
            return _userRepository.Get(x => x.Email == email);
        }

        public User UpdateIfUserIsComplete(User userReceived)
        {
            User user = _userRepository.Get(x => x.Id == userReceived.Id);
            user.IsComplete = true;
            _userRepository.Update(user);
            return user;
        }

        public void DeleteAdministratorById(string id)
        {
            var user = _userRepository.Get(x => x.Id == id, x => x.Roles);
            if (user == null) { throw new EntityNotFoundException("User not found"); }

            if (!user.Roles.Any(r => r.RoleName == "Administrator"))
            {
                throw new ServiceException("Only users with role 'Administrator' can be deleted");
            }

            _userRepository.Remove(user);
        }

        public List<User> GetResidentsByHome(string homeId)
        {
            return _userRepository.GetAll(x => x.Residents.Any(m => m.Home.Id == homeId));
        }
        
        public User GetUserById(string userId)
        {
            User user = _userRepository.Get(
                x => x.Id == userId, 
                x => x.Roles,
                x => x.Residents);
            List<Role> roles = new List<Role>();
            foreach (var role in user.Roles)
            {
                roles.Add(_roleRepository.Get(x => x.Id == role.Id, x => x.SystemPermissions));
            }
            user.Roles = roles;
            return user;
        }

        public void AddResidentToUser(User user, Resident resident)
        {
            user.Residents.Add(resident);
            _userRepository.Update(user);
        }

        public void AddHomeOwnerRoleToUser(string userId)
        {
            var user = _userRepository.Get(x => x.Id == userId, x => x.Roles);
            if (user == null) { throw new EntityNotFoundException("User not found"); }

            if (user.Roles.Any(r => r.RoleName == "HomeOwner"))
            {
                throw new ServiceException("User already has role 'HomeOwner'");
            }

            Role homeOwnerRole = _roleRepository.Get(x => x.RoleName == "HomeOwner");
            user.Roles.Add(homeOwnerRole);
            _userRepository.Update(user);
        }
    }
}
