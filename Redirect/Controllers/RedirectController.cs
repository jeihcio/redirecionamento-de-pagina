using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using Redirect.Domain.Database.Context;
using Redirect.Domain.Database.Entities;
using Redirect.Models;

namespace Redirect.Controllers
{
    public class RedirectController : ApiController
    {
        #region Public

        [HttpGet]
        public IHttpActionResult Index([FromUri] String url = "")
        {
            try
            {
                string defaultWebsiteUrl = "[LINK DO SEU SITE]";
                bool isValid = ValidateUrl(url);
                var urlRedirect = url.Trim();

                if (isValid)
                {
                    String guid = GenerateOrGetGuid();
                    RegiterDataBase(urlRedirect, guid);
                }
                else
                {
                    urlRedirect = defaultWebsiteUrl;
                }

                return Redirect(urlRedirect);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet, Route("redirect/GetStatistic")]
        public IHttpActionResult GetStatistic([FromUri]String url)
        {
            using (var db = new DataContext())
            {
                var _url = url.Trim();

                _url = Regex.Replace(_url, "http://", "", RegexOptions.IgnoreCase);
                _url = Regex.Replace(_url, "https://", "", RegexOptions.IgnoreCase);

                var uniques = db.Register
                    .Where(x => x.Url.EndsWith(_url.Trim()))
                    .Select(x => new
                    {
                        Guid = x.Guid
                    })
                    .Distinct()
                    .Count();

                var general = db.Register
                    .Where(x => x.Url.EndsWith(_url.Trim()))
                    .Count();

                var result = new RegisterResult
                {
                    Url = _url.Trim(),
                    QuantitySingleAccesses = uniques,
                    QuantityAccessesGeneral = general
                };

                return Json(result);
            }
        }

        [HttpGet, Route("redirect/GetStatistic/All")]
        public IHttpActionResult GetStatisticAll()
        {
            using (var db = new DataContext())
            {
                var uniques = db.Register
                    .Select(x => new
                    {
                        Guid = x.Guid
                    })
                    .Distinct()
                    .Count();

                var general = db.Register.Count();
                var result = new RegisterResult
                {
                    Url = "TODOS OS LINKS",
                    QuantitySingleAccesses = uniques,
                    QuantityAccessesGeneral = general
                };

                return Json(result);
            }
        }

        [HttpGet, Route("redirect/GetStatistic/All/Database")]
        public IHttpActionResult GetStatisticAllDatabase()
        {
            using (var db = new DataContext())
            {
                var datas = db.Register.ToList();
                return Json(new
                {
                    data = datas
                });
            }
        }

        [HttpGet, Route("redirect/GetStatistic/All/Database/Distinct")]
        public IHttpActionResult GetStatisticAllDatabaseDistinct()
        {
            using (var db = new DataContext())
            {
                var datas = db.Register
                    .Select(x => new
                    {
                        url = x.Url
                    })
                    .Distinct()
                    .ToList();

                return Json(new
                {
                    data = datas
                });
            }
        }

        [HttpGet, Route("redirect/GetStatistic/All/Database/Distinct2")]
        public IHttpActionResult GetStatisticAllDatabaseDistinct2()
        {
            using (var db = new DataContext())
            {
                String sql = "select distinct(A.Url) As Url,                   " +
                             "       (                                         " +
                             "           Select COUNT(distinct(Registro.Guid)) " +
                             "             From Registro                       " +
                             "            Where Registro.Url = A.Url           " +
                             "       ) as AcessoUnico,                         " +
                             "       (                                         " +
                             "           Select COUNT(*)                       " +
                             "             From Registro                       " +
                             "            Where Registro.Url = A.Url           " +
                             "       ) as AcessoGeral                          " +
                             "  from Registro As A                             ";

                var result = db.Database
                    .SqlQuery<DatabaseDistinct2>(sql)
                    .ToList();

                return Json(result);
            }
        }

        [HttpDelete, Route("redirect/all")]
        public IHttpActionResult Delete()
        {
            using (var db = new DataContext())
            {
                try
                {
                    var all = db.Register.ToArray();
                    db.Register.RemoveRange(all);
                    db.SaveChanges();
                    return Ok("Banco limpo");
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }
        }
        #endregion

        #region Private

        private String GenerateOrGetGuid()
        {
            try
            {
                HttpContext context = HttpContext.Current;

                var guid = context.Request.Cookies["VisitorId"];
                if (guid == null)
                {
                    var cookies = new HttpCookie("VisitorId");
                    cookies.Value = Guid.NewGuid().ToString();
                    cookies.Expires = DateTime.MaxValue;

                    context.Response.Cookies.Add(cookies);
                }

                return guid.Value.ToString();
            }
            catch (Exception e)
            {
                return "Erro: " + e.Message + " Guid: " + Guid.NewGuid();
            }

        }

        private void RegiterDataBase(string url, string guid)
        {
            using (var db = new DataContext())
            {
                var uri = new Uri(url.Trim());
                String _url = uri
                    .AbsoluteUri
                    .Replace(uri.Scheme + "://", "");

                String lastChar = _url[_url.Length - 1].ToString();
                if (lastChar.Equals("/"))
                {
                    _url = _url.Substring(0, _url.Length - 1);
                }

                try
                {
                    var newRegister = new Register
                    {
                        Url = _url,
                        Date = DateTime.Now,
                        Guid = guid.Trim()
                    };

                    db.Register.Add(newRegister);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                }
            }
        }

        private bool ValidateUrl(string url)
        {
            Uri validatedUri;
            if (Uri.TryCreate(url, UriKind.Absolute, out validatedUri))
            {
                return (validatedUri.Scheme == Uri.UriSchemeHttp || validatedUri.Scheme == Uri.UriSchemeHttps);
            }
            return false;
        }

        #endregion
    }
}
