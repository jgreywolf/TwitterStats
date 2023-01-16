using System;
using System.Threading;
using Tweetinvi;
using Tweetinvi.Models;
using Tweetinvi.Streaming.V2;
using TwitterStats.Models;

namespace TwitterStats;

public class TwitterCourier
{
    private readonly TwitterClient _appClient;

    public TwitterCourier(TwitterApiSettings apiSettings)
    {
        var appCredentials = new ConsumerOnlyCredentials(apiSettings.TwitterApiKey, apiSettings.TwitterApiSecret)
        {
            BearerToken = apiSettings.BearerToken
        };

        _appClient = new TwitterClient(appCredentials);
    }

    public virtual void StartSampleStream(CancellationTokenSource cts)
    {
        Console.WriteLine($"Connecting to stream...");

        ISampleStreamV2 sampleStreamV2 = _appClient.StreamsV2.CreateSampleStream();
        sampleStreamV2.TweetReceived += (sender, args) =>
        {
            FlattenedTweetData tweetData = new FlattenedTweetData(args.Tweet);
            TwitterStatisticsService.Instance.AddOrUpdate(tweetData);
        };

        try
        {
            sampleStreamV2.StartAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            cts.Cancel();
        }
    }
}