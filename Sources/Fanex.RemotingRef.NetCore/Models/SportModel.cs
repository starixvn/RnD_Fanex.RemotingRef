using PublicEnum.Language;

namespace Fanex.RemotingRef.NetCore.Models;
public class SportModel
{
    public int SportType { get; set; }

    [Language(LanguageEnum.En)]
    public string SportName { get; set; }

    [Language(LanguageEnum.Th)]
    public string SportNameTh { get; set; }


    [Language(LanguageEnum.Vn)]
    public string SportNameVN { get; set; }

    [Language(LanguageEnum.Cn)]
    public string SportNameCs { get; set; }

    [Language(LanguageEnum.Ch)]
    public string SportNameCh { get; set; }

    [Language(LanguageEnum.Sc2)]
    public string SportNameSc2 => SportNameCs;

    [Language(LanguageEnum.Id)]
    public string SportNameID { get; set; }

    [Language(LanguageEnum.Jp)]
    public string SportNameJp { get; set; }

    [Language(LanguageEnum.Ko)]
    public string SportNameKo { get; set; }
}