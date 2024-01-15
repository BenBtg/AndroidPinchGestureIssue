using System;
using Microsoft.Maui.Handlers;

namespace AndroidPinchGestureIssue
{
	public partial class GestureListenerViewHandler
	{
        public static IPropertyMapper<GestureListenerView, GestureListenerViewHandler> PropertyMapper =
            new PropertyMapper<GestureListenerView, GestureListenerViewHandler>(ViewHandler.ViewMapper) { };

        public GestureListenerViewHandler() : base(PropertyMapper) { }
       
    }
}

