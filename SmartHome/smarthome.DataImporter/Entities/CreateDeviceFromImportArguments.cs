using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smarthome.DataImporter.Entities
{
    public class CreateDeviceFromImportArguments
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ModelNumber { get; set; }
        public string Description { get; set; }
        public string MainPicture { get; set; }
        public List<string> Photographies { get; set; }
        public string Type { get; set; }
        public string? UsageType { get; set; }
        public bool? MotionDetectionEnabled { get; set; }
        public bool? PersonDetectionEnabled { get; set; }

    }
}