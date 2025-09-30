using AutoMapper;
using Hr.LeaveManagement.BlazorUI.Services.Base;
using Hr.LeaveManagement.BlazorUI.Models.LeaveTypes;

namespace Hr.LeaveManagement.BlazorUI.MappingProfiles
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<LeaveTypeDto,LeaveTypeVM>().ReverseMap();
            CreateMap<CreateLeaveTypeCommand, LeaveTypeVM>().ReverseMap();
            CreateMap<UpdateLeaveTypeCommand, LeaveTypeVM>().ReverseMap();

        }
    }
}
