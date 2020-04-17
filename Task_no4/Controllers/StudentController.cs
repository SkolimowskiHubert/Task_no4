using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Task_no4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private string connectionString =
           @"Data Source=db-mssql;Initial Catalog = s17782; Integrated Security = True";

        [HttpGet]
        public IActionResult GetStudent()
        {
            var result = new List<Student>();

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText = "select * from Student";
                con.Open();
                SqlDataReader reader = com.ExecuteReader();

                while (reader.Read())
                {
                    var student = new Student();
                    student.Name = reader["FirstName"].ToString();
                    student.Surname = reader["LastName"].ToString();
                    student.Birthdate = reader["BirthDate"].ToString();
                    student.IndexNumber = reader["IndexNumber"].ToString();
                    student.IdEnrollment = reader["IdEnrollment"].ToString();
                    result.Add(student);
                }
            }

            return Ok(result);
        }


        //Task 3.3 - fetching data – endpoint returns semester entries(WpisNaSemestr)

        [HttpGet("{id}")]
        public IActionResult GetSemester(string id)
        {
            var result = new List<Enrollment>();
            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText = "select * from enrollment as s join student as e on s.idenrollment = e.idenrollment where e.indexnumber = @index ;";
                com.Parameters.AddWithValue("index", id);

                con.Open();
                SqlDataReader reader = com.ExecuteReader();

                while (reader.Read())
                {
                    Enrollment e = new Enrollment();
                    e.IdEnrollment = reader["IdEnrollment"].ToString();
                    e.Semester = reader["Semester"].ToString();
                    e.IdStudy = reader["IdStudy"].ToString();
                    e.StartDate = reader["StartDate"].ToString();
                    result.Add(e);
                }
            }

            return Ok(result);


        }
    }
}