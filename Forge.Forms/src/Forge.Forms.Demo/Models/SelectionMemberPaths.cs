using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Forge.Forms.Annotations;

namespace Forge.Forms.Demo.Models
{
    public class SelectionUser
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string ComputedDisplay => $"[{Id}] {FirstName} {LastName}";

        public override string ToString() => $"{FirstName} {LastName} (ToString())";
    }

    public class UserWrapper
    {
        public UserWrapper(SelectionUser value)
        {
            Value = value;
        }

        public SelectionUser Value { get; set; }

        public override string ToString()
        {
            return $"[{Value.Id}] {Value.FirstName} {Value.LastName}";
        }
    }

    public class SelectionMemberPaths
    {
        public List<SelectionUser> Users { get; } = new List<SelectionUser>
        {
            new SelectionUser { Id = 1, FirstName = "First1", LastName = "Last1" },
            new SelectionUser { Id = 2, FirstName = "First2", LastName = "Last2" },
            new SelectionUser { Id = 3, FirstName = "First3", LastName = "Last3" },
            new SelectionUser { Id = 4, FirstName = "First4", LastName = "Last4" }
        };

        public List<UserWrapper> WrappedUsers => Users.Select(user => new UserWrapper(user)).ToList();

        public string UserDisplayProperty => "FirstName";

        [SelectFrom("{Binding Users}")]
        public SelectionUser WithImplicitToString { get; set; }

        [SelectFrom("{Binding Users}", DisplayPath = "LastName")]
        public SelectionUser WithConstantDisplayPath { get; set; }

        [SelectFrom("{Binding Users}", DisplayPath = "ComputedDisplay")]
        public SelectionUser WithGetterDisplayPath { get; set; }

        [SelectFrom("{Binding Users}", DisplayPath = "{Binding UserDisplayProperty}")]
        public SelectionUser WithDynamicallyChosenProperty { get; set; }

        [SelectFrom("{Binding Users}", ValuePath = "Id")]
        public int WithValuePath { get; set; }

        [SelectFrom("{Binding WrappedUsers}", ValuePath = "Value")]
        public SelectionUser WithWrapper { get; set; }
    }
}
