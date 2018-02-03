
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Forge.Forms.Annotations;

namespace Forge.Forms.Demo.Models
{
    public class EnvManager : IActionHandler, INotifyPropertyChanged
    {
        private string currentEnv;
        private string environmentKey;

        [Title("Environments")]

        [Text("Try adding values 'create', 'update', 'delete'")]
        
        [Text("Current env: {Binding CurrentEnv}")]

        [Text("Env 'create'", IsVisible = "{Env CREATE}")] // Env keys are case insensitive.
        [Text("Env 'update'", IsVisible = "{Env Update}")]
        [Text("Env 'delete'", IsVisible = "{Env delete}")]
        [Text("All envs", IsVisible = "{Env CREATE} && {Env UPDATE} && {Env DELETE}")]

        [Break]

        [Action("add", "ADD KEY", Validates = true)]
        [Action("remove", "REMOVE KEY", Validates = true)]

        [Value(Must.NotBeEmpty)]
        public string EnvironmentKey
        {
            get => environmentKey;
            set
            {
                environmentKey = value;
                OnPropertyChanged();
            }
        }

        public string CurrentEnv
        {
            get => currentEnv;
            private set
            {
                currentEnv = value;
                OnPropertyChanged();
            }
        }

        public void HandleAction(IActionContext actionContext)
        {
            switch (actionContext.Action)
            {
                case "add":
                    actionContext.ResourceContext.Environment.Add(EnvironmentKey);
                    break;
                case "remove":
                    actionContext.ResourceContext.Environment.Remove(EnvironmentKey);
                    break;
            }

            CurrentEnv = string.Join("; ", actionContext.ResourceContext.Environment);
            EnvironmentKey = "";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
