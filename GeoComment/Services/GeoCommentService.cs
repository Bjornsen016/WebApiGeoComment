using AutoMapper;
using GeoComment.DTOs;
using Microsoft.EntityFrameworkCore;

namespace GeoComment.Services;

public class GeoCommentService
{
    private readonly GeoDbContext _geoDbContext;
    private readonly IMapper _mapper;

    public GeoCommentService(GeoDbContext geoDbContext, IMapper mapper)
    {
        _geoDbContext = geoDbContext;
        _mapper = mapper;
    }

    public async Task<CommentReturnDTO> GetCommentById(int id)
    {
        var com = new Comment
            {Author = new GeoUser {Id = 1, Name = "Johan"}, Id = 1, Latitude = 1, Longitude = 1, Message = "Hej"};
        var cm = _mapper.Map<CommentReturnDTO>(com);
        var comment = await _geoDbContext.Comments
            .Include(c => c.Author)
            .FirstOrDefaultAsync(c => c.Id == id);
        var cc = _mapper.Map<CommentReturnDTO>(comment);
        return cc;
    }

    public async Task<CommentReturnDTO> CreateCommentInDb(CommentInputDTO comment)
    {
        //TODO: Make it use id instead later.
        var author = await _geoDbContext.Authors.FirstOrDefaultAsync(a => a.Name == comment.Author);

        if (author is null) throw new AuthorNotFoundException();

        var newComment = new Comment
        {
            Latitude = comment.Latitude,
            Longitude = comment.Longitude,
            Message = comment.Message,
            Author = author
        };

        var cmt = await _geoDbContext.AddAsync(newComment);
        await _geoDbContext.SaveChangesAsync();

        var commentReturn = new CommentReturnDTO
        {
            Author = cmt.Entity.Author.Name,
            Id = cmt.Entity.Id,
            Latitude = cmt.Entity.Latitude,
            Longitude = cmt.Entity.Longitude,
            Message = cmt.Entity.Message
        };

        return commentReturn;
    }

    public async Task<List<CommentReturnDTO>> GetComments(double minLon, double maxLon, double minLat, double maxLat)
    {
        var query = _geoDbContext.Comments
            .Where(c =>
                c.Latitude >= minLat &&
                c.Latitude <= maxLat &&
                c.Longitude >= minLon &&
                c.Longitude <= maxLon)
            .Select(com => new CommentReturnDTO
            {
                Author = com.Author.Name,
                Id = com.Id,
                Latitude = com.Latitude,
                Longitude = com.Longitude,
                Message = com.Message
            });
        var comments = await query.ToListAsync();

        return comments;
    }
}

public class AuthorNotFoundException : Exception
{
}