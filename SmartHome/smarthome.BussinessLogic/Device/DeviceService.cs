using Domain.Exceptions;
using Domain;
using Domain.DeviceModels;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Domain.DeviceModels.ImportDevice;
using smarthome.DataImporter.Entities;
using smarthome.DataImporter;
using Domain.DeviceModels.RegisterSmartLamp;



namespace smarthome.BussinessLogic.Services.System;

public class DeviceService : IDeviceService
{
    private readonly IRepository<Device> _deviceRepository;
    private ICompanyService _companyService;
    private ILoadAssembly<IDeviceImportService> _loadAssembly;
    private string path;

    public DeviceService(IRepository<Device> deviceRepository, ICompanyService companyService,
        ILoadAssembly<IDeviceImportService> loadAssembly)
    {
        _deviceRepository = deviceRepository;
        _companyService = companyService;
        _loadAssembly = loadAssembly;
        path = AppDomain.CurrentDomain.BaseDirectory + "Assemblies";
    }
    
    public Device RegisterSecurityCamera(RegisterSecurityCameraArguments arguments, User userLogged)
    {
        if(_deviceRepository.Exists(x => x.Name == arguments.Name))
        {
            throw new ServiceException("There is already a device with this name");
        }
        if(_deviceRepository.Exists(x => x.ModelNumber == arguments.ModelNumber))
        {
            throw new ServiceException("There is already a device with this model number");
        }
        if(arguments.UsageType != "interior".ToLower() && arguments.UsageType != "exterior".ToLower())
        {
            throw new ServiceException("Usage type must be interior or exterior");
        }

        var company = _companyService.GetCompanyByUserId(userLogged.Id);
        if (company == null)
        {
            throw new ServiceException("Users with no company associated, cannot register a device");
        }

        var device = new Device()
        {
            Name = arguments.Name,
            ModelNumber = arguments.ModelNumber,
            Description = arguments.Description,
            MainPicture = arguments.MainPicture,
            Photographies = arguments.Photographies,
            Type = "securityCamera",
            UsageType = arguments.UsageType,
            MotionDetectionEnabled = arguments.MotionDetectionEnabled,
            PersonDetectionEnabled = arguments.PersonDetectionEnabled,
            Company = company
        };
        _deviceRepository.Add(device);
        return device;
    }
    
    public Device RegisterWindowSensor(RegisterWindowSensorArguments arguments, User userLogged)
    {
        if(_deviceRepository.Exists(x => x.Name == arguments.Name))
        {
            throw new ServiceException("There is already a device with this name");
        }
        if(_deviceRepository.Exists(x => x.ModelNumber == arguments.ModelNumber))
        {
            throw new ServiceException("There is already a device with this model number");
        }

        var company = _companyService.GetCompanyByUserId(userLogged.Id);
        if (company == null)
        {
            throw new ServiceException("Users with no company associated, cannot register a device");
        }

        var device = new Device()
        {
            Name = arguments.Name,
            ModelNumber = arguments.ModelNumber,
            Description = arguments.Description,
            Photographies = arguments.Photographies,
            Type = "windowSensor",
            Company = company,
            CompanyId = company.Id,
            MainPicture = arguments.MainPicture,
        };
        _deviceRepository.Add(device);
        return device;
    }

    public Device RegisterMotionSensor(RegisterMotionSensorArguments arguments, User userLogged)
    {
        if (_deviceRepository.Exists(x => x.Name == arguments.Name))
        {
            throw new ServiceException("There is already a device with this name");
        }

        if (_deviceRepository.Exists(x => x.ModelNumber == arguments.ModelNumber))
        {
            throw new ServiceException("There is already a device with this model number");
        }

        var company = _companyService.GetCompanyByUserId(userLogged.Id);
        if (company == null)
        {
            throw new ServiceException("Users with no company associated, cannot register a device");
        }

        var device = new Device()
        {
            Name = arguments.Name,
            ModelNumber = arguments.ModelNumber,
            Description = arguments.Description,
            Photographies = arguments.Photographies,
            Type = "motionSensor",
            Company = company,
            CompanyId = company.Id,
            MainPicture = arguments.MainPicture,
        };
        _deviceRepository.Add(device);
        return device;
    }
    
