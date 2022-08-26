namespace DevJobs.API.Entities
{
    public class JobApplications
    {
        public JobApplications(string applicantName, string applicantEmail, int idJobVacancy)
        {
            ApplicantName = applicantName;
            ApplicantEmail = applicantEmail;
            IdJobVacancy = idJobVacancy;
        }

        public int Id { get; private set; }

        public string ApplicantName { get; private set; }

        public string ApplicantEmail { get; private set; }

        public int IdJobVacancy { get; private set; }
    }
}
