using Domain;
using Domain.Exceptions;
using Domain.HomeModels;
using smarthome.BussinessLogic.Services.System;
using smarthome.Services.System.BusinessLogic;


namespace smarthome.BussinessLogic.Services.HomeServices;

public class HomeService : IHomeService
{
    private readonly IRepository<Home> _homeRepository;
    private readonly IRepository<HardwareDevice> _hardwareDeviceRepository;
    private readonly IRepository<Resident> _residentRepository;
    private readonly IRepository<HomePermission> _homePermissionRepository;
    private readonly ISystemService _systemService;
    private readonly IDeviceService _deviceService;
    private readonly IRepository<Room> _roomRepository;
    
    public HomeService(IRepository<Home> homeRepository, IRepository<HardwareDevice> hardwareDeviceRepository,
        IRepository<HomePermission> homePermissionRepository, IRepository<Resident> residentRepository,
        ISystemService systemService, IDeviceService deviceService, IRepository<Room> roomRepository)
    {
        _homeRepository = homeRepository;
        _hardwareDeviceRepository = hardwareDeviceRepository;
        _homePermissionRepository = homePermissionRepository;
        _residentRepository = residentRepository;
        _systemService = systemService;
        _deviceService = deviceService;
        _roomRepository = roomRepository;
    }
    
    public Home CreateHome(CreateHomeArguments arguments, User homeOwner)
    {
        if (arguments.QuantityOfResidents > arguments.QuantityOfResidentsAllowed)
        {
            throw new ServiceException("Quantity of residents cannot be greater than quantity of residents allowed");
        }

        User owner = _systemService.GetUserById(homeOwner.Id);

        Home home = new Home()
        {
            Address = new Address(arguments.Street, arguments.HouseNumber),
            Location = new Location(arguments.Latitude, arguments.Longitude),
            QuantityOfResidents = arguments.QuantityOfResidents,
            QuantityOfResidentsAllowed = arguments.QuantityOfResidentsAllowed,
            Owner = owner,
            OwnerId = owner.Id,
            Alias = arguments.Alias
        };
        _homeRepository.Add(home);
        return home;
    }
    
    public Resident AddUserToHome(string homeId, string homeOwnerId, AddUserToHomeArguments arguments)
    {
        Home home = _homeRepository.Get(h => h.Id == homeId, h => h.Owner);
        if (home.Owner.Id != homeOwnerId)
        {
            throw new ForbiddenException("User is not the owner of this home");
        }
        if (home.QuantityOfResidents == home.QuantityOfResidentsAllowed)
        {
            throw new ServiceException("Home is full");
        }
        
        string userId = arguments.UserId;
        User user = _systemService.GetUserById(userId);

        if (user.Roles.Any(x => x.RoleName != "HomeOwner"))
        {
            throw new ServiceException("User is not a home owner");
            //Esto habria que cambiarla por una forbidden exception
            //A Chequear, cambiar el mensaje igual
        }

        if (home.Owner.Id == user.Id)
        {
            throw new ServiceException("User is already the owner of this home");
            //Cambiar el mensaje por uno mas descriptivo
        }
        
        if (user.Residents == null)
        {
            user.Residents = new List<Resident>();
        }
        
        foreach (var residentshipDto in user.Residents)
        {
            Resident residentship = _residentRepository.Get(x => x.Id == residentshipDto.Id, x => x.Home);
            if (residentship.Home.Id == homeId)
            {
                throw new ServiceException("User is already a resident of this home");
            }
        }
        
        Resident resident = new Resident(home, new List<HomePermission>());

        _systemService.AddResidentToUser(user, resident);

        home.QuantityOfResidents++;
        _homeRepository.Update(home);
        return resident;
    }
    
    public List<HomePermission> GetUserHomePermissions(string homeId, string userId)
    {
        User user = _systemService.GetUserById(userId);
        if (user.Residents == null || user.Residents.FirstOrDefault(x => x.Home.Id == homeId) == null)
        {
            throw new ServiceException("User is not a resident of this home");
            //Esta raro que tire una excepcion aca, es un metodo de uso interno no?
        }

        List<HomePermission> permissions = _residentRepository.Get(r => r.Home.Id == homeId, r => r.HomePermissions).HomePermissions;
        return permissions;
    }
    
