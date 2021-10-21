using API.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace API.Service.Controllers
{
    [RoutePrefix("api/dao")]
    [Authorize]
    public partial class DAOController : ApiController
    {
        [HttpGet]
        [Route("{storeProcedure}")]
        public IHttpActionResult ExecuteQuery(string storeProcedure)
        { return Json(new ExecuteQuery().Run(storeProcedure)); }

        [HttpGet]
        [Route("{storeProcedure}/")]
        public IHttpActionResult ExecuteQuery(string storeProcedure, [FromUri] string args)
        {
            object[] parmeterlist = Uri.UnescapeDataString(args).Split(',');
            return Json(new ExecuteQuery().Run(storeProcedure, parmeterlist));

        }
    }
}
