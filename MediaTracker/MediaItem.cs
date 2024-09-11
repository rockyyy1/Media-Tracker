using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Globalization;
using System.Text.Json.Serialization;

public enum UserStatusEnum
{
    WATCHLIST, 
    CURRENTLY_VIEWING,
    COMPLETED
}
public enum AvailabilityStatusEnum
{
    AVAILABLE_NOW,
    COMING_SOON
}
public class MediaItem
{
    public string? Title { get; set; }
    public string? Plot { get; set; }
    public string? Type { get; set; }
    public DateTime Date { get; set; }
    public UserStatusEnum UserStatus { get; set; }
    public AvailabilityStatusEnum Availability { get; set; }
}
public class Movie : MediaItem
{
    public string? Year { get; set; }
    public string? Rated { get; set; }
    public string? Released { get; set; }
    public string? DaysUntilRelease
    {
        get
        {
            if (string.IsNullOrEmpty(Released))
            {
                return "Release date not available";
            }
            else
            {
                return DateTime.Parse(Released).Subtract(DateTime.Now).Days.ToString();
            }
        }
    }
public string? Runtime { get; set; }
    public string? Genre { get; set; }
    public string? Director { get; set; }
    public string? Writer { get; set; }
    public string? Actors { get; set; }
    public string? Country { get; set; }
    public string? Awards { get; set; }
    public string? Poster { get; set; }
    public string? Metascore { get; set; }
    public string? imdbRating { get; set; }
    public string? BoxOffice { get; set; }
}
public class TVShow : MediaItem
{
    public string? Year { get; set; }
    public string? Rated { get; set; }
    public string? Released { get; set; }
    public string? DaysUntilRelease
    {
        get
        {
            if (string.IsNullOrEmpty(Released))
            {
                return "Release date not available";
            }
            else
            {
                return DateTime.Parse(Released).Subtract(DateTime.Now).Days.ToString();
            }
        }
    }
    public string? Runtime { get; set; }
    public string? Genre { get; set; }
    public string? Director { get; set; }
    public string? Writer { get; set; }
    public string? Actors { get; set; }
    public string? Country { get; set; }
    public string? Awards { get; set; }
    public string? Poster { get; set; }
    public string? Metascore { get; set; }
    public string? imdbRating { get; set; }
    public string? BoxOffice { get; set; }
    public string? totalSeasons { get; set; }
}
public class Book : MediaItem
{
    public int totalItems { get; set; }
    public Volumeinfo? volumeInfo { get; set; }
    public items[]? items { get; set; }
    //public string? DaysUntilRelease { get; set; }
    public string? DaysUntilRelease
    {
        get
        {
            if (volumeInfo?.publishedDate == null || volumeInfo.publishedDate.Length == 4)
            {
                return "N/A";
            }
            else
            {
                DateTime publishedDateTime;
                if (DateTime.TryParseExact(volumeInfo.publishedDate, "d MMM yyyy", null, System.Globalization.DateTimeStyles.None, out publishedDateTime))
                {
                    return (publishedDateTime - DateTime.Today).Days.ToString();
                }
                else
                {
                    return "Invalid date format";
                }
            }
        }
    }
}
public class items
{
    public string? id { get; set; }
}
public class Volumeinfo : Book
{
    public string? title { get; set; }
    public string[]? authors { get; set; }
    public string? publisher { get; set; }
    public string? publishedDate { get; set; }
    public string? description { get; set; }
    public int? pageCount { get; set; }
    public string[]? categories { get; set; }
    public Imagelinks? imageLinks { get; set; }
}
public class Imagelinks
{
    public string? thumbnail { get; set; }
}
