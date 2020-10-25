
namespace CustomerSurvey3.Web.Services
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Data;
    using System.Linq;
    using System.ServiceModel.DomainServices.EntityFramework;
    using System.ServiceModel.DomainServices.Hosting;
    using System.ServiceModel.DomainServices.Server;
    using CustomerSurvey3.Web;
    using System.Web;


    // Implements application logic using the SurveyEntitiesContainer context.
    // TODO: Add your application logic to these methods or in additional methods.
    // TODO: Wire up authentication (Windows/ASP.NET Forms) and uncomment the following to disable anonymous access
    // Also consider adding roles to restrict access as appropriate.
    // [RequiresAuthentication]
    [EnableClientAccess()]
    public class SurveyService : LinqToEntitiesDomainService<SurveyEntitiesContainer>
    {

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'Lookups' query.
        public IQueryable<Lookup> GetLookups()
        {
            return this.ObjectContext.Lookups.Where(l => l.Active).OrderBy(l => l.Type).ThenBy(l => l.Order);
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'SurveyAnswers' query.
        public IQueryable<SurveyAnswer> GetSurveyAnswers()
        {
            return this.ObjectContext.SurveyAnswers;
        }

        public void InsertSurveyAnswer(SurveyAnswer surveyAnswer)
        {
            if ((surveyAnswer.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(surveyAnswer, EntityState.Added);
            }
            else
            {
                this.ObjectContext.SurveyAnswers.AddObject(surveyAnswer);
            }
        }

        public void UpdateSurveyAnswer(SurveyAnswer currentSurveyAnswer)
        {
            this.ObjectContext.SurveyAnswers.AttachAsModified(currentSurveyAnswer, this.ChangeSet.GetOriginal(currentSurveyAnswer));
        }

        public void DeleteSurveyAnswer(SurveyAnswer surveyAnswer)
        {
            if ((surveyAnswer.EntityState == EntityState.Detached))
            {
                this.ObjectContext.SurveyAnswers.Attach(surveyAnswer);
            }
            this.ObjectContext.SurveyAnswers.DeleteObject(surveyAnswer);
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'SurveyInstances' query.
        public IQueryable<SurveyInstance> GetSurveyInstances()
        {
            return this.ObjectContext.SurveyInstances;
        }

        public void InsertSurveyInstance(SurveyInstance surveyInstance)
        {
            if ((surveyInstance.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(surveyInstance, EntityState.Added);
            }
            else
            {
                this.ObjectContext.SurveyInstances.AddObject(surveyInstance);
            }
        }

        public void UpdateSurveyInstance(SurveyInstance currentSurveyInstance)
        {
            this.ObjectContext.SurveyInstances.AttachAsModified(currentSurveyInstance, this.ChangeSet.GetOriginal(currentSurveyInstance));
        }

        public void DeleteSurveyInstance(SurveyInstance surveyInstance)
        {
            if ((surveyInstance.EntityState == EntityState.Detached))
            {
                this.ObjectContext.SurveyInstances.Attach(surveyInstance);
            }
            this.ObjectContext.SurveyInstances.DeleteObject(surveyInstance);
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'SurveyQuestions' query.
        public IQueryable<SurveyQuestion> GetSurveyQuestions()
        {
            return this.ObjectContext.SurveyQuestions.OrderBy(sq => sq.Order);
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'SurveyTypes' query.
        public IQueryable<SurveyType> GetSurveyTypes()
        {
            return this.ObjectContext.SurveyTypes.Where(st => st.Active);
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'SurveyUsers' query.
        public IQueryable<SurveyUser> GetSurveyUsers()
        {
            return this.ObjectContext.SurveyUsers.Where(su => su.active);
        }

        public IQueryable<FindUHTenant_Result> GetTenants(string name, string address)
        {
            return this.ObjectContext.FindUHTenant(name, address).AsQueryable();
        }

        public string GetUserName()
        {
            return System.Web.HttpContext.Current.User.Identity.Name;
        }
    }
}


