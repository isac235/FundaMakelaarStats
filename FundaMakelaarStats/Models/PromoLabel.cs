namespace FundaMakelaarStats.Models
{
    public record PromoLabel(
        bool HasPromotionLabel,
        List<object> PromotionPhotos,
        object PromotionPhotosSecure,
        int? PromotionType,
        int? RibbonColor,
        string RibbonText,
        string Tagline
        );
}
