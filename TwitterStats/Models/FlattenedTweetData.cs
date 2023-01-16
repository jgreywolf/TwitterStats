using System.Collections.Generic;
using System.Linq;
using Tweetinvi.Models.V2;

namespace TwitterStats.Models;

public sealed class FlattenedTweetData
{
    public string Id { get; set; }
    public string RootId { get; set; }
    public readonly IEnumerable<Hashtag> Hashtags = new List<Hashtag>();
    public readonly IEnumerable<Mention> Mentions = new List<Mention>();

    public FlattenedTweetData(TweetV2 tweet)
    {
        Id = tweet.Id;
        RootId = tweet.ConversationId;

        if (tweet.Entities?.Hashtags is not null)
        {
            Hashtags = tweet.Entities?.Hashtags?.Select(x => new Hashtag(x.Tag));
        }

        if (tweet.Entities?.Mentions is not null)
        {
            Mentions = tweet.Entities?.Mentions?.Select(x => new Mention(x.Username));
        }
    }
}