
using Dashboard.Entity.Business;
using PowerBI.Api.Client;

namespace PowerBI.CleanTable
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //This is the alpha version of PowerBI API Client. 
                //This version is obsolete, pls use the release one. 
                //More informations : https://github.com/Vtek/PowerBI.Api.Client
                new PowerBiApi().Authenticate().Delete<AverageData>("myDatasetId");
                new PowerBiApi().Authenticate().Delete<DeviceData>("myDatasetId");
            }
            catch
            {
                // ';..;' i'm a bad guy... 
            }
        }
    }
}
