using System;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace WeatherApplicationServer.Models
{
    [Table("weather")]
    public class WeatherModel : Supabase.Postgrest.Models.BaseModel
    {
        [PrimaryKey("id", false)] // "id" is the name of your primary key column in the database
        public int Id { get; set; }

        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}
