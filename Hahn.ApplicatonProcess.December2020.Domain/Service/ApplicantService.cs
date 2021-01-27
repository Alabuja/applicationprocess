using FluentValidation;
using Hahn.ApplicatonProcess.December2020.Data.Data;
using Hahn.ApplicatonProcess.December2020.Domain.Models;
using Hahn.ApplicatonProcess.December2020.Domain.Models.InputModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hahn.ApplicatonProcess.December2020.Domain.Service
{
    public class ApplicantService
    {
        private readonly ApplicationProcessDbContext _context;
        private readonly ILogger<ApplicantService> _logger;

        public ApplicantService(ApplicationProcessDbContext context, ILogger<ApplicantService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Applicant> CreateApplicant(ApplicantInputModel model)
        {
            var applicant = new Applicant
            {
                Name = model.Name,
                FamilyName = model.FamilyName,
                Address = model.Address,
                EmailAddress = model.EmailAddress,
                CountryOfOrigin = model.CountryOfOrigin,
                Age = model.Age,
                Hired = model.Hired
            };

            await _context.Applicants.AddAsync(applicant);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Applicant Details is {applicant}");
            return applicant;
        }

        public async Task<List<Applicant>> GetApplicants()
        {
            var query = await _context.Applicants
                            .Where(g => g.Name != null).ToListAsync();

            return query;
        }

        public async Task<Applicant> GetApplicant(int id)
        {
            var query = await _context.Applicants.FirstOrDefaultAsync(p => p.Id == id);

            if (query == null)
            {
                _logger.LogInformation($"Applicant with Id of {id} not found");
                throw new KeyNotFoundException("Applicant not found");
            }

            return query;
        }

        public async Task<Applicant> UpdateApplicant(ApplicantInputModel model, int id)
        {
            var applicant = await GetApplicant(id);

            applicant.Name = model.Name;
            applicant.FamilyName = model.FamilyName;
            applicant.Address = model.Address;
            applicant.EmailAddress = model.EmailAddress;
            applicant.CountryOfOrigin = model.CountryOfOrigin;
            applicant.Age = model.Age;
            applicant.Hired = model.Hired;

            _context.Applicants.Update(applicant);
            _context.Entry(applicant).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Applicant with Id of {id} has been updated.");

            return applicant;
        }

        public async Task DeleteApplicant(int id)
        {
            var applicant = await GetApplicant(id);

            _context.Applicants.Remove(applicant);
            _context.Entry(applicant).State = EntityState.Deleted;

            _logger.LogInformation($"Applicant with Id of {id} deleted successfully");

            await _context.SaveChangesAsync();
        }
    }
}
