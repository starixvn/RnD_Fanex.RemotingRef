using PublicEnum.Language;

namespace Fanex.RemotingRef.NetCore.Models;
public class BetTypeModel
{
    public int TypeId { get; set; }

    [PublicEnum.Language.Language(LanguageEnum.En)]
    public string TypeName { get; set; }


    [PublicEnum.Language.Language(LanguageEnum.Th)]
    public string TYpeNameTh { get; set; }


    [PublicEnum.Language.Language(LanguageEnum.Vn)]
    public string TypeNameVn { get; set; }


    [PublicEnum.Language.Language(LanguageEnum.Cn)]
    public string TypeNameCn { get; set; }


    [PublicEnum.Language.Language(LanguageEnum.Ch)]
    public string TypeNameC { get; set; }

    [PublicEnum.Language.Language(LanguageEnum.Sc2)]
    public string TypeNameSc2 => TypeNameCn;

    [PublicEnum.Language.Language(LanguageEnum.Id)]
    public string TypeNameID { get; set; }

    [PublicEnum.Language.Language(LanguageEnum.Jp)]
    public string TypeNameJp { get; set; }

    [PublicEnum.Language.Language(LanguageEnum.Ko)]
    public string LeagueNameKo { get; set; }

    public long TimeSpan { get; set; }
}