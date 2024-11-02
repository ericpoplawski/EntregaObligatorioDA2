using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DeviceModels.ImportDevice
{
    public class CreateDeviceImportArguments
    {
        public string Implementation { get; set; }
        public string FilePath { get; set; }

        public CreateDeviceImportArguments(string implementation, string filePath)
        {
            Implementation = implementation;
            FilePath = filePath;
        }
    }
}