using FancyFix.Mapper.Profiles;

namespace FancyFix.Mapper
{
    public class ProfileRegister
    {
        public static void Register()
        {
            AutoMapper.Mapper.Initialize(cfg =>
           {
               cfg.AddProfile<ModelToViewModelProfile>();
               cfg.AddProfile<ViewModelToModelProfile>();
           });
            //严格验证-因为model之间字段个数不同，不需要验证验证
            //Mapper.AssertConfigurationIsValid();
        }
    }
}
