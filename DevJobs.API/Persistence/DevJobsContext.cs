using DevJobs.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace DevJobs.API.Persistence
{
    public class DevJobsContext : DbContext
    {
        public DevJobsContext(DbContextOptions<DevJobsContext> options) : base(options)
        {
            //JobVacancies = new List<JobVacancy>();
            //jobApplications = new List<JobApplications>();
        }
        public DbSet<JobVacancy> JobVacancies { get; set; }

        public DbSet<JobApplications> JobApplications { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<JobVacancy>(e =>
            {
                e.HasKey(jv => jv.Id); //Definindo chave primária
                //e.ToTable("dbo.JobVacancies"); Nome da tabela personalizado

                //Configurando RELACIONAMENTO!!!

                e.HasMany(jv => jv.Applications) //jobvacancy tem muitas applications
                   .WithOne() //Uma application tem apenas uma vaga.
                   .HasForeignKey(ja => ja.IdJobVacancy)
                   .OnDelete(DeleteBehavior.Restrict); //Impedindo caso alguém tente excluir minha vaga do banco de dados.
                //Se deletarmos uma vaga, vai deletar tudo que tem referência a essa vaga. < com o Cascade
            });
            builder.Entity<JobApplications>(e =>
            {
                e.HasKey(ja => ja.Id);
            });
        }
    }
}
