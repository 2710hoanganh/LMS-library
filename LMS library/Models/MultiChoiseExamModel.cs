using System.ComponentModel;

namespace LMS_library.Models
{
    public class MultiChoiseExamModel
    {
        public string examName { get; set; }
        [DefaultValue("Selected")]
        public string examType { get; set; }
        public string courseName { get; set; }
        public string time { get; set; }
        public List<MultipleChoiseQuestionModel> questions { get; set;}
    }
}
