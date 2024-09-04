

namespace LMSApi.App.Extentions
{
    public static class EntitiesExtenctions
    {

        public static async Task<string> TranslateFeild(this IEntity entity, AppDbContext context, string fieldName, int languageId)
        {

            if (entity == null)
            {
                return string.Empty;
            }
            if (entity.GetType().GetProperty(fieldName) == null)
            {
                throw new ArgumentException("Invalid field name");
            }

            var translation = await context.ClassTranslations
                .Where(ct => ct.ClassId == entity.Id && ct.LanguageId == languageId)
                .Select(ct => EF.Property<string>(ct, fieldName))
                .FirstOrDefaultAsync();

            return translation ?? entity.GetType().GetProperty(fieldName).GetValue(entity).ToString();

        }
    }
}
