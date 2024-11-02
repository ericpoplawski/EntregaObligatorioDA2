using CQ.Utility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using smarthome.DataImporter.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smarthome.DataImporter.ImporterFromJSON
{
    public class DeviceImportFromJSONService : IDeviceImportService
    {
        public List<CreateDeviceFromImportArguments> ImportDevice(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentException("File path cannot be null or empty", nameof(filePath));
            }

            string jsonContent;
            try
            {
                jsonContent = File.ReadAllText(filePath);
            }
            catch (Exception)
            {
                throw new ArgumentException("Error reading the file, verify that filepath is correct and has content");
            }

            IsValidJson(jsonContent);
            var deviceData = JsonConvert.DeserializeObject<DeviceDataToImport>(jsonContent);

            if (deviceData == null || deviceData.Devices == null || deviceData.Devices.Count == 0)
            {
                throw new ArgumentException("No building data found in the JSON file");
            }

            try
            {
                var devices = new List<CreateDeviceFromImportArguments>();

                foreach (var device in deviceData.Devices)
                {
                    var name = device.Nombre;
                    Guard.ThrowIsNullOrEmpty(name, nameof(name));
                    var modelNumber = device.Modelo;
                    Guard.ThrowIsNullOrEmpty(modelNumber, nameof(modelNumber));
                    var mainPicturePath = device.Fotos.FirstOrDefault(f => f.EsPrincipal)?.Path;

                    CreateDeviceFromImportArguments newDevice = null;

                    if (device.Tipo == "camera")
                    {
                        if (device.Fotos == null || !device.Fotos.Any() || string.IsNullOrEmpty(mainPicturePath))
                        {
                            throw new ArgumentException("Device must have at least one photo", nameof(device.Fotos));
                        }

                        //Faltaria una validación de interior o exterior pero en el que pasaron no ingresaba usagetype
                        //A averiguar esto, porque nuestro sistema se maneja en que usage type tiene que haber si o si.

                        newDevice = new CreateDeviceFromImportArguments
                        {
                            Id = device.Id,
                            Type = "securityCamera",
                            Name = device.Nombre,
                            ModelNumber = device.Modelo,
                            Description = "",
                            MainPicture = device.Fotos.FirstOrDefault(f => f.EsPrincipal)?.Path,
                            Photographies = device.Fotos.Select(f => f.Path).ToList(),
                            MotionDetectionEnabled = device.MovementDetection ?? false,
                            PersonDetectionEnabled = device.PersonDetection ?? false,
                            UsageType = "interior"
                        };
                    }
                    else if (device.Tipo == "sensor-open-close")
                    {
                        newDevice = new CreateDeviceFromImportArguments
                        {
                            Id = device.Id,
                            Type = "windowSensor",
                            Name = device.Nombre,
                            ModelNumber = device.Modelo,
                            Description = "",
                            MainPicture = device.Fotos.FirstOrDefault(f => f.EsPrincipal)?.Path,
                            Photographies = device.Fotos.Select(f => f.Path).ToList(),
                        };
                    }
                    else if (device.Tipo == "sensor-movement")
                    {
                        newDevice = new CreateDeviceFromImportArguments
                        {
                            Id = device.Id,
                            Type = "motionSensor",
                            Name = device.Nombre,
                            ModelNumber = device.Modelo,
                            Description = "",
                            MainPicture = device.Fotos.FirstOrDefault(f => f.EsPrincipal)?.Path,
                            Photographies = device.Fotos.Select(f => f.Path).ToList(),
                        };
                    }
                    else if (device.Tipo == "smart-lamp")
                    {
                        newDevice = new CreateDeviceFromImportArguments
                        {
                            Id = device.Id,
                            Type = "smartLamp",
                            Name = device.Nombre,
                            ModelNumber = device.Modelo,
                            Description = "",
                            MainPicture = device.Fotos.FirstOrDefault(f => f.EsPrincipal)?.Path,
                            Photographies = device.Fotos.Select(f => f.Path).ToList()
                        };
                    }

                    if (newDevice != null)
                    {
                        devices.Add(newDevice);
                    }
                }


                return devices;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("");
            }
        }

        void IsValidJson(string strInput)
        {
            var isValid = true;
            if (string.IsNullOrWhiteSpace(strInput))
            {
                isValid = false;
            }

            strInput = strInput.Trim();
            if ((strInput.StartsWith("{") && strInput.EndsWith("}")) ||
                (strInput.StartsWith("[") && strInput.EndsWith("]")))
            {
                try
                {
                    var obj = JToken.Parse(strInput);
                }
                catch (JsonReaderException)
                {
                    isValid = false;
                }
                catch (Exception)
                {
                    isValid = false;
                }
            }
            else
            {
                isValid = false;
            }
            if (isValid == false)
            {
                throw new ArgumentException("The file content is not valid JSON.");
            }
        }

        private sealed class DeviceDataToImport
        {
            public List<DeviceData> Devices { get; set; }

            public DeviceDataToImport()
            { 
                Devices = new List<DeviceData>();
            }
        }

        private sealed class DeviceData
        {
            public string Id { get; set; }
            public string Tipo { get; set; }
            public string Nombre { get; set; }
            public string Modelo { get; set; }
            public List<Foto> Fotos { get; set; }
            public bool? PersonDetection { get; set; }
            public bool? MovementDetection { get; set; }
        }

        private sealed class Foto
        {
            public string Path { get; set; }
            public bool EsPrincipal { get; set; }
        }

    }
}