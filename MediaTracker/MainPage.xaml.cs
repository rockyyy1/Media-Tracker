
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using static System.Net.WebRequestMethods;

// start date : 05/09/2024
namespace MediaTracker
{
    public partial class MainPage : ContentPage
    {
        string API_KEY = "&apikey=4210cd04";
        string film_search = "http://www.omdbapi.com/?t=";
        string book_search = "https://openlibrary.org/search.json?title=";

        private Movie movieData;
        private TVShow tvData;
        private Book bookData;

        public MainPage()
        {
            InitializeComponent();
        }

        // The Open Movie Database: https://www.omdbapi.com/
        // Terms of use: https://www.omdbapi.com/legal.htm
        async void ReadAPI(string websiteURL, string type)
        {
            try
            {
                var client = new HttpClient();
                var response = await client.GetAsync(websiteURL);

                //http response is successfull
                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine(responseString);

                    switch (type)
                    {
                        case "movie":
                            movieData = JsonConvert.DeserializeObject<Movie>(responseString);
                            LoadSuggestion(movieData);
                            break;
                        case "tv":
                            tvData = JsonConvert.DeserializeObject<TVShow>(responseString);
                            LoadSuggestion(tvData);
                            break;
                        default:
                            Debug.WriteLine("Invalid type");
                            break;
                    }
                }
                else
                {
                    await DisplayAlert("Connection Error", "Check API Address\n" + websiteURL, "Ok");
                    Debug.WriteLine($"Error - check API address");
                }
            }
            catch (HttpRequestException ex)
            {
                Debug.WriteLine($"Network error: {ex.Message}");

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unexpected error: {ex.Message}");
            }
        }

        //API: https://openlibrary.org/dev/docs/api/search
        async void ReadBookAPI(string websiteURL)
        {
            try
            {
                var client = new HttpClient();
                var response = await client.GetAsync(websiteURL);

                //http response is successfull
                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    //Debug.WriteLine(responseString);

                    BookResponse bookResponse = JsonConvert.DeserializeObject<BookResponse>(responseString);
                    Book bookData = bookResponse.docs.FirstOrDefault();
                    //LoadSuggestion()
                    string title = bookData.Title;
                    string author = bookData.author_name[0];
                    int year_published = bookData.first_publish_year;
                    int pages = bookData.number_of_pages_median;
                    string bookCoverKey = bookData.cover_edition_key;

                    string key = bookData.key;
                    //Debug.WriteLine(key);
                    var client_second = new HttpClient();
                    string worksURL = "https://openlibrary.org" + key + ".json";
                    Debug.WriteLine(worksURL);
                    var response_second = await client_second.GetAsync(worksURL);
                    var responseString_second = await response_second.Content.ReadAsStringAsync();
                    //Debug.WriteLine(responseString_second);

                    var settings = new JsonSerializerSettings();
                    settings.Converters.Add(new DescriptionConverter());

                    Book bookResponse_second = JsonConvert.DeserializeObject<Book>(responseString_second, settings);
                    string plot = "N/A";

                    if (bookResponse_second.description != null)
                    {
                        plot = bookResponse_second.description?.description ?? bookResponse_second.description?.value;
                    }

                    Debug.WriteLine(plot);
                    bookData.Plot = plot;
                    Debug.WriteLine(bookData.Plot);


                    JSONresponse.Text =
                    "Title: " + title + "\n" +
                    "Author: " + author + "\n" +
                    "Year Published: " + year_published + "\n" +
                    "Pages: " + pages + "\n" +
                    "Plot: " + plot + "\n";

                    string bookCoverURL = "https://covers.openlibrary.org/b/olid/" + bookCoverKey + ".jpg";
                    //Debug.WriteLine(bookCoverURL);
                    posterImage.Source = ImageSource.FromUri(new Uri(bookCoverURL));

                }

                else
                {
                    await DisplayAlert("Connection Error", "Check API Address\n" + websiteURL, "Ok");
                    Debug.WriteLine($"Error - check API address");
                }
            }
            catch (HttpRequestException ex)
            {
                Debug.WriteLine($"Network error: {ex.Message}");

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
        //Display Movie suggestion
        private void LoadSuggestion(Movie movieData)
        {
            string title = movieData.Title;
            string year = movieData.Year;
            string plot = movieData.Plot;
            string genre = movieData.Genre;
            string imbd = movieData.imdbRating;
            string director = movieData.Director;
            string runtime = movieData.Runtime;
            DateTime released = movieData.Released;
            string dateReleased = released.Date.ToShortDateString();
            string posterURL = movieData.Poster;
            JSONresponse.Text =
                "Title: " + title + "\n" +
                "Year: " + year + "\n" +
                "Director: " + director + "\n" +
                "Genre: " + genre + "\n" +
                "Plot: " + plot + "\n" +
                "Runtime: " + runtime + "\n" +
                "Release Date: " + dateReleased + "\n" +
                "IMDB Rating: " + imbd;

            posterImage.Source = ImageSource.FromUri(new Uri(posterURL));
        }

        //Display TVShow suggestion
        private void LoadSuggestion(TVShow showData)
        {
            string title = showData.Title;
            string year = showData.Year;
            string plot = showData.Plot;
            string genre = showData.Genre;
            string imbd = showData.imdbRating;
            string writer = showData.Writer;
            string runtime = showData.Runtime;
            DateTime released = showData.Released;
            string dateReleased = released.Date.ToShortDateString();
            string seasons = showData.totalSeasons;
            string posterURL = tvData.Poster;

            JSONresponse.Text =
                "Title: " + title + "\n" +
                "Year: " + year + "\n" +
                "Writer: " + writer + "\n" +
                "Genre: " + genre + "\n" +
                "Plot: " + plot + "\n" +
                "Runtime: " + runtime + "\n" +
                "Release Date: " + dateReleased + "\n" +
                "Number of Seasons: " + seasons + "\n" +
                "IMDB Rating: " + imbd;
            posterImage.Source = ImageSource.FromUri(new Uri(posterURL));

        }

        private void SearchBtn_Clicked(object sender, EventArgs e)
        {
            string selectedMediaType = GetSelectedMediaType();

            switch (selectedMediaType)
            {
                case "Movie":
                    //Debug.WriteLine("Issa movie!");
                    string movieTitle = titleEntry.Text;
                    string movieRequest = film_search + movieTitle + API_KEY;
                    ReadAPI(movieRequest, "movie");
                    break;
                case "TVShow":
                    //Debug.WriteLine("Issa tvshow!");
                    string TVTitle = titleEntry.Text;
                    string TVRequest = film_search + TVTitle + API_KEY;
                    ReadAPI(TVRequest, "tv");
                    break;
                case "Book":
                    //Debug.WriteLine("Issa book!");
                    string bookTitle = titleEntry.Text;
                    string bookRequest = book_search + bookTitle;
                    ReadBookAPI(bookRequest);
                    break;
                default:
                    Debug.WriteLine("No media type selected");
                    break;
            }
        }

        private string GetSelectedMediaType()
        {
            foreach (var child in MediaTypeStackLayout.Children)
            {
                if (child is RadioButton radioButton && radioButton.IsChecked)
                {
                    return radioButton.Value.ToString();
                }
            }
            return null;
        }
    }

}
