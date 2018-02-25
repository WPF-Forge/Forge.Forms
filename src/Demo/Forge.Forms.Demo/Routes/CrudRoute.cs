using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using Forge.Forms.Annotations;
using Forge.Forms.Collections;
using Forge.Forms.Collections.Interfaces;
using Material.Application.Routing;
using MaterialDesignThemes.Wpf;
using Microsoft.EntityFrameworkCore;

namespace Forge.Forms.Demo.Routes
{
    public class CrudExample : DbContext
    {
        public DbSet<Person> Persons { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=crud.db");
        }
    }

    class CrudInterceptor : IAddActionInterceptor, IUpdateActionInterceptor, IRemoveActionInterceptor
    {
        private CrudExample Context { get; }

        public CrudInterceptor(CrudExample context)
        {
            Context = context;
        }

        public IAddActionContext Intercept(IAddActionContext modelContext)
        {
            try
            {
                Context.Add(modelContext.NewModel);
                Context.SaveChanges();
                return null;
            }
            catch
            {
                return modelContext;
            }
        }

        public IUpdateActionContext Intercept(IUpdateActionContext modelContext)
        {
            try
            {
                if (modelContext.NewModel is Person person)
                {
                    Context.Entry(person).State = EntityState.Modified;
                    Context.SaveChanges();
                }

                return modelContext;
            }
            catch
            {
                return modelContext;
            }
        }

        public void Intercept(IRemoveActionContext modelContext)
        {
            try
            {
                if (modelContext.OldModel is Person person)
                {
                    Context.Remove(person);
                    Context.SaveChanges();
                }
            }
            catch
            {
                // ignored
            }
        }
    }

    public class CrudRoute : Route
    {
        public CrudExample DbContext { get; }
        public ObservableCollection<Person> Items { get; }

        public CrudRoute()
        {
            RouteConfig.Title = "Crud examples";
            RouteConfig.Icon = PackIconKind.Table;

            DbContext = new CrudExample();
            DbContext.Database.EnsureCreated();

            DbContext.Persons.Load();
            Items = DbContext.Persons.Local.ToObservableCollection();

            var interceptor = new CrudInterceptor(DbContext);
            DynamicDataGrid.AddInterceptorChain.Add(interceptor);
            DynamicDataGrid.UpdateInterceptorChain.Add(interceptor);
            DynamicDataGrid.RemoveInterceptorChain.Add(interceptor);
        }
    }

    [DisplayName(nameof(Profession.Name))]
    public class Profession
    {      
        public string Name { get; set; }
    }

    public class Person : INotifyPropertyChanged
    {
        [FieldIgnore]
        public int Id { get; set; }

        private string firstName;
        private string lastName;
        private Person parent;
        private Profession profession = new Profession();

        [StringLength(15)]
        [Value(Must.NotBeEmpty)]
        public string FirstName
        {
            get => firstName;
            set
            {
                firstName = value;
                OnPropertyChanged();
            }
        }

        public string LastName
        {
            get => lastName;
            set
            {
                lastName = value;
                OnPropertyChanged();
            }
        }

        [SelectFrom("{ContextBinding Items}", DisplayPath = nameof(FirstName))]
        [Value(Must.NotBeEqualTo, "{Binding}", Message = "You can't be your own parent!")]
        [Value(Must.NotBeEmpty)]
        public Person Parent
        {
            get => parent;
            set
            {
                parent = value;
                OnPropertyChanged();
            }
        }

        [NotMapped]
        [FieldIgnore]
        public List<Profession> PossibleProfessions { get; } = new List<Profession>
        {
            new Profession{Name = "Medic"},
            new Profession{Name = "Engineer"}
        };

        [NotMapped]
        [SelectFrom("{Binding PossibleProfessions}", DisplayPath = "Name")]
        public Profession Profession
        {
            get => profession;
            set
            {
                profession = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
