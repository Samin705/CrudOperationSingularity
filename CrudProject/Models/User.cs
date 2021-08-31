using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrudProject.Models
{
    public class User
    {
		public int Id { get; set; }
		public string Name { get; set; }
        public string Password { get; set; }
        public int Age { get; set; }
		public string City { get; set; }
		public string Country { get; set; }
		public int ContactNumber { get; set; }
	}
}
