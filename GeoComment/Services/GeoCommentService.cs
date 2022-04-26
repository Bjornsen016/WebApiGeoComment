using AutoMapper;
using GeoComment.DTOs;
using Microsoft.EntityFrameworkCore;

namespace GeoComment.Services;

public class GeoCommentService
{
    private readonly GeoDbContext _geoDbContext;

    public GeoCommentService(GeoDbContext geoDbContext)
    {
        _geoDbContext = geoDbContext;
    }

    public async Task<Comment> GetCommentById(int id)
    {
        var comment = await _geoDbContext.Comments
            .Include(c => c.Author)
            .FirstOrDefaultAsync(c => c.Id == id);

        return comment;
    }

    public async Task<Comment> CreateCommentInDb(Comment comment)
    {
        //TODO: Make it use id instead later.
        var author = await _geoDbContext.Authors.FirstOrDefaultAsync(a => a.Name == comment.Author.Name);

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

        /*var cmrtn = _mapper.Map<CommentReturnDTO>(cmt.Entity);

        var commentReturn = new CommentReturnDTO
        {
            Author = cmt.Entity.Author.Name,
            Id = cmt.Entity.Id,
            Latitude = cmt.Entity.Latitude,
            Longitude = cmt.Entity.Longitude,
            Message = cmt.Entity.Message
        };*/

        return cmt.Entity;
    }

    public async Task<List<Comment>> GetComments(double minLon, double maxLon, double minLat, double maxLat)
    {
        var query = _geoDbContext.Comments.Include(c => c.Author)
            .Where(c =>
                c.Latitude >= minLat &&
                c.Latitude <= maxLat &&
                c.Longitude >= minLon &&
                c.Longitude <= maxLon);

        var comments = await query.ToListAsync();

        return comments;
    }
}

public class AuthorNotFoundException : Exception
{
}