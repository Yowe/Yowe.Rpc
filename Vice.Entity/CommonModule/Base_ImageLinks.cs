using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vice.Entity
{
    public class Base_ImageLinks
    {
        /// <summary>
        /// 主键
        /// </summary>
         [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ImageId { get; set; }

        /// <summary>
        /// 图片路径
        /// </summary>
        public string ImageUrl { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 再要
        /// </summary>
        public string Abstract { get; set; }

        /// <summary>
        /// 新闻作者
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// 新闻内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string UserId { get; set; }
       
    }
}
