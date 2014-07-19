namespace Db.Entity.Finder
{
    public class Finder
    {
        /// <summary>
        /// ИД пользователя
        /// </summary>
        public int? UserId { get; set; }

        /// <summary>
        /// ИД организации пользователя
        /// </summary>
        public int? UserOrganizationId { get; set; }

        public bool IsOperator
        {
            get 
            { 
                return UserOrganizationId == 1440 ||
                    UserOrganizationId == 1147 ||
                    UserOrganizationId == 1444 ||
                    UserOrganizationId == 1445 ||
                    UserOrganizationId == 1446 ||
                    UserOrganizationId == 1448 ||
                    UserOrganizationId == 1449 ||
                    UserOrganizationId == 1447 ||
                    UserOrganizationId == 1443; 
            }
        }

        public int Skip { get; set; }
        public int Rows { get; set; }
    }
}