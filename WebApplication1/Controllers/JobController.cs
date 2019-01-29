using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassLibrary1;
using Hangfire;
using Hangfire.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {
        private static readonly ILog Logger = LogProvider.GetLogger(typeof(JobController));
        // POST api/values
        [HttpGet("{input}")]
        public void JobGet(string input)
        {
            Logger.Debug("logger custom");
            //BackgroundJob.Enqueue(() => Console.WriteLine("Fire-and-forget Job Executed"));
            //BackgroundJob.Schedule(() => Console.WriteLine("Delayed job executed"), TimeSpan.FromMinutes(1));
            RecurringJob.AddOrUpdate<IEmailService>((x) => x.EmailSearch(input), Cron.Minutely,queue:"critical" );
            //var id = BackgroundJob.Enqueue(() => Console.WriteLine("Hello, "));
            //BackgroundJob.ContinueWith(id, () => Console.WriteLine("world!"));
        }

       
    }


}

//https://medium.com/@madhufuture/setting-up-windows-service-with-topshelf-and-hangfire-e29fcf250600
//https://www.michaelgmccarthy.com/2016/12/09/web-service-integrations-using-hangfire-in-asp-net-core/
//https://www.michaelgmccarthy.com/2016/11/07/using-hangfire-to-schedule-jobs-in-asp-net-core/