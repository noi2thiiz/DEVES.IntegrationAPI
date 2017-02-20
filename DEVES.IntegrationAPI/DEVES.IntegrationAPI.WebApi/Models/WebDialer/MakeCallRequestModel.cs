using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DEVES.IntegrationAPI.WebApi.Models.WebDialer
{
    public class MakeCallRequestModel
    {
        /// <summary>
        /// user ที่ตรงกับ โต้ะทำงาน
        /// </summary>
        [Required]
        public string UserName { get; set; }
        /// <summary>
        /// เบอร์ที่จะโทรออก มี 9 นำหน้า
        /// </summary>
        [Required]
        public string DestinationNumber { get; set; }
    }
}