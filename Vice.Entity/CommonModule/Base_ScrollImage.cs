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
    public class Base_ScrollImage
    {
        [DisplayName("主键")]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string SrollImageId { get; set; }

        [DisplayName("标题")]
        public string ScrollTitle { get; set; } 
        
        [DisplayName("图片路径")]
        public string ScrollImageUrl { get; set; }

        /// <summary>
        /// 摘要
        /// </summary>
        public string ScrollAbstract { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string ScrollContent { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 浏览统计
        /// </summary>
        public int ScrollStatistical { get; set; }
        /// <summary>
        /// 作者
        /// </summary>
        public string Author { get; set; }

     

    }
}
