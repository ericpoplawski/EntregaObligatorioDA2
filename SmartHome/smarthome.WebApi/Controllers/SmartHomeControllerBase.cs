using Domain;
using Microsoft.AspNetCore.Mvc;

namespace smarthome.WebApi.Controllers
{
    public class SmartHomeControllerBase : ControllerBase
    {
        protected User GetUserLogged()
        {
            var userLogged = HttpContext.Items[Items.UserLogged];

            var userLoggedMapped = (User)userLogged;
            
            return userLoggedMapped;
        }
    }
}