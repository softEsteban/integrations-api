using HtmlAgilityPack;
using IntegrationsApi.Models;

namespace IntegrationsApi.Services
{
    public class SworkzService
    {
        private readonly HttpClient _httpClient;

        public SworkzService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<JobOfferingDto>> GetSworkzCareers()
        {
            string url = "https://sworkzgroup.com/careers/";
            string html = await _httpClient.GetStringAsync(url);

            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            var jobOfferings = new List<JobOfferingDto>();

            var articleElements = htmlDocument.DocumentNode.SelectNodes("//article[@class='job-offerings-item']");
            if (articleElements != null)
            {
                foreach (var article in articleElements)
                {
                    var jobOffering = new JobOfferingDto
                    {
                        Title = article.SelectSingleNode(".//h3/strong").InnerText.Trim(),
                        Location = article.SelectSingleNode(".//span[contains(text(), 'Medellín')]").InnerText.Trim(),
                        JobDescription = article.SelectNodes(".//ul/li[contains(., 'Job Description')]")
                                               ?.Select(node => node.InnerText.Trim()).ToList(),
                        Requirements = article.SelectNodes(".//ul/li[contains(., 'Work experience')]")
                                              ?.Select(node => node.InnerText.Trim()).ToList(),
                        DesirableSkills = article.SelectNodes(".//ul/li[starts-with(., 'Skills')]")
                                                ?.Select(node => node.InnerText.Trim()).ToList()
                    };

                    jobOfferings.Add(jobOffering);
                }
            }

            return jobOfferings;
        }
    }
}
