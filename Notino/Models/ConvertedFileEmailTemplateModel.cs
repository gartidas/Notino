namespace Notino.Models
{
    public class ConvertedFileEmailTemplateModel
    {
        public ConvertedFileEmailTemplateModel(string url)
        {
            Url = url;
        }

        public string Url { get; }
    }
}