    public Device RegisterSmartLamp(RegisterSmartLampArguments arguments, User userLogged)
    {
        if (_deviceRepository.Exists(x => x.Name == arguments.Name))
        {
            throw new ServiceException("There is already a device with this name");
        }
        if (_deviceRepository.Exists(x => x.ModelNumber == arguments.ModelNumber))
        {
            throw new ServiceException("There is already a device with this model number");
        }
        var company = _companyService.GetCompanyByUserId(userLogged.Id);
        if (company == null)
        {
            throw new ServiceException("Users with no company associated, cannot register a device");
        } 
        var device = new Device()
        {
            Name = arguments.Name,
            ModelNumber = arguments.ModelNumber,
            Description = arguments.Description,
            Photographies = arguments.Photographies,
            Type = "smartLamp",
            Company = company,
            CompanyId = company.Id,
            MainPicture = arguments.MainPicture,
        };
        _deviceRepository.Add(device);
        return device;
    }

    public List<Device> GetDevices(GetAllDevicesArguments arguments)
    {
        if (string.IsNullOrEmpty(arguments.DeviceName) && string.IsNullOrEmpty(arguments.ModelNumber)
            && string.IsNullOrEmpty(arguments.CompanyName) && string.IsNullOrEmpty(arguments.DeviceType))
        {
            return _deviceRepository.GetAllWithPagination(arguments.PageNumber, arguments.PageSize);
        }
        if (string.IsNullOrEmpty(arguments.DeviceName) && string.IsNullOrEmpty(arguments.ModelNumber)
            && string.IsNullOrEmpty(arguments.CompanyName))
        {
            return _deviceRepository.GetAllWithPagination(arguments.PageNumber, arguments.PageSize, x => x.MainPicture == arguments.DeviceType);
        } 
        if (string.IsNullOrEmpty(arguments.ModelNumber) && string.IsNullOrEmpty(arguments.CompanyName)
                                                          && string.IsNullOrEmpty(arguments.DeviceType))
        {
            return _deviceRepository.GetAllWithPagination(arguments.PageNumber, arguments.PageSize, x => x.Name == arguments.DeviceName);
        }
        if (string.IsNullOrEmpty(arguments.DeviceName) && string.IsNullOrEmpty(arguments.CompanyName)
            && string.IsNullOrEmpty(arguments.DeviceType))
        {
            return _deviceRepository.GetAllWithPagination(arguments.PageNumber, arguments.PageSize, x => x.ModelNumber == arguments.ModelNumber);
        }
        if (string.IsNullOrEmpty(arguments.DeviceName) && string.IsNullOrEmpty(arguments.ModelNumber)
            && string.IsNullOrEmpty(arguments.DeviceType))
        {
            return _deviceRepository.GetAllWithPagination(arguments.PageNumber, arguments.PageSize, x => x.Company.Name == arguments.CompanyName);
        }
        if (string.IsNullOrEmpty(arguments.ModelNumber) && string.IsNullOrEmpty(arguments.CompanyName))
        {
            return _deviceRepository.GetAllWithPagination(arguments.PageNumber, arguments.PageSize, x => x.Name == arguments.DeviceName && x.MainPicture == arguments.DeviceType);
        }
        if (string.IsNullOrEmpty(arguments.DeviceName) && string.IsNullOrEmpty(arguments.CompanyName))
        {
            return _deviceRepository.GetAllWithPagination(arguments.PageNumber, arguments.PageSize, x => x.ModelNumber == arguments.ModelNumber && x.MainPicture == arguments.DeviceType);
        }
        if (string.IsNullOrEmpty(arguments.DeviceName) && string.IsNullOrEmpty(arguments.ModelNumber))
        {
            return _deviceRepository.GetAllWithPagination(arguments.PageNumber, arguments.PageSize, x => x.Company.Name == arguments.CompanyName && x.MainPicture == arguments.DeviceType);
        }
        if (string.IsNullOrEmpty(arguments.DeviceName) && string.IsNullOrEmpty(arguments.DeviceType))
        {
            return _deviceRepository.GetAllWithPagination(arguments.PageNumber, arguments.PageSize, x => x.ModelNumber == arguments.ModelNumber && x.Company.Name == arguments.CompanyName);
        }
        if (string.IsNullOrEmpty(arguments.ModelNumber) && string.IsNullOrEmpty(arguments.DeviceType))
        {
            return _deviceRepository.GetAllWithPagination(arguments.PageNumber, arguments.PageSize, x => x.Name == arguments.DeviceName && x.Company.Name == arguments.CompanyName);
        }
        if (string.IsNullOrEmpty(arguments.DeviceName))
        {
            return _deviceRepository.GetAllWithPagination(arguments.PageNumber, arguments.PageSize, x => x.ModelNumber == arguments.ModelNumber && x.Company.Name == arguments.CompanyName && x.MainPicture == arguments.DeviceType);
        }
        if (string.IsNullOrEmpty(arguments.ModelNumber))
        {
            return _deviceRepository.GetAllWithPagination(arguments.PageNumber, arguments.PageSize, x => x.Name == arguments.DeviceName && x.Company.Name == arguments.CompanyName && x.MainPicture == arguments.DeviceType);
        }
        if (string.IsNullOrEmpty(arguments.CompanyName))
        {
            return _deviceRepository.GetAllWithPagination(arguments.PageNumber, arguments.PageSize, x => x.Name == arguments.DeviceName && x.ModelNumber == arguments.ModelNumber && x.MainPicture == arguments.DeviceType);
        }
        return _deviceRepository.GetAllWithPagination(arguments.PageNumber, arguments.PageSize, x => x.Name == arguments.DeviceName && x.ModelNumber == arguments.ModelNumber && x.Company.Name == arguments.CompanyName);
    }

