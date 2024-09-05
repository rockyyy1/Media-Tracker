using System;

public enum StatusEnum
{
    AVAILABLE_NOW,
    CURRENTLY_VIEWING,
    COMING_SOON,
    COMPLETED
}

public class MediaItem
{
    public string Title { get; set; }
    public string Plot { get; set; }
    public StatusEnum Status { get; set; }
}

public class Movie : MediaItem
{
    public string Director { get; set; }
    public string Year { get; set; }
    public DateTime Released { get; set; }
    public string Genre { set; get; }
    public string Runtime { get; set;}
    public string imdbRating { get; set; }
    public string Poster { get; set; }
}

public class TVShow : MediaItem
{
    public string Writer { get; set; }
    public string Year {  set; get; }
    public DateTime Released { get; set; }
    public string Genre {  set; get; }
    public string Runtime { get; set; }
    public string totalSeasons { get; set; }
    public string imdbRating { get; set; }
    public string Poster { get; set; }

}

public class Book : MediaItem
{
    public string Author { get; set; }
    public string Publisher { get; set; }
    public int YearPublication { get; set; }
    public int PageCount { get; set; }
}
