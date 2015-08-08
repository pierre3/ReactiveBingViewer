using Reactive.Bindings.Interactivity;
using System;
using System.Reactive.Linq;
using System.Windows.Input;

namespace ReactiveBingViewer.Converters
{
    public class ReturnKeyDownConverter : ReactiveConverter<KeyEventArgs, object>
    {
        protected override IObservable<object> OnConvert(IObservable<KeyEventArgs> source)
        {
            return source.Where(arg => arg.Key == Key.Return).Select<KeyEventArgs,object>(_ => null);
        }
    }
}
