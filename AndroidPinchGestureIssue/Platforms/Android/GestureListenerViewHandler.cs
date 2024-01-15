using System;
using Android.Graphics.Drawables;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using AndroidViews = Android.Views;


namespace AndroidPinchGestureIssue
{
	public partial class GestureListenerViewHandler: ViewHandler<GestureListenerView, AndroidViews.View>
	{
        protected override AndroidViews.View CreatePlatformView()
        {
            return new GestureListenerAndroidView(this.Context)
            { Background = new ColorDrawable(Color.FromArgb("#33FF0055").ToPlatform())}; 
        }
    }
}

