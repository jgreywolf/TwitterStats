using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using FluentAssertions;
using Newtonsoft.Json;
using Tweetinvi.Models.V2;
using TwitterStats;
using TwitterStats.Models;
using TwitterStats.Scribes;

namespace TwitterStatsTests;

[TestClass]
public class ScribeTests
{
    
    private readonly TwitterStatisticsService _statisticsService = TwitterStatisticsService.Instance;

    [TestMethod]
    public void GivenAnInstanceOfConsoleScribe_WhenCallingWriteAsync_ShouldWriteStatsToConsole()
    {
        TwitterCourier courier = new TwitterCourierWrapper();
        
        var stringWriter = new StringWriter();
        Console.SetOut(stringWriter);
        CancellationTokenSource cts = new CancellationTokenSource();

        courier.StartSampleStream(cts);
        _statisticsService.StartService(new ConsoleScribe(), cts);

        string actual = stringWriter.ToString();

        actual.Should().Contain("Total # of Tweets: 5");
        actual.Should().Contain("Buca                   4");
    }

    [TestMethod]
    [Ignore("Cosmos Scribe not added yet")]
    public void GivenAnInstanceOfCosmosScribe_WhenCallingWriteAsync_ShouldWriteJsonObjectToCosmosService()
    {

    }
}

public class TwitterCourierWrapper : TwitterCourier
{
    private const string StreamData =
        "[{\"data\":{\"id\":\"1613580805883793411\",\"conversation_id\":\"1613580805883793411\",\"entities\":{\"hashtags\":[{\"start\":48,\"end\":53,\"tag\":\"Buca\"},{\"start\":48,\"end\":53,\"tag\":\"Cuca\"},{\"start\":48,\"end\":53,\"tag\":\"Duca\"},{\"start\":48,\"end\":53,\"tag\":\"Euca\"},{\"start\":48,\"end\":53,\"tag\":\"Fuca\"},{\"start\":48,\"end\":53,\"tag\":\"Guca\"},{\"start\":48,\"end\":53,\"tag\":\"Iuca\"}]},\"id\":\"1613580805883793411\"}},{\"data\":{\"entities\":{\"hashtags\":[{\"start\":48,\"end\":53,\"tag\":\"Buca\"},{\"start\":48,\"end\":53,\"tag\":\"Cuca\"},{\"start\":48,\"end\":53,\"tag\":\"Duca\"},{\"start\":48,\"end\":53,\"tag\":\"Euca\"},{\"start\":48,\"end\":53,\"tag\":\"Fuca\"},{\"start\":48,\"end\":53,\"tag\":\"Guca\"},{\"start\":48,\"end\":53,\"tag\":\"Iuca\"}]},\"id\":\"16135808058793411\"}},{\"data\":{\"entities\":{\"hashtags\":[{\"start\":48,\"end\":53,\"tag\":\"Buca\"},{\"start\":48,\"end\":53,\"tag\":\"Cuca\"},{\"start\":48,\"end\":53,\"tag\":\"dd\"},{\"start\":48,\"end\":53,\"tag\":\"Euca\"},{\"start\":48,\"end\":53,\"tag\":\"hh\"},{\"start\":48,\"end\":53,\"tag\":\"Guca\"},{\"start\":48,\"end\":53,\"tag\":\"Cha\"}]},\"id\":\"1613580805883795411\"}},{\"data\":{\"entities\":{\"hashtags\":[{\"start\":48,\"end\":53,\"tag\":\"Buca\"},{\"start\":48,\"end\":53,\"tag\":\"Cuca\"},{\"start\":48,\"end\":53,\"tag\":\"Duca\"}]},\"id\":\"16135808083793411\"}},{\"data\":{\"entities\":{\"hashtags\":[{\"start\":48,\"end\":53,\"tag\":\"Iuca\"}]},\"id\":\"161380805883793411\"}}]";

    private readonly TwitterStatisticsService _statisticsService = TwitterStatisticsService.Instance;

    public TwitterCourierWrapper() : base(new TwitterApiSettings()) { }

    public override void StartSampleStream(CancellationTokenSource cts)
    {
        Console.WriteLine($"Connecting to stream...");

        List<TweetV2Response> response = JsonConvert.DeserializeObject<List<TweetV2Response>>(StreamData);

        foreach (TweetV2Response tweetResponse in response)
        {
            FlattenedTweetData data = new FlattenedTweetData(tweetResponse.Tweet);
            _statisticsService.AddOrUpdate(data);
        }

        cts.Cancel();
    }
}