namespace DidacticalEnigma.SugoiSuite;

public class TextRectangle
{
    public Point2D TopLeft { get; }
    
    public Point2D BottomRight { get; }

    public int Width => BottomRight.X - TopLeft.X;

    public int Height => BottomRight.Y - TopLeft.Y;

    public TextRectangle(Point2D topLeft, Point2D bottomRight)
    {
        TopLeft = topLeft;
        BottomRight = bottomRight;
    }
}