using BookStoreDomain.Contracts;
using BookStoreDomain.Models;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BookstoreWebAPI.Controllers
{
    [RoutePrefix(ApiRoutes.Auth)]
    public class AuthController : ApiController
    {
        private readonly IUserAuth _auth;
        private readonly IUserRoleManager _roleManager;

        public AuthController(IUserAuth auth, IUserRoleManager roleManager)
        {
            _auth = auth;
            _roleManager = roleManager;

        }


        // POST api/<controller>
        [HttpPost]
        [Route(ApiRoutes.Login)]
        public IHttpActionResult Login([FromBody] UserLoginViewModel user)
        {
            var response = _auth.AuthUser(user);

            if (response.IsSuccess && response.results.Count > 0)
            {
                return Ok(response.results[0]);
            }
            else
            {
                return ResponseMessage(Request.CreateResponse(
                HttpStatusCode.BadRequest,
                response));
            }
        }

        [HttpPost]
        [Route(ApiRoutes.Register)]
        public IHttpActionResult Register([FromBody] UserRegistrationViewModel user)
        {
            var response = _auth.RegisterUser(user);

            if (response.IsSuccess)
                return Ok(response);
            else
            {
                return ResponseMessage(Request.CreateResponse(
                HttpStatusCode.BadRequest,
                response));
            }
        }

        [HttpGet]
        [Route(ApiRoutes.CheckPseudonym)]
        public IHttpActionResult CheckAuthorPseudonym(string name)
        {
            var response = _auth.CheckAuthorPseudonym(name);

            if (response.IsSuccess)
                return Ok(response);
            else
            {
                return ResponseMessage(Request.CreateResponse(
                HttpStatusCode.BadRequest,
                response));
            }
        }

        [HttpPost]
        [Route(ApiRoutes.AddUserRole)]
        public IHttpActionResult AddUserRole([FromBody] UserRoleCreateViewModel userRole)
        {
            var response = _roleManager.AddUserRole(userRole);

            if (response.IsSuccess)
                return Ok(response);
            else
            {
                return ResponseMessage(Request.CreateResponse(
                HttpStatusCode.BadRequest,
                response));
            }
        }

        [HttpPost]
        [Route(ApiRoutes.AddRole)]
        public IHttpActionResult AddRole([FromBody] RoleCreateViewModel role)
        {
            var response = _roleManager.AddRole(role);

            if (response.IsSuccess)
                return Ok(response);
            else
            {
                return ResponseMessage(Request.CreateResponse(
                HttpStatusCode.BadRequest,
                response));
            }
        }


    }
}