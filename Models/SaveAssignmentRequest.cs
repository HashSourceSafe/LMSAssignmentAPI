namespace LMSAssignmentAPI.Models
{
    public class SaveAssignmentRequest
    {

        public string Module { get; set; }
        public int CollegeId { get; set; }
        public int BatchId { get; set; }
        public int CourseId { get; set; }
        public int StreamId { get; set; }
        public int SemesterId { get; set; }
        public int SectionId { get; set; }
        public int GroupId { get; set; }
        public int SubjectId { get; set; }
        public string FacultyCode { get; set; }
        public int PeriodId { get; set; }
        public string FacultyDate { get; set; }
        public string BranchId { get; set; }
    }
}
