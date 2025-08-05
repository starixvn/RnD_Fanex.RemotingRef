namespace Fanex.RemotingRef.NetCore.Models
{
    using PublicEnum.Language;

    public class League3rdModel
    {
        public int League3rdId { get; set; }

        [PublicEnum.Language.Language(LanguageEnum.En)]
        public string LeagueName { get; set; }

        [PublicEnum.Language.Language(LanguageEnum.Th)]
        public string LeagueNameTh { get; set; }

        [PublicEnum.Language.Language(LanguageEnum.Vn)]
        public string LeagueNameVn { get; set; }

        [PublicEnum.Language.Language(LanguageEnum.Cn)]
        public string LeagueNameSc { get; set; }

        [PublicEnum.Language.Language(LanguageEnum.Ch)]
        public string LeagueNameTc { get; set; }

        [PublicEnum.Language.Language(LanguageEnum.Sc2)]
        public string LeagueNameSc2 => LeagueNameSc;

        [PublicEnum.Language.Language(LanguageEnum.Id)]
        public string LeagueNameIn { get; set; }

        [PublicEnum.Language.Language(LanguageEnum.Jp)]
        public string LeagueNameJp { get; set; }

        [PublicEnum.Language.Language(LanguageEnum.Ko)]
        public string LeagueNameKo { get; set; }

        public long TimeSpan { get; set; }
    }
}
