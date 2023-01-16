using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using FluentAssertions;
using System.Linq;
using Newtonsoft.Json;
using Tweetinvi.Models.V2;
using TwitterStats;
using TwitterStats.Models;

namespace TwitterStatsTests;

[TestClass]
public class TwitterStatisticsServiceTests
{
    private const string StreamData = "{\"data\":{\"edit_history_tweet_ids\":[\"1613580805883793411\"],\"entities\":{\"hashtags\":[{\"start\":48,\"end\":53,\"tag\":\"Buca\"}],\"mentions\":[{\"start\":3,\"end\":14,\"username\":\"_2_BooBear\",\"id\":\"1601520432175353857\"}],\"urls\":[{\"start\":16,\"end\":39,\"url\":\"https://t.co/xi0O3cZQKw\",\"expanded_url\":\"https://twitter.com/_2_BooBear/status/1613580796819902486/photo/1\",\"display_url\":\"pic.twitter.com/xi0O3cZQKw\",\"media_key\":\"3_1613580792055173137\"}]},\"id\":\"1613580805883793411\",\"text\":\"RT @_2_BooBear: https://t.co/xi0O3cZQKw Olmuyor #Buca kıymet\"}}";
    private readonly TwitterStatisticsService _statisticsService = TwitterStatisticsService.Instance;

    [TestMethod]
    public void GivenASingleTweet_WhenGettingCount_ShouldReturnOne()
    {
        TweetV2Response response = JsonConvert.DeserializeObject<TweetV2Response>(StreamData); 
        FlattenedTweetData data = new FlattenedTweetData(response.Tweet);
        _statisticsService.AddOrUpdate(data);

        _statisticsService.GetCountOfTweets().Should().Be(1);
    }

    [TestMethod]
    public void GivenMultipleTweetsWithMultipleHashtags_WhenGettingTopTen_ShouldReturnListInCorrectOrder()
    {
        string dataWithMultipleTweets = "[{\"data\":{\"id\":\"1613580805883793411\",\"conversation_id\":\"1613580805883793411\",\"entities\":{\"hashtags\":[{\"start\":48,\"end\":53,\"tag\":\"Buca\"},{\"start\":48,\"end\":53,\"tag\":\"Cuca\"},{\"start\":48,\"end\":53,\"tag\":\"Duca\"},{\"start\":48,\"end\":53,\"tag\":\"Euca\"},{\"start\":48,\"end\":53,\"tag\":\"Fuca\"},{\"start\":48,\"end\":53,\"tag\":\"Guca\"},{\"start\":48,\"end\":53,\"tag\":\"Iuca\"}]},\"id\":\"1613580805883793411\"}},{\"data\":{\"entities\":{\"hashtags\":[{\"start\":48,\"end\":53,\"tag\":\"Buca\"},{\"start\":48,\"end\":53,\"tag\":\"Cuca\"},{\"start\":48,\"end\":53,\"tag\":\"Duca\"},{\"start\":48,\"end\":53,\"tag\":\"Euca\"},{\"start\":48,\"end\":53,\"tag\":\"Fuca\"},{\"start\":48,\"end\":53,\"tag\":\"Guca\"},{\"start\":48,\"end\":53,\"tag\":\"Iuca\"}]},\"id\":\"16135808058793411\"}},{\"data\":{\"entities\":{\"hashtags\":[{\"start\":48,\"end\":53,\"tag\":\"Buca\"},{\"start\":48,\"end\":53,\"tag\":\"Cuca\"},{\"start\":48,\"end\":53,\"tag\":\"dd\"},{\"start\":48,\"end\":53,\"tag\":\"Euca\"},{\"start\":48,\"end\":53,\"tag\":\"hh\"},{\"start\":48,\"end\":53,\"tag\":\"Guca\"},{\"start\":48,\"end\":53,\"tag\":\"Cha\"}]},\"id\":\"1613580805883795411\"}},{\"data\":{\"entities\":{\"hashtags\":[{\"start\":48,\"end\":53,\"tag\":\"Buca\"},{\"start\":48,\"end\":53,\"tag\":\"Cuca\"},{\"start\":48,\"end\":53,\"tag\":\"Duca\"}]},\"id\":\"16135808083793411\"}},{\"data\":{\"entities\":{\"hashtags\":[{\"start\":48,\"end\":53,\"tag\":\"Iuca\"}]},\"id\":\"161380805883793411\"}}]";
        List<TweetV2Response> response = JsonConvert.DeserializeObject<List<TweetV2Response>>(dataWithMultipleTweets);

        foreach (TweetV2Response tweetResponse in response)
        {
            FlattenedTweetData data = new FlattenedTweetData(tweetResponse.Tweet);
            _statisticsService.AddOrUpdate(data);
        }
        
        Dictionary<Hashtag, int> topTen = _statisticsService.GetTopTenHashtags();
        topTen.Count.Should().Be(10);
        topTen.Should().BeInDescendingOrder(orderBy => orderBy.Value);
    }

