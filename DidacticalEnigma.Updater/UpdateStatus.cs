using System;

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

            public FailureStatus(string reason)
            {
                Reason = reason;
            }
        }
    }
}