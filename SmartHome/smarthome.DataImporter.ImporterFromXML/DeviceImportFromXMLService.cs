using CQ.Utility;
using smarthome.DataImporter.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace smarthome.DataImporter.ImporterFromXML
{
    public class DeviceImportFromXMLService : IDeviceImportService
    {
        public List<CreateDeviceFromImportArguments> ImportDevice(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentException("File path cannot be null or empty", nameof(filePath));
            }

            XDocument xmlDocument;
            try
            {
                xmlDocument = XDocument.Load(filePath);
            }
            catch (Exception)
            {
                throw new ArgumentException("Error reading the file, verify that filepath is correct and has content");
            }

            var devicesToImport = xmlDocument.Descendants("devices");
            //Revisar si esta bien esto

            if (devicesToImport == null || !devicesToImport.Any())
            {
                throw new ArgumentException("No building data found in the XML file");
            }

            try
            {
                var devices = new List<CreateDeviceFromImportArguments>();

                foreach (var deviceElement in devicesToImport)
                {
                    var type = deviceElement.Element("type")?.Value;
                    var arguments = new CreateDeviceFromImportArguments();

                    var photos = deviceElement.Element("photos")?.Elements("photo").ToList();
                    var mainPhotoElement = photos?.FirstOrDefault(p => (bool?)p.Element("isMain") == true);
                    var mainPicturePath = mainPhotoElement?.Element("path")?.Value;

                    var name = deviceElement.Element("name")?.Value;
                    Guard.ThrowIsNullOrEmpty(name, nameof(name));
                    var modelNumber = deviceElement.Element("model")?.Value;
                    Guard.ThrowIsNullOrEmpty(modelNumber, nameof(modelNumber));

                    if (type == "camera")
                    {
                        if (photos == null || !photos.Any() || string.IsNullOrEmpty(mainPicturePath))
                        {
                            throw new ArgumentException("No main picture found for the device.");
                        }
                        arguments = new CreateDeviceFromImportArguments
                        {
                            Name = deviceElement.Element("name")?.Value,
                            ModelNumber = deviceElement.Element("model")?.Value,
                            MainPicture = mainPicturePath,
                            Photographies = photos.Select(p => p.Element("path")?.Value).Where(p => p != null).ToList(),
                            MotionDetectionEnabled = bool.TryParse(deviceElement.Element("movementDetection")?.Value, out var motionDetection) && motionDetection,
                            PersonDetectionEnabled = bool.TryParse(deviceElement.Element("personDetection")?.Value, out var personDetection) && personDetection,
                            UsageType = deviceElement.Element("usageType")?.Value
                        };
                    }
                    else if (type == "sensor-open-close")
                    {
                        arguments = new CreateDeviceFromImportArguments
                        {
                            Name = deviceElement.Element("name")?.Value,
                            ModelNumber = deviceElement.Element("model")?.Value,
                            MainPicture = mainPicturePath,
                            Photographies = photos.Select(p => p.Element("path")?.Value).Where(p => p != null).ToList()
                        };
                    }
                    else if (type == "sensor-movement")
                    {
                        arguments = new CreateDeviceFromImportArguments
                        {
                            Name = deviceElement.Element("name")?.Value,
                            ModelNumber = deviceElement.Element("model")?.Value,
                            MainPicture = mainPicturePath,
                            Photographies = photos.Select(p => p.Element("path")?.Value).Where(p => p != null).ToList()
                        };
                    }
                    else if (type == "smartLamp")
                    {
                        arguments = new CreateDeviceFromImportArguments
                        {
                            Name = deviceElement.Element("name")?.Value,
                            ModelNumber = deviceElement.Element("model")?.Value,
                            MainPicture = mainPicturePath,
                            Photographies = photos.Select(p => p.Element("path")?.Value).Where(p => p != null).ToList()
                        };
                    }

                    devices.Add(arguments);
                }

                return devices;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Error in XML, verify that file has valid format to import");
            }
        }
    }
}

