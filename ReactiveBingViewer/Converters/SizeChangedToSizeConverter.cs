using Reactive.Bindings.Interactivity;
using System;
using System.Reactive.Linq;
using System.Windows;

namespace ReactiveBingViewer.Converters
{

    public class SizeChangedToSizeConverter : ReactiveConverter<SizeChangedEventArgs, Size>
    {
        protected override IObservable<Size> OnConvert(IObservable<SizeChangedEventArgs> source)
        {
            return source.Select(arg => arg.NewSize)
                .Where(size => size.Width > 0 && size.Height > 0);
        }
    }
    
}
