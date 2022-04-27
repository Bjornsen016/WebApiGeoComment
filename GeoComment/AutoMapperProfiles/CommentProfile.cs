using AutoMapper;
using GeoComment.DTOs;
using Microsoft.AspNetCore.Mvc.TagHelpers;

namespace GeoComment.AutoMapperProfiles;

public class CommentProfile : Profile
{
    public CommentProfile()
    {
        CreateMap<CommentInputDTO, Comment>(); //TODO: Kolla hur det ska fungera med input till comment.
        CreateMap<Comment, CommentReturnDTO>()
            .ForMember(dest => dest.Author, opt =>
                opt.MapFrom(src => src.AuthorName));


        //TODO: Fixa mapping med v2
    }
}