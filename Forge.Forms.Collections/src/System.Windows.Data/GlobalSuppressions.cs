//-----------------------------------------------------------------------
// <copyright file="GlobalSuppressions.cs" company="Microsoft">
//      (c) Copyright Microsoft Corporation.
//      This source is subject to the Microsoft Public License (Ms-PL).
//      Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
//      All other rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace", Target = "System.Windows.Data", Justification = "ListCollectionView shipped in namespace System.Windows.Data.")]
[assembly: SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Scope = "member", Target = "System.Windows.Data.PagedCollectionView.#ProcessCollectionChanged(System.Collections.Specialized.NotifyCollectionChangedEventArgs)")]
[assembly: SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope = "member", Target = "System.Windows.Data.PagedCollectionView.#ItemCount", Justification = "This is part of an interface definition and is called through the interface.")]
[assembly: SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope = "member", Target = "System.Windows.Data.PagedCollectionView.#TotalItemCount", Justification = "This is part of an interface definition and is called through the interface.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope = "member", Target = "System.Windows.Data.PagedCollectionView.#ActiveComparer")]
