namespace NGitLab.Models {
    public class BranchRemovalStatus {
        public readonly string Message;
        public readonly bool Succeed;

        BranchRemovalStatus(bool succeed, string message) {
            Succeed = succeed;
            Message = message;
        }

        public static BranchRemovalStatus FromReponseMessage(string message) {
            return Success();
        }

        public static BranchRemovalStatus Success() {
            return new BranchRemovalStatus(true, string.Empty);
        }

        public static BranchRemovalStatus Failure(string message) {
            return new BranchRemovalStatus(false, message);
        }
    }
}