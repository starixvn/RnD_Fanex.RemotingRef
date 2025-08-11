namespace Fanex.RemotingRef.NetCore.Models
{
    using PublicEnum.Language;

    public class Team3rdModel
    {
        public int Team3rdId { get; set; }

        [Language(LanguageEnum.En)]
        public string TeamName { get; set; }

        [Language(LanguageEnum.Th)]
        public string TeamNameTh { get; set; }

        [Language(LanguageEnum.Vn)]
        public string TeamNameVn { get; set; }

        [Language(LanguageEnum.Cn)]
        public string TeamNameSc { get; set; }

        [Language(LanguageEnum.Ch)]
        public string TeamNameTc { get; set; }

        [Language(LanguageEnum.Sc2)]
        public string TeamNameSc2 => TeamNameSc;

        [Language(LanguageEnum.Id)]
        public string TeamNameIn { get; set; }

        [Language(LanguageEnum.Jp)]
        public string TeamNameJp { get; set; }

        [Language(LanguageEnum.Ko)]
        public string TeamNameKo { get; set; }

        public long TimeSpan { get; set; }
    }
}
