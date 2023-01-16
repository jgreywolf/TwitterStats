using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TwitterStats.Models;

namespace TwitterStats.Scribes;

public class ConsoleScribe : IScribe
{
    // Need to address where sometimes messages are not displaying in white (black on black)
    public async Task Write(int numberOfTweets, Dictionary<Hashtag, int> topTenHashtags)
    {
        if (numberOfTweets <= 0) return;

        Console.Clear();
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("========================");
        Console.WriteLine();

        Console.WriteLine($"Total # of Tweets: {numberOfTweets}");

        Console.WriteLine("{0,-20} {1,5}\n", "Hashtag:","Count:");
        foreach (var item in topTenHashtags)
        {
            Console.WriteLine("{0,-20} {1,5:F0}", item.Key, item.Value);
        }

        Console.WriteLine();
    }
}