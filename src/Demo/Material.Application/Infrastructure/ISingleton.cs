using System.ComponentModel;
using System.Runtime.CompilerServices;
using Material.Application.Properties;

namespace Material.Application.Infrastructure
{
    public interface ISingleton<T> where T : class
    {
        T Instance { get; set; }
    }

    public class Singleton<T> : ISingleton<T>, INotifyPropertyChanged where T : class
    {
        private T instance;

        public Singleton(T instance)
        {
            Instance = instance;
        }

        public T Instance
        {
            get { return instance; }
            set
            {
                if (Equals(value, instance)) return;
                instance = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
