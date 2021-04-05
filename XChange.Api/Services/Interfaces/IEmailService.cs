using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XChange.Api.DTO;

namespace XChange.Api.Services.Interfaces
{
    public interface IEmailService
    {
        void SendEmail(Message message);
    }
}
