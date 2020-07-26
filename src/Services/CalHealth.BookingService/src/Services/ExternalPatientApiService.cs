using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CalHealth.BookingService.Models;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace CalHealth.BookingService.Services
{
    public class ExternalPatientApiService : IExternalPatientApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _cache;
        
        public ExternalPatientApiService(HttpClient httpClient, IMemoryCache cache)
        {
            _httpClient = httpClient;
            _cache = cache;
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
                return true;
            }

            var response = await _httpClient.GetAsync(
                $"api/patient/exists?firstName={patient.FirstName}&lastName={patient.LastName}&dateOfBirth={patient.DateOfBirth.ToShortDateString()}");

            var stringContent = await response.Content.ReadAsStringAsync();
            
            var exists = JsonConvert.DeserializeObject<bool>(stringContent);
            
            if (!exists)
            {
                return false;
            }
            
            cacheEntry.Add(patient);
            return true;
        }

        private static class CacheKeys
        {
            public static string PatientEntry => "_Patient";
        }
    }
}