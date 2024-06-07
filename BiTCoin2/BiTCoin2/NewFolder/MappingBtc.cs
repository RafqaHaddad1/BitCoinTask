using AutoMapper;
using System;

namespace BiTCoin2.NewFolder
{
    public class MappingBtc : Profile
    {
        public MappingBtc()
        {
            CreateMap<BTC1, BCT12>()
                 .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.id))
                 .ForMember(dest => dest.LastPrices, opt => opt.MapFrom(src =>  src.last ))
                 .ForMember(dest => dest.SourceID, opt => opt.MapFrom(_ => 1))
                 .ForMember(dest => dest.timestamp, opt => opt.MapFrom(src => src.ConvertTimestampToDateTime()));

            CreateMap<BCT2, BCT12>()
                .ForMember(dest => dest.id, opt => opt.Ignore())
                .ForMember(dest => dest.LastPrices, opt => opt.MapFrom(src => src.last_price ))
                .ForMember(dest => dest.SourceID, opt => opt.MapFrom(_ => 2))
                .ForMember(dest => dest.timestamp, opt => opt.MapFrom(src => src.ConvertTimestampToDateTime()));
        }
    }
}
