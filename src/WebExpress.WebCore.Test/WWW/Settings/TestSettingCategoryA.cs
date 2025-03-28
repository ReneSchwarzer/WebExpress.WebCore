using WebExpress.WebCore.WebAttribute;
using WebExpress.WebCore.WebSettingPage;

namespace WebExpress.WebCore.Test.WWW.Settings
{
    /// <summary>
    /// A dummy setting category for testing purposes.
    /// </summary>
    [WebIcon<TestIconBell>]
    [Name("SettingCategory A")]
    [Description("Description of category a.")]
    [SettingSection(SettingSection.Preferences)]
    public sealed class TestSettingCategoryA : ISettingCategory
    {
    }
}
