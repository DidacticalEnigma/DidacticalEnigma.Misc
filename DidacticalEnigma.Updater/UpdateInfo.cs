using System;

namespace DidacticalEnigma.Updater
{
    public class UpdateInfo
    {
        public Version Version { get; }
        
        public Uri DownloadUrl { get; }

        public UpdateInfo(Version version, Uri downloadUrl)
        {
            Version = version;
            DownloadUrl = downloadUrl;
        }
    }
}