using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XChange.Api.DTO
{
    public class Message
    {
        public List<MailboxAddress> To { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public Message(IEnumerable<string> to, string subject, string content)
        {
            To = new List<MailboxAddress>();
            //To.AddRange(to.Select(y => new MailboxAddress(y)));
            To.AddRange(to.Select(y => MailboxAddress.Parse(y)));
            Subject = subject;
            Content = content;
        }
    }
}
