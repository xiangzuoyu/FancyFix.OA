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
            CreateMap<ProductInfoModel, Product_Info>()
                .ForMember(o => o.Id, opt => opt.MapFrom(o => o.id))
                .ForMember(o => o.Content, opt => opt.Ignore())
                .ForMember(o => o.FirstPic, opt => opt.Ignore())
                .ForMember(o => o.Pics, opt => opt.Ignore())
                .ForMember(o => o.Attachment, opt => opt.Ignore())
                .ForMember(o => o.Videos, opt => opt.Ignore());
        }
    }
}
