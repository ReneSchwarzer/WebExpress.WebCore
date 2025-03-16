using WebExpress.WebCore.WebAttribute;
using WebExpress.WebCore.WebSettingPage;

namespace WebExpress.WebCore.Test
{
    /// <summary>
    /// A dummy setting category for testing purposes.
    /// </summary>
    [Name("SettingCategory C")]
    [Description("Description of category c.")]
    [SettingSection(SettingSection.Secondary)]
    public sealed class TestSettingCategoryC : ISettingCategory
    {
    }
}
