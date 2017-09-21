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
    public class Base_Article
    {
        [DisplayName("主键")]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ArticleId { get; set; }

        [DisplayName("摘要")]
        public string ArticleTitle { get; set; }

        [DisplayName("摘要")]
        public string ArticleAbstract { get; set; }

        [DisplayName("内容")]
        public string ArticleContent { get; set; }

        [DisplayName("分类主键")]
        public string ArticleCategoryId { get; set; }

        [DisplayName("阅读统计")]
        public int ArticleStatistical { get; set; }

        [DisplayName("图片列表")]
        public string ArticleImages { get; set; }

        [DisplayName("创建时间")]
        public DateTime CreateDate { get; set; }

        [DisplayName("创建人主键")]
        public string UserId { get; set; }
        /// <summary>
        /// 作者
        /// </summary>
        public string Author { get; set; }
       
    }
}
