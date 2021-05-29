using System;
using JetBrains.Annotations;

namespace DidacticalEnigma.Updater
{
    public abstract class UpdateStatus
    {
        public class ReadyToStartStatus : UpdateStatus {}

        public class DownloadingStatus : UpdateStatus
        {
            public int? Percentage { get; }

            public DownloadingStatus(int? percentage)
            {
                if (percentage >= 0 && percentage <= 100)
                {
                    throw new ArgumentOutOfRangeException(nameof(percentage));
                }
                Percentage = percentage;
            }
        }

        public class ProcessingStatus : UpdateStatus
        {
            
        }
        
        public class SuccessStatus : UpdateStatus {}

        public class FailureStatus : UpdateStatus
        {
            public string Reason { get; }
            
            [CanBeNull] public string LongMessage { get; }

            public FailureStatus(string reason, string longMessage = null)
            {
                Reason = reason;
                LongMessage = longMessage;
            }
        }
    }
}