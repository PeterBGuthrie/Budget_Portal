using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Budget_Portal.Models
{
    public class Notification
    {
        public int Id { get; set; }

        public int HouseHoldId { get; set; }

        public virtual HouseHold HouseHold { get; set; }

        public string RecipientId { get; set; }

        public virtual ApplicationUser Recipient { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public DateTime Created { get; set; }

        public bool IsRead { get; set; }

        public Notification()
        {
            Created = DateTime.Now;
        }


    }
}