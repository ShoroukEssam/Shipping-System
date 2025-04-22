namespace Shipping.Constants
{
    public enum Modules
    {
        Controls,
        Merchants,
        Deliveries,
        Employees,
        Orders,
        Branches,
        Governments,
        Cities,
        WeightSettings

    }
    public static class EnglishVsArabic
    {
        public static readonly Dictionary<string, string> ModulesInEn_AR = new Dictionary<string, string>
        {
           // {"key","Value" },
            { "Controls", "المجموعات" },
            { "Merchants", "التجار" },
            { "Deliveries", "المناديب" },
            { "Employees", "الموظفين" },
            { "Orders", "الطلبات" },
            { "Branches", "الافرع" },
            { "Governments", "المحافظات" },
            { "Cities", "المدن" },
            { "WeightSettings", "اعدادات الوزن" }


        };
    }


}
