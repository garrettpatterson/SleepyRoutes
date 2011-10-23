using System.Web;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Collections.Specialized;

namespace SleepyRoutes
{
    public class HandleAll : IHttpHandler
    {
        #region IHttpHandler Members

        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            // This handler is called whenever a file ending 
            // in .sample is requested. A file with that extension
            // does not need to exist.
            //path
            bool handle = false;
            string file = "";
            if (handle)
            {
                file = "/data/" + Request.Path.Replace("/", ".").Substring(1);
                //Response.Write(file);
                NameValueCollection qs = Request.QueryString;
                foreach (string k in qs)
                {
                    file += "." + k;
                }
                if (Request.HttpMethod != "GET")
                {
                    file += "." + Request.HttpMethod;
                }

                file = file + ".json";
                Response.ContentType = "text/json";
                Response.AddHeader("Content-Type", "text/plain");
                Response.AddHeader("content-disposition", "inline");
                try
                {
                    Response.TransmitFile(context.Server.MapPath(file));
                }
                catch (System.IO.FileNotFoundException ex)
                {
                    //Response.Write(file);
                    //Response.TransmitFile(context.Server.MapPath("/data/404.json"));
                    if (context.Server.MapPath("/data/404.json") == null)
                    {
                        Response.Write("{\"status\":\"error\",\"data\":\"SleepyRoutes, no file found for: " + file + "\"}");
                    }
                    else
                    {
                        Response.TransmitFile(context.Server.MapPath("/data/404.json"));
                    }
                }
                catch (System.IO.DirectoryNotFoundException ex)
                {
                    //Response.Write(file);
                    //Response.TransmitFile(context.Server.MapPath("/data/404.json"));
                    if (context.Server.MapPath("/data/404.json") == null)
                    {
                        Response.Write("{\"status\":\"error\",\"data\":\"SleepyRoutes, no file found for: " + file + "\"}");
                    }
                    else
                    {
                        Response.TransmitFile(context.Server.MapPath("/data/404.json"));
                    }
                }
                Response.Flush();
                Response.End();
            }
            else
            {

            }
        }

        #endregion
    }
}
