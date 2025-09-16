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

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Commands.ChangeLeaveRequestApproval
{
    public class ChangeLeaveRequestApprovalCommandHandler : IRequestHandler<ChangeLeaveRequestApprovalCommand, Unit>
    {
        private readonly ILeaveRequestRepository _leaveRequestRepository;
        private readonly IMapper _mapper;
        private readonly IEmailSender _emailSender;
        private readonly ILeaveTypeRepository _leaveTypeRepository;
        private readonly IAppLogger<ChangeLeaveRequestApprovalCommandHandler> _logger;

        public ChangeLeaveRequestApprovalCommandHandler(
            ILeaveRequestRepository leaveRequestRepository,
            IMapper mapper,
            IEmailSender emailSender,
            ILeaveTypeRepository leaveTypeRepository,
            IAppLogger<ChangeLeaveRequestApprovalCommandHandler> logger
            )
        {
            _leaveRequestRepository = leaveRequestRepository;
            _mapper = mapper;
            _emailSender = emailSender;
            _leaveTypeRepository = leaveTypeRepository;
            _logger = logger;
        }

        public async Task<Unit> Handle(ChangeLeaveRequestApprovalCommand request, CancellationToken cancellationToken)
        {

            var leaveRequest = await _leaveRequestRepository.GetByIdAsync(request.Id);
            if ( leaveRequest == null ) 
                throw new NotFoundException(nameof(leaveRequest), request.Id);

            leaveRequest.Approved = request.Approved;
            await _leaveRequestRepository.UpdateAsync( leaveRequest );

            try
            {
                var email = new EmailMessage
                {
                    To = string.Empty,
                    Body = $"The approval for your leave request for {leaveRequest.StartDate:D} " +
                    $"to {leaveRequest.EndDate:D} has been updated.",
                    Subject = "Leave Request Approval status updated"
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
