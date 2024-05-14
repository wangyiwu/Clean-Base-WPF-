using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace UI.ViewModel
{
    public abstract class BaseViewModel :
                           INotifyPropertyChanged,
                           IDisposable
    {
        private bool _disposed;
        protected Dispatcher Dispatcher;

        public event PropertyChangedEventHandler PropertyChanged;

        public BaseViewModel(BaseViewModel parent, bool initializeNotifier)
        {
            if (parent != null)
                Dispatcher = parent.Dispatcher;

            if (initializeNotifier)
            {
               //todo
            }
        }

        protected virtual void DisposeManagedResources()
        {
            //todo
        }

        public void Dispose()
        {
            if (_disposed)
                return;
            _disposed = true;
            DisposeManagedResources();
        }

        protected virtual void OnPropertyChanged(string name)
        {
            var temp = Interlocked.CompareExchange(ref PropertyChanged, null, null);
            if (temp != null)
                temp(this, new PropertyChangedEventArgs(name));
        }

        protected void SetValue<T>(T value, string propertyName, Action<T> setter)
        {
            setter(value);
            OnPropertyChanged(propertyName);
        }

        protected EventHandler<TArgs> CreateSubscriber<TArgs>(Action<TArgs> method)
          where TArgs : EventArgs
        {
            Action<TArgs> safeMethod = a =>
            {
                try
                {
                    method(a);
                }
                catch (Exception e)
                {
                    //todo
                }
            };

            // For subscribing on events object event, it can not unsubscribe.
            // Because events object creatred for each viewmodel, and removed with viewmodel.
            return (sender, eventArgs) => Dispatcher.BeginInvoke(safeMethod, eventArgs);
        }
    }
}
