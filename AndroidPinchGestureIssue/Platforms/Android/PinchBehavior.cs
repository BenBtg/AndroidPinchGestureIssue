using System;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using AndroidPinchGestureIssue.Platforms.Android;
using Microsoft.Maui.Platform;
using MauiView = Microsoft.Maui.Controls.View;
using AndroidView = Android.Views.View;

namespace AndroidPinchGestureIssue;

public partial class PinchBehavior : PlatformBehavior<MauiView, AndroidView>
{
    private ScaleGestureDetector mScaleGestureDetector;

    protected override void OnAttachedTo(MauiView bindable, AndroidView platformView)
    {
        base.OnAttachedTo(bindable, platformView);

        if (bindable is null)
            return;

        ApplyScaleGestureDetector(platformView);
    }

    protected override void OnDetachedFrom(MauiView bindable, AndroidView platformView)
    {
        base.OnDetachedFrom(bindable, platformView);

        if (bindable is null)
            return;
        ClearScaleGestureDetector(platformView);
    }

    void ApplyScaleGestureDetector(AndroidView view)
    {
        view.Touch += ImageView_Touch;
        mScaleGestureDetector = new ScaleGestureDetector(view.Context, new ScaleGestureListener(view));
    }

    void ClearScaleGestureDetector(AndroidView view)
    {
        view.Touch -= ImageView_Touch;
        mScaleGestureDetector = null;
    }

    private void ImageView_Touch(object sender, Android.Views.View.TouchEventArgs e)
    {
        LogTouchEvent(e.Event);

        mScaleGestureDetector?.OnTouchEvent(e.Event);
    }

    private void LogTouchEvent(MotionEvent e)
    {
        if (e.Action == MotionEventActions.Down)
        {
            Console.WriteLine("Touch event started at X: " + e.GetX() + ", Y: " + e.GetY());
        }
        else if (e.Action == MotionEventActions.Move)
        {
            Console.WriteLine("Finger moved to X: " + e.GetX() + ", Y: " + e.GetY());
        }
        else if (e.Action == MotionEventActions.Up)
        {
            Console.WriteLine("Touch event ended at X: " + e.GetX() + ", Y: " + e.GetY());
        }
    }
}

