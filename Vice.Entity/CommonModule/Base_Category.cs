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
    public class Base_Category
    {
        [DisplayName("分类主键")]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string CategoryId { get; set; }

        [DisplayName("分类名称")]
        public string CategoryName { get; set; }

        [DisplayName("创建时间")]
        public DateTime CreateDate { get; set; }

        [DisplayName("创建人")]
        public string UserId { get; set; }
    }
}
