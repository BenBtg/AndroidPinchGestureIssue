namespace AndroidPinchGestureIssue;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
		BindingContext = new MainViewModel();
	}

    private void Cv_CurrentItemChanged(object sender, CurrentItemChangedEventArgs e)
    {
        if (cv.VisibleViews.Count > 0)
        {
            foreach (Grid gv in cv.VisibleViews)
            {
                AttachToBehaviorIfCurrent(e.CurrentItem.ToString(), gv);
            }
        }
    }

    private void AttachToBehaviorIfCurrent(string current, Grid gv)
    {
        if (gv != null && gv.Children[0] is Image)
        {
            var first = gv.First();
            if (((first as Image).Source as UriImageSource).Uri.OriginalString == current)
            {
                var pb = gv.Behaviors[0] as PinchBehavior;
                if (pb != null)
                {
                    pb.PropertyChanged += Pb_PropertyChanged;
                }
            }
        }
    }

    private void Pb_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == "SwipeEnabled")
        {
            
            var pb = (sender as PinchBehavior);

            if (pb != null)
            {
                if (pb.SwipeEnabled)
                {
                    Console.WriteLine("Swipe Enabled");
                    cv.IsSwipeEnabled = true;
                }
                else
                {
                    Console.WriteLine("Swipe Disabled");
                    cv.IsSwipeEnabled = false;
                }
            }
        }
    }
    //  void PinchGestureRecognizer_PinchUpdated(object sender, PinchGestureUpdatedEventArgs e)
    //  {
    //((sender as Image).Parent as ZoomImageContentView).OnPinchUpdated(sender, e);
    //  }
}


