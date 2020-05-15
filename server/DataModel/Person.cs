using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;

namespace server
{
    [Serializable]
    public class Person
    {
        [NonSerialized]
        private int id;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public string Name { get; set; }
        public string Number { get; set; }
        public Person() { }
        public Person(string name, string number)
        {
            Name = name;
            Number = number;
        }
    }
}
