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

namespace CustomerSurvey3.Views
{
    public partial class FindResident : Page
    {
        Web.Services.SurveyContext context;

        public FindResident()
        {
            InitializeComponent();
            context = new Web.Services.SurveyContext();
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            EntityQuery<FindUHTenant_Result> q = context.GetTenantsQuery("","");
            context.Load(q);
            var results = context.FindUHTenant_Results.FirstOrDefault();
            //IQueryable<Web.FindUHTenant_Result> results = q.Query. as IQueryable<Web.FindUHTenant_Result>;

            context.Load(context.GetLookupsQuery());
            var l = context.Lookups;
            

            this.reultsListBox.DataContext = context;
        }

    }
}
