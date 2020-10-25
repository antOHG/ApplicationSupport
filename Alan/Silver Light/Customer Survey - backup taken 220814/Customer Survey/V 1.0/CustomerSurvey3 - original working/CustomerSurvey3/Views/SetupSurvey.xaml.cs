namespace CustomerSurvey3
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Animation;
    using System.Windows.Shapes;
    using System.Windows.Navigation;
    using CustomerSurvey3;
    using CustomerSurvey3.Web;
    using CustomerSurvey3.Web.Services;
    using System.ServiceModel.DomainServices.Client;

    /// <summary>
    /// <see cref="Page"/> class to present information about the current application.
    /// </summary>
    public partial class About : Page
    {
        /// <summary>
        /// Creates a new instance of the <see cref="About"/> class.
        /// </summary>
        public About()
        {
            InitializeComponent();
            this.Title = "Setting up survey";
            App.selectedResident = null;
            surveyTypeComboBox.ItemsSource = App.context.SurveyTypes;
         }

        /// <summary>
        /// Executes when the user navigates to this page.
        /// </summary>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void findUHTenant_ResultDomainDataSource_LoadedData(object sender, System.Windows.Controls.LoadedDataEventArgs e)
        {

            if (e.HasError)
            {
                System.Windows.MessageBox.Show(e.Error.ToString(), "Load Error", System.Windows.MessageBoxButton.OK);
                e.MarkErrorAsHandled();
            }

            DomainDataSource dds = (DomainDataSource)sender;
            if (dds.DataView.Count == 0)
            {
                System.Windows.MessageBox.Show("No records found.","Customer Surveys database", MessageBoxButton.OK);
            }
        }

        private void takeSurveyButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            App.selectedResident = findUHTenant_ResultDataGrid.SelectedItem as FindUHTenant_Result;

            if (App.selectedResident == null) MessageBox.Show("No resident selected.", "Customer surveys database", MessageBoxButton.OK);
            else if ((bool)relatedPersonCheckBox.IsChecked && (relatedNameTextBox.Text.Length ==0 || relatedRelationTextBox.Text.Length==0))
            {
                MessageBox.Show("Name and relation need to be completed.", "Customer surveys database", MessageBoxButton.OK);
            }
            else if (surveyTypeComboBox.SelectedItem==null) MessageBox.Show("No survey selected.","Customer surveys database", MessageBoxButton.OK);
            else
            {
                App.currentSurvey = new SurveyInstance()
                {
                    UHTenantRef = App.selectedResident.UHTenancyRef,
                    SurveyType = (SurveyType)surveyTypeComboBox.SelectedItem,
                    Date = DateTime.Now,
                    User = App.currentUser
                };
                if ((bool)relatedPersonCheckBox.IsChecked)
                {
                    App.currentSurvey.UHPersonNo = 0;
                    App.currentSurvey.Name = relatedNameTextBox.Text;
                    App.currentSurvey.Relationship = relatedRelationTextBox.Text;
                }
                else App.currentSurvey.UHPersonNo = App.selectedResident.PersNo;

                Uri u = new Uri("#/TakeSurvey", UriKind.Relative);            
                System.Windows.Browser.HtmlPage.Window.Navigate(u);
            }
        }
        private void surveyTypeDomainDataSource_LoadedData(object sender, LoadedDataEventArgs e)
        {

            if (e.HasError)
            {
                System.Windows.MessageBox.Show(e.Error.ToString(), "Load Error", System.Windows.MessageBoxButton.OK);
                e.MarkErrorAsHandled();
            }
        }
    }
}