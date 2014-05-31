using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApolloWP.Data.Form
{
    public class ProfileForm
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public long DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string AboutMe { get; set; }
        public string Phone { get; set; }
        public double Weight { get; set; }
        public double Height { get; set; }
        public string ProfileImage { get; set; }
        public string CoverImage { get; set; }
    }
}
