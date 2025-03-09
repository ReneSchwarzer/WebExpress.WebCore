using WebExpress.WebCore.WebAttribute;
using WebExpress.WebCore.WebSettingPage;
using WebExpress.WebCore.WebSettingPage.Model;

namespace WebExpress.WebCore.Test
{
    /// <summary>
    /// A dummy setting category for testing purposes.
    /// </summary>
    [Icon("InfoCircle")]
    [Name("SettingCategory A")]
    [Description("Description of category a.")]
    [SettingSection(SettingSection.Preferences)]
    public sealed class TestSettingCategoryA : ISettingCategory
    {
    }
}
