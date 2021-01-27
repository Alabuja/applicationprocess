using AutoMapper;
using Hahn.ApplicatonProcess.December2020.Data.Models.Validator;
using Hahn.ApplicatonProcess.December2020.Domain.Models.InputModel;
using Hahn.ApplicatonProcess.December2020.Domain.Models.OutputModel;
using Hahn.ApplicatonProcess.December2020.Domain.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Hahn.ApplicatonProcess.December2020.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicantsController : ControllerBase
    {
        private readonly ApplicantService _applicantService;
        private readonly ExceptionService _exceptionService;
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;

        public ApplicantsController(ApplicantService applicantService, 
            IMapper mapper, ExceptionService exceptionService)
        {
            _applicantService = applicantService;
            _mapper = mapper;
            _exceptionService = exceptionService;
            _httpClient = new HttpClient();
        }

        // GET: api/Applicants/2
        /// <summary>
        /// Endpoint will fetch the applicant by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<ApplicantOutputModel>> GetApplicant(int id)
        {
            try
            {
                var applicant = await _applicantService.GetApplicant(id);

                if (applicant == null) return NotFound();

                return _mapper.Map<ApplicantOutputModel>(applicant);
            }
            catch (Exception ex)
            {
                return _exceptionService.GetActionResult(ex);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult> CreateApplicant([FromBody] ApplicantInputModel model)
        {
            var validator = new ApplicantValidator(_httpClient);
            var result = await validator.ValidateAsync(model);

            if (result.IsValid)
            {
                try
                {
                    var applicant = await _applicantService.CreateApplicant(model);

                    return CreatedAtAction(
                            nameof(GetApplicant),
                            applicant,
                            _mapper.Map<ApplicantOutputModel>(applicant)
                        );
                }
                catch (Exception ex)
                {
                    return _exceptionService.GetActionResult(ex);
                }
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        // PUT: api/Applicants/2
        /// <summary>
        /// Updates the Applicant with the ID specified with new values in the applicant model submitted in the request body.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<ApplicantOutputModel>> UpdateApplicant(int id, [FromBody] ApplicantInputModel model)
        {
            var validator = new ApplicantValidator(_httpClient);
            var result = await validator.ValidateAsync(model);

            if (result.IsValid)
            {
                try
                {
                    var activity = await _applicantService.UpdateApplicant(model, id);

                    return _mapper.Map<ApplicantOutputModel>(activity);
                }
                catch (Exception ex)
                {
                    return _exceptionService.GetActionResult(ex);
                }
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        // DELETE: api/Applicants/2
        /// <summary>
        /// Deletes the Applicant with the specified ID from the system.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> DeleteApplicant(int id)
        {
            try
            {
                await _applicantService.DeleteApplicant(id);

                return NoContent();
            }
            catch (Exception ex)
            {
                return _exceptionService.GetActionResult(ex);
            }
        }
    }
}
