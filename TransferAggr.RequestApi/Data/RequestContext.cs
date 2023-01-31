using CsvHelper.Configuration;
using CsvHelper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TransferAggr.RequestApi.Models;
using Microsoft.AspNetCore.Hosting;

namespace TransferAggr.RequestApi.Data
{
    public class RequestContext : DbContext
    {
        private readonly IWebHostEnvironment _env;

        public RequestContext(DbContextOptions<RequestContext> options, IWebHostEnvironment env) : base(options)
        {
            _env = env;
        }
        public DbSet<Request> RequestItems { get; set; }
        public DbSet<Place> PlaceItem { get; set; }

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
