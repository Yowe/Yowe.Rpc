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
    public class Base_User
    {
        [DisplayName("用户主键")]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string UserId { get; set; }
        [DisplayName("姓名")]

        public string UserName { get; set; }

        [DisplayName("登录名")]
        public string AccountName { get; set; }

        /// <summary>
        /// 照片
        /// </summary>
        public string UserIcon { get; set; }
        
        /// <summary>
        /// 登录密码
        /// </summary>
        [DisplayName("登录密码")]
        public string UserPass { get; set; }
    }
}
