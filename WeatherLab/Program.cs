using System;
using System.Linq;


namespace WeatherLab
{
    class Program
    {
        static string dbfile = @".\data\climate.db";

        static void Main(string[] args)
        {
            var measurements = new WeatherSqliteContext(dbfile).Weather;

            var total_2020_precipitation = measurements.Where(n=>n.year==2020).Sum(n => n.precipitation);
            Console.WriteLine($"Total precipitation in 2020: {total_2020_precipitation} mm\n");

            //
            // Heating Degree days have a mean temp of < 18C
            //   see: https://en.wikipedia.org/wiki/Heating_degree_day
            //
 
                var HeatingDegreeDays = measurements.Where(n => n.meantemp < 18);

            //
            // Cooling degree days have a mean temp of >=18C
            //
            var CoolingDegreeDays = measurements.Where(n => n.meantemp >= 18);


            //
            // Most Variable days are the days with the biggest temperature
            // range. That is, the largest difference between the maximum and
            // minimum temperature
            //
            // Oh: and number formatting to zero pad.
            // 
            // For example, if you want:
            //      var x = 2;
            // To display as "0002" then:
            //      $"{x:d4}"
            //
            Console.WriteLine("Year\tHDD\tCDD");
            for(var yr=2016;yr<=2020;yr++)
            {
                var hdd = HeatingDegreeDays.Where(n => n.year == yr);
                var cdd = CoolingDegreeDays.Where(n => n.year == yr);
                Console.WriteLine($"{yr}\t{hdd.Count():d3}\t{cdd.Count():d3}");
            }


            Console.WriteLine("\nTop 5 Most Variable Days");
            Console.WriteLine("YYYY-MM-DD\tDelta");
            var vday = measurements.OrderByDescending(n => (n.maxtemp - n.mintemp));
            var only = 5;
            var delta = vday.Take(only).Select(n=>(n.maxtemp - n.mintemp)).ToArray(); ;
            var y = vday.Take(only).Select(n => n.year).ToArray();
            var m = vday.Take(only).Select(n => n.month).ToArray(); ;
            var d =vday.Take(only).Select(n => n.day).ToArray(); ;
            for (var i= 0; i < 5; i++)
            {
                Console.WriteLine($"{y[i]:d4}-{m[i]:d2}-{d[i]:d2}\t {delta[i]:f5} ");
            }
        }
    }
}
