using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace CV4SWeb.Controllers
{
    //[RoutePrefix("api/Upload")]
    public class ImageController : ApiController
    {
        /*[Route("user/PostUserImage")]
        [AllowAnonymous]
        public async Task<HttpResponseMessage> PostUserImage()
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            try
            {
                var httpRequest = HttpContext.Current.Request;
                
                foreach (string file in httpRequest.Files)
                {
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);

                    var postedFile = httpRequest.Files[file];
                    if (postedFile != null && postedFile.ContentLength > 0)
                    {
                        var filePath = HttpContext.Current.Server.MapPath("~/Userimage/" + postedFile.FileName + ".bmp");

                        postedFile.SaveAs(filePath);
                    }

                    var message1 = string.Format("Image Updated Successfully.");
                    return Request.CreateErrorResponse(HttpStatusCode.Created, message1);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                var res = string.Format("some Message");
                dict.Add("error", res);
                return Request.CreateResponse(HttpStatusCode.NotFound, dict);
            }
            return Request.CreateResponse(HttpStatusCode.NotFound, dict);
        }*/

        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        public string Post()
        {
            var task = this.Request.Content.ReadAsStreamAsync();
            task.Wait();
            Stream requestStream = task.Result;

            try
            {
                Stream fileStream = File.Create(HttpContext.Current.Server.MapPath("~/img3.jpg"));
                requestStream.CopyTo(fileStream);
                fileStream.Close();
                requestStream.Close();
            }
            catch (IOException)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }

            //HttpResponseMessage response = new HttpResponseMessage();
            //response.StatusCode = HttpStatusCode.Created;
            return JsonConvert.SerializeObject(new { Dummy = "descriptor" });
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
