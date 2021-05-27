using System;

namespace DidacticalEnigma.Updater
{
    public class Disposable
    {
        public static TOutput With<TInput, TOutput>(TInput input, Func<TInput, TOutput> func)
            where TInput : IDisposable
        {
            using (input)
            {
                return func(input);
            }
        }
    }
}