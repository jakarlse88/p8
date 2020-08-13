using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CalHealth.BookingService.Infrastructure;
using CalHealth.BookingService.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Serilog;

namespace CalHealth.BookingService.Services
{
    public class ExternalPatientApiService : IExternalPatientApiService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IMemoryCache _cache;
        private readonly ExternalPatientApiOptions _options;
        
        public ExternalPatientApiService(IMemoryCache cache, IHttpClientFactory clientFactory, IOptions<ExternalPatientApiOptions> options)
        {
            _cache = cache;
            _clientFactory = clientFactory;
            _options = options.Value;
        }
        
        /// <summary>
        /// Verifies with the external PatientService API that a patient entity matching the supplied personal data
        /// exists.
        /// </summary>
        /// <param name="patient"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<bool> PatientExists(PatientDTO patient)
        {
            if (patient == null)
            {
                throw new ArgumentNullException(nameof(patient));
            }

            if (string.IsNullOrWhiteSpace(patient.FirstName))
            {
                throw new ArgumentNullException(nameof(patient.FirstName));
            }
            
            if (string.IsNullOrWhiteSpace(patient.LastName))
            {
                throw new ArgumentNullException(nameof(patient.FirstName));
            }

            var exists = false;
            
            var cacheEntry = _cache.GetOrCreate(CacheKeys.PatientEntry, entry =>
            {
                entry.SetSlidingExpiration(TimeSpan.FromSeconds(3));
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(20);
                return new List<PatientDTO>();
            });

            if (cacheEntry.Any(p => p.FirstName == patient.FirstName
                                    && p.LastName == patient.LastName
                                    && p.DateOfBirth.ToShortDateString().Equals(patient.DateOfBirth.ToShortDateString())))
            {
                exists = true;
            }

            if (!exists)
            {
                try
                {
                    using var client = _clientFactory.CreateClient();
                    
                    var response = await client.GetAsync(
                        $"{_options.Protocol}://{_options.HostName}:{_options.Port}/api/patient/exists?firstName={patient.FirstName}&lastName={patient.LastName}&dateOfBirth={patient.DateOfBirth.ToShortDateString()}");
                    
                    response.EnsureSuccessStatusCode();
                    
                    var stringContent = await response.Content.ReadAsStringAsync();
            
                    exists = JsonConvert.DeserializeObject<bool>(stringContent);
            
                    if (exists)
                    {
                        cacheEntry.Add(patient);
                    }
                }
                catch (Exception e)
                {
                    Log.Error("An error occurred while attempting to query an external service: {ex}", e);
                }    
            }
            
            return exists;
        }

        private static class CacheKeys
        {
            public static string PatientEntry => "_Patient";
        }
    }
}