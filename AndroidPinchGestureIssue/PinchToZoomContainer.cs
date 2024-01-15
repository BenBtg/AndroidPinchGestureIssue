using System;
namespace AndroidPinchGestureIssue
{
    public class PinchToZoomContainer : ContentView
    {
        double _currentScale = 1;
        double startScale = 1;
        double xOffset = 0;
        double yOffset = 0;

        public const double ConstantMinScale = 1;
        public const double ConstantMaxScale = 8;
        public const double ConstantMinDelta = 0.1;

        private TapGestureRecognizer _tapGesture = new TapGestureRecognizer { NumberOfTapsRequired = 1 };

        public PinchToZoomContainer()
        {
            PinchGestureRecognizer pinchGesture = new PinchGestureRecognizer();
            pinchGesture.PinchUpdated += OnPinchUpdated;
            GestureRecognizers.Add(pinchGesture);

        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            _tapGesture.Tapped += OnTapped;
            this.GestureRecognizers.Add(_tapGesture);
   
        }

        public void OnTapped(object sender, EventArgs e)
        {


            if (Content.Scale > ConstantMinScale)
            {
                Content.ScaleTo(ConstantMinScale, 250, Easing.CubicInOut);
                Content.TranslateTo(0, 0, 250, Easing.CubicInOut);
                _currentScale = 1;
              //  _xOffset = _yOffset = 0;
                if (Parent is CarouselView carouselView && _currentScale == 1)
                {
                    carouselView.IsSwipeEnabled = false;
                }

               // RemovePanGesture();
            }
        }


        void OnPinchUpdated(object sender, PinchGestureUpdatedEventArgs e)
        {
            if (e.Status == GestureStatus.Started)
            {
                if (Parent is CarouselView carouselView)
                {
                    carouselView.IsSwipeEnabled = false;
                }
                // Store the current scale factor applied to the wrapped user interface element,
                // and zero the components for the center point of the translate transform.
                startScale = Content.Scale;
                Content.AnchorX = 0;
                Content.AnchorY = 0;
            }
            if (e.Status == GestureStatus.Running)
            {
                // Calculate the scale factor to be applied.
                _currentScale += (e.Scale - 1) * startScale;
                _currentScale = Math.Max(1, _currentScale);

                // The ScaleOrigin is in relative coordinates to the wrapped user interface element,
                // so get the X pixel coordinate.
                double renderedX = Content.X + xOffset;
                double deltaX = renderedX / Width;
                double deltaWidth = Width / (Content.Width * startScale);
                double originX = (e.ScaleOrigin.X - deltaX) * deltaWidth;

                // The ScaleOrigin is in relative coordinates to the wrapped user interface element,
                // so get the Y pixel coordinate.
                double renderedY = Content.Y + yOffset;
                double deltaY = renderedY / Height;
                double deltaHeight = Height / (Content.Height * startScale);
                double originY = (e.ScaleOrigin.Y - deltaY) * deltaHeight;

                // Calculate the transformed element pixel coordinates.
                double targetX = xOffset - (originX * Content.Width) * (_currentScale - startScale);
                double targetY = yOffset - (originY * Content.Height) * (_currentScale - startScale);

                // Apply translation based on the change in origin.
                Content.TranslationX = Math.Clamp(targetX, -Content.Width * (_currentScale - 1), 0);
                Content.TranslationY = Math.Clamp(targetY, -Content.Height * (_currentScale - 1), 0);

                // Apply scale factor
                Content.Scale = _currentScale;
            }
            if (e.Status == GestureStatus.Completed)
            {
                // Store the translation delta's of the wrapped user interface element.
                xOffset = Content.TranslationX;
                yOffset = Content.TranslationY;
            }
        }
    }
}

