namespace LMSAssignmentAPI.Models
{
    public class Assignment
    {
        public int AssignmentID { get; set; }
        public string Assignment_name { get; set; }
        public DateTime Assignment_date { get; set; }
        public string Module { get; set; }
        public int QuestionID { get; set; }
        public string Uploaded_by { get; set; }
        public string Assignment_file_name { get; set; }
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
        public string faculty_date { get; set; }
        public int order_by { get; set; }
        public int display { get; set; }
    }
}
