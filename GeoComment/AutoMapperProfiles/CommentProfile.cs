using AutoMapper;
using GeoComment.DTOs;

namespace GeoComment.AutoMapperProfiles;

public class CommentProfile : Profile
{
    public CommentProfile()
    {
        CreateMap<CommentInputDTO, Comment>(); //TODO: Kolla hur det ska fungera med input till comment.
        CreateMap<Comment, CommentReturnDTO>().ForMember(dest => dest.Author, opt =>
            opt.MapFrom(src => src.Author.UserName));

        //TODO: Fixa mapping med v2
    }
}