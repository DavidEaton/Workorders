﻿using Workorders.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Workorders.Web.ViewModels.AlertRecipient
{
    public class AlertRecipientGridViewModel
    {
        public int Id { get; set; }
        public int AreaId { get; set; }
        public string AreaName { get; set; }
        public string Emails { get; set; }
        public SelectList Areas { get; set; }
    }
}