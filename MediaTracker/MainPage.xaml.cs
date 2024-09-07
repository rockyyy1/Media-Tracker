
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.Reflection;
using static System.Net.WebRequestMethods;

// start date : 05/09/2024
namespace MediaTracker
{
    public partial class MainPage : ContentPage
    {
        //API for movies/tv
        string API_KEY = "&apikey=4210cd04";
        string film_search = "http://www.omdbapi.com/?t=";

        //API for books
        string GOOG_API_KEY = "&key=AIzaSyAxxY4NqHPLmeht65gkLEprkqsOLzaS4kc";
        string google_book_search = "https://www.googleapis.com/books/v1/volumes?q=";

        public Movie movieData;
        public TVShow tvData;
        public Book volData;
        public List<MediaItem> library = new List<MediaItem>();


        public MainPage()
        {
            InitializeComponent();
        }

        //Fetch api data
        async void ReadAPI(string websiteURL, string mediaType)
        {
            try
            {
                var client = new HttpClient();
                var response = await client.GetAsync(websiteURL);

                //http response is successfull
                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    //Debug.WriteLine("Response string:\n" + responseString);
                    MediaItem mediaItem = JsonConvert.DeserializeObject<MediaItem>(responseString);
                    string type = mediaItem.Type;
                    //Debug.WriteLine(type);
                    switch (mediaType)
                    {
                        case "movie":
                            if (type == "movie")
                            {
                                movieData = JsonConvert.DeserializeObject<Movie>(responseString);
                                LoadSuggestion(movieData);
                            }
                            break;
                        case "tv":
                            if (type == "series")
                            {
                                tvData = JsonConvert.DeserializeObject<TVShow>(responseString);
                                LoadSuggestion(tvData);
                            }
                            break;
                        case "book":
                            Book GbookData = JsonConvert.DeserializeObject<Book>(responseString);
                            // Check for results
                            if (GbookData.totalItems > 0)
                            {
                                // Access the first book information
                                var firstBook = GbookData.items[0];
                                string bookId = firstBook.id;

                                volData = await GetBookDetails(bookId);
                                LoadSuggestion(volData.volumeInfo);
                            }
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
            string rated = movieData.Rated;
            string released = movieData.Released;
            string runtime = movieData.Runtime;
            string genre = movieData.Genre;
            string director = movieData.Director;
            string writer = movieData.Writer;
            string actors = movieData.Actors;
            string plot = movieData.Plot;
            string country = movieData.Country;
            string awards = movieData.Awards;
            string posterURL = movieData.Poster;
            string metascore = movieData.Metascore;
            string imdbRating = movieData.imdbRating;
            string boxOffice = movieData.BoxOffice;

            JSONresponse.Text =
            "Title: " + title + "\n" +
            "Year: " + year + "\n\n" +
            "Plot: " + plot + "\n\n" +
            "Rated: " + rated + "\n" +
            "Released: " + released + "\n" +
            "Runtime: " + runtime + "\n" +
            "Genre: " + genre + "\n" +
            "Director: " + director + "\n" +
            "Writer: " + writer + "\n" +
            "Actors: " + actors + "\n" +
            "Country: " + country + "\n" +
            "Awards: " + awards + "\n" +
            "Metascore: " + metascore + "\n" +
            "IMDB Rating: " + imdbRating + "\n" +
            "Box Office: " + boxOffice + "\n";

            posterImage.Source = ImageSource.FromUri(new Uri(posterURL));

            DateTime releaseDate = DateTime.ParseExact(released, "dd MMM yyyy", null);
            movieData.Availability = CheckAvailability(releaseDate);
            CreateStatusCheckStackLayout(movieData.Availability, "film");
        }

        // Used AI to convert to code to save heaps of time - could have done this in XAML but wanted custom radio button for  each media type
        // Method to create and configure a StackLayout with RadioButtons
        public void CreateStatusCheckStackLayout(AvailabilityStatusEnum availability, string type)
        {
            statusStackLayout.Children.Clear();
            var mainStackLayout = new StackLayout();

            // Create an empty Label (you can set properties like Text, FontSize, etc., if needed)
            var label = new Label();

            // Create a StackLayout for the RadioButton group
            var statusCheckStackLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.Center
            };

            // Create the first RadioButton for "Add to watchlist"
            var watchlistRadioButton = new RadioButton
            {
                Value = "watchlist"
            };

            // Create a StackLayout for the watchlist RadioButton's content
            var watchlistStackLayout = new StackLayout();

            // Create an Image for the watchlist RadioButton
            var watchlistImage = new Image
            {
                Source = "bookmark.png",
                HeightRequest = 30,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };

            // Create a Label for the watchlist RadioButton
            var watchlistLabel = new Label
            {
                Text = "Add to watchlist",
                HorizontalTextAlignment = TextAlignment.Center,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.End
            };

            // Add the Image and Label to the watchlist's StackLayout
            watchlistStackLayout.Children.Add(watchlistImage);
            watchlistStackLayout.Children.Add(watchlistLabel);

            // Assign the StackLayout as the Content for the RadioButton
            watchlistRadioButton.Content = watchlistStackLayout;

            // Create the second RadioButton for "Currently watching"
            var watchingRadioButton = new RadioButton
            {
                Value = "currently_watching"
            };

            // Create a StackLayout for the currently watching RadioButton's content
            var watchingStackLayout = new StackLayout();

            // Create an Image for the currently watching RadioButton
            var watchingImage = new Image
            {
                Source = "eye.png",
                HeightRequest = 30,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };

            // Create a Label for the currently watching RadioButton
            var watchingLabel = new Label
            {
                Text = (type == "film") ? "Currently watching" : "Currently reading",
                HorizontalTextAlignment = TextAlignment.Center,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.End
            };

            // Add the Image and Label to the watching's StackLayout
            watchingStackLayout.Children.Add(watchingImage);
            watchingStackLayout.Children.Add(watchingLabel);

            // Assign the StackLayout as the Content for the RadioButton
            watchingRadioButton.Content = watchingStackLayout;

            // Create the third RadioButton for "Completed"
            var completedRadioButton = new RadioButton
            {
                Value = "completed"
            };

            // Create a StackLayout for the completed RadioButton's content
            var completedStackLayout = new StackLayout();

            // Create an Image for the completed RadioButton
            var completedImage = new Image
            {
                Source = "checkmark.png",
                HeightRequest = 30,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };

            // Create a Label for the completed RadioButton
            var completedLabel = new Label
            {
                Text = "Completed",
                HorizontalTextAlignment = TextAlignment.Center,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.End
            };

            // Add the Image and Label to the completed's StackLayout
            completedStackLayout.Children.Add(completedImage);
            completedStackLayout.Children.Add(completedLabel);

            // Assign the StackLayout as the Content for the RadioButton
            completedRadioButton.Content = completedStackLayout;

            // Add the RadioButtons to the statusCheckStackLayout based on availability
            if (availability == AvailabilityStatusEnum.AVAILABLE_NOW)
            {
                statusCheckStackLayout.Children.Add(watchlistRadioButton);
                statusCheckStackLayout.Children.Add(watchingRadioButton); 
                statusCheckStackLayout.Children.Add(completedRadioButton);
            }
            else if (availability == AvailabilityStatusEnum.COMING_SOON)
            {
                statusCheckStackLayout.Children.Add(watchlistRadioButton);
            }

            // Add the Label and statusCheckStackLayout to the main StackLayout
            //mainStackLayout.Children.Add(label);
            mainStackLayout.Children.Add(statusCheckStackLayout);

            // Set the GroupName for the RadioButton group
            RadioButtonGroup.SetGroupName(statusCheckStackLayout, "CheckTypes");

            statusStackLayout.Children.Add(mainStackLayout);
        }

        //Display TVShow suggestion
        private void LoadSuggestion(TVShow showData)
        {
            string title = showData.Title;
            string year = showData.Year;
            string rated = showData.Rated;
            string released = showData.Released;
            string runtime = showData.Runtime;
            string genre = showData.Genre;
            string writer = showData.Writer;
            string actors = showData.Actors;
            string plot = showData.Plot;
            string country = showData.Country;
            string awards = showData.Awards;
            string posterURL = showData.Poster;
            string metascore = showData.Metascore;
            string imdbRating = showData.imdbRating;
            string seasons = showData.totalSeasons;

            JSONresponse.Text =
            "Title: " + title + "\n" +
            "Year: " + year + "\n\n" +
            "Plot: " + plot + "\n\n" +
            "Total Seasons: " + seasons + "\n" +
            "Rated: " + rated + "\n" +
            "Released: " + released + "\n" +
            "Runtime: " + runtime + "\n" +
            "Genre: " + genre + "\n" +
            "Writer: " + writer + "\n" +
            "Actors: " + actors + "\n" +
            "Country: " + country + "\n" +
            "Awards: " + awards + "\n" +
            "Metascore: " + metascore + "\n" +
            "IMDB Rating: " + imdbRating + "\n";

            posterImage.Source = ImageSource.FromUri(new Uri(posterURL));

            DateTime releaseDate = DateTime.ParseExact(released, "dd MMM yyyy", null);
            showData.Availability = CheckAvailability(releaseDate);

            CreateStatusCheckStackLayout(showData.Availability, "film");
        }

        //Display Google Books suggestion
        private void LoadSuggestion(Volumeinfo volData)
        {
            string title = volData.title;
            string authors = string.Join(", ", volData.authors);
            string year_published = volData.publishedDate;
            DateTime releaseDate;
            string formattedReleaseDate = string.Empty;

            //sometimes json will give 2020-01-01 or just 2020, very inconsistent and frustrating!
            string[] formats = { "yyyy-MM-dd", "yyyy" };
            if (DateTime.TryParseExact(year_published, formats, null, System.Globalization.DateTimeStyles.None, out releaseDate))
            {
                if (year_published.Length == 4)  //json just has the year
                {
                    formattedReleaseDate = releaseDate.ToString("yyyy");
                }
                else  //full date
                {
                    formattedReleaseDate = releaseDate.ToString("d");
                }
            }
            else
            {
                Debug.WriteLine("could not parse publication date");
                formattedReleaseDate = "N/A";
            }

            string publisher = volData.publisher;
            string description = volData.description;
            string categoriesString = volData.categories != null ? string.Join(", ", volData.categories) : string.Empty;
            int pages = volData.pageCount;
            //string bookCoverURL = volData.imageLinks.thumbnail;
            string bookCoverURL = volData.imageLinks != null ? volData.imageLinks.thumbnail : null;

            JSONresponse.Text =
            "Title: " + title + "\n" +
            "Author(s): " + authors + "\n" +
            "Year Published: " + formattedReleaseDate + "\n" +
            "Publisher: " + publisher + "\n" +
            "Categories: " + categoriesString + "\n" +
            "Number of Pages: " + pages + "\n";
            descriptionSummary.Text = "Description: " + description + "\n";


            if (bookCoverURL!=null)
            {
                posterImage.Source = ImageSource.FromUri(new Uri(bookCoverURL));
            }

            volData.Availability = CheckAvailability(releaseDate);
            volData.Title = title;
            volData.Plot = description;

            volData.Availability = CheckAvailability(releaseDate);
            CreateStatusCheckStackLayout(volData.Availability, "book");
        }
        //Search button
        private void SearchBtn_Clicked(object sender, EventArgs e)
        {
            if (titleEntry.Text != "")
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
                        string TVTitle = titleEntry.Text.Replace(" ", "-");
                        string TVRequest = film_search + TVTitle + API_KEY;
                        ReadAPI(TVRequest, "tv");
                        break;
                    case "Book":
                        //Debug.WriteLine("Issa book!");
                        string bookTitle = titleEntry.Text.Replace(" ", "+");
                        string GoogleAPIURL = google_book_search + bookTitle + "+intitle:" + bookTitle + GOOG_API_KEY;
                        ReadAPI(GoogleAPIURL, "book");
                        break;
                    default:
                        Debug.WriteLine("No media type selected");
                        break;
                }
            }
        }

