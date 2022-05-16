using AutoMapper;

namespace BookingHotel.ApplicationService.AutoMapper
{
    public class AutoMapperConfig
    {
        public static MapperConfiguration RegisterMappings()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ObjectToModelMappingProfile());
                cfg.AddProfile(new ModelToObjectMappingProfile());
            });
        }
    }
}
