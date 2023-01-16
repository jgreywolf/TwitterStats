using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TwitterStats.Models;
using TwitterStats.Scribes;

namespace TwitterStats;

public sealed class TwitterStatisticsService
{
    private static TwitterStatisticsService _twitterStatistics;
    public static TwitterStatisticsService Instance => _twitterStatistics ??= new TwitterStatisticsService();

    private readonly ConcurrentDictionary<Hashtag, int> _hashTags = new();
    private readonly ConcurrentDictionary<string, string> _tweetIds = new();

    private bool _isRunning;
    private IScribe _scribe;

    public void StartService(IScribe scribe, CancellationTokenSource cts)
    {
        if (_isRunning) return;

        _scribe = scribe;
        _isRunning = true;

        do
        {
            DisplayOutput();
            // setting this to sleep so it is only updating the output every 30 seconds
            Thread.Sleep(TimeSpan.FromSeconds(30));
        } while (cts.IsCancellationRequested is false);
    }

    public void AddOrUpdate(FlattenedTweetData flattenedTweetData)
    {
        string tweetId = flattenedTweetData.Id;
        string rootId = flattenedTweetData.RootId;

        // not handling edits at this point
        if (_tweetIds.TryAdd(tweetId, rootId) is false) return;

        foreach (Hashtag hashtag in flattenedTweetData.Hashtags)
        {
            _hashTags.TryGetValue(hashtag, out int count);

            if (count <= 0)
            {
                if (_hashTags.TryAdd(hashtag, 1)) continue;
            }

            _hashTags[hashtag] = count + 1;
        }
    }

    public int GetCountOfTweets() => _tweetIds.Count;

    public Dictionary<Hashtag, int> GetTopTenHashtags()
    {
        return _hashTags.ToArray()
            .OrderByDescending(count => count.Value)
            .Take(10)
            .ToDictionary(kp => kp.Key, kp => kp.Value);
    }

    private void DisplayOutput()
    {
        _scribe.Write(GetCountOfTweets(), GetTopTenHashtags());
    }
}