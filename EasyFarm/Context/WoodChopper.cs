using System.Collections.Generic;
using Pathfinder.People;

namespace EasyFarm.Context
{
    public class WoodChopper
    {
        public Queue<Person> LoggingPoints { get; set; } = new Queue<Person>();
        public Person NextPoint { get; set; }
    }
}