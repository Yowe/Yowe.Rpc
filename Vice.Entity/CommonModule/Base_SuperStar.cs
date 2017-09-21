using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vice.Entity
{
    public class Base_SuperStar
    {
        [DisplayName("主键ID")]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string StarId { get; set; }

        [DisplayName("用户名")]
        public string UserName { get; set; }

        [DisplayName("用户照片")]
        public string Photo_Url { get; set; }

        [DisplayName("当选时间")]
        public string SelectDate { get; set; }

        [DisplayName("创建日期")]
        public DateTime CurrentDate { get; set; }
    }
}
