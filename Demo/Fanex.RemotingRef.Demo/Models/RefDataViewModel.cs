namespace Fanex.RemotingRef.Demo.Models
{
    public class RefDataViewModel
    {
        public bool IsFirstLoad { get; set; } = true;

        public string TypeId { get; set; }

        public string Language { get; set; } = "en-US";

        public RefDataResult RefDataResult { get; set; }

        public string DataType { get; set; }

        public string TypeIdLabel { get; set; }

        public string[] Languages { get; set; } = new[]
        {
            "en-US",
            "zh-TW",
            "zh-CN",
            "ja-JP",
            "th-TH",
            "ko-KR",
            "vi-VN",
            "zh-CHS"
        };
    }
}