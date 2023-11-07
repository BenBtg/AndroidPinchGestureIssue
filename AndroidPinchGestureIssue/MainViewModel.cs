using System;
namespace AndroidPinchGestureIssue
{
    public class MainViewModel
    {
        public List<string> Sources { get; set; }
        public List<List<string>> Listes { get; set; }
        public MainViewModel()
        {

            Sources = new List<string> {
            "https://upload.wikimedia.org/wikipedia/commons/7/70/Example.png",
            "https://maui.graphics/images/dotnet-bot-painting.png",
            "https://maui.graphics/images/dotnet-bot-painting.png"
            };
            Listes = new List<List<string>>
            {
                Sources
            };

        }
    }
}