    public HardwareDevice AddDeviceToHome(AddDeviceToHomeArguments arguments, string userLoggedId)
    {
        Home home = _homeRepository.Get(h => h.Id == arguments.HomeId, h => h.Owner);
        if (home == null)
        {
            throw new EntityNotFoundException("Home not found");
        }
        if (home.Owner.Id != userLoggedId)
        {
            List<HomePermission> permissions = GetUserHomePermissions(home.Id, userLoggedId);
            if (permissions == null || !permissions.Any(x => x.Name == "BindDeviceToHome"))
            {
                throw new ForbiddenException("User does not have permission to add a device to this home");
            }
        }

        Room room = _roomRepository.Get(x => x.Id == arguments.RoomId);
        if (room == null)
        {
            throw new EntityNotFoundException("Room not found");
        }

        Device device = _deviceService.GetDeviceById(arguments.DeviceId);
        HardwareDevice hardwareDevice;
        hardwareDevice = new HardwareDevice(device)
        {
            Home = home,
            ConnectionState = "connected",
            Room = room
        };

        if (device.Type == "windowSensor")
        {
            hardwareDevice.OpeningState = "closed";
        }
        else if (device.Type == "smartLamp")
        {
            hardwareDevice.PowerState = "OFF";
        }
        _hardwareDeviceRepository.Add(hardwareDevice);
        return hardwareDevice;
    }
    
    public List<GetAllResidentsResponse> GetResidents(string homeId, string userLoggedId)
    {
        Home home = _homeRepository.Get(h => h.Id == homeId);
        if (home == null)
        {
            throw new EntityNotFoundException("Home not found");
        }
        if (home.OwnerId != userLoggedId)
        {
            throw new ForbiddenException("User is not the owner of this home");
        }
        List<User> users = _systemService.GetResidentsByHome(homeId) ?? new List<User>();

        List<GetAllResidentsResponse> residents =
            users.Select(x => new GetAllResidentsResponse
            {
                FullName = x.FullName,
                Email = x.Email,
                ProfilePicture = x.ProfilePicture,
                HomePermissions = x.Residents.FirstOrDefault(m => m.Home.Id == homeId).HomePermissions.ToList(),
                DoesUserMustBeNotified = x.Residents
                .FirstOrDefault(m => m.Home.Id == homeId)?
                .HomePermissions.Any(p => p.Name == PermissionKey.CanReceiveNotifications.ToString()) ?? false
            }).ToList();

        return residents;
    }

    public List<HardwareDevice> GetHardwareDevicesByHome(string homeId, string userLoggedId, string roomName = null)
    {
        Home home = _homeRepository.Get(h => h.Id == homeId);
        if (home == null)
        {
            throw new EntityNotFoundException("Home not found");
        }

        if (home.OwnerId != userLoggedId)
        {
            List<HomePermission> permissions = GetUserHomePermissions(homeId, userLoggedId);
            if (permissions == null || !permissions.Any(x => x.Name == "ListHomeDevices") ||
                home.Owner.Id != userLoggedId)
            {
                throw new ForbiddenException("User does not have permission to list home devices");
            }
        }

        if (roomName != null)
        {
            return _hardwareDeviceRepository.GetAll(x => x.Home.Id == homeId && x.Room.Name == roomName) ??
                   new List<HardwareDevice>();
        }

        return _hardwareDeviceRepository.GetAll(x => x.Home.Id == homeId) ?? new List<HardwareDevice>();
    }
    

    public List<HomePermission> ConfigureResidentsPermissions(string homeId, ConfigureResidentsHomePermissionsArguments arguments, string userLoggedId)
    {
        User user = _systemService.GetUserById(arguments.UserId);
        if (user == null)
        {
            throw new EntityNotFoundException("User not found");
        }
        
        Home home = _homeRepository.Get(h => h.Id == homeId);
        if (home == null)
        {
            throw new EntityNotFoundException("Home not found");
        }
        if (home.OwnerId != userLoggedId)
        {
            throw new ForbiddenException("User is not the owner of this home");
        }

        Resident residentDto = user.Residents.FirstOrDefault(x => x.Home.Id == homeId);
        //Aca no falta una excepcion? para pensar, estoy escribiendo estos comentarios a las 23:35
        Resident resident = _residentRepository.Get(x => x.Id == residentDto.Id, r => r.HomePermissions);

        var homePermission = _homePermissionRepository.Get(x => x.Name == arguments.HomePermission);
        resident.HomePermissions.Add(homePermission);
        _residentRepository.Update(resident);

        return resident.HomePermissions;
    }

    public HardwareDevice GetHardwareById(string hardwareDeviceId, bool includes = false)
    {
        HardwareDevice hardwareDevice;
        if (includes)
        {
            hardwareDevice = _hardwareDeviceRepository.Get(x => x.Id == hardwareDeviceId, x => x.Home, x => x.Device);
        }
        else
        {
            hardwareDevice = _hardwareDeviceRepository.Get(x => x.Id == hardwareDeviceId);
        }
        
        if (hardwareDevice == null)
        {
            throw new EntityNotFoundException("Hardware device not found");
        }
        //Este metodo no es para uso interno? no debería tirar una excepcion, no?
        
        return hardwareDevice;
    }

    public Home GetHomeById(string homeId)
    {
        Home home = _homeRepository.Get(x => x.Id == homeId);
        if (home == null)
        {
            throw new EntityNotFoundException("Home not found");
        }

        return home;
    }

