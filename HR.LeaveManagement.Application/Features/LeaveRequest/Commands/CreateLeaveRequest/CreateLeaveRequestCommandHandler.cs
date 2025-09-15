using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Email;
using HR.LeaveManagement.Application.Contracts.Logging;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.Exceptions;
using HR.LeaveManagement.Application.Models.Email;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Commands.CreateLeaveRequest
{
    public class CreateLeaveRequestCommandHandler : IRequestHandler<CreateLeaveRequestCommand, Unit>
    {
        private readonly ILeaveTypeRepository _leaveTypeRepository;
        private readonly ILeaveRequestRepository _leaveRequestRepository;
        private readonly IMapper _mapper;
        private readonly IAppLogger<CreateLeaveRequestCommandHandler> _logger;
        private readonly IEmailSender _emailSender;

        public CreateLeaveRequestCommandHandler(
            ILeaveTypeRepository leaveTypeRepository,
            ILeaveRequestRepository leaveRequestRepository,
            IMapper mapper,
            IAppLogger<CreateLeaveRequestCommandHandler> logger,
            IEmailSender emailSender

            )
        {
            _leaveTypeRepository = leaveTypeRepository;
            _leaveRequestRepository = leaveRequestRepository;
            _mapper = mapper;
            _logger = logger;
            _emailSender = emailSender;
        }

        public async Task<Unit> Handle(CreateLeaveRequestCommand request, CancellationToken cancellationToken)
        {
            var validator = new CreateLeaveRequestCommandValidator(_leaveTypeRepository);
            var validationResult = await validator.ValidateAsync(request);

            if (validationResult.Errors.Any())
                throw new BadRequestException("Invalid Leave Request", validationResult);

            var leaveRequest = _mapper.Map<Domain.LeaveRequest>(request);
            await _leaveRequestRepository.CreateAsync(leaveRequest);

            try
            {
                var email = new EmailMessage
                {
                    To = string.Empty,
                    Body = $"Your leave request for {request.StartDate:D} to {request.EndDate:D}" +
                           $"has been submitted successfully.",
                    Subject = "Leave Request submitted"
                };
                await _emailSender.SendEmail(email);

            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.Message);
            }

            return Unit.Value;

        }
    }
}
