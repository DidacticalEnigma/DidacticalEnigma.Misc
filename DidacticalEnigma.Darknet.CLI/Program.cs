using System.Text.Json;
using DidacticalEnigma.Darknet;
using DidacticalEnigma.Darknet.CLI;

using var yolo = new Yolo(
    "/home/milleniumbug/dokumenty/PROJEKTY/NotMine/sugoi-japanese-translator/ImageTrans-Balloons-Model-v2/model.cfg",
    "/home/milleniumbug/dokumenty/PROJEKTY/NotMine/sugoi-japanese-translator/ImageTrans-Balloons-Model-v2/model.weights",
    0);

var darknetDetector = new DarknetDetector(yolo);

await using var file = System.IO.File.OpenRead("/home/milleniumbug/dokumenty/099.png");

Console.WriteLine(JsonSerializer.Serialize(await darknetDetector.DetectTextBoxesInImage(file)));