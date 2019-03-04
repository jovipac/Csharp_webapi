using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Net.Http;
using System.Web.Http;
using System.Configuration;
using WebService.Models;
using CryptSharp;
using MySql.Data.MySqlClient;
using System.Data;

namespace WebService.Controllers
{
    /// <summary>
    /// login controller class for authenticate users
    /// </summary>
    [AllowAnonymous]
    [RoutePrefix("api/login")]
    public class LoginController : ApiController
    {
        [HttpGet]
        [Route("echoping")]
        public IHttpActionResult EchoPing()
        {
            return Ok(true);
        }

        [HttpGet]
        [Route("echouser")]
        public IHttpActionResult EchoUser()
        {
            var identity = Thread.CurrentPrincipal.Identity;
            return Ok($" IPrincipal-user: {identity.Name} - IsAuthenticated: {identity.IsAuthenticated}");
        }

        [HttpPost]
        [Route("authenticate")]
        public IHttpActionResult Authenticate(LoginRequest login)
        {
            if (login == null )
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            //TODO: Validate credentials Correctly, this code is only for demo !!
            string connectionString = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            string hashPassword = string.Empty;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            { // Open your Connection
                connection.Open();
                MySqlCommand command = new MySqlCommand("SELECT username,password,email FROM users WHERE username LIKE ?username OR email LIKE ?email", connection);
                // Add your parameter
                command.Parameters.AddWithValue("username", login.username);
                command.Parameters.AddWithValue("email", login.email);

                using (command)
                {
                    // Create a datareader to iterate through your results
                    using (var dbread = command.ExecuteReader())
                    {
                        // If your reader can read
                        if (dbread.Read())
                        {
                            hashPassword = dbread[1].ToString();
                        }
                    }
                }
            }
            //isCredentialValid = Crypter.CheckPassword(login.password, "$2y$10$sAd9P7V3mn5jX8T8DRm96eJNO6zUw5xWKWCrEap2Azefv42VLeiv6");
            bool isCredentialValid = false;
            if (string.IsNullOrWhiteSpace(hashPassword) == false)
                isCredentialValid = Crypter.CheckPassword(login.password, hashPassword);

            if (isCredentialValid)
            {
                var token = TokenGenerator.GenerateTokenJwt(login.username);
                return Ok(token);
            }
            else
            {
                return Unauthorized();
            }
        }
    }
}
