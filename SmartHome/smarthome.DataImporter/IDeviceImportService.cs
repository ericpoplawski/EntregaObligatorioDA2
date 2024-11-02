using smarthome.DataImporter.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smarthome.DataImporter
{
    public interface IDeviceImportService
    {
        List<CreateDeviceFromImportArguments> ImportDevice(string filePath);
    }
}
