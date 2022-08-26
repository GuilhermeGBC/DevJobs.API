using Microsoft.AspNetCore.Mvc;
using System.Linq;
using DevJobs.API.Models;
using DevJobs.API.Persistence;
using DevJobs.API.Entities;
using Microsoft.EntityFrameworkCore;
using DevJobs.API.Persistence.Repositories;
using Serilog;

namespace DevJobs.API.Controllers
{

    [Route("api/job-vacancies")]
    [ApiController]
    public class JobVacanciesController : ControllerBase
    {
        private readonly IJobVacancyRepository _repository;


        public JobVacanciesController(IJobVacancyRepository repository)

        {
            _repository = repository;
        }

        //GET api/jov-vacancies
        [HttpGet("api/job-vacancies")]
        public IActionResult GetAll()
        {
            var jobVacancies = _repository.GetAll(); //Buscando todos

            return Ok(jobVacancies);
        }

        //GET api/job-vacancies/{id}
        [HttpGet("{id}")]
        public IActionResult GetById(int id) //Buscando apenas 1 (por id)
        {
            var jobVacancy = _repository.GetById(id);//JobVacancies
                                                     //.Include(jv => jv.Applications) //Forçar para que carregue as informações da <List> JobApplications.
                                                     //.SingleOrDefault(jv => jv.Id == id);

            if (jobVacancy == null)
                return NotFound();

            return Ok(jobVacancy);
        }

        // POST api/job-vacancies mesma URL do começo, porém esse método é POST.
        /// <summary>
        /// Cadastrar uma vaga de emprego.
        /// </summary>
        /// <remarks>
        /// {
        /// "title": "Desenvolvedor Júnior C#",
        /// "description": "Vaga para desenvolvimento de aplicações em .NET C#",
        /// "company": "Amazon",
        /// "isRemote": "true",
        /// "SalaryRange": "3000-5000" 
        /// } 
        /// </remarks>
        /// <param name="model">Dados da vaga</param>
        /// <returns>Objeto recém-criado</returns>
        /// <response code="201">Sucesso</response>
        /// <response code="400">Dados inválidos</response>
        [HttpPost("api/cadastro-vaga")]
        //[Route("cadastro-vaga")]
        public IActionResult Post(AddJobVacancyInputModel model)
        {
            //Log.Information("POST JobVacancy chamado");

            var jobVacancy = new JobVacancy(model.Title,
                model.Description,
                model.Company,
                model.IsRemote,
                model.SalaryRange);

            if (jobVacancy.Title.Length > 30)
                return BadRequest("Título precisa ter menos de 30 caracteres!");

            _repository.Add(jobVacancy);//JobVacancies.Add(jobVacancy);
            //_context.SaveChanges();
            return CreatedAtAction("GetById", new { id = jobVacancy.Id }, jobVacancy); //Action onde podemos consultar quando for criado, e o jobVacancy que acabamos de instanciar acima.
            // retorno 201, tudo preenchido.
        }

        //PUT api/job-vacancies/{id} ATUALIZAR!!
        [HttpPut("{id}")]
        public IActionResult Put(int id, UpdateJobVacancyInputModel model)//id para identificar se há alguma vaga de emprego com esse id.
        {
            var jobVacancy = _repository.GetById(id); // JobVacancies.SingleOrDefault(jv => jv.Id == id);

            if (jobVacancy == null)
            {
                return NotFound();
            }
            jobVacancy.Update(model.Title, model.Description);

            _repository.Update(jobVacancy);
            return NoContent();
        }
    }
}
