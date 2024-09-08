namespace MediaTracker;

public partial class CurrentlyWatchingPage : ContentPage
{
	public CurrentlyWatchingPage()
	{
        InitializeComponent();
        MovieList.ItemsSource = LibraryPage.Instance.movieList
            .Where(m => m.UserStatus == UserStatusEnum.CURRENTLY_VIEWING);

        TVShowList.ItemsSource = LibraryPage.Instance.tvShowList
            .Where(m => m.UserStatus == UserStatusEnum.CURRENTLY_VIEWING);

        BookList.ItemsSource = LibraryPage.Instance.bookList
            .Where(m => m.UserStatus == UserStatusEnum.CURRENTLY_VIEWING);
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