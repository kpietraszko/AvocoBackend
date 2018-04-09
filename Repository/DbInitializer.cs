using AvocoBackend.Data.Models;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AvocoBackend.Repository
{
	public static class DbInitializer
	{
		public static void Seed(ApplicationDbContext context)
		{
			Console.WriteLine("Seeding");
			context.Database.EnsureCreated();
			if (context.Interests.Any())
			{
				return;
			}
			context.Interests.AddRange(new Interest { InterestName = "Filmy" },
				new Interest { InterestName = "Zwierzęta" },
				new Interest { InterestName = "Gotowanie" },
				new Interest { InterestName = "Fotografia" });
			context.SaveChanges();
		}
	}
}
