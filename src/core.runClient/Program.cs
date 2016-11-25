using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.Text;
using core.runClient.Extensions;

namespace core.runClient
{
    public class Program
    {
        public static void Main(string[] args)
        {

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .Build();

            var host = new WebHostBuilder()
                .UseConfiguration(config)
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();

            var dm = host.Services.GetService(typeof(phoneDevice.DeviceManage)) as phoneDevice.DeviceManage;
            var db = host.Services.GetService(typeof(DataEntities.runClientDbContext)) as DataEntities.runClientDbContext;


            var jts = from t in db.SmokeTestJobTask
                      where t.RunStatus == 0
                      select t;
            foreach (var jt in jts) {
                var CT = jt.ConventToTask();
                dm.addPublicTask(CT);
            }


            host.Run();
            
        }
    }
}
