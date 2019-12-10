namespace MailTangy
{
    public class CreateCaseApiResponse
    {
        public string Message { get; set; }
        public string CaseNumber { get; set; }
        public string OwnerId { get; set; }
        public string CaseId { get; set; }
    }

    /*
     * OUTPUT:-
        {
          "message" : "Case created successfully",
          "casenumber" : "00001027",
          "ownerId" : "0056F00000AEKaPQAX",
          "caseId" : "5006F00001tkyxtQAA"
        }
     */
}
