// See https://aka.ms/new-console-template for more information
using MediaToolkit;
using MediaToolkit.Model;
using VideoLibrary;
using YouTubeSearch;

#region Code will download youtube video then convert it to audio
//Console.WriteLine("Hello, World!");
//var source = "";
//var youtube = YouTube.Default;
//var vid = youtube.GetVideo("https://www.youtube.com/watch?v=zW_tg3tr3ak");
//File.WriteAllBytes(source + vid.FullName, vid.GetBytes());

//var inputFile = new MediaFile { Filename = source + vid.FullName };
//var outputFile = new MediaFile { Filename = $"{source + vid.FullName}.mp3" };

//using (var engine = new Engine())
//{
//    engine.GetMetadata(inputFile);

//    engine.Convert(inputFile, outputFile);
//}
#endregion
#region Code will download audio only version
//SaveMP3("videos/", "https://www.youtube.com/watch?v=zW_tg3tr3ak");
#endregion
#region Search on youtube
Console.WriteLine((await SearchYoutube("jojo op 4 v2")).Url);
#endregion
static void SaveMP3(string SaveToFolder, string VideoURL, string MP3Name=null)
{
    var source = @SaveToFolder;
    var youtube = YouTube.Default;
    var vid = youtube.GetVideo(VideoURL);
    File.WriteAllBytes(source + vid.FullName, vid.GetBytes());

    if (string.IsNullOrWhiteSpace(MP3Name))
        MP3Name = $"{source + vid.FullName}";
    var inputFile = new MediaFile { Filename = source + vid.FullName };
    var outputFile = new MediaFile { Filename = $"{MP3Name}.mp3" };

    using (var engine = new Engine())
    {
        engine.GetMetadata(inputFile);

        engine.Convert(inputFile, outputFile);
    }
}
static async Task<Wally.Test.Video> SearchYoutube(string keyword)
{

    VideoSearch items = new VideoSearch();
    List<Wally.Test.Video> list = new List<Wally.Test.Video>();
    //Search video with paging
    var itemsCompnennt = await items.GetVideosPaged(keyword, 0);
    foreach (var item in itemsCompnennt)
    {
        Wally.Test.Video video = new Wally.Test.Video();
        video.Title = item.getTitle();
        video.Author = item.getAuthor();
        video.Url = item.getUrl();
        list.Add(video);
    }
    return list.FirstOrDefault();


}
Console.Write("Done");