using System.Runtime.Serialization;

namespace $rootnamespace$.XmlRpc.Models
{
    public class MediaObject
    {
        [DataMember(Name = "type")]
        public string TypeName { get; set; }
		
		[DataMember(Name = "name")]
        public string Name { get; set; }
        
		[DataMember(Name = "bits")]
		public byte[] Bits { get; set; }
    }
}
