using Application.Dto.CustomerMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Services.CustomerMessageService
{
    public interface ICustomerMessageService
    {
        Task<CustomerMessageDto> SaveCustomerMessageAsync(CustomerMessageDto message);
    }
}
