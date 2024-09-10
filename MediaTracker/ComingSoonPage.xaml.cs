namespace MediaTracker;

public partial class ComingSoonPage : ContentPage
{
    public ComingSoonPage()
	{
		InitializeComponent();
        MovieList.ItemsSource = LibraryPage.Instance.movieList
            .Where(m => m.Availability == AvailabilityStatusEnum.COMING_SOON).ToList();

        TVShowList.ItemsSource = LibraryPage.Instance.tvShowList
            .Where(m => m.Availability == AvailabilityStatusEnum.COMING_SOON).ToList();

        BookList.ItemsSource = LibraryPage.Instance.bookList
            .Where(m => m.Availability == AvailabilityStatusEnum.COMING_SOON).ToList();

        LibraryPage.Instance.LibraryUpdated += OnLibraryUpdated;

    }
    private void OnLibraryUpdated(object sender, EventArgs e)
    {
        // Update list views with latest filtered data
        MovieList.ItemsSource = LibraryPage.Instance.movieList.Where(m => m.Availability == AvailabilityStatusEnum.COMING_SOON).ToList();
        TVShowList.ItemsSource = LibraryPage.Instance.tvShowList.Where(m => m.Availability == AvailabilityStatusEnum.COMING_SOON).ToList();
        BookList.ItemsSource = LibraryPage.Instance.bookList.Where(m => m.Availability == AvailabilityStatusEnum.COMING_SOON).ToList();
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
}