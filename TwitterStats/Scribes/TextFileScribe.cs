using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitterStats.Models;

namespace TwitterStats.Scribes;

public class TextFileScribe : IScribe
{
    const string OutputTextFileName = "statsOutput.txt";

    public async Task Write(int numberOfTweets, Dictionary<Hashtag, int> topTenHashtags)
    {
        if (numberOfTweets <= 0) return;
        
        StringBuilder builder = new StringBuilder();
        builder.AppendLine($"Total # of Tweets: {numberOfTweets}");
        builder.AppendLine();

        builder.AppendFormat("{0,-20} {1,5}\n", "Hashtag:","Count:");
        if (topTenHashtags.Any())
        {
            foreach (var item in topTenHashtags)
            {
                builder.AppendFormat("{0,-20} {1,5:F0}\n", item.Key, item.Value);
            }
        }
        else
        {
            builder.AppendLine("No HashTag data available");
        }

        Console.WriteLine($"Updating {OutputTextFileName} with current stats");
        await using StreamWriter file = new(OutputTextFileName);
        await file.WriteLineAsync(builder.ToString());
    }
}