    [TestMethod]
    public void GivenMultipleTweetsWithMultipleHashtags_WhenGettingTopTen_ShouldReturnCorrectCountValues()
    {
        string dataWithMultipleTweets = "[{\"data\":{\"id\":\"1613580805883793411\",\"conversation_id\":\"1613580805883793411\",\"entities\":{\"hashtags\":[{\"start\":48,\"end\":53,\"tag\":\"Buca\"},{\"start\":48,\"end\":53,\"tag\":\"Cuca\"},{\"start\":48,\"end\":53,\"tag\":\"Duca\"},{\"start\":48,\"end\":53,\"tag\":\"Euca\"},{\"start\":48,\"end\":53,\"tag\":\"Fuca\"},{\"start\":48,\"end\":53,\"tag\":\"Guca\"},{\"start\":48,\"end\":53,\"tag\":\"Iuca\"}]},\"id\":\"1613580805883793411\"}},{\"data\":{\"entities\":{\"hashtags\":[{\"start\":48,\"end\":53,\"tag\":\"Buca\"},{\"start\":48,\"end\":53,\"tag\":\"Cuca\"},{\"start\":48,\"end\":53,\"tag\":\"Duca\"},{\"start\":48,\"end\":53,\"tag\":\"Euca\"},{\"start\":48,\"end\":53,\"tag\":\"Fuca\"},{\"start\":48,\"end\":53,\"tag\":\"Guca\"},{\"start\":48,\"end\":53,\"tag\":\"Iuca\"}]},\"id\":\"16135808058793411\"}},{\"data\":{\"entities\":{\"hashtags\":[{\"start\":48,\"end\":53,\"tag\":\"Buca\"},{\"start\":48,\"end\":53,\"tag\":\"Cuca\"},{\"start\":48,\"end\":53,\"tag\":\"dd\"},{\"start\":48,\"end\":53,\"tag\":\"Euca\"},{\"start\":48,\"end\":53,\"tag\":\"hh\"},{\"start\":48,\"end\":53,\"tag\":\"Guca\"},{\"start\":48,\"end\":53,\"tag\":\"Cha\"}]},\"id\":\"1613580805883795411\"}},{\"data\":{\"entities\":{\"hashtags\":[{\"start\":48,\"end\":53,\"tag\":\"Buca\"},{\"start\":48,\"end\":53,\"tag\":\"Cuca\"},{\"start\":48,\"end\":53,\"tag\":\"Duca\"}]},\"id\":\"16135808083793411\"}},{\"data\":{\"entities\":{\"hashtags\":[{\"start\":48,\"end\":53,\"tag\":\"Iuca\"}]},\"id\":\"161380805883793411\"}}]";
        List<TweetV2Response> response = JsonConvert.DeserializeObject<List<TweetV2Response>>(dataWithMultipleTweets);

        foreach (TweetV2Response tweetResponse in response)
        {
            FlattenedTweetData data = new FlattenedTweetData(tweetResponse.Tweet);
            _statisticsService.AddOrUpdate(data);
        }
        
        Dictionary<Hashtag, int> topTen = _statisticsService.GetTopTenHashtags();
        topTen.First().Value.Should().Be(4);
        topTen.Last().Value.Should().Be(1);
    }
}