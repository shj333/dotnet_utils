using BerwickHeights.Platform.Core.CurrentUser;
using BerwickHeights.Platform.IoC;

namespace BerwickHeights.Platform.Core.Test
{
    public static class CurrentUserSvcTest
    {
        public const string TestUserId = "1a21adf2-6c71-46ef-b280-743de205fa72";
        public const string TestUserName = "test user";
        public const string TestSessionId = "fa14da72-b9d4-4ec2-a447-2e772d9bca94";

        public static void SetTestCurrentUserData()
        {
            ICurrentUserSvc curUserSvc = IoCContainerManagerFactory.GetIoCContainerManager().Resolve<ICurrentUserSvc>();
            curUserSvc.SetCurrentUserData(new CurrentUserData(TestUserId, TestUserName, TestSessionId));
        }
    }
}
