using AutoMapper;
using FancyFix.OA.Model;
using FancyFix.OA.ViewModel;

namespace FancyFix.Mapper.Profiles
{
    public class ModelToViewModelProfile : Profile
    {
        public ModelToViewModelProfile()
        {
            CreateMap<Develop_Demand, DemandModel>();
        }
    }
}
