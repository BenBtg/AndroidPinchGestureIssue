using System;
namespace AndroidPinchGestureIssue
{
	public class ZoomImageContentView : ContentView
	{
        public const double ConstantMinScale = 1;
        public const double ConstantMaxScale = 8;
        public const double ConstantMinDelta = 0.1;

        public static readonly BindableProperty IsScrollableProperty = BindableProperty.Create(
          nameof(IsScrollable),
          typeof(bool),
          typeof(ZoomImageContentView),
          true);

        private double _startX;
        private double _startY;
        private double _currentScale = 1;
        private double _startScale = 1;
        private double _xOffset = 0;
        private double _yOffset = 0;

        private PanGestureRecognizer _panGesture = new PanGestureRecognizer();
        private TapGestureRecognizer _tapGesture = new TapGestureRecognizer { NumberOfTapsRequired = 2 };

        public ZoomImageContentView()
        {
        }

        public double CurrentScale
        {
            get => _currentScale;
            set => _currentScale = value;
        }

        public double StartScale
        {
            get => _startScale;
            set => _startScale = value;
        }

        public double XOffset
        {
            get => _xOffset;
            set => _xOffset = value;
        }

        public double YOffset
        {
            get => _yOffset;
            set => _yOffset = value;
        }

        public bool IsScrollable
        {
            get => (bool)GetValue(IsScrollableProperty);
            set => SetValue(IsScrollableProperty, value);
        }

        /// <inheritdoc/>
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            PinchGestureRecognizer pinchGesture = new PinchGestureRecognizer();
            pinchGesture.PinchUpdated += OnPinchUpdated;
            if (GestureRecognizers.Count == 0)
            {
                GestureRecognizers.Add(pinchGesture);
            }

            _tapGesture.Tapped += OnTapped;
            Content.GestureRecognizers.Add(_tapGesture);
        }

        private void AddPanGesture()
        {
            if (Content.GestureRecognizers.Contains(_panGesture))
            {
                return;
            }

            _panGesture.PanUpdated += OnPanUpdated;
            Content.GestureRecognizers.Add(_panGesture);

            if (Parent is CarouselView carouselView)
            {
                carouselView.IsSwipeEnabled = false;
            }
        }

        public void RemovePanGesture()
        {
            _panGesture.PanUpdated -= OnPanUpdated;
            Content.GestureRecognizers.Remove(_panGesture);

            if (Parent is CarouselView carouselView)
            {
                carouselView.IsSwipeEnabled = true;
            }
        }

        public void OnTapped(object sender, EventArgs e)
        {
            if (Content.Scale > ConstantMinScale)
            {
                Content.ScaleTo(ConstantMinScale, 250, Easing.CubicInOut);
                Content.TranslateTo(0, 0, 250, Easing.CubicInOut);
                _currentScale = 1;
                _xOffset = _yOffset = 0;
                if (Parent is CarouselView carouselView && _currentScale == 1)
                {
                    carouselView.IsSwipeEnabled = true;
                }

                RemovePanGesture();
            }
        }

        public void OnPinchUpdated(object sender, PinchGestureUpdatedEventArgs e)
        {
            switch (e.Status)
            {
                case GestureStatus.Started:
                    AddPanGesture();
                    _startScale = Content.Scale;
                    Content.AnchorX = 0;
                    Content.AnchorY = 0;
                    break;
                case GestureStatus.Running:
                    {
                        _currentScale += (e.Scale - 1) * _startScale;
                        _currentScale = Math.Max(1, _currentScale);

                        if (_currentScale >= ConstantMaxScale)
                        {
                            return;
                        }

                        double renderedX = Content.X + _xOffset;
                        double deltaX = renderedX / Width;
                        double deltaWidth = Width / (Content.Width * _startScale);
                        double originX = (e.ScaleOrigin.X - deltaX) * deltaWidth;

                        double renderedY = Content.Y + _yOffset;
                        double deltaY = renderedY / Height;
                        double deltaHeight = Height / (Content.Height * _startScale);
                        double originY = (e.ScaleOrigin.Y - deltaY) * deltaHeight;

                        _startX = _xOffset - (originX * Content.Width) * (_currentScale - _startScale);
                        _startY = _yOffset - (originY * Content.Height) * (_currentScale - _startScale);

                        Content.TranslationX = double.Clamp(_startX, -Content.Width * (_currentScale - 1), 0);
                        Content.TranslationY = double.Clamp(_startY, -Content.Height * (_currentScale - 1), 0);

                        Content.Scale = _currentScale;
                        break;
                    }

                case GestureStatus.Completed:
                    _xOffset = Content.TranslationX;
                    _yOffset = Content.TranslationY;
                    DisableGestures();
                    break;

                case GestureStatus.Canceled:
                    _xOffset = _yOffset = 0;
                    Content.Scale = 1;
                    DisableGestures();
                    break;
            }
        }

        public void DisableGestures()
        {
            if (Parent is CarouselView carouselView && _currentScale <= ConstantMinScale + ConstantMinDelta)
            {
                _currentScale = 1;
                RemovePanGesture();
                carouselView.IsSwipeEnabled = true;
            }
        }

        public void OnPanUpdated(object sender, PanUpdatedEventArgs e)
        {
            switch (e.StatusType)
            {
                case GestureStatus.Started:
                    _startX = Content.TranslationX;
                    _startY = Content.TranslationY;
                    break;

                case GestureStatus.Running:

                    double newX = _startX + e.TotalX;
                    double newY = _startY + e.TotalY;

                    double maxTranslationX = Content.Width * (Content.Scale - 1);
                    double maxTranslationY = Content.Height * (Content.Scale - 1);

                    Content.TranslationX = Math.Max(Math.Min(newX, maxTranslationX), -maxTranslationX);
                    Content.TranslationY = Math.Max(Math.Min(newY, maxTranslationY), -maxTranslationY);
                    break;

                case GestureStatus.Completed:
                case GestureStatus.Canceled:
                    _xOffset = Content.TranslationX;
                    _yOffset = Content.TranslationY;

                    break;
            }
        }
    }
}

