namespace NGitLab.Models
{
    public class BranchRemovalStatus
    {
        public readonly bool Succeed;
        public readonly string Message;

        public static BranchRemovalStatus FromReponseMessage(string message)
        {
            return Success();
        }

        public static BranchRemovalStatus Success()
        {
            return new BranchRemovalStatus(true, string.Empty);
        }

        public static BranchRemovalStatus Failure(string message)
        {
            return new BranchRemovalStatus(false, message);
        }

        private BranchRemovalStatus(bool succeed, string message)
        {
            Succeed = succeed;
            Message = message;
        }
    }
}