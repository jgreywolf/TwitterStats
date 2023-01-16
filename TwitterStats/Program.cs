using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using TwitterStats.Scribes;

namespace TwitterStats
{
    internal class Program
    {
        private static readonly TwitterStatisticsService StatisticsService = TwitterStatisticsService.Instance;
        public static TwitterApiSettings ApiSettings { get; private set; }

        static void Main(string[] args)
        {
            ApiSettings = GetCredentials();
            CancellationTokenSource cts = new CancellationTokenSource();
            TwitterCourier courier = new TwitterCourier(ApiSettings);
            var task = new Task(() => courier.StartSampleStream(cts));
            task.Start();

            StatisticsService.StartService(new TextFileScribe(), cts);

            Console.ReadKey();
        }

        public static TwitterApiSettings GetCredentials()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false);

            IConfiguration config = builder.Build();

            return config.GetSection("TwitterApiSettings").Get<TwitterApiSettings>();
        }
    }
}
