using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Employee.WebApiRepository.Models;

namespace Employee.WebApiRepository.Controllers
{
    public class EmployeeController : ApiController
    {

        public static List<EmployeeEntity> employees = new List<EmployeeEntity>()
        {
            new EmployeeEntity()
            {
                id= "1",
                DateOfBirth =DateTime.Now ,EmailID= "a" ,
                FullName ="a" ,Gender="m" , Password="a" ,
                SecurityAnswer ="a",SecurityQuestion="a" , Username="a"
            },
             new EmployeeEntity()
            {
                id= "2",
                DateOfBirth =DateTime.Now ,EmailID= "b" ,
                FullName ="b" ,Gender="f" , Password="b" ,
                SecurityAnswer ="b",SecurityQuestion="b" , Username="b"
            }

        };
        public static List<String> errorLog = new List<string>();

        // GET api/values
        /// <summary>
        /// API is used to get all employee details 
        /// </summary>
        /// <returns>All Employee</returns>
        [HttpGet]
        [Route("EmpMgt/getAllEmpDetails")]
        public HttpResponseMessage Get()
        {
            return Request.CreateResponse(HttpStatusCode.OK, employees.ToList());
        }

        /// <summary>
        /// Get Employee with specified id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Employee</returns>
        [HttpGet]
        [Route("EmpMgt/getByEmpId")]
        // GET api/values/5
        public HttpResponseMessage Get(string id)
        {

            var element = employees.Where(x => x.id == id).FirstOrDefault();

            if (element != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, element);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, $"No Employee found with id {id}");
            }

        }

        /// <summary>
        ///  Use to check valid user
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>ok if user is authenticate else 403</returns>
        [HttpPost]
        [Route("EmpMgt/checkLogin")]
        public HttpResponseMessage Post([FromBody]string username, [FromBody]string password)
        {
            var user = employees.Where(x => x.Username == username && x.Password == password).FirstOrDefault();
            if (user == null)
            {
                return Request.CreateResponse(HttpStatusCode.Forbidden, "Invalid Username or Password.");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, "Employee has authenticated successfully");
            }
        }


        /* API Test  */




        [HttpPut]
        [Route("EmpMgt/addEmp")]
        public HttpResponseMessage AddNewEmployee([FromBody]EmployeeEntity NewEmployee)
        {

            if ((string.IsNullOrEmpty(NewEmployee.Username) || string.IsNullOrEmpty(NewEmployee.Password))
                   || checkExistingUser(NewEmployee.Username, NewEmployee.Password)
                   )
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Input Data Mismatch or username already Exist");
            }

            try
            {

                NewEmployee.id = (employees.Count() + 1).ToString();

                employees.Add(NewEmployee);

                return Request.CreateResponse(HttpStatusCode.OK, " Employee data inserted successfully.");
            }
            catch (Exception ex)
            {
                errorLog.Add($"Error Occured for User {NewEmployee.Username} and below is the error \n {ex.StackTrace}");

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                    "There is some issue at server side. Please check the log.");

            }


        }

        private bool checkExistingUser(string NewUsername, string NewPassword)
        {
            var isExist = employees.Where(x => x.Username == NewUsername && x.Password == NewPassword).Count();
            return isExist == 0 ? false : true;
        }


        [HttpPut]
        [Route("EmpMgt/deleteEmp")]
        public HttpResponseMessage DeleteEmployee(string empId)
        {
            var isExist = employees.FindIndex(x => x.id == empId);

            if (isExist != -1)
            {
                employees.RemoveAt(isExist);
                return Request.CreateResponse(HttpStatusCode.OK, " Employee data deleted successfully.");

            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, $" Employee with {empId} not found.");
            }

        }


        [HttpGet]
        [Route("EmpMgt/getErrorLog")]
        public HttpResponseMessage getErrorLog()
        {
            if (errorLog.Count() > 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, errorLog.ToList());
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, "No Error Has been logged yet ");
            }
        }
    }
}
