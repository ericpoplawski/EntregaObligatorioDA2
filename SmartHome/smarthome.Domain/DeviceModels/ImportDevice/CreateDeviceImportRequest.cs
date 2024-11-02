using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DeviceModels.ImportDevice
{
    public sealed record class CreateDeviceImportRequest
    {
        public string Implementation { get; set; }
        public string FilePath { get; set; }
    }
}