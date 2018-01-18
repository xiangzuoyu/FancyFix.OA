using AutoMapper;
using FancyFix.OA.Model;
using FancyFix.OA.ViewModel;

namespace FancyFix.Mapper.Profiles
{
    public class ViewModelToModelProfile : Profile
    {
        public ViewModelToModelProfile()
        {
            CreateMap<AdminInfoModel, Mng_User>().ForMember(o => o.Id, opt => opt.MapFrom(o => o.id));
        }
    }
}
