using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public async Task<string> PostAsync()
        {
            /*var task = this.Request.Content.ReadAsStreamAsync();
            task.Wait();
            Stream requestStream = task.Result;*/

            try
            {
                string root = HttpContext.Current.Server.MapPath("~/App_Data");
                var provider = new MultipartFormDataStreamProvider(root);

                await Request.Content.ReadAsMultipartAsync(provider);
                
                MultipartFileData file = provider.FileData[0];
                    string filename=file.LocalFileName;
                    string newfilename = file.Headers.ContentDisposition.FileName.Trim(new char[]{ '\"'});
                    string newfilepath = root + '\\' + newfilename;
                    try
                    {
                        File.Delete(newfilepath);
                        File.Move(filename, newfilepath);
                    }
                    catch(Exception e)
                    {
                        Trace.WriteLine(e);
                    }

                    try
                    {
                        using (Mat src = new Mat(newfilepath, ImreadModes.Color))
                        using (Mat gray = new Mat())
                        using (Mat dst = src.Clone())
                        {
                            Cv2.CvtColor(src, gray, ColorConversionCodes.BGR2GRAY);

                            MSER mser = MSER.Create();
                            Point[][] msers;
                            Rect[] bboxes;
                            mser.DetectRegions(gray, out msers, out bboxes);
                        }
                    }
                    catch(Exception e)
                    {
                        Trace.WriteLine(e);
                    }
            }
            catch (IOException)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }

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
