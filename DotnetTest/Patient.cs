using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DotnetTest
{
    public class Patient
    {
        public int Id { get; set; }
        public string Name { get; set; } = String.Empty;
        public string Surname { get; set; } = String.Empty;
        public string Email { get; set; } = String.Empty;
        public string Phone { get; set; } = String.Empty;
        public string StreetAdress { get; set; } = String.Empty;
        public string City { get; set; } = String.Empty;
        public string Region { get; set; } = String.Empty;
        public DateTime DateTime { get; set; }


        public int DoctorId { get; set; }
        public Doctor? Doctor { get; set; }
    }
}
