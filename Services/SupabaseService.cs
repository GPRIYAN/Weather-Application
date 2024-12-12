using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeatherApplicationServer.Models;
using Supabase;
using static Supabase.Postgrest.Constants;

namespace WeatherApplicationServer.Services
{
    public class SupabaseService
    {
        private readonly Client _client;

        public SupabaseService(Client client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task InitializeAsync()
        {
            await _client.InitializeAsync();
        }

        public async Task<List<WeatherModel>> GetWeatherDataAsync()
        {
            var response = await _client.From<WeatherModel>().Get();
            return response.Models;
        }

        public async Task CreateAsync(WeatherModel weatherModel)
        {
            await _client.From<WeatherModel>().Insert(weatherModel);
        }

        public async Task UpdateAsync(WeatherModel weatherModel)
        {
            await _client.From<WeatherModel>().Update(weatherModel);
        }

        public async Task DeleteAsync(int id)
        {
            
            await _client
                .From<WeatherModel>()
                .Filter("Id", Operator.Equals, id)
                .Delete();
            
        }

    }
}
