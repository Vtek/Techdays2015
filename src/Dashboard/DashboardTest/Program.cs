using Dashboard.Entity.Business;
using Dashboard.Entity.Storm;
using Microsoft.AspNet.SignalR.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Threading;

namespace DashboardTest
{
    class Program
    {
        private static DeviceDataStorm[] DeviceDataStorms = 
        {
            new DeviceDataStorm { Datas = "19", Device = "1", DeviceName = "Température", Unit = "°C"},
            new DeviceDataStorm { Datas = "10", Device = "1", DeviceName = "Température", Unit = "°C" },
            new DeviceDataStorm { Datas = "23", Device = "1", DeviceName = "Température", Unit = "°C" },
            new DeviceDataStorm { Datas = "7" , Device = "1", DeviceName = "Température", Unit = "°C" },
            new DeviceDataStorm { Datas = "81", Device = "2", DeviceName = "Luminosité", Unit = "lux" },
            new DeviceDataStorm { Datas = "43", Device = "2", DeviceName = "Luminosité", Unit = "lux" },
            new DeviceDataStorm { Datas = "23", Device = "2", DeviceName = "Luminosité", Unit = "lux" },
            new DeviceDataStorm { Datas = "7" , Device = "2", DeviceName = "Luminosité", Unit = "lux" },
            new DeviceDataStorm { Datas = "81", Device = "3", DeviceName = "Son", Unit = "db" },
            new DeviceDataStorm { Datas = "43", Device = "3", DeviceName = "Son", Unit = "db" },
            new DeviceDataStorm { Datas = "23", Device = "3", DeviceName = "Son", Unit = "db" },
            new DeviceDataStorm { Datas = "7" , Device = "3", DeviceName = "Son", Unit = "db" },
            new DeviceDataStorm { Datas = "81", Device = "4", DeviceName = "Humidité", Unit = "%" },
            new DeviceDataStorm { Datas = "43", Device = "4", DeviceName = "Humidité", Unit = "%" },
            new DeviceDataStorm { Datas = "23", Device = "4", DeviceName = "Humidité", Unit = "%" },
            new DeviceDataStorm { Datas = "7" , Device = "4", DeviceName = "Humidité", Unit = "%" }
        };

        private static AverageDataStorm[] AverageDataStorms = 
        {
            new AverageDataStorm { Average = 23.14, Device = "1", DeviceName = "Température" },
            new AverageDataStorm { Average = 21.42, Device = "1", DeviceName = "Température" },
            new AverageDataStorm { Average = 19.79, Device = "1", DeviceName = "Température" },
            new AverageDataStorm { Average = 25.01, Device = "1", DeviceName = "Température" }
        };

        private static AlertMessageStorm[] AlertMessage = 
        {
            new AlertMessageStorm { IsActive = true , DeviceId = "5", DeviceName = "Porte" },
            new AlertMessageStorm { IsActive = false, DeviceId = "5", DeviceName = "Porte" },
            new AlertMessageStorm { IsActive = true , DeviceId = "6", DeviceName = "Interrupteur" },
            new AlertMessageStorm { IsActive = false, DeviceId = "6", DeviceName = "Interrupteur" }
        };

        static void Main()
        {
            //Set connection
            var connection = new HubConnection("http://localhost:57422/");
            //Make proxy to hub based on hub name on server
            var myHub = connection.CreateHubProxy("DashHub");
            //Start connection

            connection.Start().ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    Console.WriteLine("There was an error opening the connection:{0}",
                                      task.Exception.GetBaseException());
                }
                else
                {
                    Console.WriteLine("Connected");
                }

            }).Wait();

            var rand = new Random();

            var i = 0;

            while (true)
            {
                var deviceData = DeviceDataStorms[rand.Next(0, DeviceDataStorms.Length)];
                deviceData.Timestamp = DateTime.Now.ToString("O");

                var json = JsonConvert.SerializeObject(deviceData,
                    new JsonSerializerSettings {ContractResolver = new CamelCasePropertyNamesContractResolver()});
                myHub.Invoke<string>("Send", json).ContinueWith(task =>
                {
                    if (task.IsFaulted)
                    {
                        Console.WriteLine("There was an error calling send: {0}",
                                          task.Exception.GetBaseException());
                    }
                    else
                    {

                    }
                });

                var alertData = AlertMessage[rand.Next(0, AlertMessage.Length)];

                var jsonAlert = JsonConvert.SerializeObject(alertData,
                    new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
                myHub.Invoke<string>("Alert", jsonAlert).ContinueWith(task =>
                {
                    if (task.IsFaulted)
                    {
                        Console.WriteLine("There was an error calling send: {0}",
                                          task.Exception.GetBaseException());
                    }
                    else
                    {

                    }
                });

                i++;
                Thread.Sleep(1000);

                if (i > 10)
                {
                    var averageData = AverageDataStorms[rand.Next(0, AverageDataStorms.Length)];
                    averageData.Timestamp = DateTime.Now.ToString("O");

                    var jsonAvg = JsonConvert.SerializeObject(averageData,
                    new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
                    myHub.Invoke<string>("Average", jsonAvg).ContinueWith(task =>
                    {
                        if (task.IsFaulted)
                        {
                            Console.WriteLine("There was an error calling send: {0}",
                                              task.Exception.GetBaseException());
                        }
                        else
                        {

                        }
                    });
                    i = 0;
                }
            }


            Console.Read();
            connection.Stop();
        }
    }
}
