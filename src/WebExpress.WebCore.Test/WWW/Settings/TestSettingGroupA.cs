using WebExpress.WebCore.WebAttribute;
using WebExpress.WebCore.WebSettingPage;

namespace WebExpress.WebCore.Test.WWW.Settings
{
    /// <summary>
    /// A dummy setting group for testing purposes.
    /// </summary>
    [WebIcon<TestIconPalette>]
    [Name("SettingGroup A")]
    [Description("Description of group a.")]
    [SettingCategory<TestSettingCategoryA>()]
    [SettingSection(SettingSection.Preferences)]
    public sealed class TestSettingGroupA : ISettingGroup
    {
    }
}
