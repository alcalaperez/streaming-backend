using Microsoft.Extensions.Configuration;
using Stream;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RecYouBackend
{
    public class StreamAPI : IStreamAPI
    {
        static IConfigurationBuilder builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appSecrets.json", optional: false, reloadOnChange: true);
        IConfigurationRoot configuration = builder.Build();
        public StreamClient StreamClient
        {
            get { return new StreamClient(configuration.GetConnectionString("StreamAPIKey"), configuration.GetConnectionString("StreamAPISecret")); }
        }
    }

    public interface IStreamAPI
    {
        StreamClient StreamClient { get; }
    }
}
