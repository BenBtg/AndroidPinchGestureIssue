using System;
namespace AndroidPinchGestureIssue;

public class SwiperProperties
{
    public static readonly BindableProperty EnableSwipeProperty =
        BindableProperty.CreateAttached("EnableSwipe", typeof(bool), typeof(SwiperProperties), false);

    public static bool GetEnabledSwipe(BindableObject view)
    {
        return (bool)view.GetValue(EnableSwipeProperty);
    }

    public static void SetEnabledSwipe(BindableObject view, bool value)
    {
        view.SetValue(EnableSwipeProperty, value);
    }
}