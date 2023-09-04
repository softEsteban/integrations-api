using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace IntegrationsApi.Models
{
    public class JobOfferingDto
    {
        public string Title { get; set; }
        public string Location { get; set; }
        public List<string> JobDescription { get; set; }
        public List<string> Requirements { get; set; }
        public List<string> DesirableSkills { get; set; }
    }
}
