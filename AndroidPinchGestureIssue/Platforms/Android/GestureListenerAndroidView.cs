using System;
using Android.OS;
using Android.Views;
using static Android.Views.ScaleGestureDetector;
using AndroidViews = Android.Views;
using AndroidView = Android.Views.View;

namespace AndroidPinchGestureIssue
{
	public class GestureListenerAndroidView : AndroidViews.View
    {
        private ScaleGestureDetector? mScaleGestureDetector;
        private long _lastDownTime;

        public GestureListenerAndroidView(Android.Content.Context context):base(context)
		{
            mScaleGestureDetector = new ScaleGestureDetector(context, new ScaleListener(this));
        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            System.Diagnostics.Debug.WriteLine(e.PointerCount.ToString());
            System.Diagnostics.Debug.WriteLine(e.DownTime.ToString());
            
            var isTwoFingers = (e.DownTime != _lastDownTime);
            _lastDownTime = e.DownTime;
            if (isTwoFingers)
            {
                mScaleGestureDetector?.OnTouchEvent(e);
            }
            return isTwoFingers;
        }
    }


    public class ScaleListener : SimpleOnScaleGestureListener
    {
        float mScaleFactor = 1.0f;

        AndroidView _view;

        public ScaleListener(AndroidView view)
        {
            _view = view;
        }

        public override bool OnScale(ScaleGestureDetector detector)
        {
            mScaleFactor *= detector.ScaleFactor;
            mScaleFactor = Math.Max(0.1f,
            Math.Min(mScaleFactor, 10.0f));
            _view.ScaleX = mScaleFactor;
            _view.ScaleY = mScaleFactor;
            return true;
        }
    }
}

