using System;

namespace ClassLibrary1
{

    public interface IEmailService
    {
        void EmailSearch(string emailid);
    }

    [LogEverything]
    public class EmailService : IEmailService
    {
        public void EmailSearch(string emailid)
        {
            Console.WriteLine("Fire-and-forget Job Executed");
            Console.WriteLine("Fire-and-forget Job Executed" + emailid);
        }
    }
}
