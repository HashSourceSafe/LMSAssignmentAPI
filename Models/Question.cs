namespace LMSAssignmentAPI.Models
{
    //public class Question
    //{
    //    public int QuestionId { get; set; }
    //    public string QuestionText { get; set; }
    //    public string Module { get; set; }
    //    public int Marks { get; set; }
    //    public string AnswerKey { get; set; }

    //    // Add Choices property
    //    public List<string> Choices { get; set; }
    //}

    public class Question
    {
        public int QuestionId { get; set; }
        public string QuestionText { get; set; }
        public int Marks { get; set; }
        public string Module { get; set; }
        public int? QNo { get; set; }
        public string AnswerKey { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;

        // List to store multiple choices
        public List<QuestionPart> Choices { get; set; }
    }

    public class QuestionPart
    {
        public int QuestionPartId { get; set; }
        public int QuestionId { get; set; }
        public int QnPartOrder { get; set; } // Order of choices (1,2,3,4)
        public string Label { get; set; } // "A", "B", "C", "D"
        public string QuestionText { get; set; } // Answer choice text
    }

}
