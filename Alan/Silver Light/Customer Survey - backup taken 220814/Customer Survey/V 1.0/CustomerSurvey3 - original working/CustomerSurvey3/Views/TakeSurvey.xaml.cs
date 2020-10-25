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
    /// Home page for the application.
    /// </summary>
    public partial class Home : Page
    {
        
        /// <summary>
        /// Creates a new <see cref="Home"/> instance.
        /// </summary>
        public Home()
        {
            InitializeComponent();
            this.Title = "Taking survey";
            
            if (App.currentSurvey.Name == null)
            {
                titleTextBlock.Text = string.Format("Survey for: {0} ... {1}.  Region: {2}  Tenure: {3}.", App.selectedResident.Name, App.selectedResident.Address, App.selectedResident.Region, App.selectedResident.Tenure);
            }
            else
            {
                titleTextBlock.Text = string.Format("Survey for: {0} ({1} of {2}) ... {3}.  Region:{4}  Tenure: {5}.", App.currentSurvey.Name, App.currentSurvey.Relationship, App.selectedResident.Name, App.selectedResident.Address, App.selectedResident.Region, App.selectedResident.Tenure);
            }

            if (App.currentSurvey == null)
            {
                System.Windows.MessageBox.Show("Redirecting to first page", "Error", MessageBoxButton.OK);
                Uri u = new Uri("#/SetupSurvey", UriKind.Relative);
                System.Windows.Browser.HtmlPage.Window.Navigate(u);
            }
            else
            {
                takeAnotherSurveyComboBox.ItemsSource = App.context.SurveyTypes;
                populateQuestions();
            }
        }

        private void populateQuestions()
        {
            App.context.SurveyQuestions.Clear();
            var qry = App.context.GetSurveyQuestionsQuery().Where(q => q.SurveyTypeId == App.currentSurvey.SurveyTypeId);
            var op2 = App.context.Load(qry, LoadBehavior.RefreshCurrent, false);
            op2.Completed += (s, ea) =>
            {
                List<SurveyQuestion> questions = App.context.SurveyQuestions.ToList();

                foreach (SurveyQuestion question in questions)
                {
                    UIElement ctrl;
                    TextBlock txtblock;

                    switch (question.Type)
                    {
                        case "MULTI": ctrl = new ComboBox()
                        {
                            Name = question.Id.ToString(),
                            Margin = new Thickness(10, 0, 0, 0),
                            ItemsSource = App.lookups.Where(i => i.Type == question.ValidationRule),
                            DisplayMemberPath = "Text",
                            Height = 26
                        };
                            txtblock = new TextBlock() { Width = 450, Text = question.Text, TextWrapping = TextWrapping.Wrap, Margin = new Thickness(10, 0, 0, 0)};
                            break;
                        case "TEXTBOX":
                            ctrl = new TextBox() { Name = question.Id.ToString(), Margin = new Thickness(10, 0, 0, 0), TextWrapping = TextWrapping.Wrap, Width = 350 };
                            txtblock = new TextBlock() { Width = 450, Text = question.Text, TextWrapping = TextWrapping.Wrap, Margin = new Thickness(10, 0, 0, 0) };
                            break;
                        case "DATE":
                            ctrl = new DatePicker() { Name = question.Id.ToString(), Margin = new Thickness(10, 0, 0, 0), Width = 120 };
                            txtblock = new TextBlock() { Width = 450, Text = question.Text, TextWrapping = TextWrapping.Wrap, Margin = new Thickness(10, 0, 0, 0) };
                            break;
                        case "LISTBOX":
                            ctrl = new ListBox()
                            {
                                Name = question.Id.ToString(),
                                Margin = new Thickness(10, 0, 0, 0),
                                ItemsSource = App.lookups.Where(i => i.Type == question.ValidationRule),
                                DisplayMemberPath = "Text",
                                SelectionMode= SelectionMode.Multiple
                            };
                            txtblock = new TextBlock() { Width = 450, Text = question.Text, TextWrapping = TextWrapping.Wrap, Margin = new Thickness(10, 0, 0, 0) };
                            break;
                        default:
                            ctrl = null;
                            txtblock = new TextBlock() { Width = 900, Text = question.Text, FontWeight = FontWeights.Bold, TextWrapping = TextWrapping.Wrap, Margin = new Thickness(0,5, 0, 5) };
                            break;
                    }

                    StackPanel panel = new StackPanel() { Orientation = System.Windows.Controls.Orientation.Horizontal, Margin = new Thickness(5, 0, 0, 5) };
                    panel.Children.Add(txtblock);
                    if (ctrl != null) panel.Children.Add(ctrl);
                    surveyStackPanel.Children.Add(panel);
                }
            };
        }


        /// <summary>
        /// Executes when the user navigates to this page.
        /// </summary>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)takeAnotherSurveyCheckbox.IsChecked && takeAnotherSurveyComboBox.SelectedItem == null)
                MessageBox.Show("'Take another survey' ticked but no survey selected.  Data not saved.");
            else
            {
                App.context.SurveyInstances.Add(App.currentSurvey);
                createSurveyAnswers();
                App.context.SubmitChanges();

                if ((bool)takeAnotherSurveyCheckbox.IsChecked)
                {
                    this.surveyStackPanel.Children.Clear();
                    takeAnotherSurveyCheckbox.IsChecked = false;

                    SurveyInstance si = new SurveyInstance()
                    {
                        SurveyType = (SurveyType)takeAnotherSurveyComboBox.SelectedItem,
                        UHPersonNo = App.currentSurvey.UHPersonNo,
                        Name = App.currentSurvey.Name,
                        Relationship = App.currentSurvey.Relationship,
                        Date = DateTime.Now,
                        UHTenantRef = App.currentSurvey.UHTenantRef,
                        User = App.currentSurvey.User
                    };
                    App.currentSurvey = si;
                    populateQuestions();
                    PageScrollViewer.UpdateLayout();
                    PageScrollViewer.ScrollToVerticalOffset(0);
                }
                else
                {
                    Uri u = new Uri("#/SetupSurvey", UriKind.Relative);
                    System.Windows.Browser.HtmlPage.Window.Navigate(u);
                }
            }
        }

        private void createSurveyAnswers()
        {
            foreach (UIElement ui in surveyStackPanel.Children)
            {
                foreach (UIElement ctrl in ((StackPanel)ui).Children)
                {
                    if (ctrl.GetType() == typeof(TextBox))
                    {
                        TextBox tbx = (TextBox)ctrl;
                        if (tbx.Text.Length > 0)
                        {
                            SurveyAnswer sa = new SurveyAnswer()
                            {
                                SurveyInstance = App.currentSurvey,
                                SurveyQuestionId = Convert.ToInt32(tbx.Name),
                                Text = tbx.Text
                            };
                            App.context.SurveyAnswers.Add(sa);
                        }
                    }
                    else if (ctrl.GetType() == typeof(ComboBox))
                    {
                        ComboBox cbx = (ComboBox)ctrl;
                        if (cbx.SelectedItem != null)
                        {
                            Lookup l = (Lookup)cbx.SelectedItem;
                            SurveyAnswer sa = new SurveyAnswer()
                            {
                                SurveyInstance = App.currentSurvey,
                                SurveyQuestionId = Convert.ToInt32(cbx.Name),
                                Text = l.Text,
                                Ref = l.Id.ToString()
                            };
                            App.context.SurveyAnswers.Add(sa);
                        }
                    }
                    else if (ctrl.GetType() == typeof(DatePicker))
                    {
                        DatePicker dp = (DatePicker)ctrl;
                        if (dp.SelectedDate != null)
                        {
                            SurveyAnswer sa = new SurveyAnswer()
                            {
                                SurveyInstance = App.currentSurvey,
                                SurveyQuestionId = Convert.ToInt32(dp.Name),
                                Text = dp.SelectedDate.Value.ToSQLFormatString()
                            };
                            App.context.SurveyAnswers.Add(sa);
                        }
                    }
                    else if (ctrl.GetType() == typeof(ListBox))
                    {
                        ListBox lb = (ListBox)ctrl;
                        foreach(Lookup l in lb.SelectedItems)
                        {
                            SurveyAnswer sa = new SurveyAnswer()
                            {
                                SurveyInstance = App.currentSurvey,
                                SurveyQuestionId = Convert.ToInt32(lb.Name),
                                Text = l.Text,
                                Ref = l.Id.ToString()
                            };
                            App.context.SurveyAnswers.Add(sa);
                        }
                    }
                }
            }
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want abandon this survey?  Press OK to confirm or cancel to return", "Question", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                Uri u = new Uri("#/SetupSurvey", UriKind.Relative);
                System.Windows.Browser.HtmlPage.Window.Navigate(u);
            }
        }

        private void takeAnotherSurveyCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            takeAnotherSurveyComboBox.Visibility = System.Windows.Visibility.Visible;
            PageScrollViewer.UpdateLayout();
            PageScrollViewer.ScrollToVerticalOffset(double.MaxValue);
        }

        private void takeAnotherSurveyCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            takeAnotherSurveyComboBox.Visibility = System.Windows.Visibility.Collapsed;
        }
    }
}