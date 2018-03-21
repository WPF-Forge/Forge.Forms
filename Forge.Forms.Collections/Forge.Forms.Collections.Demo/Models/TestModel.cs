using System.Collections.Generic;
using Bogus;

namespace Forge.Forms.Collections.Demo.Models
{
    internal class TestModel
    {
        public List<Person> People { get; } = new List<Person>();

        public TestModel()
        {
            var x = new Faker<Person>().RuleFor(i => i.FirstName, f => f.Name.FirstName())
                .RuleFor(i => i.LastName, f => f.Name.LastName())
                .RuleFor(i => i.Gender, f => f.PickRandom("Male", "Female"))
                .RuleFor(i => i.Age, f => f.Random.Int(1, 120));
            People.AddRange(x.Generate(100));
        }
    }
}