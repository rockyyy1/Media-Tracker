namespace MediaTracker;

public partial class CompletedPage : ContentPage
{
	public CompletedPage()
	{
        InitializeComponent();
        MovieList.ItemsSource = LibraryPage.Instance.movieList
            .Where(m => m.UserStatus == UserStatusEnum.COMPLETED);

        TVShowList.ItemsSource = LibraryPage.Instance.tvShowList
            .Where(m => m.UserStatus == UserStatusEnum.COMPLETED);

        BookList.ItemsSource = LibraryPage.Instance.bookList
            .Where(m => m.UserStatus == UserStatusEnum.COMPLETED);
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