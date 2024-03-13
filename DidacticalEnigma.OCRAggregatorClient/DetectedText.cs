using System.Drawing;

namespace DidacticalEnigma.OCRAggregatorClient;

public class DetectedText
{
    public string Text { get; }
    
    public Rectangle Rectangle { get; }

    public DetectedText(string text, Rectangle rectangle)
    {
        Text = text ?? throw new ArgumentNullException(nameof(text));
        Rectangle = rectangle;
    }
}
