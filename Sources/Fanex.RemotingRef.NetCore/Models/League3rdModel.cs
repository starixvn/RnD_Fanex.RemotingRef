namespace Fanex.RemotingRef.NetCore.Models
{
    using PublicEnum.Language;

    public class League3rdModel
    {
        public int League3rdId { get; set; }

        [Language(LanguageEnum.En)]
        public string LeagueName { get; set; }

        [Language(LanguageEnum.Th)]
        public string LeagueNameTh { get; set; }

        [Language(LanguageEnum.Vn)]
        public string LeagueNameVn { get; set; }

        [Language(LanguageEnum.Cn)]
        public string LeagueNameSc { get; set; }

        [Language(LanguageEnum.Ch)]
        public string LeagueNameTc { get; set; }

        [Language(LanguageEnum.Sc2)]
        public string LeagueNameSc2 => LeagueNameSc;

        [Language(LanguageEnum.Id)]
        public string LeagueNameIn { get; set; }

        [Language(LanguageEnum.Jp)]
        public string LeagueNameJp { get; set; }

        [Language(LanguageEnum.Ko)]
        public string LeagueNameKo { get; set; }

        public long TimeSpan { get; set; }
    }
}
