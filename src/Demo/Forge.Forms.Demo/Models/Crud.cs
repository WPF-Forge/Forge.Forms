using System.Collections.ObjectModel;
using Forge.Forms.Annotations;
using Forge.Forms.Collections.Annotations;

namespace Forge.Forms.Demo.Models
{
    [Title("Crud example")]
    public class Crud
    {
        [Crud]
        public ObservableCollection<Login> Logins { get; set; } = new ObservableCollection<Login>(); // what if we have a null?
    }
}
