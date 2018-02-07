using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using Forge.Forms.Annotations;
using Forge.Forms.Collections;
using Forge.Forms.Collections.Interfaces;
using Material.Application.Routing;
using MaterialDesignThemes.Wpf;
using Microsoft.EntityFrameworkCore;

namespace Forge.Forms.Demo.Routes
{
    class CrudExample : DbContext
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
            Context.Add((Person)modelContext.NewModel);
            Context.SaveChanges();
            return null;
        }

        public IUpdateActionContext Intercept(IUpdateActionContext modelContext)
        {
            if (modelContext.NewModel is Person person)
            {
                Context.Entry(person).State = EntityState.Modified;
                Context.SaveChanges();
            }

            return modelContext;
        }

        public void Intercept(IRemoveActionContext modelContext)
        {

            if (modelContext.OldModel is Person person)
            {
                Context.Remove(person);
                Context.SaveChanges();
            }
        }
    }

    public class CrudRoute : Route
    {
        public ObservableCollection<Person> Items { get; }

        public CrudRoute()
        {
            RouteConfig.Title = "Crud examples";
            RouteConfig.Icon = PackIconKind.Table;

            var context = new CrudExample();
            context.Database.EnsureCreated();

            context.Persons.Load();
            Items = context.Persons.Local.ToObservableCollection();

            var interceptor = new CrudInterceptor(context);
            DynamicDataGrid.AddInterceptorChain.Add(interceptor);
            DynamicDataGrid.UpdateInterceptorChain.Add(interceptor);
            DynamicDataGrid.RemoveInterceptorChain.Add(interceptor);
        }
    }

    public class Person : INotifyPropertyChanged
    {
        [FieldIgnore]
        public int Id { get; set; }

        private string firstName;
        private string lastName;

        [StringLength(15)]
        [Value(Must.BeGreaterThan, 5)]
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

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}