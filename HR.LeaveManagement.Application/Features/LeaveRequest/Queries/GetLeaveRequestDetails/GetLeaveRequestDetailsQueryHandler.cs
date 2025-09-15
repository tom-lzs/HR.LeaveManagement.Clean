﻿using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Persistence;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Queries.GetLeaveRequestDetails
{
    public class GetLeaveRequestDetailsQueryHandler : IRequestHandler< GetLeaveRequestDetailsQuery, LeaveRequestDetailsDto>
    {
        private readonly ILeaveRequestRepository _leaveRequestRepository;
        private readonly IMapper _mapper;

        public GetLeaveRequestDetailsQueryHandler(ILeaveRequestRepository leaveRequestRepository, IMapper mapper)
        {
            _leaveRequestRepository = leaveRequestRepository;
            _mapper = mapper;
        }

        public async Task<LeaveRequestDetailsDto> Handle(GetLeaveRequestDetailsQuery request, CancellationToken cancellationToken)
        {
           var leaveRequest = _mapper.Map<LeaveRequestDetailsDto>(
               await _leaveRequestRepository.GetLeaveRequestWithDetails(request.Id));

            return leaveRequest;
        }
    }
}