    public HardwareDevice UpdateHardwareDeviceConnectionState
        (string hardwareDeviceId, UpdateHardwareDeviceConnectionStateArguments arguments)
    {
        HardwareDevice hardwareDevice = _hardwareDeviceRepository.Get(x => x.Id == hardwareDeviceId);
        ValidateUpdateConnectionStateArguments(hardwareDevice, arguments.NewConnectionState);

        hardwareDevice.ConnectionState = arguments.NewConnectionState;
        _hardwareDeviceRepository.Update(hardwareDevice);

        return hardwareDevice;
    }

    public void UpdateHardwareDevice
        (HardwareDevice hardwareDevice)
    {
        _hardwareDeviceRepository.Update(hardwareDevice);
    }

    public void ValidateUpdateConnectionStateArguments(HardwareDevice hardwareDevice, string newConnectionState)
    {
        if (hardwareDevice == null)
        {
            throw new EntityNotFoundException("Hardware device not found");
        }

        if (newConnectionState != "connected" && newConnectionState != "disconnected")
        {
            throw new ServiceException("Connection state must only be connected or disconnected");
        }

        if (hardwareDevice.ConnectionState == newConnectionState)
        {
            throw new ServiceException("New connection state must differ from actual state");
        }
    }

    public Resident GetResidentById(string residentId)
    {
        var resident = _residentRepository.Get(x => x.Id == residentId, x => x.HomePermissions);
        return resident;
    }
    
    public void ChangeHardwareDeviceName(string hardwareDeviceId, string homeId, ChangeHardwareDeviceNameArguments arguments, string userLoggedId)
    {
        Home home = _homeRepository.Get(h => h.Id == homeId, x => x.Owner);
        User user = _systemService.GetUserById(userLoggedId);
        bool isResident = user.Residents.Any(x => x.Home.Id == homeId);
        if (!isResident)
        {
            throw new ForbiddenException("User is not a resident of this home");
        }
        var residentDto = user.Residents.FirstOrDefault(x => x.Home.Id == homeId);
        Resident resident = _residentRepository.Get(x => x.Id == residentDto.Id, x => x.HomePermissions);
        if (home.Owner.Id != userLoggedId && !resident.HomePermissions.Any(x => x.Name == "ChangeHardwareDeviceName"))
        {
            throw new ForbiddenException("User does not have permission to change hardware device name");
        }

        HardwareDevice hardwareDevice = _hardwareDeviceRepository.Get(x => x.Id == hardwareDeviceId);
        hardwareDevice.Name = arguments.NewName;
        _hardwareDeviceRepository.Update(hardwareDevice);
    }
    
    public Home ChangeHomeAlias(string homeId, ChangeHomeAliasArguments arguments, User userLogged)
    {
        Home home = _homeRepository.Get(x => x.Id == homeId);
        if (home == null)
        {
            throw new EntityNotFoundException("Home not found");
        }
        
        var isResident = userLogged.Residents.Any(x => x.Home.Id == homeId);
        
        if (home.OwnerId != userLogged.Id && !isResident)
        {
            throw new ForbiddenException("User is not the owner or resident of this home");
        }

        home.Alias = arguments.Alias;
        _homeRepository.Update(home);

        return home;
    }
    
    public Room AddRoomToHome(AddRoomToHomeArguments arguments, string userLoggedId)
    {
        Home home = _homeRepository.Get(x => x.Id == arguments.HomeId);
        if (home == null)
        {
            throw new EntityNotFoundException("Home not found");
        }
        
        if (home.OwnerId != userLoggedId)
        {
            List<HomePermission> permissions = GetUserHomePermissions(home.Id, userLoggedId);
            if (permissions == null || !permissions.Any(x => x.Name == "BindRoomToHome"))
            {
                throw new ForbiddenException("User does not have permission to add a room to this home");
            }
        }
        
        if (_roomRepository.Exists(x => x.Home.Id == home.Id && x.Name == arguments.Name))
        {
            throw new ServiceException("A room with the same name already exists in this home");
        }
        
        
        Room room = new Room(arguments.Name, new List<Device>(), home); 
        
        _roomRepository.Add(room);
    
        return room;
    }

    public bool DoesUserHaveNotificationPermissionInSpecificHome(string userId, string homeId)
    {
        User user = _systemService.GetUserById(userId);
        if (user.Residents == null || user.Residents.Count == 0)
        {
            return false;
        }
        var residentDto = user.Residents.FirstOrDefault(x => x.Home.Id == homeId);
        if (residentDto == null)
        {
            return false;
        }

        var resident = _residentRepository.Get(x => x.Id == residentDto.Id, x => x.HomePermissions);
        return resident.HomePermissions.Any(x => x.Name == "DoesResidentCanReceiveNotifications");
    }

}