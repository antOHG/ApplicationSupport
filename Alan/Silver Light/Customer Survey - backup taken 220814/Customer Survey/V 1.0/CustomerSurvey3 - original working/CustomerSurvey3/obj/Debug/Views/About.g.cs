#pragma checksum "C:\VS Projects\CustomerSurvey3\CustomerSurvey3\Views\About.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "6E801D0113E781B7A01F69177F5AB6BC"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.235
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace CustomerSurvey3 {
    
    
    public partial class About : System.Windows.Controls.Page {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.ScrollViewer PageScrollViewer;
        
        internal System.Windows.Controls.StackPanel ContentStackPanel;
        
        internal System.Windows.Controls.TextBlock HeaderText;
        
        internal System.Windows.Controls.TextBlock ContentText;
        
        internal System.Windows.Controls.DomainDataSource findUHTenant_ResultDomainDataSource;
        
        internal System.Windows.Controls.TextBox nameTextBox;
        
        internal System.Windows.Controls.TextBox addressTextBox;
        
        internal System.Windows.Controls.Button findUHTenant_ResultDomainDataSourceLoadButton;
        
        internal System.Windows.Controls.DataGrid findUHTenant_ResultDataGrid;
        
        internal System.Windows.Controls.DataGridTextColumn uHTenantRefColumn;
        
        internal System.Windows.Controls.DataGridTextColumn persNoColumn;
        
        internal System.Windows.Controls.DataGridTextColumn name;
        
        internal System.Windows.Controls.DataGridTextColumn address;
        
        internal System.Windows.Controls.CheckBox relatedPersonCheckBox;
        
        internal System.Windows.Controls.TextBox relatedNameTextBox;
        
        internal System.Windows.Controls.TextBox relatedRelationTextBox;
        
        internal System.Windows.Controls.ComboBox surveyTypeComboBox;
        
        internal System.Windows.Controls.Button takeSurveyButton;
        
        internal System.Windows.Controls.DomainDataSource surveyTypeDomainDataSource;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/CustomerSurvey3;component/Views/About.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.PageScrollViewer = ((System.Windows.Controls.ScrollViewer)(this.FindName("PageScrollViewer")));
            this.ContentStackPanel = ((System.Windows.Controls.StackPanel)(this.FindName("ContentStackPanel")));
            this.HeaderText = ((System.Windows.Controls.TextBlock)(this.FindName("HeaderText")));
            this.ContentText = ((System.Windows.Controls.TextBlock)(this.FindName("ContentText")));
            this.findUHTenant_ResultDomainDataSource = ((System.Windows.Controls.DomainDataSource)(this.FindName("findUHTenant_ResultDomainDataSource")));
            this.nameTextBox = ((System.Windows.Controls.TextBox)(this.FindName("nameTextBox")));
            this.addressTextBox = ((System.Windows.Controls.TextBox)(this.FindName("addressTextBox")));
            this.findUHTenant_ResultDomainDataSourceLoadButton = ((System.Windows.Controls.Button)(this.FindName("findUHTenant_ResultDomainDataSourceLoadButton")));
            this.findUHTenant_ResultDataGrid = ((System.Windows.Controls.DataGrid)(this.FindName("findUHTenant_ResultDataGrid")));
            this.uHTenantRefColumn = ((System.Windows.Controls.DataGridTextColumn)(this.FindName("uHTenantRefColumn")));
            this.persNoColumn = ((System.Windows.Controls.DataGridTextColumn)(this.FindName("persNoColumn")));
            this.name = ((System.Windows.Controls.DataGridTextColumn)(this.FindName("name")));
            this.address = ((System.Windows.Controls.DataGridTextColumn)(this.FindName("address")));
            this.relatedPersonCheckBox = ((System.Windows.Controls.CheckBox)(this.FindName("relatedPersonCheckBox")));
            this.relatedNameTextBox = ((System.Windows.Controls.TextBox)(this.FindName("relatedNameTextBox")));
            this.relatedRelationTextBox = ((System.Windows.Controls.TextBox)(this.FindName("relatedRelationTextBox")));
            this.surveyTypeComboBox = ((System.Windows.Controls.ComboBox)(this.FindName("surveyTypeComboBox")));
            this.takeSurveyButton = ((System.Windows.Controls.Button)(this.FindName("takeSurveyButton")));
            this.surveyTypeDomainDataSource = ((System.Windows.Controls.DomainDataSource)(this.FindName("surveyTypeDomainDataSource")));
        }
    }
}

