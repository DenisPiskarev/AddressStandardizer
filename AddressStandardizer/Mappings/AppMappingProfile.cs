using AddressStandardizer.Models;
using AutoMapper;
using Newtonsoft.Json;

namespace AddressStandardizer.Mappings
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            CreateMap<string, Rootobject>()
                .ForMember(dest => dest.results, opt => opt.MapFrom(src => JsonConvert.DeserializeObject<Result[]>(src)));
        }
    }
}
