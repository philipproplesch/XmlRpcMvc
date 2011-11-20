using System.Runtime.Serialization;

namespace $rootnamespace$.XmlRpc.Models
{
    public class UserInfo
    {
		[DataMember(Name = "userid")]
        public string UserId { get; set; }
		
		[DataMember(Name = "firstname")]
        public string FirstName { get; set; }
		
		[DataMember(Name = "lastname")]
        public string LastName { get; set; }
		
		[DataMember(Name = "url")]
        public string Url { get; set; }
		
		[DataMember(Name = "email")]
        public string Email { get; set; }
		
		[DataMember(Name = "nickname")]
        public string NickName { get; set; }
    }
}
