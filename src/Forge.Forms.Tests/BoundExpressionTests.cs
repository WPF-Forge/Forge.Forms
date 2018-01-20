using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using Forge.Forms.DynamicExpressions;
using Forge.Forms.FormBuilding;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Forge.Forms.Tests
{
    public class DummyForm : IDynamicForm, INotifyPropertyChanged
    {
        private object context;
        private object model;
        private object value;

        public object Model
        {
            get => model;
            set
            {
                model = value;
                OnPropertyChanged();
            }
        }

        public object Context
        {
            get => context;
            set
            {
                context = value;
                OnPropertyChanged();
            }
        }

        public event EventHandler<ActionEventArgs> OnAction;

        public object Value
        {
            get => value;
            set
            {
                this.value = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class DummyFormContext : IResourceContext
    {
        private readonly DummyForm form;

        public DummyFormContext(DummyForm form)
        {
            this.form = form;
        }

        public object GetModelInstance()
        {
            return form.Value;
        }

        public BindingExpressionBase[] GetBindings()
        {
            return null;
        }

        public object GetContextInstance()
        {
            return form.Context;
        }

        public Binding CreateDirectModelBinding()
        {
            return new Binding(nameof(form.Model))
            {
                Source = form
            };
        }

        public Binding CreateModelBinding(string path)
        {
            return new Binding(nameof(form.Value) + Resource.FormatPath(path))
            {
                Source = form
            };
        }

        public Binding CreateContextBinding(string path)
        {
            return new Binding(nameof(form.Context) + Resource.FormatPath(path))
            {
                Source = form
            };
        }

        public object TryFindResource(object key)
        {
            throw new NotImplementedException();
        }

        public object FindResource(object key)
        {
            throw new NotImplementedException();
        }

        public void AddResource(object key, object value)
        {
            throw new NotImplementedException();
        }

        public void OnAction(object model, string action, object parameter)
        {
            throw new NotImplementedException();
        }
    }

    [TestClass]
    public class BoundExpressionTests
    {
        [TestMethod]
        public void TestSingleResource()
        {
            T TestSingleResource<T>(string str) where T : Resource
            {
                var expression = BoundExpression.Parse(str);
                Assert.IsNull(expression.StringFormat);
                Assert.AreEqual(1, expression.Resources.Count);
                Assert.IsInstanceOfType(expression.Resources[0], typeof(T));
                return (T)expression.Resources[0];
            }

            var staticResource = TestSingleResource<StaticResource>("{StaticResource SRName}");
            Assert.AreEqual("SRName", staticResource.ResourceKey);
            Assert.IsFalse(staticResource.IsDynamic);

            var dynamicResource = TestSingleResource<DynamicResource>("{DynamicResource DRName}");
            Assert.AreEqual("DRName", dynamicResource.ResourceKey);
            Assert.IsTrue(dynamicResource.IsDynamic);

            var binding = TestSingleResource<PropertyBinding>("{Binding Person.Name}");
            Assert.AreEqual("Person.Name", binding.PropertyPath);
            Assert.IsFalse(binding.OneTimeBinding);
            Assert.IsTrue(binding.IsDynamic);

            binding = TestSingleResource<PropertyBinding>("{Property Person.Name}");
            Assert.AreEqual("Person.Name", binding.PropertyPath);
            Assert.IsTrue(binding.OneTimeBinding);
            Assert.IsFalse(binding.IsDynamic);

            var contextBinding = TestSingleResource<ContextPropertyBinding>("{ContextBinding Person.Name}");
            Assert.AreEqual("Person.Name", contextBinding.PropertyPath);
            Assert.IsFalse(contextBinding.OneTimeBinding);
            Assert.IsTrue(contextBinding.IsDynamic);

            contextBinding = TestSingleResource<ContextPropertyBinding>("{ContextProperty Person.Name}");
            Assert.AreEqual("Person.Name", contextBinding.PropertyPath);
            Assert.IsTrue(contextBinding.OneTimeBinding);
            Assert.IsFalse(contextBinding.IsDynamic);
        }

        [TestMethod]
        public void TestSingleResourceWithFormat()
        {
            void TestSingleResource<T>(string str, string format) where T : Resource
            {
                var expression = BoundExpression.Parse(str);
                Assert.AreEqual(format, expression.StringFormat);
                Assert.AreEqual(1, expression.Resources.Count);
                Assert.IsInstanceOfType(expression.Resources[0], typeof(T));
            }

            TestSingleResource<StaticResource>("{StaticResource Name:dd/MM/yyyy}", "{0:dd/MM/yyyy}");
            TestSingleResource<DynamicResource>("{DynamicResource Name:c}", "{0:c}");
            TestSingleResource<PropertyBinding>("{Binding Name,20:0.00}", "{0,20:0.00}");
            TestSingleResource<PropertyBinding>("{Property Name:dd/MM/yyyy}", "{0:dd/MM/yyyy}");
            TestSingleResource<ContextPropertyBinding>("{ContextBinding Name:dd/MM/yyyy}", "{0:dd/MM/yyyy}");
            TestSingleResource<ContextPropertyBinding>("{ContextProperty Name:dd/MM/yyyy}", "{0:dd/MM/yyyy}");
        }

        [TestMethod]
        public void TestMultipleResources()
        {
            var expression =
                BoundExpression.Parse(
                    "Your name is {Binding Name}. Hello {Binding Name,-30}, welcome to {ContextProperty Place}. It is year {DynamicResource CurrentYear:yyyy}!");

            Assert.AreEqual("Your name is {0}. Hello {0,-30}, welcome to {1}. It is year {2:yyyy}!",
                expression.StringFormat);
            Assert.AreEqual(3, expression.Resources.Count);
        }

        [TestMethod]
        public void TestBraceEscapes()
        {
            try
            {
                BoundExpression.Parse("Invalid curly brace { in this sentence.");
                Assert.Fail();
            }
            catch
            {
            }

            var expression =
                BoundExpression.Parse("Escaped {{Binding Name}} {StaticResource }}N{{a}}me{{}}{{:{{dd/MM/yyyy}}}");
            Assert.AreEqual("Escaped {{Binding Name}} {0:{{dd/MM/yyyy}}}", expression.StringFormat);
            Assert.AreEqual(1, expression.Resources.Count);
            var resource = (StaticResource)expression.Resources[0];
            Assert.AreEqual("}N{a}me{}{", resource.ResourceKey);
        }

        [TestMethod]
        public void TestVerbatimExpression()
        {
            var expression = BoundExpression.Parse("@{Binding Name}");
            Assert.AreEqual("{Binding Name}", expression.StringFormat);

            expression = BoundExpression.Parse("@");
            Assert.AreEqual("", expression.StringFormat);

            expression = BoundExpression.Parse("\\");
            Assert.AreEqual("", expression.StringFormat);

            expression = BoundExpression.Parse("\\\\");
            Assert.AreEqual("\\", expression.StringFormat);

            expression = BoundExpression.Parse("\\@");
            Assert.AreEqual("@", expression.StringFormat);

            expression = BoundExpression.Parse("@@");
            Assert.AreEqual("@", expression.StringFormat);

            expression = BoundExpression.Parse("\\@@");
            Assert.AreEqual("@@", expression.StringFormat);

            expression = BoundExpression.Parse("\\@{Binding Name}");
            Assert.AreEqual("@{0}", expression.StringFormat);
        }

        [TestMethod]
        public void TestObjectBinding()
        {
            var expression = BoundExpression.Parse("{Binding} {Binding Test} {ContextBinding}");
            var binding = (PropertyBinding)expression.Resources[0];
            Assert.AreEqual("", binding.PropertyPath);
            var contextBinding = (ContextPropertyBinding)expression.Resources[2];
            Assert.AreEqual("", contextBinding.PropertyPath);
        }

        [TestMethod]
        public void TestGetValueSingleResource()
        {
            var context = new DummyFormContext(new DummyForm
            {
                Value = new List<int> { 1, 2, 3 },
                Context = new List<int> { 1, 2, 3, 4, 5 }
            });

            var expression1 = BoundExpression.Parse("{Binding Count} items.");
            var expression2 = BoundExpression.Parse("{Property Count} items.");
            var expression3 = BoundExpression.Parse("{ContextBinding Count} items.");
            var expression4 = BoundExpression.Parse("{ContextProperty Count} items.");
            var value1 = expression1.GetValue(context).Value;
            var value2 = expression2.GetValue(context).Value;
            var value3 = expression3.GetValue(context).Value;
            var value4 = expression4.GetValue(context).Value;
            var string1 = expression1.GetStringValue(context).Value;
            var string2 = expression2.GetStringValue(context).Value;
            var string3 = expression3.GetStringValue(context).Value;
            var string4 = expression4.GetStringValue(context).Value;
            Assert.AreEqual(3, value1);
            Assert.AreEqual(3, value2);
            Assert.AreEqual(5, value3);
            Assert.AreEqual(5, value4);
            Assert.AreEqual("3 items.", string1);
            Assert.AreEqual("3 items.", string2);
            Assert.AreEqual("5 items.", string3);
            Assert.AreEqual("5 items.", string4);

            var expression = BoundExpression.Parse("{Binding Count}");
            Assert.AreEqual(3, expression.GetValue(context).Value);
            Assert.AreEqual("3", expression.GetStringValue(context).Value);
        }

        [TestMethod]
        public void TestGetValueMultipleResources()
        {
            var context = new DummyFormContext(new DummyForm
            {
                Value = new List<int> { 1, 2, 3 },
                Context = new List<int> { 1, 2, 3, 4, 5 }
            });

            var expression = BoundExpression.Parse("{Binding Count} items from {ContextBinding Count}.");
            Assert.IsNull(expression.GetValue(context).Value);
            Assert.AreEqual("3 items from 5.", expression.GetStringValue(context).Value);
        }

        [TestMethod]
        public void TestContextualResources()
        {
            var proxy = new BindingProxy
            {
                Value = 42
            };

            var expression = BoundExpression.Parse("Input number must be between {MinValue:0.00} and {MaxValue}.",
                new Dictionary<string, object>
                {
                    ["MinValue"] = 15.1125d,
                    ["MaxValue"] = proxy
                });

            var value = expression.GetStringValue(null);
            Assert.AreEqual("Input number must be between 15.11 and 42.", value.Value);
        }

        [TestMethod]
        public void TestContextualResourcesNestedProperties()
        {
            var expression = BoundExpression.Parse("{Model.Name} {Model.Value} {Model.Grades[1]}",
                new Dictionary<string, object>
                {
                    ["Model"] = new Model
                    {
                        Name = "Test",
                        Value = 42,
                        Grades = new List<int> { 8, 9, 10 }
                    }
                });

            var value = expression.GetStringValue(null);
            Assert.AreEqual("Test 42 9", value.Value);

            expression = BoundExpression.Parse("{^List[2].Date:yyyy-MM-dd}", new Dictionary<string, object>
            {
                ["List"] = new List<Model>
                {
                    new Model(),
                    new Model(),
                    new Model { Date = new DateTime(2017, 02, 03) }
                }
            });

            value = expression.GetStringValue(null);
            Assert.AreEqual("2017-02-03", value.Value);
        }

        [TestMethod]
        public void TestSelfBinding()
        {
            var expression = BoundExpression.Parse("{Binding}");
            Assert.IsTrue(expression.Resources[0] is PropertyBinding b && b.PropertyPath == "");
        }

        [TestMethod]
        public void TestValueConverters()
        {
            var context = new DummyFormContext(new DummyForm
            {
                Value = new Model
                {
                    Name = "Test"
                },
                Context = new List<int> { 1, 2, 3, 4, 5 }
            });

            var expression =
                BoundExpression.Parse(
                    "Default: {Binding Name}, Uppercase: {Binding Name|ToUpper}, Lowercase: {Binding Name|ToLower}");

            var str = expression.GetStringValue(context).Value;
            Assert.AreEqual("Default: Test, Uppercase: TEST, Lowercase: test", str);

            expression = BoundExpression.Parse("{Binding Name|ToUpper}");
            Assert.IsNull(expression.StringFormat);
        }

        [TestMethod]
        public void TestApostropheSyntax()
        {
            var expression = BoundExpression.Parse("Hello, {Binding 'FirstName'}");
            Assert.AreEqual("FirstName", ((PropertyBinding)expression.Resources[0]).PropertyPath);

            expression = BoundExpression.Parse("{FileBinding 'C:/Documents, Files/my file.txt'}");
            Assert.AreEqual("C:/Documents, Files/my file.txt", ((FileBinding)expression.Resources[0]).FilePath);
            Assert.IsTrue(((FileBinding)expression.Resources[0]).IsDynamic);

            expression = BoundExpression.Parse("{File 'C:/Documents, Files/{my file}.txt',500|ToUpper}");
            Assert.AreEqual("C:/Documents, Files/{my file}.txt", ((FileBinding)expression.Resources[0]).FilePath);
            Assert.IsFalse(((FileBinding)expression.Resources[0]).IsDynamic);
        }

        private class Model
        {
            public string Name { get; set; }

            public int Value { get; set; }

            public DateTime Date { get; set; }

            public List<int> Grades { get; set; }
        }
    }
}
