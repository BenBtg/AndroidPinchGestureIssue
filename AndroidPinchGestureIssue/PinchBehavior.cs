using System;
namespace AndroidPinchGestureIssue;

public partial class PinchBehavior
{
    public PinchBehavior()
    {
    }

    public static readonly BindableProperty PinchScaleProperty =
       BindableProperty.Create(nameof(PinchScale), typeof(float), typeof(PinchBehavior), 1f);

    public float PinchScale
    {
       get => (float)GetValue(PinchScaleProperty);
       set => SetValue(PinchScaleProperty, value);
    }

}