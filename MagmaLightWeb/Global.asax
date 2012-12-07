<%@ Application Language="C#" %>

<script runat="server">

    void Application_Start(object sender, EventArgs e) 
    {
        // Code that runs on application startup

    }
    
    void Application_End(object sender, EventArgs e) 
    {
        //  Code that runs on application shutdown

    }
        
    void Application_Error(object sender, EventArgs e) 
    {
        // ignore CryptographicException
        if ((((ASP.global_asax)sender).Context != null) && (((ASP.global_asax)sender).Context.Error is System.Security.Cryptography.CryptographicException)) return;
        
        // Code that runs when an unhandled error occurs
        HttpContext httpContext = ((ASP.global_asax)sender).Context;
        //MagmaLightWeb.Common.ServiceManager.NaplampaService.SendEmail("ERRORREPORT", "hu-HU", null, new object[] { httpContext.Error.ToString(), httpContext.Error.Message, httpContext.Error.Source, httpContext.Error.StackTrace });
    }

    void Session_Start(object sender, EventArgs e) 
    {
        // Code that runs when a new session is started

    }

    void Session_End(object sender, EventArgs e) 
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.

    }


    public override string GetVaryByCustomString(HttpContext context, string custom)
    {
        if (custom == "Culture")
        {
            string result = MagmaLightWeb.Common.CultureInitializer.InitializeCulture(context.Request, context.Response, context.Session);

            if (context.Response.Cookies["CountryId"] != null) result += "_" + context.Response.Cookies["CountryId"].Value;
            
            return result;
        }

        return base.GetVaryByCustomString(context, custom);
    }

    void Application_PreRequestHandlerExecute(object sender, EventArgs e)
    {
        HttpApplication app = sender as HttpApplication;
        string acceptEncoding = app.Request.Headers["Accept-Encoding"];
        System.IO.Stream prevUncompressedStream = app.Response.Filter;

        if (!(app.Context.CurrentHandler is Page ||
            app.Context.CurrentHandler.GetType().Name == "SyncSessionlessHandler") ||
            app.Request["HTTP_X_MICROSOFTAJAX"] != null)
            return;

        if (acceptEncoding == null || acceptEncoding.Length == 0)
            return;

        //acceptEncoding = acceptEncoding.ToLower();

        //if (acceptEncoding.Contains("deflate") || acceptEncoding == "*")
        //{
        //    // defalte
        //    app.Response.Filter = new System.IO.Compression.DeflateStream(prevUncompressedStream,
        //        System.IO.Compression.CompressionMode.Compress);
        //    app.Response.AppendHeader("Content-Encoding", "deflate");
        //}
        //else if (acceptEncoding.Contains("gzip"))
        //{
        //    // gzip
        //    app.Response.Filter = new System.IO.Compression.GZipStream(prevUncompressedStream,
        //        System.IO.Compression.CompressionMode.Compress);
        //    app.Response.AppendHeader("Content-Encoding", "gzip");
        //}
    }    
           
</script>
