using Newtonsoft.Json;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using SQLite;
using Microsoft.Maui.Storage;



namespace MediaTracker;

public partial class LibraryPage : ContentPage
{
    public static LibraryPage? Instance { get; private set; }
    public static List<MediaItem>? Library { get; set; }
    public List<Movie> movieList { get; set; } = new List<Movie>();
    public List<TVShow> tvShowList { get; set; } = new List<TVShow>();
    public List<Book> bookList { get; set; } = new List<Book>();
    public event EventHandler LibraryUpdated;

    SQLiteAsyncConnection LibraryDatabase;
    


    public LibraryPage()
    {
        InitializeComponent();
        Instance = this;
        Library = new List<MediaItem>();

        /*var localDataPath = FileSystem.Current.AppDataDirectory;
        var filePath = Path.Combine(localDataPath, "database.sql");
        LibraryDatabase = new SQLiteAsyncConnection(filePath);
        LibraryDatabase.CreateTableAsync<MediaItem>();*/
        //uses sql database for loading/saving
        LoadDatabase();

        //uses json for loading/saving
        //LoadLibrary("userLibrary.json");
    }

    //Refresh Lists
    public void RefreshListDisplay()
    {
        movieList = Library.OfType<Movie>().ToList();
        tvShowList = Library.OfType<TVShow>().ToList();
        bookList = Library.OfType<Book>().ToList();
        MovieList.ItemsSource = movieList;
        TVShowList.ItemsSource = tvShowList;
        BookList.ItemsSource = bookList;
        LibraryUpdated?.Invoke(this, EventArgs.Empty);
        //json
        //SaveLibrary("userLibrary.json");

        //sql database
        //SaveDatabase();
    }
    //Toggle List visibility
    private void ToggleMovieListVisibility(object sender, EventArgs e)
    {
        tooltip.IsVisible = false;
        MovieList.IsVisible = !MovieList.IsVisible;
        MovieTapImage.Source = MovieList.IsVisible ? "show.png" : "hide.png";
    }
    private void ToggleTVShowListVisibility(object sender, EventArgs e)
    {
        tooltip.IsVisible = false;
        TVShowList.IsVisible = !TVShowList.IsVisible;
        TVShowTapImage.Source = TVShowList.IsVisible ? "show.png" : "hide.png";

    }
    private void ToggleBookListVisibility(object sender, EventArgs e)
    {
        tooltip.IsVisible = false;
        BookList.IsVisible = !BookList.IsVisible;
        BookTapImage.Source = BookList.IsVisible ? "show.png" : "hide.png";
    }
    //User clicking on Item in list
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
    //loads pre-existing json file
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
            // Load pre-loaded data from exampleLibrary.json in Resources\Raw
            try
            {
                var assembly = Assembly.GetExecutingAssembly();

                //read exampleLibrary.json in Resources\Raw
                using var stream = assembly.GetManifestResourceStream("MediaTracker.Resources.Raw.exampleLibrary.json");

                if (stream != null)
                {
                    var settings = new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Auto
                    };
                    using var reader = new StreamReader(stream);
                    var json = reader.ReadToEnd();
                    var mediaItems = JsonConvert.DeserializeObject<List<MediaItem>>(json, settings) ?? new List<MediaItem>();

                    Library = mediaItems;
                    movieList = mediaItems.OfType<Movie>().ToList();
                    tvShowList = mediaItems.OfType<TVShow>().ToList();
                    bookList = mediaItems.OfType<Book>().ToList();
                    RefreshListDisplay();
                }
                else
                {
                    Debug.WriteLine("Resource file not found.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error loading example library: " + ex.Message);
            }
        }
           /* else
            {
                // Create new lists
                Library = new List<MediaItem>();
                movieList = new List<Movie>();
                tvShowList = new List<TVShow>();
                bookList = new List<Book>();
            }*/
    }
    //saves json file to appdata
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
    public void LoadDatabase()
    {
        var localDataPath = FileSystem.Current.AppDataDirectory;
        var filePath = Path.Combine(localDataPath, "database.sql");
        Debug.WriteLine(filePath);
        LibraryDatabase = new SQLiteAsyncConnection(filePath);

        LibraryDatabase.CreateTableAsync<MediaItem>().Wait();
        //LibraryDatabase.CreateTableAsync<Movie>().Wait();
    }
    public void SaveDatabase()
    {
        var localFolder = FileSystem.Current.AppDataDirectory;
        var filePath = Path.Combine(localFolder, "database.sql");
    }
    public void AddToDatabase(MediaItem item)
    {
        LibraryDatabase.InsertAsync(item).Wait();
        List<Movie> list = LibraryDatabase.Table<Movie>().ToListAsync().Result;
        foreach (Movie i in list)
        {
            Debug.WriteLine("ID: " + i.ID + " Name: " + i.Title);
        }
    }
}