using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.Model.CTI
{
    public class VoiceRecordRequestModel : BaseDataModel
    {
        /// <summary> ID เพื่อเชื่อมโยง และใช้ผูกกับ CRM </summary>

        [JsonProperty(PropertyName = "callTransactionId", Required = Required.AllowNull)]
        public string callTransactionId { get; set; }

        /// <summary> ID ของไฟลเ์สียง </summary>
        [Required]
        [JsonProperty(PropertyName = "sessionId", Required = Required.Always)]
        public string sessionId { get; set; }

        /// <summary> Action ไฟลเสียงเริ่มบันทึก หรือ จบการบันทึก </summary>
        //[Required]
        [EnumDataType(typeof(CTIEventAction))]
        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty(PropertyName = "eventAction", Required = Required.Always)]
        public CTIEventAction eventAction { get; set; }

        /// <summary> User ID ของ User ที่ถูกบันทึกเสียง</summary>
        [Required]
        public string userid { get; set; }

        /// <summary> หมายเลขที่โทรออก</summary>
        [Required]
        public string callernumber { get; set; }

        /// <summary> หมายเลขที่รับสาย</summary>
        [Required]
        public string callednumber { get; set; }

        /// <summary> เป็นการประชุมสายหรือไม่</summary>
        [Required]
        //[JsonConverter(typeof(BooleanJsonConverter))]
        public bool isConference { get; set; }

        /// <summary> sessionStartDate long เวลาที่เริ่มบันทึกเสียงเป็น UNIX Timestamp ในหน่วย millisecond</summary>
        [Required]
        public long sessionStartDate { get; set; }

        /// <summary>ความยาวของไฟลเ์สียงในหน่วย millisecond (จะมีใน eventAction ENDED เท่านั้น) </summary>

        public long sessionDuration { get; set; }

        /// <summary>State ของไฟลเ์สียง</summary>
        [Required]
        public string sessionState { get; set; }

        /// <summary> </summary>
        [Required]
        public string url { get; set; }


        public string callType { get; set; }
    }


    [JsonConverter(typeof(StringEnumConverter))]
    public enum CTIEventAction
    {
        /// <summary>STARTED</summary>
        [EnumMember(Value = "STARTED")]
        STARTED,

        /// <summary>ENDED</summary>
        [EnumMember(Value = "ENDED")]
        ENDED,
    }
}
