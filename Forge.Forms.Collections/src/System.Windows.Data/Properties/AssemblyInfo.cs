//-----------------------------------------------------------------------
// <copyright company="Microsoft">
//      (c) Copyright Microsoft Corporation.
//      This source is subject to the Microsoft Public License (Ms-PL).
//      Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
//      All other rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Markup;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("System.Windows.Data")]
[assembly: AssemblyDescription("Silverlight Common Controls")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Microsoft Corporation")]
[assembly: AssemblyProduct("Microsoft® Silverlight")]
[assembly: AssemblyCopyright("© Microsoft Corporation. All rights reserved.")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("31faf385-370c-488b-a72c-06bd99c1153a")]

[assembly: CLSCompliant(true)]

[assembly: XmlnsPrefix("http://schemas.microsoft.com/winfx/2006/xaml/presentation/sdk", "sdk")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/winfx/2006/xaml/presentation/sdk", "System.Windows.Data")]

[assembly: NeutralResourcesLanguageAttribute("en-us")]
[assembly: InternalsVisibleTo("Microsoft.AppFx.UnitTests.Silverlight, PublicKey=0024000004800000940000000602000000240000525341310004000001000100e338f2a3d602838b9188d41698e3e386fb88eb1f754a24d2b0dda5f4d4523dfe5bf40666e0c18e62ffa17a7bf4a89db0713a58a70401a28ce628a89eede3676c0a458ccbd4ccbda13bc095cca3d8c73e14d186f087953fcc77f10689bf57d85eae579e4819c71847d48a7a8a81fa3ec6a81a86cf4a5510767678b555fab5e999")]
[assembly: InternalsVisibleTo("Microsoft.AppFx.UnitTests.SilverlightVb, PublicKey=0024000004800000940000000602000000240000525341310004000001000100e338f2a3d602838b9188d41698e3e386fb88eb1f754a24d2b0dda5f4d4523dfe5bf40666e0c18e62ffa17a7bf4a89db0713a58a70401a28ce628a89eede3676c0a458ccbd4ccbda13bc095cca3d8c73e14d186f087953fcc77f10689bf57d85eae579e4819c71847d48a7a8a81fa3ec6a81a86cf4a5510767678b555fab5e999")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Revision and Build Numbers 
// by using the '*' as shown below:
[assembly: AssemblyVersion("2.0.5.0")]
[assembly: AssemblyFileVersion("4.0.40413.2002")]

// Type-forwarding the types that used to be defined in this assembly and are now
// defined in core's System.Windows.dll assembly.
[assembly: TypeForwardedTo(typeof(System.Windows.Data.CollectionViewGroup))]
[assembly: TypeForwardedTo(typeof(System.Windows.Data.PropertyGroupDescription))]
[assembly: TypeForwardedTo(typeof(System.ComponentModel.NewItemPlaceholderPosition))]
[assembly: TypeForwardedTo(typeof(System.ComponentModel.IEditableCollectionView))]
