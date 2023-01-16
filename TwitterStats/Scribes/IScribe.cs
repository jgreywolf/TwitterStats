using System.Collections.Generic;
using System.Threading.Tasks;
using TwitterStats.Models;

namespace TwitterStats.Scribes;

public interface IScribe
{
    Task Write(int numberOfTweets, Dictionary<Hashtag, int> topTenHashtags);
}