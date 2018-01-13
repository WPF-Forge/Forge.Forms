using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Material.Application.Commands;

namespace Material.Application.Controls
{
    public class PageableCollection<T> : INotifyPropertyChanged
    {
        #region Public Properties

        // Default Entries per page Number
        private int pageSize = 10;

        public int PageSize
        {
            get
            {
                return pageSize;
            }
            set
            {
                if (pageSize != value)
                {
                    pageSize = value;
                    SendPropertyChanged(nameof(PageSize));
                    OnPageNumberChange();
                    Reset();
                }
            }
        }

        public int TotalPagesNumber
        {
            get
            {
                if (AllObjects != null && PageSize > 0)
                {
                    return (AllObjects.Count - 1) / PageSize + 1;
                }
                return 0;
            }
        }

        private int currentPageNumber = 1;
        public int CurrentPageNumber
        {
            get
            {
                return currentPageNumber;
            }

            protected set
            {
                if (currentPageNumber != value)
                {
                    currentPageNumber = value;
                    OnPageNumberChange();
                }
            }
        }

        public int PageStart => TotalItems == 0 ? 0 : (CurrentPageNumber - 1) * PageSize + 1;

        public int PageEnd => CurrentPageNumber != TotalPagesNumber ? CurrentPageNumber * PageSize : TotalItems;

        public int TotalItems => AllObjects.Count;

        private ObservableCollection<T> currentPageItems;

        public ObservableCollection<T> CurrentPageItems
        {
            get
            {
                return currentPageItems;
            }
            private set
            {
                if (currentPageItems != value)
                {
                    currentPageItems = value;
                    SendPropertyChanged(nameof(CurrentPageItems));
                }
            }
        }

        #endregion

        #region Protected Properties

        protected ObservableCollection<T> AllObjects { get; set; }

        #endregion

        #region ctor

        //private PageableCollection()
        //{
        //}

        public PageableCollection(IEnumerable<T> allObjects, int? entriesPerPage = null)
        {
            AllObjects = new ObservableCollection<T>(allObjects);
            if (entriesPerPage != null)
                PageSize = (int)entriesPerPage;
            Calculate(CurrentPageNumber);

            NextPageCommand = new UntrackedCommand(nothing =>
            {
                GoToNextPage();
            }, nothing => CurrentPageNumber < TotalPagesNumber);
            PreviousPageCommand = new UntrackedCommand(nothing =>
            {
                GoToPreviousPage();
            }, nothing => CurrentPageNumber > 1);
        }

        public UntrackedCommand NextPageCommand { get; }

        public UntrackedCommand PreviousPageCommand { get; }

        #endregion

        #region Public Methods

        public void GoToNextPage()
        {
            if (CurrentPageNumber != TotalPagesNumber)
            {
                CurrentPageNumber++;
                Calculate(CurrentPageNumber);
            }
        }

        public void GoToPreviousPage()
        {
            if (CurrentPageNumber > 1)
            {
                CurrentPageNumber--;
                Calculate(CurrentPageNumber);
            }
        }

        public virtual void Remove(T item)
        {
            AllObjects.Remove(item);

            // Update the total number of pages

            // if the last item on the last page is removed
            if (currentPageNumber > TotalPagesNumber)
                currentPageNumber--;

            Calculate(CurrentPageNumber);
            OnTotalChange();
        }

        public virtual void Add(T item)
        {
            AllObjects.Add(item);
            currentPageNumber = TotalPagesNumber;
            Calculate(currentPageNumber);
            OnTotalChange();
        }

        #endregion

        #region INotifyPropertyChanged Implementation

        public event PropertyChangedEventHandler PropertyChanged;
        public void SendPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion

        #region Private Methods

        protected void Calculate(int pageNumber)
        {
            var upperLimit = pageNumber * PageSize;

            CurrentPageItems =
                new ObservableCollection<T>(
                    AllObjects.Where(x => AllObjects.IndexOf(x) > upperLimit - (PageSize + 1) && AllObjects.IndexOf(x) < upperLimit));
        }

        private void Reset()
        {
            CurrentPageNumber = 1;
            Calculate(CurrentPageNumber);
            OnTotalChange();
        }

        private void OnPageNumberChange()
        {
            SendPropertyChanged(nameof(CurrentPageNumber));
            SendPropertyChanged(nameof(PageStart));
            SendPropertyChanged(nameof(PageEnd));
            PreviousPageCommand.RaiseCanExecuteChanged();
            NextPageCommand.RaiseCanExecuteChanged();
        }

        private void OnTotalChange()
        {
            SendPropertyChanged(nameof(TotalPagesNumber));
            SendPropertyChanged(nameof(TotalItems));
            OnPageNumberChange();
        }

        #endregion
    }
}