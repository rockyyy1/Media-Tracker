
using Newtonsoft.Json;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;

// start date : 05/09/2024
namespace MediaTracker
{
    public partial class MainPage : ContentPage
    {
        string API_KEY = "&apikey=4210cd04";
        string film_search = "http://www.omdbapi.com/?t=";

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
                        case "book":
                            // Do later
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
