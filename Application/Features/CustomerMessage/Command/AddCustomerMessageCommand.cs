using Application.Dto.CustomerMessage;
using Application.Dto.Doctor;
using Application.Interfaces.Services.CustomerMessageService;
using Application.Interfaces.Services.DoctorService;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.CustomerMessage.Command
{
    public class AddCustomerMessageCommand : CustomerMessageDto, IRequest<CustomerMessageDto>
    {
    }

    public class AddCustomerMessageCommandHandler : IRequestHandler<AddCustomerMessageCommand, CustomerMessageDto>
    {
        private readonly ICustomerMessageService _customerMessageService;

        public AddCustomerMessageCommandHandler(ICustomerMessageService customerMessageService)
        {
            _customerMessageService = customerMessageService;
        }

        public async Task<CustomerMessageDto> Handle(AddCustomerMessageCommand request, CancellationToken cancellationToken)
        {
            var response = await _customerMessageService.SaveCustomerMessageAsync(request);
            return response;
        }
    }
}
