using BookStore.Constants;

namespace BookStore.Helpers
{
    public class RoleModules
    {
        private readonly string[] _CrudsArr;

        private readonly Dictionary<string, string[]> _roleCruds ;

        private RoleModules()
        {
            _CrudsArr = Enum
                .GetNames(typeof(Cruds))
                .Cast<string>()
                .ToArray();


            _roleCruds =new Dictionary<string, string[]>()
            {
                // Admins
                {$"{Roles.Admin}.{Modules.Category}",[Cruds.Read.ToString(), Cruds.Update.ToString()]},
                {$"{Roles.Admin}.{Modules.Author}",_CrudsArr },
                {$"{Roles.Admin}.{Modules.Book}",_CrudsArr},

                // Basic
                {$"{Roles.Basic}.{Modules.Book}",_CrudsArr},
                {$"{Roles.Basic}.{Modules.Author}",[Cruds.Read.ToString()] },
                {$"{Roles.Basic}.{Modules.Category}",[Cruds.Read.ToString()] }

            };
        }

        private static RoleModules _lock = new ();
        private static RoleModules _instance = new ();
        public static RoleModules instance 
        {
            get
            {
                lock (_lock)
                {
                    if(_instance == null)
                        _instance = new();
                    return _instance;
                }
            }
        }

        public string[] cruds(string roleName, string Module)
        {
            if (roleName == Roles.SuperAdmin.ToString())
                return _CrudsArr;
            return _roleCruds[$"{roleName}.{Module}"];
        }
    }
}
