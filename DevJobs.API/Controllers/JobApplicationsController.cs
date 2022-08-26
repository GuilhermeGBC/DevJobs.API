using DevJobs.API.Entities;
using DevJobs.API.Models;
using DevJobs.API.Persistence;
using DevJobs.API.Persistence.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace DevJobs.API.Controllers
{

    [Route("api/job-vacancies/{id}/applications")]
    [ApiController]
    public class JobApplicationsController : ControllerBase
    {

        private readonly IJobVacancyRepository _repository;

        public JobApplicationsController(IJobVacancyRepository repository)
        {
            _repository = repository;
        }
        //Post api/job-vacancies/4/applications
        [HttpPost]
        public IActionResult Post(int id, AddJobApplicationInputModel model)
        {
            var jobVacancy = _repository.GetById(id);//_context.JobVacancies.SingleOrDefault(jv => jv.Id == id); //Verificando se o id existe

            if (jobVacancy == null)
            {
                return NotFound();
            }

            var application = new JobApplications(model.ApplicantName, model.ApplicantEmail, model.IdJobVacancy);

            _repository.AddApplication(application);
            //_context.JobApplications.Add(application);
            //_context.SaveChanges();

            return NoContent();
        }
    }
}
