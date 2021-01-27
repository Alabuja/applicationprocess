using AutoMapper;
using Hahn.ApplicatonProcess.December2020.Domain.Models;
using Hahn.ApplicatonProcess.December2020.Domain.Models.OutputModel;

namespace Hahn.ApplicatonProcess.December2020.Domain
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile(): base()
        {
            CreateMap<Applicant, ApplicantOutputModel>().ReverseMap();
        }
    }
}
