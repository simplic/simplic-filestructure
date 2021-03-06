﻿using Simplic.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Simplic.FileStructure.UI
{
    /// <summary>
    /// Interaction logic for DocumentPathOverview.xaml
    /// </summary>
    public partial class DocumentPathOverview : UserControl
    {
        /// <summary>
        /// Dependency property
        /// </summary>
        public static readonly DependencyProperty DocumentIdProperty =
            DependencyProperty.Register("DocumentId", typeof(Guid), typeof(DocumentPathOverview), new PropertyMetadata(Guid.Empty, DocumentIdChangedCallback));

        /// <summary>
        /// Document id changed callback
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void DocumentIdChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as DocumentPathOverview;
            if (control != null)
            {
                var viewModel = new DocumentPathOverViewViewModel((Guid)e.NewValue);
                control.rootGrid.DataContext = viewModel;
            }
        }

        /// <summary>
        /// Initialize document overview control
        /// </summary>
        public DocumentPathOverview()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Save changes
        /// </summary>
        public void Save()
        {
            ViewModel?.Save();
        }

        /// <summary>
        /// Gets or sets the document id
        /// </summary>
        public Guid DocumentId
        {
            get { return (Guid)GetValue(DocumentIdProperty); }
            set { SetValue(DocumentIdProperty, value); }
        }

        /// <summary>
        /// Gets the path overview viewmodel
        /// </summary>
        public DocumentPathOverViewViewModel ViewModel
        {
            get
            {
                return rootGrid.DataContext as DocumentPathOverViewViewModel;
            }
        }
    }
}
