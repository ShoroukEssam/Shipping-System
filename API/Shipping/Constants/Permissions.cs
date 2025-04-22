namespace Shipping.Constants
{
    public static class Permissions
    {
        public static List<string> GeneratePermissionList(String module)
        {

            return new List<string>()
            {
                $"Permissions.{module}.View",
                $"Permissions.{module}.Create",
                $"Permissions.{module}.Edit",
                $"Permissions.{module}.Delete",
            };

        }

        public static List<string> GenerateAllPermissions()
        {

            var allPermissions = new List<string>();
            var modules = Enum.GetValues(typeof(Modules));

            foreach (var module in modules)
            {
                allPermissions.AddRange(GeneratePermissionList(module.ToString()));
            }

            return allPermissions;
        }

        #region Nested Classes for Roles 
        public static class Controls
        {
            public const string View = "Permissions.Controls.View";
            public const string Create = "Permissions.Controls.Create";
            public const string Edit = "Permissions.Controls.Edit";
            public const string Delete = "Permissions.Controls.Delete";
        }
        public static class Merchants
        {
            public const string View = "Permissions.Merchants.View";
            public const string Create = "Permissions.Merchants.Create";
            public const string Edit = "Permissions.Merchants.Edit";
            public const string Delete = "Permissions.Merchants.Delete";
        }
        public static class Employees
        {
            public const string View = "Permissions.Employees.View";
            public const string Create = "Permissions.Employees.Create";
            public const string Edit = "Permissions.Employees.Edit";
            public const string Delete = "Permissions.Employees.Delete";
        }
        public static class Deliveries
        {
            public const string View = "Permissions.Deliveries.View";
            public const string Create = "Permissions.Deliveries.Create";
            public const string Edit = "Permissions.Deliveries.Edit";
            public const string Delete = "Permissions.Deliveries.Delete";
        }
        public static class Orders
        {
            public const string View = "Permissions.Orders.View";
            public const string Create = "Permissions.Orders.Create";
            public const string Edit = "Permissions.Orders.Edit";
            public const string Delete = "Permissions.Orders.Delete";
        }
        public static class Branches
        {
            public const string View = "Permissions.Branches.View";
            public const string Create = "Permissions.Branches.Create";
            public const string Edit = "Permissions.Branches.Edit";
            public const string Delete = "Permissions.Branches.Delete";
        }
        public static class Governments
        {
            public const string View = "Permissions.Governments.View";
            public const string Create = "Permissions.Governments.Create";
            public const string Edit = "Permissions.Governments.Edit";
            public const string Delete = "Permissions.Governments.Delete";
        }
        public static class Cities
        {
            public const string View = "Permissions.Cities.View";
            public const string Create = "Permissions.Cities.Create";
            public const string Edit = "Permissions.Cities.Edit";
            public const string Delete = "Permissions.Cities.Delete";
        }

        public static class WeightSettings
        {
            public const string View = "Permissions.WeightSettings.View";
            public const string Create = "Permissions.WeightSettings.Create";
            public const string Edit = "Permissions.WeightSettings.Edit";
            public const string Delete = "Permissions.WeightSettings.Delete";
        }
        #endregion


    }
}
