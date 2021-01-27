using FluentValidation;
using Hahn.ApplicatonProcess.December2020.Domain.Models.InputModel;
using System.Net.Http;

namespace Hahn.ApplicatonProcess.December2020.Data.Models.Validator
{
    public class ApplicantValidator : AbstractValidator<ApplicantInputModel>
    {
        private readonly HttpClient _httpClient;
        public ApplicantValidator(HttpClient httpClient)
        {
            _httpClient = httpClient;

            RuleFor(m => m.Name).NotEmpty().MinimumLength(5);
            RuleFor(m => m.FamilyName).NotEmpty().MinimumLength(5);
            RuleFor(m => m.Address).NotEmpty().MinimumLength(10);
            RuleFor(m => m.Age).InclusiveBetween(20, 60);
            RuleFor(m => m.EmailAddress).EmailAddress();
            RuleFor(m => m.CountryOfOrigin).MustAsync(async (country, cancellation) => (await _httpClient.GetAsync($"https://restcountries.eu/rest/v2/name/{country}?fullText=true")).IsSuccessStatusCode).WithMessage("Country name is not valid.");
        }
    }
}
