using FitnessClubWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessClubWeb.Data
{
    public static class DbInitializer
    {
        public static void Initialize(FitnessClubContext context)
        {
            context.Database.EnsureCreated();
            if (context.Clients.Any())
            {
                return;
            }
            var clients = new Client[]
            {
                new Client()
                {
                    ID = 1,
                    BirthDay = new DateTime(1998, 10, 9),
                    FIO = "Богатова",
                    Subscription = new Subscription()
                    {

                    }
                }
            };
        }
    }
}