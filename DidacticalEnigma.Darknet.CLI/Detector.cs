using DidacticalEnigma.SugoiSuite;

namespace DidacticalEnigma.Darknet.CLI;

public interface IDetector
{
    Task<IEnumerable<TextRectangle>> DetectTextBoxesInImage(Stream stream);
}

public class DarknetDetector : IDetector
{
    private readonly Yolo yolo;

    public DarknetDetector(Yolo yolo)
    {
        this.yolo = yolo;
    }
    
    public Task<IEnumerable<TextRectangle>> DetectTextBoxesInImage(Stream stream)
    {
        return Task.Run(() =>
        {
            using var memory = new MemoryStream();
            stream.CopyTo(memory);
            var detected = this.yolo.Detect(memory.ToArray()).ToArray();
            return detected.Select(r =>
            {
                var tl = new Point2D((int)r.x, (int)r.y);
                var br = new Point2D((int)(r.x + r.w), (int)(r.y + r.h));
                return new TextRectangle(tl, br);
            });
        });
    }
}