using Android.Views;
using Android.Widget;
using static Android.Views.ScaleGestureDetector;
using AndroidView = Android.Views.View;

namespace AndroidPinchGestureIssue.Platforms.Android;

public class ScaleGestureListener : SimpleOnScaleGestureListener
{
    private readonly AndroidView _view;
    private readonly PinchBehavior _pinchBehavior;
    private float _scaleFactor = 1.0f;

    public ScaleGestureListener(AndroidView view, PinchBehavior pinchBehavior)
    {
        _view = view;
        _pinchBehavior = pinchBehavior;
    }

    public override bool OnScale(ScaleGestureDetector detector)
    {
        _scaleFactor *= detector.ScaleFactor;
        _scaleFactor = Math.Max(0.1f, Math.Min(_scaleFactor, 10.0f));

        // Update the scale of the view
       // _view.ScaleX = _scaleFactor;
       // _view.ScaleY = _scaleFactor;

        // Update the pinch scale in the pinch behavior
        _pinchBehavior.PinchScale = _scaleFactor;

        return true;
    }
}
