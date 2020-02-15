using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Shop.ViewModel
{
    public class LoginViewModel
    {
        /// <summary>
        /// 登录账号
        /// </summary>
        [JsonProperty("username")]
        [Required]
        public string Username { get; set; }
        /// <summary>
        /// 登录密码
        /// </summary>
        [JsonProperty("password")]
        [Required]
        public string Password { get; set; }
    }
}
