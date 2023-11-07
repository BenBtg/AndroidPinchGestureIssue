namespace AndroidPinchGestureIssue;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
		BindingContext = new MainViewModel();
	}

    void PinchGestureRecognizer_PinchUpdated(object sender, PinchGestureUpdatedEventArgs e)
    {
		((sender as Image).Parent as ZoomImageContentView).OnPinchUpdated(sender, e);
    }
}


