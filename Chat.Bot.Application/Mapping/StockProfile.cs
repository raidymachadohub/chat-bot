using AutoMapper;
using Chat.Bot.Domain.DTO;
using Chat.Bot.Domain.Model;

namespace Chat.Bot.Application.Mapping
{
    public class StockProfile : Profile
    {
        public StockProfile()
        {
            CreateMap<StockDTO, Stock>()
                .ForMember(x => x.Symbol, p => p.MapFrom(d => d.symbol))
                .ForMember(x => x.Volume, p => p.MapFrom(d => d.volume))
                .ForMember(x => x.Close, p => p.MapFrom(d => d.close))
                .ForMember(x => x.Low, p => p.MapFrom(d => d.low))
                .ForMember(x => x.Time, p => p.MapFrom(d => d.time))
                .ForMember(x => x.Open, p => p.MapFrom(d => d.open))
                .ForMember(x => x.Date, p => p.MapFrom(d => d.date))
                .ReverseMap();
        }
    }
}