    public List<string> GetSupportedTypes()
    {
        return _deviceRepository.GetAll()
                                .Select(device => device.Type)
                                .Distinct()
                                .ToList();
    }

    public Device GetDeviceById(string id)
    {
        Device deviceFound = _deviceRepository.Get(x => x.Id == id);
        if (deviceFound == null)
        {
            throw new EntityNotFoundException("Device not found");
        }
        return deviceFound;
    }

    public List<string> GetDeviceImportImplementations()
    {
        return _loadAssembly.GetImplementations(path);
    }

    public List<Device> Import(CreateDeviceImportArguments arguments, User userLogged)
    {
        var company = _companyService.GetCompanyByUserId(userLogged.Id);
        if (company == null)
        {
            throw new ServiceException("Users with no company associated, cannot import a device");
        }

        IDeviceImportService implementationSelected = _loadAssembly.GetImplementation(arguments.Implementation, path);
        List<CreateDeviceFromImportArguments> argumentsImported = implementationSelected.ImportDevice(arguments.FilePath);
        List<Device> devicesToSave = new List<Device>();

        foreach (var args in argumentsImported)
        {
            bool existsName = _deviceRepository.Exists(x => x.Name == args.Name);
            if (existsName)
            {
                throw new ServiceException($"There is already a device with this name '{args.Name}'");
            }

            bool existsModelNumber = _deviceRepository.Exists(x => x.ModelNumber == args.ModelNumber);
            if (existsModelNumber)
            {
                throw new ServiceException($"There is already a device with this model number '{args.ModelNumber}'");
            }

            Device device = new Device()
            {
                Id = args.Id,
                Name = args.Name,
                ModelNumber = args.ModelNumber,
                Description = args.Description,
                MainPicture = args.MainPicture,
                Photographies = args.Photographies,
                Type = args.Type,
                UsageType = args.UsageType,
                MotionDetectionEnabled = args.MotionDetectionEnabled,
                PersonDetectionEnabled = args.PersonDetectionEnabled,
                Company = company
            };
            devicesToSave.Add(device);
        }

        foreach(var device in devicesToSave)
        {
            _deviceRepository.Add(device);
        }

        return devicesToSave;
    }
}