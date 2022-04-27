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
        var author = await _geoDbContext.Authors.FirstOrDefaultAsync(a => a.UserName == comment.AuthorName);

        Comment newComment = new Comment
        {
            Latitude = comment.Latitude,
            Longitude = comment.Longitude,
            Message = comment.Message,
            AuthorName = comment.AuthorName
        };
        if (author is not null) newComment.Author = author;

        var cmt = await _geoDbContext.AddAsync(newComment);
        await _geoDbContext.SaveChangesAsync();

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