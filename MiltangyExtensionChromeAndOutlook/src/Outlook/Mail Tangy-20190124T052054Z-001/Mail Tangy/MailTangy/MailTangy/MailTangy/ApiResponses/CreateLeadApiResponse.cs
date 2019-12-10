namespace MailTangy
{
    public class CreateLeadApiResponse
    {
        public string Message { get; set; }
        public string OwnerId { get; set; }
        public string Lead_No { get; set; }
        public string LeadId { get; set; }
    }

    /*
     * OUTPUT:-
        {
          "message" : "Lead created successfully",
          "ownerId" : "0056F00000AiGYnQAN",
          "Lead_No." : "L-2",
          "LeadId" : "00Q6F00001EuitbUAB"
        }
     */
}
