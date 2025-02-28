using LMSAssignmentAPI.Models;
using LMSAssignmentAPI.Repositories;
using LMSAssignmentAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace LMSAssignmentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssignmentController : Controller
    {
        private readonly CohereService _cohereService;
        private readonly PdfService _pdfService;
        private readonly AssignmentRepository _assignmentRepository;
        private readonly QuestionRepository _questionRepository;
        private readonly IConfiguration _configuration;

        public AssignmentController(
     CohereService cohereService,
     PdfService pdfService,
     AssignmentRepository assignmentRepository,
     QuestionRepository questionRepository,
     IConfiguration configuration) 
        {
            _cohereService = cohereService;
            _pdfService = pdfService;
            _assignmentRepository = assignmentRepository;
            _questionRepository = questionRepository;
            _configuration = configuration; 
        }
        [HttpPost("generate")]
        public async Task<IActionResult> GenerateAssignment([FromBody] SaveAssignmentRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Module))
            {
                return BadRequest("Module is required.");
            }

            _assignmentRepository.SetBranchConnection(request.BranchId);

            var existingAssignmentId = await _assignmentRepository.GetExistingAssignmentIdAsync(
       request.Module, request.CollegeId, request.BatchId, request.CourseId,
       request.StreamId, request.SemesterId, request.SectionId, request.GroupId,
       request.SubjectId, request.FacultyCode, request.PeriodId);

            var assignment = new Assignment
            {
                Assignment_name = $"Assignment - {request.Module}",
                Module = request.Module,
                Uploaded_by = "System",
                Assignment_file_name = "assignment.pdf",
                CollegeId = request.CollegeId,
                BatchId = request.BatchId,
                CourseId = request.CourseId,
                StreamId = request.StreamId,
                SemesterId = request.SemesterId,
                SectionId = request.SectionId,
                GroupId = request.GroupId,
                SubjectId = request.SubjectId,
                FacultyCode = request.FacultyCode,
                PeriodId = request.PeriodId,
                faculty_date = request.FacultyDate
              

            };

            if (existingAssignmentId.HasValue)
            {
                assignment.AssignmentID = existingAssignmentId ?? 0;
                await _assignmentRepository.AssignQuestionsToStudents(assignment);
                return Ok(new { Message = "Questions assigned to students for existing assignment.", AssignmentId = existingAssignmentId });
            }

            var jsonResponse = await _cohereService.GenerateQuestionsJsonAsync(request.Module);
            if (string.IsNullOrEmpty(jsonResponse))
            {
                return BadRequest("Failed to generate questions.");
            }


            bool isSaved = await _assignmentRepository.SaveAssignment(assignment, jsonResponse);


            if (!isSaved)
            {
                return StatusCode(500, "Error saving Assignemtn & questions.");
            }

            return Ok(new { Message = "Assignment Generated and Saved" });


        }


    }
}
