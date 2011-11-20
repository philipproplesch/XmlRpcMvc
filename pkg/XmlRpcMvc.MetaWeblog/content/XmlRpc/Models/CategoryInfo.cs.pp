using System.Runtime.Serialization;

namespace $rootnamespace$.XmlRpc.Models
{
    public class CategoryInfo
    {
        [DataMember(Name = "description")]
        public string Description { get; set; }
		
		[DataMember(Name = "title")]
        public string Title { get; set; }
    }
}
