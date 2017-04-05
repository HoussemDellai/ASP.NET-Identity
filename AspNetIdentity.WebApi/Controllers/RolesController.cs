using System;
using System.Threading.Tasks;
using System.Web.Http;
using AspNetIdentity.WebApi.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace AspNetIdentity.WebApi.Controllers
{
    /// <summary>
    /// The controller which will be responsible to manage roles in the system 
    /// (add new roles, delete existing ones, getting single role by id, etc…), 
    /// but this controller should only be accessed by users in “Admin” role 
    /// because it doesn’t make sense to allow any authenticated user to delete 
    /// or create roles in the system, 
    /// so we will see how we will use the [Authorize] attribute 
    /// along with the Roles to control this.
    /// </summary>
    // The “Roles” property accepts comma separated values 
    // so you can add multiple roles if needed. 
    // In other words the user who will have an access to this controller 
    // should have valid JSON Web Token which contains claim 
    // of type “Role” and value of “Admin”.
    [Authorize(Roles = "Admin")]
    [RoutePrefix("api/roles")]
    public class RolesController : BaseApiController
    {
        /// <summary>
        /// return a single role based on it is identifier, 
        /// this will happen when we call the method “FindByIdAsync”, 
        /// this method returns object of type “RoleReturnModel”.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("{id:guid}", Name = "GetRoleById")]
        public async Task<IHttpActionResult> GetRole(string id)
        {
            var role = await AppRoleManager.FindByIdAsync(id);

            if (role != null)
            {
                return Ok(TheModelFactory.Create(role));
            }

            return NotFound();

        }

        /// <summary>
        /// Returns all the roles defined in the system.
        /// </summary>
        /// <returns></returns>
        [Route("", Name = "GetAllRoles")]
        public IHttpActionResult GetAllRoles()
        {
            var roles = AppRoleManager.Roles;

            return Ok(roles);
        }

        /// <summary>
        /// Creates new roles in the system, it will accept model of type 
        /// “CreateRoleBindingModel”. 
        /// This method will call “CreateAsync” and will return response of type 
        /// “RoleReturnModel”.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("create")]
        public async Task<IHttpActionResult> Create(CreateRoleBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var role = new IdentityRole { Name = model.Name };

            var result = await AppRoleManager.CreateAsync(role);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            Uri locationHeader = new Uri(Url.Link("GetRoleById", new { id = role.Id }));

            return Created(locationHeader, TheModelFactory.Create(role));

        }

        /// <summary>
        /// Deletes existing role by passing the unique id of the role 
        /// then calling the method “DeleteAsync”.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("{id:guid}")]
        public async Task<IHttpActionResult> DeleteRole(string id)
        {

            var role = await AppRoleManager.FindByIdAsync(id);

            if (role != null)
            {
                IdentityResult result = await AppRoleManager.DeleteAsync(role);

                if (!result.Succeeded)
                {
                    return GetErrorResult(result);
                }

                return Ok();
            }

            return NotFound();

        }

        /// <summary>
        /// Accept a request body containing an object of type “UsersInRoleModel” 
        /// where the application will add or remove users from a specified role.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("ManageUsersInRole")]
        public async Task<IHttpActionResult> ManageUsersInRole(UsersInRoleModel model)
        {
            var role = await AppRoleManager.FindByIdAsync(model.Id);

            if (role == null)
            {
                ModelState.AddModelError("", "Role does not exist");
                return BadRequest(ModelState);
            }

            foreach (string user in model.EnrolledUsers)
            {
                var appUser = await AppUserManager.FindByIdAsync(user);

                if (appUser == null)
                {
                    ModelState.AddModelError("", $"User: {user} does not exists");
                    continue;
                }

                if (!AppUserManager.IsInRole(user, role.Name))
                {
                    IdentityResult result = await AppUserManager.AddToRoleAsync(user, role.Name);

                    if (!result.Succeeded)
                    {
                        ModelState.AddModelError("", $"User: {user} could not be added to role");
                    }

                }
            }

            foreach (string user in model.RemovedUsers)
            {
                var appUser = await AppUserManager.FindByIdAsync(user);

                if (appUser == null)
                {
                    ModelState.AddModelError("", $"User: {user} does not exists");
                    continue;
                }

                IdentityResult result = await AppUserManager.RemoveFromRoleAsync(user, role.Name);

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", $"User: {user} could not be removed from role");
                }
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok();
        }
    }
}