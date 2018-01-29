using System.Web.Mvc;

namespace FancyFix.OA.Areas.Questionnaire
{
    public class QuestionnaireAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Questionnaire";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Questionnaire_default",
                "Questionnaire/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "FancyFix.OA.Areas.Questionnaire.Controllers" }
            );
        }
    }
}