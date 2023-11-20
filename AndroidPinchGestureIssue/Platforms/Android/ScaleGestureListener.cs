using Android.Views;
using Android.Widget;
using static Android.Views.ScaleGestureDetector;
using AndroidView = Android.Views.View;

namespace AndroidPinchGestureIssue.Platforms.Android;

public class ScaleGestureListener(AndroidView view) : SimpleOnScaleGestureListener
{
    float mScaleFactor = 1.0f;

    public override bool OnScale(ScaleGestureDetector detector)
    {
        mScaleFactor *= detector.ScaleFactor;
        mScaleFactor = Math.Max(0.1f,
        Math.Min(mScaleFactor, 10.0f));
        view.ScaleX = mScaleFactor;
        view.ScaleY = mScaleFactor;
        return true;
    }
}
