namespace MediaTracker;

public partial class AvailableNowPage : ContentPage
{
	public AvailableNowPage()
	{
        InitializeComponent();
        MovieList.ItemsSource = LibraryPage.Instance.movieList
            .Where(m => m.Availability == AvailabilityStatusEnum.AVAILABLE_NOW);

        TVShowList.ItemsSource = LibraryPage.Instance.tvShowList
            .Where(m => m.Availability == AvailabilityStatusEnum.AVAILABLE_NOW);

        BookList.ItemsSource = LibraryPage.Instance.bookList
            .Where(m => m.Availability == AvailabilityStatusEnum.AVAILABLE_NOW);

        LibraryPage.Instance.LibraryUpdated += OnLibraryUpdated;

    }
    private void OnLibraryUpdated(object sender, EventArgs e)
    {
        // Update list views with latest filtered data
        MovieList.ItemsSource = LibraryPage.Instance.movieList.Where(m => m.Availability == AvailabilityStatusEnum.AVAILABLE_NOW);
        TVShowList.ItemsSource = LibraryPage.Instance.tvShowList.Where(m => m.Availability == AvailabilityStatusEnum.AVAILABLE_NOW);
        BookList.ItemsSource = LibraryPage.Instance.bookList.Where(m => m.Availability == AvailabilityStatusEnum.AVAILABLE_NOW);
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