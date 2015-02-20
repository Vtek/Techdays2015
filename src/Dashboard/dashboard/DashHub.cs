using System.Threading;
using Dashboard.Entity.Business;
using Dashboard.Entity.Storm;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PowerBI.Api.Client;
using System;
using System.Threading.Tasks;

namespace dashboard
{
    public class DashHub : Hub
    {
        public DashHub()
        {

        }

        public void Information(string message)
        {
            Clients.All.broadcastInformation(message);
        }

        public void Send(string message)
        {
            //Clients.All.broadcastDeviceDataJson(message);

            var data = JsonConvert.DeserializeObject<DeviceDataStorm>(message,new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });

            var typedData = new DeviceData
            {
                DeviceId = data.Device,
                Data = Convert.ToDouble(data.Datas),
                Date = Convert.ToDateTime(data.Timestamp),
                DeviceName = data.DeviceName,
                DeviceUnit = data.Unit
            };

            Clients.All.broadcastDeviceData(JsonConvert.SerializeObject(typedData, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            }));
        }

        public void Average(string message)
        {
            Clients.All.broadcastAverageJson(message);

            var data = JsonConvert.DeserializeObject<AverageDataStorm>(message, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });

            var typedData = new AverageData
            {
                DeviceId = data.Device,
                Average = Convert.ToDouble(data.Average),
                Date = Convert.ToDateTime(data.Timestamp),
                DeviceName = data.DeviceName
            };

            Clients.All.broadcastAverageData(JsonConvert.SerializeObject(typedData, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            }));

            var task = Task.Run(() =>
            {
                try
                {
                    //This is the alpha version of PowerBI API Client. 
                    //This version is obsolete, pls use the release one. 
                    //More informations : https://github.com/Vtek/PowerBI.Api.Client
                    new PowerBiApi().Authenticate().Send("MyDatasetId", typedData);
                }
                catch
                {
                    // ';..;' i'm a bad guy... 
                }
            });
        }

        public void Alert(string message)
        {
            //Clients.All.broadcastAlertJson(message);

            var data = JsonConvert.DeserializeObject<AlertMessageStorm>(message, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });

            var typedData = new AlertMessage()
            {
                DeviceId = data.DeviceId,
                DeviceName = data.DeviceName,
                IsActive = data.IsActive
            };

            Clients.All.broadcastAlert(JsonConvert.SerializeObject(typedData, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            }));
        }
    }
}