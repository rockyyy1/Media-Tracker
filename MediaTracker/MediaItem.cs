using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
    public string Runtime { get; set; }
    public string imdbRating { get; set; }
    public string Poster { get; set; }
}

public class TVShow : MediaItem
{
    public string Writer { get; set; }
    public string Year { set; get; }
    public DateTime Released { get; set; }
    public string Genre { set; get; }
    public string Runtime { get; set; }
    public string totalSeasons { get; set; }
    public string imdbRating { get; set; }
    public string Poster { get; set; }

}
public class BookResponse
{
    public List<Book> docs { get; set; }
}

public class Book : MediaItem
{
    public List<string> author_name { get; set; }
    public string key { get; set; }
    public Description description { get; set; }
    public string cover_edition_key { get; set; }
    public int first_publish_year { get; set; }
    public int number_of_pages_median { get; set; }
}

//used ai to help below:
public class Description
{
    public string type { get; set; }
    public string value { get; set; }
    public string description { get; set; }
}

public class DescriptionConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(Description);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        var description = new Description();

        if (reader.TokenType == JsonToken.StartObject)
        {
            var obj = JObject.Load(reader);
            description.type = obj["type"]?.ToString();
            description.value = obj["value"]?.ToString();
        }
        else if (reader.TokenType == JsonToken.String)
        {
            description.description = reader.Value.ToString();
        }

        return description;
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }
}



