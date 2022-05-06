using Microsoft.EntityFrameworkCore;

namespace GeoComment.Services;

public class GeoCommentService
{
    private readonly GeoDbContext _geoDbContext;

    public GeoCommentService(GeoDbContext geoDbContext)
    {
        _geoDbContext = geoDbContext;
    }

    /// <summary>
    /// Gets the comment with <paramref name="id"/>
    /// </summary>
    /// <param name="id"></param>
    /// <returns>The comment if found</returns>
    public async Task<Comment> GetCommentById(int id)
    {
        var comment = await _geoDbContext.Comments
            .Include(c => c.Author)
            .FirstOrDefaultAsync(c => c.Id == id);

        return comment;
    }

    /// <summary>
    /// Removes the comment from the database if it exists and the correct user owns it.
    /// </summary>
    /// <param name="commentId"></param>
    /// <param name="commentAuthorId">Id of the user that posted the comment</param>
    /// <returns>The deleted comment</returns>
    /// <exception cref="UnauthorizedException"></exception>
    public async Task<Comment> DeleteCommentById(int commentId, string commentAuthorId)
    {
        var comment = await _geoDbContext.Comments
            .Include(c => c.Author)
            .FirstOrDefaultAsync(c => c.Id == commentId);

        if (comment is null || comment.Author is null) return null;
        if (comment.Author.Id != commentAuthorId) throw new UnauthorizedException();

        _geoDbContext.Comments.Remove(comment);
        await _geoDbContext.SaveChangesAsync();

        return comment;
    }

    /// <summary>
    /// Creates the provided <paramref name="comment"/> in the database
    /// </summary>
    /// <param name="comment">The comment to create</param>
    /// <returns>The created comment if successful</returns>
    public async Task<Comment> CreateCommentInDb(Comment comment)
    {
        var author = await _geoDbContext.Authors
            .FirstOrDefaultAsync(a => a.UserName == comment.AuthorName);

        if (author is not null) comment.Author = author;

        var cmt = await _geoDbContext.AddAsync(comment);
        await _geoDbContext.SaveChangesAsync();

        return cmt.Entity;
    }

    /// <summary>
    /// Gets comments according to the provided coordinates
    /// </summary>
    /// <param name="minLon"></param>
    /// <param name="maxLon"></param>
    /// <param name="minLat"></param>
    /// <param name="maxLat"></param>
    /// <returns>A List with the comments</returns>
    public async Task<List<Comment>> GetComments(double minLon, double maxLon, double minLat, double maxLat)
    {
        var query = _geoDbContext.Comments
            .Include(c => c.Author)
            .Where(c =>
                c.Latitude >= minLat &&
                c.Latitude <= maxLat &&
                c.Longitude >= minLon &&
                c.Longitude <= maxLon);

        var comments = await query.ToListAsync();

        return comments;
    }

    /// <summary>
    /// Gets all the comment by the user with the <paramref name="username"/>
    /// </summary>
    /// <param name="username"></param>
    /// <returns>A List with the comments</returns>
    public async Task<List<Comment>> GetCommentsByUser(string username)
    {
        var query = _geoDbContext.Comments
            .Include(c => c.Author)
            .Where(c => c.AuthorName == username || c.Author.UserName == username);

        var comments = await query.ToListAsync();

        return comments;
    }
}

public class UnauthorizedException : Exception
{
}