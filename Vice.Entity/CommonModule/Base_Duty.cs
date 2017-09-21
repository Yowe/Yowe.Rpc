using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vice.Entity
{
    public class Base_Schedule
    {
        [DisplayName("主键")]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string DutyId { get; set; }
        [DisplayName("值班日期")]
        public string WeekDay { get; set; }
        /// <summary>
        /// 值班人
        /// </summary>
        [DisplayName("值班人")]
        public string UserName { get; set; }

        [DisplayName("备注")]
        public string Memo { get; set; }

        [DisplayName("排班日期")]
        public DateTime CreateDate { get; set; }

        [DisplayName("排班人")]
        public string UserId { get; set; }
       
    }
}
