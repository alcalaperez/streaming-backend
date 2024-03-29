﻿using Microsoft.Extensions.Configuration;
using Npgsql;
using System.IO;

namespace RecYouBackend
{
    public class DatabaseConnection: IDatabaseConnection
    {
        static IConfigurationBuilder builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appSecrets.json", optional: false, reloadOnChange: true);
        IConfigurationRoot configuration = builder.Build();
        public NpgsqlConnection GetInstance
        {
            get { return new NpgsqlConnection(configuration.GetConnectionString("DatabaseConnectionString")); }
        }
    }

    public interface IDatabaseConnection
    {
        NpgsqlConnection GetInstance { get; }
    }
}
