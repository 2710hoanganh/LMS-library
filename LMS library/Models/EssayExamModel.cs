using System.ComponentModel;

namespace LMS_library.Models
{
    public class EssayExamModel
    {
        public string examName { get; set; }
        [DefaultValue("Contructed")]
        public string examType { get; set; }
        public string courseName { get; set; }
        public string time { get; set; }
        public List<EssayQuestionModel> questions { get; set; }
    }
}
