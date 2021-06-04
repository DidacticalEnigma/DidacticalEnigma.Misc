using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CodeHollow.FeedReader;
using Optional;
using Optional.Collections;

namespace DidacticalEnigma.Updater
{
    public class GitHubAtomReleasesChannelUpdater
    {
        private readonly string atomFeedUrl;

        private static readonly Regex githubUrlMatcher = new Regex("https://github.com/.*?/.*?/releases/tag/(.*)");

        public GitHubAtomReleasesChannelUpdater(string atomFeedUrl)
        {
            this.atomFeedUrl = atomFeedUrl;
        }
        
        public async Task<Option<UpdateInfo>> CheckForUpdate(Version currentVersion)
        {
            var feed = await FeedReader.ReadAsync(atomFeedUrl);
            return feed.Items
                .Select(item =>
                {
                    var match = githubUrlMatcher.Match(item.Link);
                    if (match.Success)
                    {
                        var versionString = match.Groups[1].Value;
                        var updateInfo = Version.Parse(versionString)
                            .Map(v => new UpdateInfo(v, new Uri(item.Link)));
                        return updateInfo;
                    }
                    else
                    {
                        return Option.None<UpdateInfo>();
                    }
                })
                .Values()
                .Where(updateInfo => updateInfo.Version > currentVersion)
                .OrderByDescending(x => x.Version)
                .FirstOrNone();
        }
    }
}