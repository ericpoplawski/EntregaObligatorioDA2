using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DeviceModels.ImportDevice
{
    public sealed record class CreateDeviceImportResponse
    {
        public List<string> Ids { get; set; }

        public CreateDeviceImportResponse(List<Device> devices)
        {
            Ids = new List<string>();
            foreach (var device in devices)
            {
                Ids.Add(device.Id);
            }
        }
    }
}