using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Specialized;

namespace SleepyRoutes
{
    class HandleRest : IHttpModule
    {
        #region IHttpModule Members

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Init(HttpApplication context)
        {
            context.BeginRequest +=
                new EventHandler(Context_BeginRequest);
        }

        void Context_BeginRequest(object sender, EventArgs e)
        {
            HttpRequest Request = ((HttpApplication)sender).Request;
            ProcessRequest((HttpApplication)sender);

        }

        #endregion

        public void ProcessRequest(HttpApplication context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;

            bool handle = !(Regex.Match(Request.RawUrl, @"\w{1,}\.\w{2,4}(\?*)?$").Success);
            string file = "";
            if (handle)
            {
                file = "/data/" + Request.Path.Replace("/", ".").Substring(1);
                //Response.Write(file);
                NameValueCollection qs = Request.QueryString;
                foreach (string k in qs)
                {
                    if (k != "Env")
                    {
                        file += "." + k;
                    }
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
                    Response.TransmitFile(context.Server.MapPath("/data/404.json"));
                }
                catch (System.IO.DirectoryNotFoundException ex)
                {
                    Response.TransmitFile(context.Server.MapPath("/data/404.json"));
                }
                Response.Flush();
                Response.End();
            }


        }
    }
}
