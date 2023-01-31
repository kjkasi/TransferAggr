using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;
using TransferAggr.GeocodeApi.Models;

namespace TransferAggr.GeocodeApi.Data
{
    public class PlaceContext: DbContext
    {
        private readonly IWebHostEnvironment _env;

        public PlaceContext(DbContextOptions<PlaceContext> options, IWebHostEnvironment env) : base(options)
        {
            _env = env;
        }
        public DbSet<Place> Items { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            string csvFilePlaces = Path.Combine(_env.ContentRootPath, "Data", "ru-msk.csv");

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                BadDataFound = args =>
                {
                    Console.WriteLine($"field: {args.Field}");
                    Console.WriteLine($"context: {args.Context}");
                },
            };

            using (var reader = new StreamReader(csvFilePlaces))
            using (var csv = new CsvReader(reader, config))
            {
                var records = csv.GetRecords<Place>();
                modelBuilder.Entity<Place>().HasData(records);
            }
        }
    }
}
