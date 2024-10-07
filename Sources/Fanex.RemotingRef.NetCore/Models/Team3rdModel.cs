using PublicEnum.Language;
namespace Fanex.RemotingRef.NetCore.Models;
public class Team3rdModel
{
    public int Team3rdId { get; set; }


    [PublicEnum.Language.Language(LanguageEnum.En)]
    public string TeamName { get; set; }

    [PublicEnum.Language.Language(LanguageEnum.Th)]
    public string TeamNameTh { get; set; }

    [PublicEnum.Language.Language(LanguageEnum.Vn)]
    public string TeamNameVn { get; set; }

    [PublicEnum.Language.Language(LanguageEnum.Cn)]
    public string TeamNameSc { get; set; }

    [PublicEnum.Language.Language(LanguageEnum.Ch)]
    public string TeamNameTc { get; set; }

    [PublicEnum.Language.Language(LanguageEnum.Sc2)]
    public string TeamNameSc2 => TeamNameSc;

    [PublicEnum.Language.Language(LanguageEnum.Id)]
    public string TeamNameIn { get; set; }

    [PublicEnum.Language.Language(LanguageEnum.Jp)]
    public string TeamNameJp { get; set; }

    [PublicEnum.Language.Language(LanguageEnum.Ko)]
    public string TeamNameKo { get; set; }
    public long TimeSpan { get; set; }
}