using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ASPCoreMVC.Spike.Models;
using Microsoft.Extensions.Options;
using System.Net.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing;

namespace ASPCoreMVC_Spike.Controllers
{
    [Route("api/[controller]")]
    public class RouteDataController : Controller
    {

        private readonly IActionDescriptorCollectionProvider _actionDescriptorCollectionProvider;

        public RouteDataController(IActionDescriptorCollectionProvider actionDescriptorCollectionProvider)
        {
            _actionDescriptorCollectionProvider = actionDescriptorCollectionProvider;
        }

        [HttpGet("[action]")]
        public string GetAllRoutes()
        {

            var routes = RouteData.Routers.OfType<RouteCollection>();
            return "adfdas";
            //var result = new ListResult<RouteModel>();
            //var routes = _actionDescriptorCollectionProvider.ActionDescriptors.Items.Where(
            //    ad => ad.AttributeRouteInfo != null).Select(ad => new RouteModel
            //    {
            //        Name = ad.AttributeRouteInfo.Name,
            //        Template = ad.AttributeRouteInfo.Template
            //    }).ToList();
            //if (routes != null && routes.Any())
            //{
            //    result.Items = routes;
            //    result.Success = true;
            //}
            //return Ok(result);
        }



    }
}
