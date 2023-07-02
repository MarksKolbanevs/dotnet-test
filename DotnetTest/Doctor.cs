using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DotnetTest
{
    public class Doctor
    {
        public int Id { get; set; }
        public string Name { get; set; } = String.Empty;
        public string Specialisation { get; set; } = String.Empty;
        public string PhotoName { get; set; } = String.Empty;

        [JsonIgnore]
        public ICollection<Patient>? Patients { get; set; }

        [NotMapped]
        public string PhotoBytes { get; set; }
    }
}
