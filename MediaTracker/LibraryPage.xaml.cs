using Newtonsoft.Json;
using System.Diagnostics;

namespace MediaTracker;

public partial class LibraryPage : ContentPage
{
    public static LibraryPage? Instance { get; private set; }
    public static List<MediaItem>? Library { get; set; }
    public List<Movie> movieList { get; set; } = new List<Movie>();
    public List<TVShow> tvShowList { get; set; } = new List<TVShow>();
    public List<Book> bookList { get; set; } = new List<Book>();
    public event EventHandler LibraryUpdated;

    public LibraryPage()
    {
        InitializeComponent();
        Instance = this;
        Library = new List<MediaItem>();
        LoadLibrary("userLibrary.json");
    }

    public void RefreshListDisplay()
    {
        movieList = Library.OfType<Movie>().ToList();
        tvShowList = Library.OfType<TVShow>().ToList();
        bookList = Library.OfType<Book>().ToList();
        MovieList.ItemsSource = movieList;
        TVShowList.ItemsSource = tvShowList;
        BookList.ItemsSource = bookList;
        LibraryUpdated?.Invoke(this, EventArgs.Empty);
        SaveLibrary("userLibrary.json");
    }

    private void ToggleMovieListVisibility(object sender, EventArgs e)
    {
        MovieList.IsVisible = !MovieList.IsVisible;
        MovieTapImage.Source = MovieList.IsVisible ? "show.png" : "hide.png";
    }

    private void ToggleTVShowListVisibility(object sender, EventArgs e)
    {
        TVShowList.IsVisible = !TVShowList.IsVisible;
        TVShowTapImage.Source = TVShowList.IsVisible ? "show.png" : "hide.png";

    }

    private void ToggleBookListVisibility(object sender, EventArgs e)
    {
        BookList.IsVisible = !BookList.IsVisible;
        BookTapImage.Source = BookList.IsVisible ? "show.png" : "hide.png";
    }
    public void HandleItemSelected(object sender, ItemTappedEventArgs e)
    {
        var listView = (ListView)sender;

        if (sender == MovieList)
        {
            Movie selectedItem = (Movie)e.Item;
            Navigation.PushModalAsync(new MovieDetailsPage(selectedItem));
        }
        else if (listView == TVShowList)
        {
            TVShow selectedItem = (TVShow)e.Item;
            Navigation.PushModalAsync(new TVShowDetailsPage(selectedItem));
        }
        else if (listView == BookList)
        {
            Book selectedItem = (Book)e.Item;
            Navigation.PushModalAsync(new BookDetailsPage(selectedItem));
        }
    }
    public void LoadLibrary(string fileName)
    {
        var localFolder = FileSystem.Current.AppDataDirectory;
        var filePath = Path.Combine(localFolder, fileName);

        if (File.Exists(filePath))
        {
            Debug.WriteLine(filePath);
            var json = File.ReadAllText(filePath);
            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            };

            try
            {
                // Deserialize JSON into a List<MediaItem>
                var mediaItems = JsonConvert.DeserializeObject<List<MediaItem>>(json, settings) ?? new List<MediaItem>();

                //Filter the deserialized items into respective lists
                movieList = mediaItems.OfType<Movie>().ToList();
                tvShowList = mediaItems.OfType<TVShow>().ToList();
                bookList = mediaItems.OfType<Book>().ToList();

                Library = mediaItems;
                RefreshListDisplay();
            }
            catch (JsonException ex)
            {
                Debug.WriteLine("JSON Deserialization error: " + ex.Message);
                Library = new List<MediaItem>();
                movieList.Clear();
                tvShowList.Clear();
                bookList.Clear();
            }
        }
        else
        {
            // Initialize with empty lists if file does not exist
            Library = new List<MediaItem>();
            movieList = new List<Movie>();
            tvShowList = new List<TVShow>();
            bookList = new List<Book>();
        }
    }

    public void SaveLibrary(string fileName)
    {
        var localFolder = FileSystem.Current.AppDataDirectory;
        var filePath = Path.Combine(localFolder, fileName);

        var settings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            TypeNameHandling = TypeNameHandling.Auto 
        };

        try
        {
            // Serialize the full list of MediaItem, including type information
            var json = JsonConvert.SerializeObject(Library, settings);
            //Debug.WriteLine("Saving JSON: " + json);
            File.WriteAllText(filePath, json);
        }
        catch (Exception ex)
        {
            Debug.WriteLine("Error saving library: " + ex.Message);
        }
    }

}