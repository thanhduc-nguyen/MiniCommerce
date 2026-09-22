namespace MiniCommerce.Web;

public static class Helper
{
    public static IEnumerable<T> TakeRandom<T>(this IEnumerable<T> items, int numberOfItemsToSelect)
    {
        var list = items.ToList();
        Random random = new();
        List<T> selectedItems = [];

        for (int i = 0; i < numberOfItemsToSelect; i++)
        {
            if (list.Count == 0)
            {
                return list;
            }

            int randomIndex = random.Next(list.Count);
            var randomItem = list[randomIndex];
            selectedItems.Add(randomItem);
            list.RemoveAt(randomIndex);
        }

        return selectedItems;
    }

    public static string GetReturnUrl(HttpContext httpContext)
    {
        var path = httpContext.Request.Path.Value;

        if (path == null)
        {
            return "/";
        }
        else if (path.StartsWith("/Account"))
        {
            return "/";
        }
        else
        {
            return path + httpContext.Request.QueryString;
        }
    }
}
