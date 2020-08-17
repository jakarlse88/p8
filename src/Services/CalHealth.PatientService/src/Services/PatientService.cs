using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CalHealth.Messages;
using CalHealth.PatientService.Messaging.Interfaces;
using CalHealth.PatientService.Models;
using CalHealth.PatientService.Repositories;
using Serilog;

namespace CalHealth.PatientService.Services
{
    public class PatientService : IPatientService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPatientPublisher _patientPublisher;
        private readonly IMapper _mapper;

        public PatientService(IUnitOfWork unitOfWork, IPatientPublisher patientPublisher, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _patientPublisher = patientPublisher;
            _mapper = mapper;
        }

        /// <summary>
        /// Handles an incoming <see cref="AppointmentMessage"/> by querying for the <seealso cref="Patient"/>
        /// entity matching the supplied personal details. On success, emits a <seealso cref="PatientMessage"/> containing the
        /// received AppointmentId as well as the PatientId of the found entity.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="Exception"></exception>
        public async Task HandleIncomingPatientData(AppointmentMessage message)
        {
            if (message == null
                || string.IsNullOrWhiteSpace(message.FirstName)
                || string.IsNullOrWhiteSpace(message.LastName))
            {
                throw new ArgumentNullException();
            }

            try
            {
                var patient = await GetPatientByPersonalDetailsAsync(message);

                if (patient == null)
                {
                    throw new Exception($"No {typeof(Patient)} entity matching the specified criteria exists.");
                }

                _patientPublisher.PushMessageToQueue(new PatientMessage
                {
                    AppointmentId = message.AppointmentId,
                    PatientId = patient.Id
                });
            }
            catch (Exception e)
            {
                Log.Error("An exception was raised: {@e}", e);
                throw;
            }
        }

        /// <summary>
        /// Asynchronously get all <see cref="Patient"/> entities as <seealso cref="PatientDTO"/> DTOs.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<PatientDTO>> GetAllAsync()
        {
            var results = 
                await _unitOfWork.PatientRepository.GetAllAsync();

            var mappedResults = _mapper.Map<IEnumerable<PatientDTO>>(results);

            return mappedResults;
        }

        /**
         *
         * Internal helper methods
         * 
         */
        private async Task<Patient> GetPatientByPersonalDetailsAsync(AppointmentMessage message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            if (string.IsNullOrWhiteSpace(message.FirstName))
            {
                throw new ArgumentNullException(nameof(message.FirstName));
            }

            if (string.IsNullOrWhiteSpace(message.LastName))
            {
                throw new ArgumentNullException(nameof(message.LastName));
            }

            var result =
                await _unitOfWork
                    .PatientRepository
                    .GetByConditionAsync(p =>
                        p.LastName.Contains(message.LastName)
                        && p.FirstName.Contains(message.FirstName));

            var patient = result.FirstOrDefault(p =>p.DateOfBirth.Date == message.DateOfBirth.Date);

            return patient;
        }

        public void Dispose()
        {
        }
    }
}