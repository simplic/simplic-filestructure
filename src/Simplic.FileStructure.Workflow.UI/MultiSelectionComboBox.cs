using Simplic.Framework.DBUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.MultiColumnComboBox;
using Telerik.Windows.Data;

namespace Simplic.FileStructure.Workflow.UI
{
    public class MultiSelectionComboBox : RadMultiColumnComboBox
    {
        public MultiSelectionComboBox()
        {
            ItemsSourceProvider = new MultiItemProvider();
            DropDownHeight = 100;
            var bc = new BrushConverter();
            Background = (Brush)bc.ConvertFrom("#aaaaaa");
            OpenDropDownOnFocus = true;
            AutoCompleteProvider.PropertyChanged += AutoCompleteProvider_PropertyChanged;

            Loaded += (s, e) =>
            {
                DropDownContentManager = new MultiSelectionComboBoxContentManager(this);
            };
        }

        private void AutoCompleteProvider_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

        }

        protected override void OnInitializeDropDownContentManager(DropDownContentManagerEventArgs args)
        {
            Console.WriteLine("OnInitializeDropDownContentManager");
            base.OnInitializeDropDownContentManager(args);
        }

        public override void ToggleDropDown()
        {
            Console.WriteLine("ToggleDropDown");
        }

        public override void OpenDropDown()
        {
            Console.WriteLine("OpenDropDown");
        }
    }

    public class MultiItemProvider : ItemsSourceProvider
    {
        private MainViewModel mainViewModel;

        public MultiItemProvider()
        {
            mainViewModel = new MainViewModel();
            ItemsSource = new ObservableCollection<NewModel>(mainViewModel.NewModels);
        }

    }

    public class MultiSelectionComboBoxContentManager : DropDownContentManager
    {
        private IntegratedGridView _listBox;
        private RadMultiColumnComboBox radMultiColumnComboBox;

        public MultiSelectionComboBoxContentManager(RadMultiColumnComboBox radMultiColumnComboBox)
        {
            var bc = new BrushConverter();
            _listBox = new IntegratedGridView();

            this.radMultiColumnComboBox = radMultiColumnComboBox;
            _listBox.Height = 500;

        }

        public override void InitializeDropDownContent(Popup dropDownPopup)
        {
            Console.WriteLine("InitializeDropDownContent");
            dropDownPopup.Child = _listBox;
        }

        public override FrameworkElement DropDownElement => _listBox;

        public override RadMultiColumnComboBox Owner => radMultiColumnComboBox;

        public override ISelectionBridge InitializeSelectionBridge()
        {
            return new Bridge(Owner);
        }

        public override void OnCollectionViewCollectionChanged(QueryableCollectionView collectionView)
        {

        }

        public override void OnMouseDown(object sender, MouseButtonEventArgs args)
        {

        }

        public override void OnMouseUp(object sender, MouseButtonEventArgs args)
        {

        }

        public override void OnPreviewMouseDown(object sender, MouseButtonEventArgs args)
        {

        }

        public override void RefreshDropDownElement()
        {

        }

        public override void SetSelectionMode()
        {
        }

       
    }

    public class Bridge : ISelectionBridge
    {
        public RadMultiColumnComboBox Owner { get; set; }

        public Bridge(RadMultiColumnComboBox radMultiColumnComboBox)
        {
            Owner = radMultiColumnComboBox;
        }

        public void ClearOwnerSelection()
        {

        }

        public void ClearSourceSelection()
        {

        }

        public void InitializeCollectionView(QueryableCollectionView sourceCollectionView)
        {

        }

        public void ItemsDeselectedInOwner(IEnumerable<object> removedItems)
        {

        }

        public void ItemsDeselectedInSource(IEnumerable<object> removedItems)
        {

        }

        public void ItemsSelectedInOwner(IEnumerable<object> addedItems)
        {

        }

        public void ItemsSelectedInSource(IEnumerable<object> addedItems)
        {

        }

        public void SelectedItemsResetInOwner()
        {

        }

        public bool SynchronizeCurrentItemWithSelection()
        {
            return true;
        }

        public void SynchronizeSelectedItemsWithSource()
        {

        }

        public void UnsubscribeFromSourceEvents()
        {

        }
    }
}
