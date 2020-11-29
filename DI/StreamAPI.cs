using Microsoft.Extensions.Configuration;
using Stream;
using System.IO;

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
