using PublicEnum.Language;

namespace Fanex.RemotingRef.NetCore.Models;
public class SportModel
{
    public int SportType { get; set; }

    [PublicEnum.Language.Language(LanguageEnum.En)]
    public string SportName { get; set; }

    [PublicEnum.Language.Language(LanguageEnum.Th)]
    public string SportNameTh { get; set; }


    [PublicEnum.Language.Language(LanguageEnum.Vn)]
    public string SportNameVn { get; set; }

    [PublicEnum.Language.Language(LanguageEnum.Cn)]
    public string SportNameCs { get; set; }


    [PublicEnum.Language.Language(LanguageEnum.Ch)]
    public string SportNameCh { get; set; }

    [PublicEnum.Language.Language(LanguageEnum.Sc2)]
    public string SportNameSc2 => SportNameCs;

    [PublicEnum.Language.Language(LanguageEnum.Id)]
    public string SportNameId { get; set; }

    [PublicEnum.Language.Language(LanguageEnum.Jp)]
    public string SportNameJp { get; set; }

    [PublicEnum.Language.Language(LanguageEnum.Ko)]
    public string SportNameKo { get; set; }
}