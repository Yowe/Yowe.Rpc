using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vice.Entity
{
    public class Base_Notify
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        public string Notify_Id
        {
            get; set;
        }
        /// <summary>
        /// 通知内容
        /// </summary>
        public string Notify_Content
        {
            get;set;
        }
        /// <summary>
        /// 通知时间
        /// </summary>
        public string Notify_ShortDate
        {
            get;set;
        }
        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreateDate
        {
            get;set;
        }
        /// <summary>
        /// 创建人
        /// </summary>
        public string UserId
        {
            get;set;
        }
     
    }
}
