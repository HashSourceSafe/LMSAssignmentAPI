using System.Data;
using Microsoft.Data.SqlClient;
using Dapper;
using LMSAssignmentAPI.Models;
using Microsoft.Extensions.Configuration;
using LMSAssignmentAPI.Utility;
using static Org.BouncyCastle.Math.EC.ECCurve;
namespace LMSAssignmentAPI.Repositories
{
    public class AssignmentRepository
    {
        private readonly IConfiguration _configuration;
        private string _connectionString;

        public AssignmentRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public void SetBranchConnection(string branchId)
        {
            string connectionName = GetConnName(branchId);
            _connectionString = _configuration.GetConnectionString(connectionName);

            if (string.IsNullOrEmpty(_connectionString))
            {
                throw new Exception($"Connection string for '{branchId}' not found.");
            }
        }
        
        public string GetConnName(string p_branch_id)
        {
            string m_RetVal = "";
            int m_BranchId = Convert.ToInt32(p_branch_id);

            if (m_BranchId == 100 || m_BranchId == 21 || m_BranchId == 101)
            {
                m_RetVal = "ErpConnectionString_Surermath";
            }
            else if (m_BranchId == 3)
            {
                m_RetVal = "ErpConnectionString_Nit";
            }
            else if (m_BranchId >= 6 && m_BranchId <= 9)
            {
                m_RetVal = "ErpConnectionString_Gnit";
            }
            else if (m_BranchId == 61 || m_BranchId == 62 || m_BranchId == 64)
            {
                m_RetVal = "ErpConnectionString_Gkcem";
            }
            else if (m_BranchId == 4 || m_BranchId == 20 || m_BranchId == 102)
            {
                m_RetVal = "ErpConnectionString_Kalyani";
            }
            else if (m_BranchId == 10 || m_BranchId == 11 || m_BranchId == 12 || m_BranchId == 14 || m_BranchId == 15 || m_BranchId == 18 || m_BranchId == 22 || m_BranchId == 23 || m_BranchId == 24 || m_BranchId == 25 || m_BranchId == 26 || m_BranchId == 27 || m_BranchId == 28 || m_BranchId == 31 || m_BranchId == 32 || m_BranchId == 34 || m_BranchId == 35 || m_BranchId == 36 || m_BranchId == 63 || m_BranchId == 65 || m_BranchId == 66 || m_BranchId == 67)
            {
                m_RetVal = "ErpConnectionString_Ho";
            }
            else if (m_BranchId >= 51 && m_BranchId <= 55)
            {
                m_RetVal = "ErpConnectionString_Abacus";
            }
            else if (m_BranchId >= 201 && m_BranchId <= 202)
            {
                m_RetVal = "ErpConnectionString_Haldia";
            }
            else if (m_BranchId >= 301 && m_BranchId <= 305)
            {
                m_RetVal = "ErpConnectionString_Canteen";
            }
            else if (m_BranchId >= 401 && m_BranchId <= 401)
            {
                m_RetVal = "ErpConnectionString_Asansole";
            }
            else if (m_BranchId >= 500 && m_BranchId <= 510)
            {
                m_RetVal = "ErpConnectionString_Jis_Unv";
            }
            else if (m_BranchId == 600)
            {
                m_RetVal = "ErpConnectionString_Hash";
            }
            else if (m_BranchId >= 610 && m_BranchId <= 620)
            {
                m_RetVal = "ErpConnectionString_Jharkhand";
            }
            else if (m_BranchId >= 621 && m_BranchId <= 625)
            {
                m_RetVal = "ErpConnectionString_Central_Model_School";
            }
            else if (m_BranchId >= 626 && m_BranchId <= 630)
            {
                m_RetVal = "ErpConnectionString_Lbs";
            }
            else if (m_BranchId == 33)
            {
                m_RetVal = "ErpConnectionString_Jqj";
            }
            else if (m_BranchId >= 641 && m_BranchId <= 650)
            {
                m_RetVal = "ErpConnectionString_Gulzar";
            }
            else if (m_BranchId == 999)
            {
                m_RetVal = "ErpConnectionString_Digital";
            }
            return m_RetVal;

        }
        public async Task<bool> SaveAssignment(Assignment assignment, string jsonData)
        {
            JSONValidator js = new JSONValidator();
            string validJson = js.ValidateAndFixJson(jsonData);
            if (string.IsNullOrEmpty(validJson))
            {
                Console.WriteLine("Error: Invalid JSON format");
                return false;
            }
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var parameters = new DynamicParameters();
            parameters.Add("@p_Assignment_name", assignment.Assignment_name, DbType.String);
            parameters.Add("@p_@Module", assignment.Module, DbType.String);
            parameters.Add("@p_user_id", assignment.Uploaded_by, DbType.String);
            parameters.Add("@p_@Assignment_file_name", assignment.Assignment_file_name, DbType.String);
            parameters.Add("@p_college_id", assignment.CollegeId, DbType.Int32);    
            parameters.Add("@p_batch_id", assignment.BatchId, DbType.Int32);
            parameters.Add("@p_course_id", assignment.CourseId, DbType.Int32);
            parameters.Add("@p_stream_id", assignment.StreamId, DbType.Int32);
            parameters.Add("@p_semesterId", assignment.SemesterId, DbType.Int32);
            parameters.Add("@p_sectionId", assignment.SectionId, DbType.Int32);
            parameters.Add("@p_groupId", assignment.GroupId, DbType.Int32);
            parameters.Add("@p_subjectId", assignment.SubjectId, DbType.Int32);
            parameters.Add("@p_facultyCode", assignment.FacultyCode, DbType.String);
            parameters.Add("@p_PeriodId", assignment.PeriodId, DbType.Int32);
            parameters.Add("@p_date", assignment.faculty_date, DbType.String);
            parameters.Add("@p_JsonData", validJson, DbType.String);
       
            try
            {
                await connection.ExecuteAsync("Proc_save_assignments", parameters, commandType: CommandType.StoredProcedure);

             
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }


        public async Task<int?> GetExistingAssignmentIdAsync(string module, int collegeId, int batchId,
    int courseId, int streamId, int semesterId, int sectionId, int groupId, int subjectId, string facultyCode, int periodId)
        {
            using var connection = new SqlConnection(_connectionString);
            var parameters = new DynamicParameters();
            parameters.Add("@p_@Module", module, DbType.String);
            parameters.Add("@p_college_id", collegeId, DbType.Int32);
            parameters.Add("@p_batch_id", batchId, DbType.Int32);
            parameters.Add("@p_course_id", courseId, DbType.Int32);
            parameters.Add("@p_stream_id", streamId, DbType.Int32);
            parameters.Add("@p_semesterId", semesterId, DbType.Int32);
            parameters.Add("@p_sectionId", sectionId, DbType.Int32);
            parameters.Add("@p_groupId", groupId, DbType.Int32);
            parameters.Add("@p_subjectId", subjectId, DbType.Int32);
            parameters.Add("@p_facultyCode", facultyCode, DbType.String);
            parameters.Add("@p_PeriodId", periodId, DbType.Int32);

            return await connection.ExecuteScalarAsync<int?>("Proc_Get_Assignment", parameters, commandType: CommandType.StoredProcedure);
        }

        public async Task<bool> AssignQuestionsToStudents(Assignment assignment)
        {

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var parameters = new DynamicParameters();
            parameters.Add("@P_COLLEGE_ID", assignment.CollegeId, DbType.Int32);
            parameters.Add("@P_FACULTY_CODE", assignment.FacultyCode, DbType.String);
            parameters.Add("@P_DATE", assignment.faculty_date, DbType.String);
            parameters.Add("@P_BATCH_ID", assignment.BatchId, DbType.Int32);
            parameters.Add("@P_COURSE_ID", assignment.CourseId, DbType.Int32);
            parameters.Add("@P_STREAM_ID", assignment.StreamId, DbType.Int32);
            parameters.Add("@P_SECTION_ID", assignment.SectionId, DbType.Int32);
            parameters.Add("@P_SEMESTER_ID", assignment.SemesterId, DbType.Int32);
            parameters.Add("@P_GROUP_ID", assignment.GroupId, DbType.Int32);
            parameters.Add("@P_SUBJECT_ID", assignment.SubjectId, DbType.Int32);
            parameters.Add("@P_PERIOD_ID", assignment.PeriodId, DbType.Int32);
            parameters.Add("@P_ASSIGNMENT_ID", assignment.AssignmentID, DbType.Int32);
            parameters.Add("@p_order_by",2, DbType.Int16);
            parameters.Add("@p_disp", 3, DbType.Int16);

            try
            {
                await connection.ExecuteAsync("[Proc_Assign_Questions_To_Students]", parameters, commandType: CommandType.StoredProcedure);

                // Retrieve the output values
                //int assignmentId = parameters.Get<int>("@m_assignment_id");
                //string errorMessage = parameters.Get<string>("@m_err_mesg");
                //int errorCode = parameters.Get<int>("@m_err_no");

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }


    }
}
