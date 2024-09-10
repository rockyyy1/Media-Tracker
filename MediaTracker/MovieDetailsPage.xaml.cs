using System.Diagnostics;

namespace MediaTracker;

public partial class MovieDetailsPage : ContentPage
{
    private Movie item;
    public MovieDetailsPage(Movie item)
    {
        InitializeComponent();
        this.item = item;
        BindingContext = item;
    }

    private void BackBtn_Clicked(object sender, EventArgs e)
    {
        Navigation.PopModalAsync();
    }

    private void MarkCurrentlyViewing_Clicked(object sender, EventArgs e)
    {
        var item = (MediaItem)((Button)sender).BindingContext;
        var libraryItem = LibraryPage.Library.Find(i => i.Title == item.Title && i.GetType() == item.GetType());
        if (libraryItem != null)
        {
            libraryItem.UserStatus = UserStatusEnum.CURRENTLY_VIEWING;
            LibraryPage.Instance.RefreshListDisplay();
        }
        Navigation.PopModalAsync();
    }

    private void MarkCompleted_Clicked(object sender, EventArgs e)
    {
        var item = (MediaItem)((Button)sender).BindingContext;
        var libraryItem = LibraryPage.Library.Find(i => i.Title == item.Title && i.GetType() == item.GetType());
        if (libraryItem != null)
        {
            libraryItem.UserStatus = UserStatusEnum.COMPLETED;
            LibraryPage.Instance.RefreshListDisplay();
        }
        Navigation.PopModalAsync();
    }

    private void RemoveFromLibrary_Clicked(object sender, EventArgs e)
    {
        var item = (MediaItem)((Button)sender).BindingContext;
        var libraryItem = LibraryPage.Library.Find(i => i.Title == item.Title && i.GetType() == item.GetType());
        if (libraryItem != null)
        {
            LibraryPage.Library.Remove(libraryItem);
            LibraryPage.Instance.RefreshListDisplay();
        }
        Navigation.PopModalAsync();
    }
}