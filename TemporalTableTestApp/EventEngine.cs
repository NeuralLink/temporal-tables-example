namespace TemporalTableTestApp
{
    using AutoBogus;
    using Bogus;
    using Microsoft.EntityFrameworkCore;

    public class EventEngine
    {
        internal EventEngine()
        {
            this.Db = new EventContext();
            this.EventFaker = this.InitializeEventFaker();
        }

        public EventContext Db { get; set; }

        public Faker<Event> EventFaker { get; set; }

        public void CreateEvents(int numberOfEvents)
        {
            for (int i = 0; i < numberOfEvents; i++)
            {
                this.CreateEvent();
            }
        }

        public void TemporalQueryExamples()
        {
            // TemporalAll returns all historical rows for a given entity
            var temporalAll = this.Db.Events.TemporalAll().Where(x => x.Id == 1).ToList();

            // TemporalAsOf returns historical data that was current at the specified time
            var temporalAsOf = this.Db.Events.TemporalAsOf(new DateTime(2022, 4, 2)).Where(x => x.Id == 1).ToList();

            // TemporalFromTo returns all historical rows that were valid between two given times, where the upper boundary is exclusive
            var temporalFromTo = this.Db.Events.TemporalFromTo(new DateTime(2022, 3, 27), new DateTime(2022, 3, 30)).Where(x => x.Id == 1).ToList();

            // TemporalBetween same as TemporalFromTo but this time, the upper boundary is inclusive
            var temporalBetween = this.Db.Events.TemporalBetween(new DateTime(2022, 3, 27), new DateTime(2022, 3, 30)).Where(x => x.Id == 1).ToList();

            // TemporalContainedIn returns all rows that both became active, and inactive between two instants in time
            var temporalContainedIn = this.Db.Events.TemporalContainedIn(new DateTime(2022, 3, 27), new DateTime(2022, 4, 6)).Where(x => x.Id == 1).ToList();
        }

        public void UpdateRandomEvent()
        {
            var randomEvent = this.GetRandomEvent();

            this.EventFaker.RuleFor(fake => fake.UpdatingUser, fake => fake.Name.FullName());
            var person = new Person();
            randomEvent.UpdatingUser = person.FullName;
            randomEvent.UpdateDate = DateTime.UtcNow;
            randomEvent.Client = person.Company.Name;

            this.Db.SaveChanges();
        }

        private void CreateEvent()
        {
            this.EventFaker.Ignore(fake => fake.UpdatingUser);

            var fakeEvent = this.EventFaker.Generate();
            fakeEvent.CreateDate = DateTime.UtcNow;
            fakeEvent.EndDate = fakeEvent.StartDate.AddHours(4);

            try
            {
                this.Db.Events.Add(fakeEvent);
                this.Db.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                Console.Error.WriteLine(ex.InnerException?.Message);
                Console.Error.WriteLine(ex.InnerException?.StackTrace);
                Console.ReadKey();
            }
        }

        private Event GetRandomEvent()
        {
            var qry = from row in this.Db.Events
                      select row;

            int count = qry.Count(); // 1st round-trip
            int index = new Random().Next(count);

            return qry.Skip(index).FirstOrDefault(); // 2nd round-trip
        }

        private Faker<Event> InitializeEventFaker()
        {
            return new AutoFaker<Event>()
                .RuleFor(fake => fake.Address1, fake => fake.Address.StreetAddress())
                .RuleFor(fake => fake.Address2, fake => fake.Address.SecondaryAddress())
                .RuleFor(fake => fake.City, fake => fake.Address.City())
                .RuleFor(fake => fake.State, fake => fake.Address.StateAbbr())
                .RuleFor(fake => fake.Zip, fake => fake.Address.ZipCode("#####"))
                .RuleFor(fake => fake.Topic, fake => fake.Random.Words(4))
                .RuleFor(fake => fake.Client, fake => fake.Company.CompanyName())
                .RuleFor(fake => fake.CreatingUser, fake => fake.Name.FullName())
                .RuleFor(fake => fake.StartDate, fake => fake.Date.Soon())
                .Ignore(fake => fake.Id)
                .Ignore(fake => fake.UpdateDate)
                .Ignore(fake => fake.EndDate)
                .RuleSet("empty", rules =>
                {
                    rules.RuleFor(fake => fake.Id, () => 0);
                });
        }
    }
}