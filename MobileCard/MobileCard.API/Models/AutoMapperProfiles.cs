using AutoMapper;
using MobileCard.API.Models.DataModels;
using MobileCard.API.Models.Entities;

namespace MobileCard.API.Models
{
    public class ViewModelToEntityProfile : Profile
    {
        public ViewModelToEntityProfile()
        {
            CreateMap<AccountEnrollmentViewModel, EnrollmentApplication>();
        }
    }

    public class EntityToViewModelProfile : Profile
    {
        public EntityToViewModelProfile()
        {
            CreateMap<ApplicationUser, AccountViewModel>();
            CreateMap<Resource, BasicResourceViewModel>()
                .ForMember(x => x.Id, opt => opt.MapFrom(x => x.ShortId));
        }
    }
}
