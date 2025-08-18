using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.DTOs.EmailMessage
{
    public class Message
    {
        public List<string> To { get; set; } = [];
        public string Subject { get; set; } = default!;
        public string Body { get; set; } = default!;

        public Message(IEnumerable<string> to, string subject, string body)
        {
            To = new List<string>();
            To.AddRange(to.Select(x => x.ToString()));
            Subject = subject;
            Body = body;
        }
    }
    

}