        //Get Radio Button selection
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

        //Return if media is available or not
        public AvailabilityStatusEnum CheckAvailability(DateTime releaseDate)
        {
            AvailabilityStatusEnum status;

            if (releaseDate <= DateTime.Today)
            {
                status = AvailabilityStatusEnum.AVAILABLE_NOW;
                availabilityLbl.Text = "~ Available Now! ~";
                dateOfReleaseLbl.Text = "";
                countdownLbl.Text = "";
            }
            else
            {
                status = AvailabilityStatusEnum.COMING_SOON;
                availabilityLbl.Text = "~ Coming Soon! ~";
                dateOfReleaseLbl.Text = "Release Date: " + releaseDate.ToString("d");
                countdownLbl.Text = "Days until: " + (releaseDate - DateTime.Now).Days.ToString();
            }

            TrackThisBtn.IsVisible = true;

            return status;
        }

        //Get Google Boook details by ID
        public async Task<Book> GetBookDetails(string bookId)
        {
            string url = "https://www.googleapis.com/books/v1/volumes/" + bookId;
            try
            {
                var client = new HttpClient();
                var response = await client.GetAsync(url);
                //http response is successfull
                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();

                    Book i = JsonConvert.DeserializeObject<Book>(responseString);
                    return i;
                }
                else
                {
                    await DisplayAlert("Connection Error", "Check API Address\n" + url, "Ok");
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
            return null;
        }

        //Button to add to library
        private void TrackThisBtn_Clicked(object sender, EventArgs e)
        {
            string selectedMediaType = GetSelectedMediaType();

            switch (selectedMediaType)
            {
                case "Movie":
                    Movie NewMovie = new Movie
                    {
                        Title = movieData.Title,
                        Year = movieData.Year,
                        Rated = movieData.Rated,
                        Released = movieData.Released,
                        Runtime = movieData.Runtime,
                        Genre = movieData.Genre,
                        Director = movieData.Director,
                        Writer = movieData.Writer,
                        Actors = movieData.Actors,
                        Plot = movieData.Plot,
                        Country = movieData.Country,
                        Awards = movieData.Awards,
                        Poster = movieData.Poster,
                        Metascore = movieData.Metascore,
                        imdbRating = movieData.imdbRating,
                        BoxOffice = movieData.BoxOffice,
                        Availability = movieData.Availability
                    };
                    Debug.WriteLine(NewMovie.Title);    
                    break;
                case "TVShow":
                    TVShow newTVShow = new TVShow();
                    break;
                case "Book":
                    Book newBook = new Book();
                    break;
                default:
                    Debug.WriteLine("No media type selected");
                    break;
            }
        }
    }

}
