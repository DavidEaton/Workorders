using System.Web.Mvc;

namespace Workorders.Web.ViewModels.ReportRecipient
{
    public class ReportRecipientGridViewModel
    {
        public int Id { get; set; }
        public int AreaId { get; set; }
        public string AreaName { get; set; }
        public string Emails { get; set; }
        public SelectList Areas { get; set; }
        public SelectList Companies { get; set; }
        public string SelectedCompany { get; set; }
    }
}