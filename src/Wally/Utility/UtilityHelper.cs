using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoLibrary;
using MediaToolkit;
using MediaToolkit.Model;
using Wally.Modules;
using YouTubeSearch;

namespace Wally.Utility
{
    public static class UtilityHelper
    {
        public static string SaveMP3(string SaveToFolder, string VideoURL, string MP3Name = null)
        {
            var source = @SaveToFolder;
            CreateDirectoryIfNotExists(source);
            var youtube = YouTube.Default;
            var vid = youtube.GetVideo(VideoURL);
            string inputFileName = source + "\\" + vid.FullName;
            File.WriteAllBytes(inputFileName, vid.GetBytes());

            if (string.IsNullOrWhiteSpace(MP3Name))
                MP3Name = $"{inputFileName}";
            var inputFile = new MediaFile { Filename = inputFileName };
            var outputFileName = $"{MP3Name}.mp3";
            var outputFile = new MediaFile { Filename = outputFileName };
            

            using (var engine = new Engine())
            {
                engine.GetMetadata(inputFile);

                engine.Convert(inputFile, outputFile);
            }
            return outputFile.Filename;
        }

        private static void CreateDirectoryIfNotExists(string source)
        {
            if (!System.IO.Directory.Exists(source))
            {
                System.IO.Directory.CreateDirectory(source);
            }
        }

        public static async Task<VideoM> SearchYoutube(string keyword)
        {

            VideoSearch items = new VideoSearch();
            List<VideoM> list = new List<VideoM>();
            //Search video with paging
            var itemsCompnennt = await items.GetVideosPaged(keyword, 0);
            foreach (var item in itemsCompnennt)
            {
                VideoM video = new VideoM();
                video.Title = item.getTitle();
                video.Author = item.getAuthor();
                video.Url = item.getUrl();
                list.Add(video);
            }
            return list.FirstOrDefault();


        }
    }
}
