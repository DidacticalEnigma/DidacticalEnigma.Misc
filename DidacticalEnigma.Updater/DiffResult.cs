using System;
using System.Collections.Generic;
using System.Linq;

namespace DidacticalEnigma.Updater
{
    public class DiffResult<T>
    {
        public bool HasDifferences { get; }
        
        public IEnumerable<T> OldEntries { get; }
        
        public IEnumerable<T> NewEntries { get; }

        public DiffResult(IReadOnlyCollection<T> oldEntries, IReadOnlyCollection<T> newEntries)
        {
            OldEntries = oldEntries;
            NewEntries = newEntries;
            HasDifferences = oldEntries.Count > 0 || newEntries.Count > 0;
        }
    }

    public static class DiffResult
    {
        public static DiffResult<string> Of(
            IReadOnlyList<string> inputOld,
            IReadOnlyList<string> inputNew)
        {
            return new DiffResult<string>(
                inputOld.Except(inputNew).ToList(),
                inputNew.Except(inputOld).ToList());
        }
    }
}