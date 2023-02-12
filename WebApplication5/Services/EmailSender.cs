using Microsoft.AspNetCore.Identity.UI.Services;

namespace LeaveManagement.Web.Services
{
    public class EmailSender : IEmailSender
    {
        private string v1;
        private int v2;
        private string v3;

        public EmailSender(string v1, int v2, string v3)
        {
            this.v1 = v1;
            this.v2 = v2;
            this.v3 = v3;
        }
    }
}
