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
}